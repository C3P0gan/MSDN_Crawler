using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;

namespace MSDN_Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //captura o HTML fonte e armazena em forma de string
            var webClient = new WebClient();
            string msdnUrl = webClient.DownloadString(
                "https://social.msdn.microsoft.com/Forums/pt-BR/home?filter=alltypes&sort=firstpostdesc");
            
            //insere os dados da página no documento
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(msdnUrl);

            string id = string.Empty;
            string titulo = string.Empty;
            string postagem = string.Empty;
            string resumo = string.Empty;
            string exibicao = string.Empty;
            string resposta = string.Empty;
            string link = string.Empty;

            //captura o link de cada postagem
            foreach(HtmlNode node in htmlDocument.GetElementbyId("threadList").ChildNodes)
            {
                if (node.Attributes.Count > 0)
                {
                    id = node.Attributes["data-threadid"].Value;
                    link = "https://social.msdn.microsoft.com/Forums/pt-BR/" + id;
                    titulo = node.Descendants().First(x => x.Id.Equals("threadTitle_" + id)).InnerText;

                    resumo = WebUtility.HtmlDecode(node.Descendants().First(x => x.Attributes["class"] != null
                    && x.Attributes["class"].Value.Equals("threadSummary")).InnerText);
                    
                    //verifica se o atributo "class" é diferente de nulo e se é o nodo que queremos
                    postagem = WebUtility.HtmlDecode(node.Descendants().First(x => x.Attributes["class"] != null 
                    && x.Attributes["class"].Value.Equals("lastpost")).InnerText.Replace("\n", "").Replace("  ", ""));
                    
                    exibicao = WebUtility.HtmlDecode(node.Descendants().First(x => x.Attributes["class"] != null 
                    && x.Attributes["class"].Value.Equals("viewcount")).InnerText);
                    
                    resposta = node.Descendants().First(x => x.Attributes["class"] != null 
                    && x.Attributes["class"].Value.Equals("replycount")).InnerText;

                    if (!string.IsNullOrEmpty(titulo))
                    {
                        Console.WriteLine("------------------------------------------------------------------------------------");
                        Console.WriteLine(titulo);
                        Console.WriteLine();
                        Console.WriteLine(resumo);
                        Console.WriteLine();
                        Console.WriteLine(postagem);
                        Console.Write($"{exibicao} | {resposta} \n");
                        Console.WriteLine();
                        Console.WriteLine(link);
                        Console.WriteLine("------------------------------------------------------------------------------------");
                    }
                }
            }
        }
    }
}
