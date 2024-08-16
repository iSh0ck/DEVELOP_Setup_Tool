using IWshRuntimeLibrary;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Vela31_Ineo.Develop_Library;
using File = System.IO.File;

namespace Vela31_Ineo
{
    public static class PrinterClass
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Printer);
    }
    static class Program
    {
        public static IDictionary<string, string> drivers = new Dictionary<string, string>();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AddPrintersDrivers();
            Application.Run(new Home());
        }

        /*
         * 
         * --------------------- OK ---------------------
         * 
         */
        public static async Task DownloadDriver(string os, string model_name)
        {
            // Téléchargement du driver depuis internet
            // Adresse du serveur de téléchargement
            String server = "drivers.domain_name.fr";

            // Création du dossier téléchargement
            string downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "Download");
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            string downloadFilePath = Path.Combine(downloadPath, "driver.zip");
            string driverUrl = "https://" + server + "/Drivers Develop/" + os + "/" + model_name + "/PCL6/driver.zip";

            Home.progressBar1.Visible = true;

            using (WebClient client = new WebClient())
            {
                // Mettre à jour la progressbar
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                try
                {
                    await client.DownloadFileTaskAsync(new Uri(driverUrl), downloadFilePath);
                    Console.WriteLine("Téléchargement terminé.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du téléchargement : " + ex.Message);
                }
            }
        }

        // Event pour suivi du téléchargement
        public static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Home.progressBar1.Value = e.ProgressPercentage;
            Console.WriteLine($"Téléchargement à {e.ProgressPercentage}%");
        }

        /*
         * 
         * ---------------------- OK ----------------------
         * 
         */

        public static void UnzipArchive(string file)
        {
            // Unzip du fichier 
            ZipFile.ExtractToDirectory(file, Directory.GetCurrentDirectory() + @"\Download");
        }

        /*
         * 
         * --------------------- OK ---------------------
         *         
         */
        public static async Task InstallDriver(string model_name, string ipaddr, bool isOfflineMode)
        {
            // Lancement d'un CMD pour utiliser les scripts windows
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            // Positionnement sur le disque C
            cmd.StandardInput.WriteLine("C:");
            cmd.StandardInput.Flush();

            // Installation du driver
            cmd.StandardInput.WriteLine(@"cd C:\Windows\System32\Printing_Admin_Scripts\fr-FR");
            cmd.StandardInput.Flush();

            // Online Mode
            if (!isOfflineMode)
            {
                string pathToInfFile = Directory.GetCurrentDirectory() + @"\Download";

                // Trouver le nom du fichier complet .inf
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Download");
                string filename = "";
                foreach (string file in files)
                {
                    filename = Path.GetFileName(file);
                    if (Path.GetExtension(filename) == ".inf")
                    {
                        filename = Path.GetFileName(file);
                        break;
                    }
                }

                // Si pas d'extension trouvée message d'erreur
                if (filename == "")
                {
                    throw new Exception("Le fichier .inf n'a pas pu être trouvé");
                }

                string downloadedInfFile = Directory.GetCurrentDirectory() + @"\Download\" + filename;
                cmd.StandardInput.WriteLine("cscript prndrvr.vbs -a -m " +
                                            '"' + GetDriverName(model_name) + '"' + " -h " +
                                            '"' + pathToInfFile + '"' + " -i " +
                                            '"' + downloadedInfFile + '"');
                cmd.StandardInput.Flush();

                // Création du port TCP/IP
                cmd.StandardInput.WriteLine("cscript prnport.vbs -a -r IP_" + ipaddr + " -h " + ipaddr + " -o raw");
                cmd.StandardInput.Flush();

                // Installation de l'imprimante
                cmd.StandardInput.WriteLine("cscript prnmngr.vbs -a -p " +
                                            '"' + "Copieur " + model_name + '"' + " -m " +
                                            '"' + GetDriverName(model_name) + '"' + " -r IP_" + ipaddr);
                cmd.StandardInput.Flush();

                // Fermeture du cmd
                cmd.StandardInput.Close();
                cmd.WaitForExit();

                if (Home.check_setAsDefault.Checked)
                {
                    PrinterClass.SetDefaultPrinter("Copieur " + model_name);
                }

                Directory.Delete(Directory.GetCurrentDirectory() + @"\Download", true);
            }
            // Offline Mode
            else
            {
                // Vérifier si le fichier "driver.zip" existe
                if (File.Exists(Directory.GetCurrentDirectory() + @"\driver.zip"))
                {
                    // Créer un dossier temporaire pour l'extraction du .zip
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\temp_driver_dir");
                }
                else
                {
                    // Affichage d'un message box pour lancer ou non le téléchargement
                    string dlog_message = "File \"driver.zip\" not found. Would you like to download the file from the internet?";
                    string dlog_title = "Missing driver.zip";
                    MessageBoxButtons dlog_buttons = MessageBoxButtons.YesNo;

                    DialogResult dlog_result = MessageBox.Show(dlog_message, dlog_title, dlog_buttons);

                    if (dlog_result == DialogResult.Yes)
                    {
                        // Si le fichier "driver.zip" n'existe pas, proposer de le télécharger via internet.
                        await DownloadDriver("Windows 10", "Universal_Printer_Driver");

                        // La fonction va créer un dossier "Download" il faut donc déplacer le fichier et le supprimer
                        File.Move(Directory.GetCurrentDirectory() + @"\Download\driver.zip", Directory.GetCurrentDirectory() + @"\driver.zip");
                        Directory.Delete(Directory.GetCurrentDirectory() + @"\Download");
                    }
                    else
                    {
                        MessageBox.Show("Operation canceled", "Canceled");
                        return;
                    }
                }

                // Unzip du fichier 
                ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + @"\driver.zip", Directory.GetCurrentDirectory() + @"\temp_driver_dir");

                // Lancer l'installation depuis le fichier.
                string pathToInfFile = Directory.GetCurrentDirectory() + @"\temp_driver_dir";

                // Trouver le nom du fichier complet .inf
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\temp_driver_dir");
                string filename = "";
                foreach (string file in files)
                {
                    filename = Path.GetFileName(file);
                    if (Path.GetExtension(filename) == ".inf")
                    {
                        filename = Path.GetFileName(file);
                        break;
                    }
                }

                // Si pas d'extension trouvée message d'erreur
                if (filename == "")
                {
                    throw new Exception("Le fichier .inf n'a pas pu être trouvé");
                }

                string downloadedInfFile = Directory.GetCurrentDirectory() + @"\temp_driver_dir\" + filename;
                cmd.StandardInput.WriteLine("cscript prndrvr.vbs -a -m " +
                                            '"' + GetDriverName("Universal_Printer_Driver") + '"' + " -h " +
                                            '"' + pathToInfFile + '"' + " -i " +
                                            '"' + downloadedInfFile + '"');
                cmd.StandardInput.Flush();

                // Création du port TCP/IP
                cmd.StandardInput.WriteLine("cscript prnport.vbs -a -r IP_" + ipaddr + " -h " + ipaddr + " -o raw");
                cmd.StandardInput.Flush();

                // Installation de l'imprimante
                cmd.StandardInput.WriteLine("cscript prnmngr.vbs -a -p " +
                                            '"' + "Copieur " + model_name + '"' + " -m " +
                                            '"' + GetDriverName("Universal_Printer_Driver") + '"' + " -r IP_" + ipaddr);
                cmd.StandardInput.Flush();

                // Fermeture du cmd
                cmd.StandardInput.Close();
                cmd.WaitForExit();

                if (Home.check_setAsDefault.Checked)
                {
                    PrinterClass.SetDefaultPrinter("Copieur " + model_name);
                }

                // Supression du dossier temporaire à la fin de l'installation
                Directory.Delete(Directory.GetCurrentDirectory() + @"\temp_driver_dir", true);
            }
            
        }

        /*
         * 
         * --------------------- OK ---------------------
         * 
         */
        public static async void SetupSMBInWebPanel(string ipaddr, string contactName, string hostname)
        {
            // Connexion au copieur + Récuppération du token
            string token = await Login_As_Public(ipaddr);

            // Envoi des infos SMB vers le copieur
            await Add_Smb_Contact(ipaddr, token, contactName, hostname, "Scans", "scan", "Sc@nner31");

            // Déconnexion
            await Logout(ipaddr);
        }

        public static void FindPrinters()
        {
            // SNMP Setup
            OctetString community = new OctetString("public");
            ObjectIdentifier sysDescrOid = new ObjectIdentifier(".1.3.6.1.2.1.1.1.0");
            int timeout = 100;

            // Déclaration du tableau contenant les imprimantes trouvées
            IDictionary<string, string> printers = new Dictionary<string, string>();

            MFP_Found mFP_Found = new MFP_Found();

            // Itérer sur chaque interface réseau
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            // Vérifier si l'adresse IP est une adresse de boucle locale
                            if (Network_Tool.IsLocalAddress(ip.Address))
                                continue;

                            // Récupérer le sous-réseau et le CIDR
                            string subnet = Network_Tool.GetSubnetAddress(ip.Address, ip.IPv4Mask);
                            int cidr = Network_Tool.GetCIDR(ip.IPv4Mask);

                            // Déterminer la plage d'adresses IP à partir du sous-réseau
                            var ipRange = Network_Tool.GetIPRange(subnet, cidr);

                            // Scanner chaque adresse IP dans la plage
                            foreach (var ipAddress in ipRange)
                            {
                                try
                                {
                                    // Crée un point de terminaison IP pour chaque adresse
                                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), 161);

                                    // Effectue une requête SNMP GET
                                    var result = Messenger.Get(VersionCode.V1, endpoint, community,
                                                                new List<Variable> { new Variable(sysDescrOid) }, timeout);

                                    if (result != null && result.Count > 0)
                                    {
                                        string sysDescr = result[0].Data.ToString();

                                        // Vérifier si le résultat contient "Develop" et "Konica"
                                        if (sysDescr.Contains("Develop") || sysDescr.Contains("Konica"))
                                        {
                                            printers.Add(ipAddress, sysDescr);
                                            Console.WriteLine(ipAddress + '\t' + sysDescr);
                                        }
                                    }
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
            }

            if (printers.Count == 0)
            {
                MessageBox.Show("No printer(s) found.", "Error");
            }
            else
            {
                foreach (var printer in printers)
                {
                    // Crée un nouvel objet ListViewItem pour chaque imprimante
                    ListViewItem item = new ListViewItem(printer.Key);
                    item.SubItems.Add(printer.Value);
                    MFP_Found.listView1.Items.Add(item);
                }

                mFP_Found.Text = printers.Count.ToString() + " printer(s) found";
                mFP_Found.Show();
            }
        }

        /*
         * 
         * --------------------- OK ---------------------
         * 
         */
        public static void SetupSMB(String smbValue)
        {
            // Vérification si on doit setup ou non le SMB
            if (smbValue == "Yes")
            {
                try
                {
                    // Création de l'utilisateur
                    DirectoryEntry ad = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                    DirectoryEntry newUser = ad.Children.Add("Scan", "user");
                    newUser.Invoke("SetPassword", new object[] { "Sc@nner31" });
                    newUser.Invoke("Put", new object[] { "Description", "Scan user for Vela printers" });
                    newUser.CommitChanges();

                    // Création du dossier
                    if (!Directory.Exists(@"C:\Scans"))
                    {
                        Directory.CreateDirectory(@"C:\Scans");
                    }

                    // Activation du partage sur le dossier
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    // Vérifier si fonctionne sur un domaine
                    cmd.StandardInput.WriteLine(@"net share Scans=C:\Scans /grant:scan,full");
                    cmd.StandardInput.Flush();

                    // Ajouter les permissions sur le dossier à l'utilisateur scan
                    cmd.StandardInput.WriteLine("icacls " + '"' + @"C:\Scans" + '"' + " /grant Scan:(OI)(CI)F /T");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();

                    // Créer un raccourcis vers le bureau
                    string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    WshShell wshShell = new WshShell();
                    string settingsLink = Path.Combine(desktopFolder, "Scans.lnk");
                    IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(settingsLink);
                    shortcut.TargetPath = @"C:\Scans";
                    shortcut.Description = "Dossier raccourcis Scans";
                    shortcut.Save();
                    if (Home.chk_sendToPrinter.Checked == true && Home.text_ip_address != null && Home.text_ip_address.Text != "" && Home.text_ip_address.Text != "Printer IP Address")
                    {
                        try
                        {
                            SetupSMBInWebPanel(Home.text_ip_address.Text, Home.txt_contactName.Text, Environment.MachineName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("SMB (WebPanel): " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SMB: " + ex.Message);
                }
            }
        }

        public static string GetDriverName(string model_name)
        {
            return drivers[model_name];
        }

        public static void AddPrintersDrivers()
        {
            // Adding Generic Universal PCL printers to the list
            drivers.Add("Ineo 224", "Generic Universal PCL");
            drivers.Add("Ineo 224e", "Generic Universal PCL");
            drivers.Add("Ineo 284e", "Generic Universal PCL");
            drivers.Add("Ineo 362", "Generic Universal PCL");
            drivers.Add("Ineo 364e", "Generic Universal PCL");
            drivers.Add("Ineo 454e", "Generic Universal PCL");
            drivers.Add("Ineo 550i", "Generic Universal PCL");
            drivers.Add("Ineo 554e", "Generic Universal PCL");
            drivers.Add("Ineo 654", "Generic Universal PCL");
            drivers.Add("Ineo 654e", "Generic Universal PCL");
            drivers.Add("Ineo 754", "Generic Universal PCL");
            drivers.Add("Ineo 754e", "Generic Universal PCL");
            drivers.Add("Ineo 3320", "Generic Universal PCL");
            drivers.Add("Ineo 4020", "Generic Universal PCL");
            drivers.Add("Ineo+ 224", "Generic Universal PCL");
            drivers.Add("Ineo+ 224e", "Generic Universal PCL");
            drivers.Add("Ineo+ 250", "Generic Universal PCL");
            drivers.Add("Ineo+ 257i", "Generic Universal PCL");
            drivers.Add("Ineo+ 284", "Generic Universal PCL");
            drivers.Add("Ineo+ 284e", "Generic Universal PCL");
            drivers.Add("Ineo+ 364", "Generic Universal PCL");
            drivers.Add("Ineo+ 364e", "Generic Universal PCL");
            drivers.Add("Ineo+ 451", "Generic Universal PCL");
            drivers.Add("Ineo+ 452", "Generic Universal PCL");
            drivers.Add("Ineo+ 454", "Generic Universal PCL");
            drivers.Add("Ineo+ 454e", "Generic Universal PCL");
            drivers.Add("Ineo+ 554", "Generic Universal PCL");
            drivers.Add("Ineo+ 554e", "Generic Universal PCL");
            drivers.Add("Ineo+ 654", "Generic Universal PCL");
            drivers.Add("Ineo+ 654e", "Generic Universal PCL");
            drivers.Add("Ineo+ 754", "Generic Universal PCL");
            drivers.Add("Ineo+ 754e", "Generic Universal PCL");
            drivers.Add("Ineo 458e", "Generic Universal PCL");
            drivers.Add("Ineo 558e", "Generic Universal PCL");
            drivers.Add("Ineo 308e", "Generic Universal PCL");
            drivers.Add("Universal_Printer_Driver", "Generic Universal PCL");

            // Adding Generic 65C-0iSeriesPCL printers to the list
            drivers.Add("Ineo+ 450i", "Generic 65C-0iSeriesPCL");
            drivers.Add("Ineo+ 550i", "Generic 65C-0iSeriesPCL");
            drivers.Add("Ineo+ 650i", "Generic 65C-0iSeriesPCL");

            // Adding Generic 36C-0iSeriesPCL printers to the list
            drivers.Add("Ineo+ 250i", "Generic 36C-0iSeriesPCL");
            drivers.Add("Ineo+ 300i", "Generic 36C-0iSeriesPCL");
            drivers.Add("Ineo+ 360i", "Generic 36C-0iSeriesPCL");

            // Adding Generic C405-0iSeriesPCL printers to the list
            drivers.Add("Ineo+ 4050i", "Generic C405-0iSeriesPCL");

            // Adding Generic C400-0iSeriesPCL printers to the list
            drivers.Add("Ineo+ 4000i", "Generic C400-0iSeriesPCL");

            // Adding Generic C400-0iSeriesPCL printers to the list
            drivers.Add("Ineo+ 3300i", "Generic C332-0i PCL");
            drivers.Add("Ineo+ 3320i", "Generic C332-0i PCL");
            drivers.Add("Ineo+ 3350i", "Generic C332-0i PCL");

            // Adding Generic 65C-9SeriesPCL printers to the list
            drivers.Add("Ineo+ 458", "Generic 65C-9SeriesPCL");
            drivers.Add("Ineo+ 558", "Generic 65C-9SeriesPCL");
            drivers.Add("Ineo+ 658", "Generic 65C-9SeriesPCL");

            // Adding Generic 36C-9SeriesPCL printers to the list
            drivers.Add("Ineo+ 258", "Generic 36C-9SeriesPCL");
            drivers.Add("Ineo+ 308", "Generic 36C-9SeriesPCL");
            drivers.Add("Ineo+ 368", "Generic 36C-9SeriesPCL");

            // Adding Generic 28C-8SeriesPCL printers to the list
            drivers.Add("Ineo+ 227", "Generic 28C-8SeriesPCL");
            drivers.Add("Ineo+ 287", "Generic 28C-8SeriesPCL");

            // Adding Generic C MF385-2SeriesPCL printers to the list
            drivers.Add("Ineo+ 3851", "Generic C MF385-2SeriesPCL");

            // Adding Generic 70C-10SeriesPCL printers to the list
            drivers.Add("Ineo+ 659", "Generic 70C-10SeriesPCL");
            drivers.Add("Ineo+ 759", "Generic 70C-10SeriesPCL");

            // Adding C MF311-1 PCL6 printers to the list
            drivers.Add("Ineo+ 3110", "C MF311-1 PCL6");

            // Adding C MF385-1 Series PCL6 printers to the list
            drivers.Add("Ineo+ 3350", "C MF385-1 Series PCL6");
            drivers.Add("Ineo+ 3850", "C MF385-1 Series PCL6");

            // Adding Generic 95BW-9SeriesPCL printers to the list
            drivers.Add("Ineo 758", "Generic 95BW-9SeriesPCL");

            // Adding Generic 55BW-9SeriesPCL printers to the list
            drivers.Add("Ineo 458", "Generic 55BW-9SeriesPCL");
            drivers.Add("Ineo 558", "Generic 55BW-9SeriesPCL");

            // Adding Generic 36BW-9SeriesPCL printers to the list
            drivers.Add("Ineo 308", "Generic 36BW-9SeriesPCL");
            drivers.Add("Ineo 368", "Generic 36BW-9SeriesPCL");

            // Adding Generic 36BW-8SeriesPCL printers to the list
            drivers.Add("Ineo 227", "Generic 36BW-8SeriesPCL");
            drivers.Add("Ineo 287", "Generic 36BW-8SeriesPCL");
            drivers.Add("Ineo 367", "Generic 36BW-8SeriesPCL");

            // Adding Generic BW MF475-3SeriesPCL printers to the list
            drivers.Add("Ineo 4052", "Generic BW MF475-3SeriesPCL");
            drivers.Add("Ineo 4752", "Generic BW MF475-3SeriesPCL");

            // Adding BW MF442-3_362-3 PCL6 printers to the list
            drivers.Add("Ineo 4422", "BW MF442-3_362-3 PCL6");

            // Adding BW MF475-1 Series PCL6 printers to the list
            drivers.Add("Ineo 4050", "BW MF475-1 Series PCL6");
            drivers.Add("Ineo 4750", "BW MF475-1 Series PCL6");

            // Adding Generic 30BW-6iSeriesPCL printers to the list
            drivers.Add("Ineo 266i", "Generic 30BW-6iSeriesPCL");
            drivers.Add("Ineo 306i", "Generic 30BW-6iSeriesPCL");
        }

        /*public static void EnableSMB1()
        {
            try
            {
                PowerShell shell = PowerShell.Create();
                shell.AddCommand("Enable-WindowsOptionalFeature")
                    .AddParameter("Online")
                    .AddParameter("FeatureName", "SMB1Protocol").Invoke();
            }
            catch (Exception)
            {
                MessageBox.Show("Un redémarrage est nécessaire afin de terminer l'activation SMB 1.0");
            }
        }*/
    }
}
