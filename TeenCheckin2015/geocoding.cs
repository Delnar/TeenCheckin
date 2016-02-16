using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TeenCheckin
{
    public class geocoding
    {
        public void Process()
        {
            List<string> lst = Database.GetAddressHTTP();

            Database.ClearGeoCode();

            lst.ForEach(s =>
            {

            });

            HttpWebRequest Request = WebRequest.Create("") as HttpWebRequest;
            Request.Method = "GET"; //Or PUT, DELETE, POST
            Request.ContentType = "application/x-www-form-urlencoded";
            using (HttpWebResponse Response = Request.GetResponse() as HttpWebResponse)
            {
                if (Response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("The request did not complete successfully and returned status code " + Response.StatusCode);
                using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                {
                    string ReturnedData = Reader.ReadToEnd();
                }
            }
        }
    }
}
