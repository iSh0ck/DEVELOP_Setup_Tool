using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Management.Automation;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Management;
using System.Security.Principal;

namespace Vela31_Ineo
{
    public static class PrinterClass
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Printer);
    }
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>

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
         * --------------------- TERMINER ---------------------
         * 
         */
        public static void DownloadDriver(String os, String model_name)
        {
            // Téléchargement du driver depuis internet
            // Adresse du serveur de téléchargement
            String server = "163.172.89.133";

            // Création du dossier téléchargement
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Download"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Download");
            }

            using (WebClient client = new WebClient())
            {
                // Mettre à jour la progressbar
                // client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFile(
                    new System.Uri("http://" + server + "/Drivers Develop/" + os + "/" + model_name + "/PCL6/driver.zip"),
                    Directory.GetCurrentDirectory() + @"\Download\driver.zip");
            }
        }

        /* // Event pour suivis du télécharment
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
        */

        /*
         * 
         * ---------------------- TERMINER ----------------------
         * 
         */

        public static void UnzipArchive(String file)
        {
            // Unzip du fichier 
            ZipFile.ExtractToDirectory(file, Directory.GetCurrentDirectory() + @"\Download");
        }

        /*
         * 
         * --------------------- TERMINER ---------------------
         *         
         */
        public static void InstallDriver(String model_name, String ipaddr, String default_mode, String default_print)
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

            String pathToInfFile = Directory.GetCurrentDirectory() + @"\Download";

            // Trouver le nom du fichier complet .inf
            String[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Download");
            String filename = "";
            foreach (String file in files)
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

            String downloadedInfFile = Directory.GetCurrentDirectory() + @"\Download\" + filename;
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

        /*
         * 
         * --------------------- A FAIRE ---------------------
         * 
         */
        public static void SetupPrinterInWebPanel(String ipaddr)
        {
            // Paramétrage depuis l'interface web (Utiliser Selenium)
        }

        /*
         * 
         * --------------------- A TERMINER ---------------------
         * 
         */
        public static void SetupSMB(String agence)
        {
            // Vérification si on doit setup ou non le SMB
            if (agence == "31" || agence == "09" || agence == "65")
            {
                try
                {
                    // Création de l'utilisateur
                    DirectoryEntry ad = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                    DirectoryEntry newUser = ad.Children.Add("Scan", "user");
                    newUser.Invoke("SetPassword", new object[] { "Sc@nner" + agence });
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SMB: " + ex.Message);
                }
            }
        }
        
        public static void EnableSMB1()
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
    }
}
