using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;
using Info_FileS;
using System.Diagnostics;

namespace UDP_Server_File
{
 
    class ProgramServer
    {
    
        private static FileDetails fileDet = new FileDetails();

        //Поля, связанные с UdpClient
        private static IPAddress remoteIPAddress;
        private const int remotePort = 5002;
        private static UdpClient sender = new UdpClient();
        private static IPEndPoint endPoint;

        //Filestream object
        private static FileStream fs;

        [STAThread]
        static void Main(string[] args)
        {


            try
            {
                //Получаем удаленный IP-адресс и создаем IPEndPoint
                Console.WriteLine("Введите удаленный IP-адрес");
                remoteIPAddress = IPAddress.Parse(Console.ReadLine().ToString());//"127.0.0.1"
                endPoint = new IPEndPoint(remoteIPAddress, remotePort);

                //Получаем путь файла и его размер(долженибыть меньше 8 kb)
                Console.WriteLine("Введите путь к файлу и его имя");
                fs = new FileStream(Console.ReadLine().ToString(), FileMode.Open, FileAccess.Read);

                if (fs.Length > 8192)
                {
                    Console.Write("Файл должен весить меньше 8 kb");
                    sender.Close();
                    fs.Close();
                    return;
                }

                //Отправляем информацию  о файле
                SendFileInfo();

                //Ждем 2 секунды
                Thread.Sleep(2000);

                //Отправляем  сам файл
                SendFile();

                Console.ReadLine();



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            DisplayFile.DisplayLastFile();
            foreach (string entry in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                DisplayFile.DisplayFileSystemInfoAttributes(new FileInfo(entry));
            }

            

            Console.Read();

          
        }

        private static void SendFile()
        {
            //Создаем файловый поток и переводим его в байты
            Byte[] bytes = new Byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            
            Console.WriteLine("Отправка файла  размером " + fs.Length + "байт");
             try
            {
                //Отправляем файл
                sender.Send(bytes, bytes.Length, endPoint);
              

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                //Закрываем соединение и очищаем поток
                fs.Close();
                sender.Close();
            }
            Console.WriteLine("Файл успешно отправлен.");
            Console.Read();

        }

        public static void SendFileInfo()
        {
            //Получаем тип и расширение файла
            fileDet.FILETYPE = fs.Name.Substring((int)fs.Name.Length - 3, 3);

            //Получаем длину файла 
            fileDet.FILESIZE = fs.Length;

            XmlSerializer  fileSerializer = new XmlSerializer(typeof(FileDetails));
            MemoryStream stream = new MemoryStream();

            // Сериализуем объект
            fileSerializer.Serialize(stream, fileDet);

            // Считываем поток в байты
            stream.Position = 0;
            Byte[] bytes = new Byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));

            Console.WriteLine("Отправка деталей  файла...");

            //Отправляем информацию о файле
            sender.Send(bytes, bytes.Length, endPoint);
            stream.Close();
        }

    }
}
