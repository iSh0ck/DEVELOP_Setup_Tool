using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.DirectoryServices;

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
         * ---------------------- A TEST ----------------------
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
         * --------------------- A TERMINER ---------------------
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
                // Création du dossier
                Directory.CreateDirectory(@"C:\Scans");

                // Création de l'utilisateur
                try
                {
                    DirectoryEntry ad = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                    DirectoryEntry newUser = ad.Children.Add("Scan", "user");
                    newUser.Invoke("SetPassword", new object[] { "Sc@nner" + agence });
                    newUser.Invoke("Put", new object[] { "Description", "Scan user for Vela printers" });
                    newUser.CommitChanges();

                    MessageBox.Show("Account Created Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 
            else
            {
                MessageBox.Show("SMB no selected");
            }
        }
    }
}
