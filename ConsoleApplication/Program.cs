using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:5000/api/Notification/ReturnDate";
            var mailsend = CallApi(url);
            Console.WriteLine(mailsend);
        }

        private static object CallApi(string url)
        {
             HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
             webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
           
          HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
          Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
          StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
           string result = string.Empty;
          result = responseStream.ReadToEnd();          
           webresponse.Close();
          return result;
        
        }
    }
}
