using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
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
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");

            // Création du navigateur
            ChromeDriver chrome = new ChromeDriver(chromeOptions);

            chrome.Navigate().GoToUrl("https://www.google.com/");
            MessageBox.Show(chrome.Title);
            chrome.Quit();
        }
    }
}
