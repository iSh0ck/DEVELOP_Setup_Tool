using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vela31_Ineo
{
    public class Develop_Library
    {

        // Voir si nécessaire de créer un objet session qui sera créer lors de l'appel de la fonction Login_As_Public ou Login_As_Admin
        // Le constructeur de l'objet Session devrait comprendre un string contenant le token et un objet de type Cookie contenant le hash du token.

        // La fonction doit retourner le token généré lors de la connexion.
        public static async Task Login_As_Public(string ip)
        {
            // URL BASE
            string url = "http://" + ip + "/wcd/ulogin.cgi";

            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("func",            "PSL_LP0_TOP"),
                new KeyValuePair<string, string>("AuthType",        "None"),
                new KeyValuePair<string, string>("TrackType",       ""),
                new KeyValuePair<string, string>("ExtSvType",       "0"),
                new KeyValuePair<string, string>("PswcForm",        ""),
                new KeyValuePair<string, string>("Mode",            "Public"),
                new KeyValuePair<string, string>("publicuser",      ""),
                new KeyValuePair<string, string>("username",        ""),
                new KeyValuePair<string, string>("password",        ""),
                new KeyValuePair<string, string>("AuthorityType",   ""),
                new KeyValuePair<string, string>("R_ADM",           ""),
                new KeyValuePair<string, string>("ExtServ",         "0"),
                new KeyValuePair<string, string>("ViewMode",        ""),
                new KeyValuePair<string, string>("BrowserMode",     ""),
                new KeyValuePair<string, string>("Lang",            ""),
                new KeyValuePair<string, string>("trackname",       ""),
                new KeyValuePair<string, string>("trackpassword",   "")
            });

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };

            using (HttpClient client = new HttpClient(handler)) {
                
                // Envois de la requête POST
                HttpResponseMessage response = await client.PostAsync(url, postData);
                response.EnsureSuccessStatusCode();

                // Gestion des cookies
                var uri = new Uri(url);
                var cookies = cookieContainer.GetCookies(uri);

                using (StreamWriter writer = new StreamWriter("cookies.txt"))
                {
                    foreach (Cookie cookie in cookies)
                    {
                        writer.WriteLine(cookie.Name + "=" + cookie.Value + '\n' +
                                         "Domain:" + cookie.Domain + "; Path: " + cookie.Path + "; Expires: " + cookie.Expires);
                    }
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }

        }

        // La requête doit retourner la liste des destinataires SMB et le nombre de contact enregistrés
        public static async Task Add_Smb_Contact(string ip, string token, string contactName, string host, string path, string user, string pass)
        {
            // URL BASE
            string url = "http://" + ip + "/wcd/api/AppReqSetCustomMessage/_007_000_ABR003";

            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("func",        "PSL_C_A_SMB_PTR"),
                new KeyValuePair<string, string>("h_token",     token),
                new KeyValuePair<string, string>("H_AKI",       "Public"),
                new KeyValuePair<string, string>("H_Kind",      "Smb"),
                new KeyValuePair<string, string>("T_NAM",       contactName),
                new KeyValuePair<string, string>("S_SER",       "Abc"),
                new KeyValuePair<string, string>("T_HOS",       host),
                new KeyValuePair<string, string>("T_DIR",       path),
                new KeyValuePair<string, string>("T_LOG",       user),
                new KeyValuePair<string, string>("P_PAS",       pass),
                new KeyValuePair<string, string>("H_NUM",       "new"),
                new KeyValuePair<string, string>("H_FAV",       ""),
                new KeyValuePair<string, string>("B_Name",      ""),
                new KeyValuePair<string, string>("B_Dest",      ""),
                new KeyValuePair<string, string>("H_Result",    "SendOk"),
                new KeyValuePair<string, string>("T_FURI",      ""),
                new KeyValuePair<string, string>("C_WEL",       "on"),
                new KeyValuePair<string, string>("H_CHK",       "false"),
                new KeyValuePair<string, string>("H_PEX",       "true"),
                new KeyValuePair<string, string>("R_REF",       "Level"),
                new KeyValuePair<string, string>("S_LEV",       "0")
            });

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };

            var cookies = File.ReadAllLines("cookies.txt");

            foreach (var cookie in cookies)
            {
                var cookieParts = cookie.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cookieKeyValue = cookieParts[0].Split(new[] { '=' }, 2);

                if (cookieKeyValue.Length == 2)
                {
                    cookieContainer.Add(new Uri("http://" + ip), new Cookie(cookieKeyValue[0].Trim(), cookieKeyValue[1].Trim()));
                }
            }

            using (HttpClient client = new HttpClient(handler))
            {

                // Envois de la requête POST
                HttpResponseMessage response = await client.PostAsync(url, postData);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }

        }

        public static async Task Get_Smb_Contact(string ip)
        {
            // URL BASE
            string url = "http://" + ip + "wcd/api/AppReqGetAbbr";
            string jsonData = "{\"AbbrListCondition\":{\"WellUse\":\"false\",\"SearchKey\":\"None\",\"ObtainCondition\":{\"Type\":\"IndexList\",\"IndexRange\":{\"Start\":\"1\",\"End\":\"50\"}},\"SortInfo\":{\"Condition\":\"No\",\"Order\":\"Ascending\"},\"AddressKind\":\"Public\",\"SearchSendMode\":\"4\"}}";

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };

            var cookies = File.ReadAllLines("cookies.txt");

            foreach(var cookie in cookies)
            {
                var cookieParts = cookie.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cookieKeyValue = cookieParts[0].Split(new { '=' }, 2);

                if (cookieKeyValue.Length == 2)
                {
                    cookieContainer.Add(new Uri("http://" + ip), new Cookie(cookieKeyValue[0].Trim(), cookieKeyValue[1].Trim()));
                }
            }

            using(HttpClient client = new HttpClient(handler))
            {
                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }
        }

        public static async Task Logout(string ip)
        {
            string url = "http://" + ip + "/wcd/user.cgi";

            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("func",    "PSL_ACO_LGO"),
                new KeyValuePair<string, string>("h_token", "")
            });

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };

            var cookies = File.ReadAllLines("cookies.txt");
            foreach (var cookie in cookies)
            {
                var cookieParts = cookie.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cookieKeyValue = cookieParts[0].Split(new[] { '=' }, 2);

                if (cookieKeyValue.Length == 2)
                {
                    cookieContainer.Add(new Uri("http://" + ip), new Cookie(cookieKeyValue[0].Trim(), cookieKeyValue[1].Trim()));
                }
            }

            using (HttpClient client = new HttpClient(handler))
            {
                HttpResponseMessage response = await client.PostAsync(url, postData);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }
        }

    }
}
