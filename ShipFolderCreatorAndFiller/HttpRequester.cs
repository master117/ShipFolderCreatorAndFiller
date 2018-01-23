using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ShipFolderCreatorAndFiller
{
    class HttpRequester
    {
        public static JArray GetHttpJSONArray(string adress, string user, string pass)
        {
            //first we get a Stream of the Json, for that we use a webrequest
            String response = GetHttpResponse(adress, user, pass);
            //GetHttpStream may return null if we got no nice answer
            if (response == null)
                return null;

            try
            {
                //turns our raw string into a key value lookup
                var json = JArray.Parse(response);

                return json;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        //This method returns a reponse string of a RESTful webrequest
        private static string GetHttpResponse(string adress, string username, string password)
        {
            try
            {
                var client = new WebClient() { Credentials = new NetworkCredential(username, password) };
                var response = client.DownloadString(adress);

                return response;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            //if we get no response for whatever reason we return null
            return null;
        }
    }
}
