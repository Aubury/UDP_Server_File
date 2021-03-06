﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Info
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (string entry in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                DisplayFileSystemInfoAttributes(new FileInfo(entry));
            }

            Console.WriteLine();
            DisplayLastFile();

               Console.Read();
        }

     
  
     public static void DisplayFileSystemInfoAttributes(FileSystemInfo fsi)
    {
        //  Assume that this entry is a file.
        string entryType = "File";

        // Determine if entry is really a directory
        if ((fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
        {
            entryType = "Directory";
        }
        //  Show this entry's type, name, and creation date.
        Console.WriteLine("{0} entry \"{1} \"  was created on {2:D}", entryType, fsi.Name, fsi.CreationTime);
    }

    public static void DisplayLastFile()
    {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(path);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            FileInfo latestFile = (from file in fileList 
                                                  let len=  file.LastAccessTime 
                                                  where len != null
                                                  orderby len ascending
                                                  select file).First();

            Console.WriteLine("Last file\" {0} \" entry  was created on ({1:D}), last access ({2:D})",  latestFile.Name, latestFile.CreationTime, latestFile.LastAccessTime);
     }
   }
}
