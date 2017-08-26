using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using ScrapySharp.Html.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using ScrapySharp.Html;
using System.Threading;
using System.Diagnostics;
using ScrapperTest.Metodos;
using System.IO.Compression;

namespace ScrapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var listaCVM = ObterListaCVM();
                var listaTeste = new List<string>() { "16284" };

                Bovespa.ConsultaEmpresas(listaTeste);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        public static List<string> ObterListaCVM()
        {

            //Baixar o Arquivo ZIP com a lista CVM atualizada
            using (var client = new WebClient())
            {
                client.DownloadFile("http://sistemas.cvm.gov.br/cadastro/SPW_CIA_ABERTA.ZIP", "SPW_CIA_ABERTA.zip");
            }

            //Extração do arquivo ZIP
            string zipPath = @"SPW_CIA_ABERTA.zip";
            string extractPath = @"extract";
            if (File.Exists(extractPath + @"\SPW_CIA_ABERTA.txt"))
            {
                File.Delete(extractPath + @"\SPW_CIA_ABERTA.txt");
            }
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            //Leitura de Arquivo TXT
            string path = extractPath + @"\SPW_CIA_ABERTA.txt";
            var lines = File.ReadAllLines(path);
            int i = 0;
            return lines.Select(c => c.Split('\t')[0]).Where(c => int.TryParse(c, out i)).ToList();
        }
    }
}




