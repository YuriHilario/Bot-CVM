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
            for (int i = 1; i < 209; i++)
            {

                var client = new RestClient("http://www.cvm.gov.br/system/modules/br.com.squadra.principal/elements/resultadoDecisaoColegiado2.jsp");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("DNT", "1");
                request.AddHeader("X-Requested-With", "XMLHttpRequest");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36";
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                request.AddHeader("Origin", "http://www.cvm.gov.br");
                request.AddHeader("Referer", "http://www.cvm.gov.br/decisoes/index.html?lastNameShow=&lastName=&filtro=todos&dataInicio=&dataFim=&buscadoDecisao=false&categoria=decisao");
                request.AddHeader("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
                request.AddHeader("Cookie", "_ga=GA1.3.1963362549.1605269473; JSESSIONID=2DACC99668D0A19EFE755E23126AA3C1; _gid=GA1.3.2063495811.1605627192; JSESSIONID=188411FCFFB6B3612432662EEFA67D34");
                request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", "searchPage="+ i +"&lastName=&filtro=todos&dataInicio=&dataFim=&buscado=false&itensPagina=50&ordenar=&dataInicioBound=2002%2F01%2F03&dataFimBound=2020%2F10%2F20&tipos=", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var data = response.Content;

                var htmlAgiPack = new HtmlDocument();
                htmlAgiPack.LoadHtml(data);

                var articles = htmlAgiPack.DocumentNode.SelectNodes("//article");
                foreach (var article in articles)
                {
                    var split = article.InnerText.Split(':');
                    var resume = split[0].Replace("Data", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    var date = "Data: " + split[1].Replace("Tipo", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
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
        }        
    }
}
