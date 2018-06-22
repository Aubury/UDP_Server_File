using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace _WebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
             WebClient client = new WebClient();
            client.DownloadFile("http://eloquentjavascript.net/Eloquent_JavaScript.pdf", "Eloquent_JavaScript.pdf");
            Console.WriteLine("Файл загружен");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

               }
           

        }
    }
}
