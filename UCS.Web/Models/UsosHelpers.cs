using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using UCS.Db;
using UCS.Db.Entities;
using UCS.Web.Models.DTOs;

namespace UCS.Web.Models
{
    public class UsosHelpers
    {
        public static string Request()
        {
            string urlCallbackEncoded = WebUtility.UrlEncode("http://localhost:58661/Account/UsosCallback");
            string urlRequestToken = "https://usosapps.prz.edu.pl/services/oauth/request_token";

            long timestamp = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
            string nonce = Guid.NewGuid().ToString("N");
            string parameters = $"oauth_callback={urlCallbackEncoded}&oauth_consumer_key=EmYdreSZDVA2AZGQTkDY&oauth_nonce={nonce}&oauth_signature_method=HMAC-SHA1&oauth_timestamp={timestamp}&oauth_version=1.0&scopes={WebUtility.UrlEncode("email")}";
            string signatureContent = $"GET&{WebUtility.UrlEncode(urlRequestToken)}&{WebUtility.UrlEncode(parameters)}";
            HMACSHA1 sha1 = new HMACSHA1() { Key = Encoding.ASCII.GetBytes(WebUtility.UrlEncode("fNterDE3qrTHsxGrgz86yvHVUyrFngamMQR5wLTB") + "&") };
            string signature = Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(signatureContent)));
            string requestUrl = $"https://usosapps.prz.edu.pl/services/oauth/request_token?{parameters}&oauth_signature={WebUtility.UrlEncode(signature)}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                string responseData = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, string> data = GetKeyData(responseData);

                string a = data["oauth_token"];
                HttpContext.Current.Session["aaaa"] = data["oauth_token_secret"];

                return "https://usosapps.prz.edu.pl/services/oauth/authorize?oauth_token=" + a;
            }
        }

        public static void Access(string oauth_token, string oauth_verifier)
        {
            string urlRequestToken = "https://usosapps.prz.edu.pl/services/oauth/access_token";

            var sesion = HttpContext.Current.Session["aaaa"];

            long timestamp = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
            string nonce = Guid.NewGuid().ToString("N");
            string parameters = $"oauth_consumer_key=EmYdreSZDVA2AZGQTkDY&oauth_nonce={nonce}&oauth_signature_method=HMAC-SHA1&oauth_timestamp={timestamp}&oauth_token={oauth_token}&oauth_verifier={oauth_verifier}&oauth_version=1.0";
            string signatureContent = $"GET&{WebUtility.UrlEncode(urlRequestToken)}&{WebUtility.UrlEncode(parameters)}";
            HMACSHA1 sha1 = new HMACSHA1() { Key = Encoding.ASCII.GetBytes(WebUtility.UrlEncode("fNterDE3qrTHsxGrgz86yvHVUyrFngamMQR5wLTB") + "&" + WebUtility.HtmlEncode(sesion.ToString())) };
            string signature = Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(signatureContent)));
            string requestUrl = $"https://usosapps.prz.edu.pl/services/oauth/access_token?{parameters}&oauth_signature={WebUtility.UrlEncode(signature)}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                string responseData = response.Content.ReadAsStringAsync().Result;

                Dictionary<string, string> data = GetKeyData(responseData);

                HttpContext.Current.Session["oauth_token"] = data["oauth_token"];
                HttpContext.Current.Session["oauth_token_secret"] = data["oauth_token_secret"];

                UserData();
            }
        }

        private static void UserData()
        {
            UCSContext context = new UCSContext();

            string urlRequestToken = "https://usosapps.prz.edu.pl/services/users/user";

            string oauth_token = HttpContext.Current.Session["oauth_token"].ToString();
            string oauth_token_secret = HttpContext.Current.Session["oauth_token_secret"].ToString();

            long timestamp = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
            string nonce = Guid.NewGuid().ToString("N");
            string parameters = $"fields={WebUtility.UrlEncode("email|first_name|last_name")}&oauth_consumer_key=EmYdreSZDVA2AZGQTkDY&oauth_nonce={nonce}&oauth_signature_method=HMAC-SHA1&oauth_timestamp={timestamp}&oauth_token={oauth_token}&oauth_version=1.0";
            string signatureContent = $"GET&{WebUtility.UrlEncode(urlRequestToken)}&{WebUtility.UrlEncode(parameters)}";
            HMACSHA1 sha1 = new HMACSHA1() { Key = Encoding.ASCII.GetBytes(WebUtility.UrlEncode("fNterDE3qrTHsxGrgz86yvHVUyrFngamMQR5wLTB") + "&" + WebUtility.HtmlEncode(oauth_token_secret)) };
            string signature = Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(signatureContent)));
            string requestUrl = $"https://usosapps.prz.edu.pl/services/users/user?{parameters}&oauth_signature={WebUtility.UrlEncode(signature)}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;

                string responseData = response.Content.ReadAsStringAsync().Result;

                StudentResponseDTO studentData = JsonConvert.DeserializeObject<StudentResponseDTO>(responseData);

                Student studentDb = context.Students.SingleOrDefault(s => s.UserName == studentData.Email);
                if (studentDb == null)
                {
                    Student newStudentDb = new Student()
                    {
                        Guid = Guid.NewGuid().ToString("N"),
                        FirstName = studentData.FirstName,
                        LastName = studentData.LastName,
                        UserName = studentData.Email,
                        IsActive = true,
                        AddedAt = DateTime.Now,
                        LastActivity = DateTime.Now
                    };
                    context.Students.Add(newStudentDb);
                    context.SaveChanges();

                    HttpContext.Current.Session["ucs_student_guid"] = newStudentDb.Guid;
                }
                else
                {
                    studentDb.LastActivity = DateTime.Now;
                    context.SaveChanges();
                    HttpContext.Current.Session["ucs_student_guid"] = studentDb.Guid;
                }
            }
        }

        private static Dictionary<string, string> GetKeyData(string responseData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            List<string> data = responseData.Split('&').ToList();

            foreach (string key in data)
            {
                string[] separatedKey = key.Split('=');

                result.Add(separatedKey[0], separatedKey[1]);
            }

            return result;
        }
    }
}