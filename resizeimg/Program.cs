using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace resizeimg
{
    public class OutputFile
    {
        public Bitmap File { get; set; }
        public string Filepath { get; set; }
    }
    
    public class Program
    {
        static DirectoryInfo dirInfo;
        static List<OutputFile> outputFiles = new List<OutputFile>();

        static void ListAllDirs()
        {
            if(dirInfo.GetDirectories().Length > 0)
            {
                for (int i = 0; i < dirInfo.GetDirectories().Length; ++i)
                {
                    Console.WriteLine(string.Format("[{0}] {1}", i, dirInfo.GetDirectories()[i].FullName));
                    for (int z = 0; z < dirInfo.GetDirectories()[i].GetFiles().Length; ++z)
                    {
                        Console.WriteLine(string.Format("\t[{0}] {1}", z, dirInfo.GetDirectories()[i].GetFiles()[z].Name));
                    }
                }
            }
            else
            {
                for (int z = 0; z < dirInfo.GetFiles().Length; ++z)
                {
                    Console.WriteLine(string.Format("[{0}] {1}", z, dirInfo.GetFiles()[z].Name));
                }
            }
        }


        static void ListAllExts()
        {
            if(dirInfo.GetDirectories().Length > 0)
            {
                for(int i = 0; i < dirInfo.GetDirectories().Length; ++i)
                {
                    for(int z = 0; z < dirInfo.GetDirectories()[i].GetFiles().Length; ++z)
                    {  
                        Console.Write(string.Format("{0}\t", dirInfo.GetDirectories()[i].GetFiles()[z].Extension));
                    }
                }
            }else
            {
                for (int z = 0; z < dirInfo.GetFiles().Length; ++z)
                {
                    Console.Write(string.Format("{0}\t", dirInfo.GetFiles()[z].Extension));
                }
            }
        }

        static void ManipulateImage(FileInfo fileInfo, float percent)
        {
            if (fileInfo.Extension.Equals(".jpg") || fileInfo.Extension.Equals(".png") || fileInfo.Extension.Equals(".jpeg") || fileInfo.Extension.Equals(".gif"))
            {
                Bitmap loadedImage = (Bitmap)Bitmap.FromFile(fileInfo.FullName);
                if (loadedImage != null)
                {
                    int newWidth = Convert.ToInt32(loadedImage.Width * percent);
                    int newHeight = Convert.ToInt32(loadedImage.Height * percent);

                    Console.WriteLine(string.Format("w:{0}|h:{1}|new width:{2}|new height:{3}", loadedImage.Width, loadedImage.Height, newWidth, newHeight));

                    Bitmap newImage = new Bitmap(loadedImage, new Size(newWidth, newHeight));
                    outputFiles.Add(new OutputFile { File = newImage, Filepath = fileInfo.DirectoryName + "\\" + "resample_" + Guid.NewGuid().ToString() + ".jpg" });
                }
                else
                {
                    Console.WriteLine("Erro ao tentar abrir imagem!");
                }

            }
        }

        static void ResampleImage(float percent)
        {
            if(dirInfo.GetDirectories().Length > 0)
            {
                for(int x = 0; x < dirInfo.GetDirectories().Length; ++x)
                {
                    for (int i = 0; i < dirInfo.GetDirectories()[x].GetFiles().Length; ++i)
                    {
                        FileInfo currentFile = dirInfo.GetDirectories()[x].GetFiles()[i];
                        ManipulateImage(currentFile, percent);
                    }
                }
            }
            else
            {
                for(int i = 0; i < dirInfo.GetFiles().Length; ++i)
                {
                    FileInfo currentFile = dirInfo.GetFiles()[i];
                    ManipulateImage(currentFile, percent);
                }
            }
            foreach (OutputFile of in outputFiles)
                of.File.Save(of.Filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Defina primeiro a pasta contendo os arquivos em seguida da porcentagem de redução");
            Console.WriteLine("-------------------------------------------------");

            InitialArea:
            string input = Console.ReadLine();
            string[] inputArgs = input.Split(" ");
            string filePath = "";
            string percent = "";

            if (inputArgs.Length == 2)
            {
                filePath = inputArgs[0];
                percent = inputArgs[1];
            }
            else
            {
                Console.WriteLine("Especifique os valores, tente novamente!");
                goto InitialArea;
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                if (Directory.Exists(filePath))
                {
                    dirInfo = new DirectoryInfo(filePath);
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Número de arquivos neste diretório: " + dirInfo.GetFiles().Length);
                    Console.WriteLine("Número de subdiretórios: " + dirInfo.GetDirectories().Length);
                    Console.WriteLine("Quer ver as extensões dos arquivos? s/n");
                    if (Console.ReadLine().ToLower().Equals("s"))
                    {
                        ListAllExts();
                    }
                    Console.WriteLine("Quer ver os arquivos? s/n");
                    if (Console.ReadLine().ToLower().Equals("s"))
                    {
                        ListAllDirs();
                    }
                    Console.WriteLine("Iniciar resample? s/n");
                    if (Console.ReadLine().ToLower().Equals("s"))
                    {
                        ResampleImage(float.Parse(percent) / 100);
                    }
                    Console.WriteLine("---------------------------------------------");
                }
                else
                {
                    Console.WriteLine("Esse diretório não existe, tente novamente!");
                    goto InitialArea;
                }
            }
            else
            {
                Console.WriteLine("Caminho em branco, tente novamente!");
                goto InitialArea;
            }

            Console.WriteLine("Aperte qualquer tecla para sair...");
            Console.ReadKey(true);
        }
    }
}
