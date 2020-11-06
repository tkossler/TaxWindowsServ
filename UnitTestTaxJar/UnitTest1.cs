using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestTaxJar
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RateResponseAttributes rra = new RateResponseAttributes();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var timeout = 100000; // Milliseconds

            var rstClient = new RestClient(Url);
            var request = new RestRequest("rates/" + usZipCode, Method.GET);
            request.AddHeader("Authorization", "Bearer " + Token);
            request.Timeout = timeout;
            var queryResult = rstClient.Execute<List<RateResponse>>(request);
            return queryResult;
        }
    }
}
