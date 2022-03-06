using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

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
        public static void SetupPrinterAsAdmin(String ipaddr)
        {
            // Paramétrage depuis l'interface web
        }

        /*
         * 
         * --------------------- A FAIRE ---------------------
         * 
         */
        public static void SetupSMB(String agence)
        {

        }
    }
}
