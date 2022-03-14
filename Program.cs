using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.DirectoryServices;
using System.Management;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;
using IWshRuntimeLibrary;
using System.Net;
using System.Runtime.InteropServices;

namespace Vela31_Ineo
{
    static class Program
    {
        [DllImport("printui.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern void PrintUIEntryW(IntPtr hwnd, IntPtr hinst, string lpszCmdLine, int nCmdShow);
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
            String server = "51.158.153.181";

            // Création du dossier téléchargement
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Download"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Download");
            }
            String url = "http://" + server + "/Drivers Develop/" + os + "/" + model_name + "/PCL6/driver.zip";

            MessageBox.Show(url);

            using (WebClient client = new WebClient())
            {
                // Mettre à jour la progressbar
                // wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                client.DownloadFileAsync(
                    new System.Uri("http://" + server + "/Drivers Develop/" + os + "/" + model_name + "/PCL6/driver.zip"),
                    Directory.GetCurrentDirectory() + @"\Download\driver.zip");
            }
        }

        /* // Event pour suivis du télécharment
         * 
         * // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
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
         * --------------------- A TEST ---------------------
         *         VERIFIER AVANT AVEC DES MESSAGES BOX
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

            // Installation du driver
            cmd.StandardInput.WriteLine(@"cd C:\Windows\System32\Printing_Admin_Scripts\fr-FR");
            cmd.StandardInput.Flush();
            String pathToInfFile = @"C:\WINDOWS\inf";

            // Trouver le nom du fichier complet .inf
            String [] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Download");
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
                                        '"' + model_name + '"' + " -h " +
                                        '"' + pathToInfFile + '"' + "-i " +
                                        '"' + downloadedInfFile + '"');
            cmd.StandardInput.Flush();

            // Création du port TCP/IP
            cmd.StandardInput.WriteLine("cscript prnport.vbs -a -r IP_" + ipaddr + " -h " + ipaddr + " -o raw");
            cmd.StandardInput.Flush();

            // Installation de l'imprimante
            cmd.StandardInput.WriteLine("cscript prnmngr.vbs -a -p " + 
                                        '"' + "Copieur " + model_name + '"' + " -m " + 
                                        '"' + model_name + '"' + " -r " + ipaddr);
            cmd.StandardInput.Flush();

            // Fermeture du cmd
            cmd.StandardInput.Close();
            cmd.WaitForExit();
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

                    cmd.StandardInput.WriteLine(@"net share Scans=C:\Scans");
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
    }
}