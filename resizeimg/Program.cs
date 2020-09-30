using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace resizeimg
{
    public class Program
    {

        public static void GravarThumb(FileInfo Arquivo, string path, string folder, int ImgLarguraDestino, int ImgAlturaDestino, string nomeArquivoDestino)
        {
            Bitmap bm;
            Bitmap thumb;
            int altura;
            int largura;

            bm = (Bitmap)Bitmap.FromFile(Arquivo.FullName);

            if (bm.Width > ImgLarguraDestino || bm.Height > ImgAlturaDestino)
            {
                altura = (int)((float)ImgAlturaDestino / bm.Width * bm.Height);
                if (altura > ImgAlturaDestino)
                {
                    largura = (int)((float)ImgLarguraDestino / bm.Height * bm.Width);
                    thumb = new Bitmap(bm, new Size(largura, ImgAlturaDestino));
                }
                else
                {
                    thumb = new Bitmap(bm, new Size(ImgLarguraDestino, altura));
                }
            }
            else
            {
                thumb = new Bitmap(bm);
            }

            string caminho = path + "\\" + folder + "\\";

            thumb.Save(caminho + nomeArquivoDestino, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        static DirectoryInfo dirInfo;

        static void ListAllDirs()
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

        static void ListAllExts()
        {
            for(int i = 0; i < dirInfo.GetDirectories().Length; ++i)
            {
                for(int z = 0; z < dirInfo.GetDirectories()[i].GetFiles().Length; ++z)
                {  
                    Console.Write(string.Format("{0}\t", dirInfo.GetDirectories()[i].GetFiles()[z].Extension));
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Defina primeiro a pasta contendo os arquivos em seguida da porcentagem de redução");
            Console.WriteLine("-------------------------------------------------");

            InitialArea:
            string input = Console.ReadLine();
            string[] inputArgs = input.Split(" ");
            string filePath = inputArgs[0];
            string percent = inputArgs[1];

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
