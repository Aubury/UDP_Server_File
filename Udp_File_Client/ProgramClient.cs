using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using Info_FileS;

namespace Udp_File_Client
{
    class ProgramClient
    {
        private static FileDetails fileDet;
         

        // Поля, связанные с UdpClient
        private static int localPort = 5002;
        private static UdpClient receiveUdpClient = new UdpClient(localPort);
        private static IPEndPoint RemoteIpEndPoint = null;


        private static FileStream fs;
        private static Byte[] receiveBytes = new Byte[0];

     
        [STAThread]
        static void Main(string[] args)
        {

            ////Получаем информацию о файле
            //GetFileDetails();

            ////Получаем файл
            //ReceiveFile();

            DisplayFile.DisplayLastFile();
            foreach (string entry in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
            DisplayFile.DisplayFileSystemInfoAttributes(new FileInfo(entry));
            }


            Console.Read();
        }

        private static void ReceiveFile()
        {
            try
            {
                Console.WriteLine("-----------------*****Ожидайте получение файла*****------------------");

                //Получение файла
                receiveBytes = receiveUdpClient.Receive(ref RemoteIpEndPoint);


                //Преобразуем и отображаем данные
                Console.WriteLine("--------Файл получен.....Сщхраняем.......");

                //Создаем временный файл с полученным расширением
                fs = new FileStream("temp." + fileDet.FILETYPE, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(receiveBytes, 0, receiveBytes.Length);

                Console.WriteLine("--------Файл сохранен.......");

                Console.WriteLine("---------Открытие файла------------");

                //Открываем файл связанной с ним программой
                Process.Start(fs.Name);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                fs.Close();
                receiveUdpClient.Close();
                Console.Read();
            }
        }

        private static void GetFileDetails()
        {
            try
            {
                Console.WriteLine("----------------****Ожидание информации о файле****------------------");

                //Получаем информацию о файл
                receiveBytes = receiveUdpClient.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("--------Информация о файле получена!");

                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
                MemoryStream stream1 = new MemoryStream();

                //Считываем информацию о  файле
                stream1.Write(receiveBytes, 0, receiveBytes.Length);
                stream1.Position = 0;

                //Вызываем метод Deserialize
                fileDet = (FileDetails)fileSerializer.Deserialize(stream1);
                Console.WriteLine("Получен файл типа ." + fileDet.FILETYPE + "имеющий размер " + fileDet.FILESIZE.ToString() + "байт");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
