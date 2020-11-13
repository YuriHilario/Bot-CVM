using RestSharp;
using System;
using HtmlAgilityPack;
using Newtonsoft;
using Newtonsoft.Json;

namespace CVM
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = DownloadData();
           

            var htmlAgiPack = new HtmlDocument();
            htmlAgiPack.LoadHtml(data);

            var articles = htmlAgiPack.DocumentNode.SelectNodes("//article");
            foreach (var article in articles)
            {
                var split = article.InnerText.Split(':');
                var resume = split[0].Replace("Data", "").Replace("\r","").Replace("\n", "").Replace("\t", "");                
                var date = "Data: " + split[1].Replace("Tipo","").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                var type = "Tipo: " + split[2].Replace("\r", "").Replace("\n", "").Replace("\t", "");

                JsonCVM jsonCvm = new JsonCVM
                {
                    resumo = resume,
                    data = date,
                    tipo = type
                };
                var json = JsonConvert.SerializeObject(jsonCvm);
                Console.WriteLine(json);
            }
            
        }

        private static string DownloadData()
        {
            var client = new RestClient("http://www.cvm.gov.br/decisoes/index.html?lastNameShow=&lastName=&filtro=todos&dataInicio=&dataFim=&buscadoDecisao=false&categoria=decisao");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Upgrade-Insecure-Requests", "2");
            request.AddHeader("DNT", "1");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36";
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Referer", "http://www.cvm.gov.br/decisoes/index.html?lastNameShow=&lastName=&filtro=todos&dataInicio=&dataFim=&buscadoDecisao=false&categoria=decisao");
            request.AddHeader("Accept-Language", "pt-BR,pt;q=0.9");
            request.AddHeader("Cookie", "_ga=GA1.3.980721162.1605268631; _gid=GA1.3.1432393966.1605268631; JSESSIONID=3B441C332CF4E94C7F821233D6115B2E; _gat_gtag_UA_33619506_3=1; JSESSIONID=99D6DE4FF5C0CFAE4BFCFBE56A230D20");
            request.AddHeader("If-Modified-Since", "Fri, 13 Nov 2020 13:03:23 GMT");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
