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

namespace Vela31_Ineo
{
    static class Program
    {
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
         * --------------------- A FAIRE ---------------------
         * 
         */
        public static void DownloadDriver(String os, String model_name)
        {
            // Téléchargement du driver depuis internet
        }

        /*
         * 
         * ---------------------- TERMINER ----------------------
         * 
         */
        public static void UnzipArchive(String file)
        {
            // Unzip du fichier 
            ZipFile.ExtractToDirectory(file, Directory.GetCurrentDirectory());
        }

        /*
         * 
         * --------------------- A FAIRE ---------------------
         * 
         */
        public static void InstallDriver(String model_name, String ipaddr, String default_mode, String default_print)
        {
            // Installation du driver
        }

        /*
         * 
         * --------------------- A FAIRE ---------------------
         * 
         */
        public static void SetupPrinterInWebPanel(String ipaddr)
        {
            // Paramétrage depuis l'interface web
        }

        /*
         * 
         * --------------------- TERMINER ---------------------
         * 
         */
        public static void SetupSMB(String agence)
        {
            /* 
             * - Paramétrage du SMB:
             *  > Activer le partage sur le dossier avec l'utilisateur créer
             *  > Réccupération du hostname + ajout dans l'interface web
             */

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

        public static void ShortcutTest()
        {
            // Créer un raccourcis vers le bureau
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            WshShell wshShell = new WshShell();
            string settingsLink = Path.Combine(desktopFolder, "Scans.lnk");
            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(settingsLink);
            shortcut.TargetPath = @"C:\Scans";
            shortcut.Description = "Dossier raccourcis Scans";
            shortcut.Save();
        }
    }
}