using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading;
using System.Net.Cache;
using System.Net;

namespace Client
{
    [TestClass]
    public class TimeServerTest
    {
        string _serverBaseAddress = "http://localhost/Server/";

        [TestInitialize]
        public void Init()
        {
            var uriBuilder = new UriBuilder(_serverBaseAddress);
            if (uriBuilder.Host == "localhost")
                uriBuilder.Host = Dns.GetHostName();

            _serverBaseAddress = uriBuilder.ToString();
        }

        void ReadDateTimeTestRun(HttpClient client, Uri uri)
        {
            for (int i = 0; i < 10; i++)
            {
                var result = client.GetStringAsync(uri).Result;
                var dateTime = DateTime.Parse(result);
                Console.WriteLine(dateTime.ToString("hh:mm:ss"));

                Thread.Sleep(1000);
            }
        }

        [TestMethod]
        public void NoCache()
        {
            var uri = new Uri(new Uri(_serverBaseAddress), "time");

            using (var client = new HttpClient(new HttpClientHandler() { Proxy = new WebProxy() }))
            {
                ReadDateTimeTestRun(client, uri);
            }
        }

        [TestMethod]
        public void ServerCache()
        {
            var uri = new Uri(new Uri(_serverBaseAddress), "time.t1");

            using (var client = new HttpClient(new HttpClientHandler()))
            {
                ReadDateTimeTestRun(client, uri);
            }
        }

        [TestMethod]
        public void ClientCache()
        {
            var uri = new Uri(new Uri(_serverBaseAddress), "cachecontrol.txt");

            using (var client = new HttpClient(
                new WebRequestHandler()
                {
                    UseProxy = true,
                    CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default)
                }))
            {
                for (int i = 0; i < 10; i++)
                {                    
                    var result = client.GetStringAsync(uri).Result;
                    Console.WriteLine(result);
                    Thread.Sleep(1000);
                }
            }
        }

    }
}
