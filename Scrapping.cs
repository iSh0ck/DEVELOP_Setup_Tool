using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Vela31_Ineo
{
    static class Scrapping
    {
        
        public static void Main(string[] args)
        {
            // Mise à jour du driver Chrome avec la version installée sur le PC
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

            // Paramétrage des options de Chrome
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--no-sandbox");
            //chromeOptions.AddArgument("--headless");
            //chromeOptions.AddArgument("--disable-gpu");

            // Création du navigateur
            ChromeDriver chrome = new ChromeDriver(chromeOptions);

            chrome.Url = "http://192.168.40.241/wcd/system_device.xml";

            //if (chrome.Url == "http://192.168.40.241/wcd/system_device.xml")
            //{
            //chrome.FindElement(By.Id("ScanFunction")).Click(); // Ouvrir le carnet d'adresses
            //chrome.FindElement(By.Id("Regist")).Click(); // Nouvel enregistrement
            //chrome.FindElement(By.Id("R_SEL3")).Click(); // Sélection du choix SMB
            //chrome.FindElement(By.Id("Next")).Click(); // Validation du choix SMB
            //chrome.FindElement(By.Id("T_NAM")).SendKeys("[Nom du destinataire]"); // Saisie du nom
            //chrome.FindElement(By.Id("C_A_SMB_WEL")).Click(); // Ajout du favoris dans le carnet d'adresses
            //chrome.FindElement(By.Id("C_A_SMB_C_HOS")).Click(); // Autoriser un nom d'hôte

            // Penser à récupérer le nom d'hôte du PC
            //chrome.FindElement(By.Id("T_HOS")).SendKeys("[Nom d'hôte]"); // Saisie du nom d'hôte
            //chrome.FindElement(By.Id("C_A_SMB_DIR")).SendKeys("Scans"); // Indication du chemin du dossier
            //chrome.FindElement(By.Id("T_LOG")).SendKeys("scan"); // Indication du nom d'utilisateur
            // Penser à récupérer le mot de passe SMB
            //chrome.FindElement(By.Id("P_PAS")).SendKeys("[Mot de passe]"); // Indication du mot de passe

            // chrome.FindElement(By.Id("C_A_SMB_Apply")).Click(); // Ajout du destinataire
            //}

            chrome.Quit();
        }
    }
}
