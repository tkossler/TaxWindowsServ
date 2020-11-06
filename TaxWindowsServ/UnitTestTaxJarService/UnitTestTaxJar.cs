using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Taxjar;
using RestRequest = RestSharp.RestRequest;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTestTaxJarDirectCall
{
    [TestClass]
    public class UnitTestTaxJar
    {
        private static string apiToken = "5da2f821eee4035db4771edab942a4cc";
        private static string apiUrl = "https://api.taxjar.com/v2/";


        [TestMethod]
        public void TestRateRequestCalltoTaxJarService()
        {
            RateResponse rateResponse = new RateResponse();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var timeout = 100000; // Milliseconds
            var err = "";

            var rstClient = new RestClient(apiUrl);
            var request = new RestRequest("rates/44236", Method.GET);
            request.AddHeader("Authorization", "Bearer " + apiToken);
            request.Timeout = timeout;
            var queryResult = rstClient.Execute<List<RateResponse>>(request);

            if (queryResult.StatusCode == HttpStatusCode.OK)
            {

                string s = queryResult.Content;
                if (queryResult.Content != null)
                {
                    rateResponse = JsonConvert.DeserializeObject<RateResponse>(queryResult.Content);
                }
            }
            else
            {
                //log error
                err = queryResult.ErrorMessage;

            }
            var t = rateResponse.Rate;

            Assert.IsNotNull(t.City, "City is null");
            Assert.IsNotNull(t.State, "State is null");
        }


        /// <summary>
        /// Returns a TaxRe
        /// </summary>
        [TestMethod]
        public void TestOrderRequestCalltoTaxJarService()
        {
            var order = testorder1();
            TaxResponse taxResponse = new TaxResponse();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var timeout = 100000; // Milliseconds

            var rstClient = new RestClient(apiUrl);
            var request = new RestRequest("taxes", Method.POST);
            request.AddHeader("Authorization", "Bearer " + apiToken);

            request.AddParameter("application/json", JsonConvert.SerializeObject(order), ParameterType.RequestBody);

            request.Timeout = timeout;
            request.RequestFormat = DataFormat.Json;


            var queryResult = rstClient.Execute<List<TaxResponse>>(request);
            string err = "";
            if (queryResult.StatusCode == HttpStatusCode.OK)
            {
                if (queryResult.Content != null)
                {
                    taxResponse = JsonConvert.DeserializeObject<TaxResponse>(queryResult.Content);
                }
            }
            else
            {
                //log error
                err = queryResult.ErrorMessage;
            }

            var t = taxResponse.Tax;
            Assert.AreNotEqual(Convert.ToDouble(0), Convert.ToDouble(t.Rate), Convert.ToDouble(0));

            Assert.AreNotEqual(Convert.ToDouble(0), Convert.ToDouble(t.AmountToCollect), Convert.ToDouble(0));


        }


        public Order testorder1()
        {
            Order order = new Order();
            order.FromCountry = "US";
            order.FromZip = "92093";
            order.FromState = "CA";
            order.FromCity = "La Jolla";
            order.FromStreet = "9500 Gilman Drive";
            order.ToCountry = "US";
            order.ToZip = "90002";
            order.ToState = "CA";
            order.ToCity = "Los Angeles";
            order.ToStreet = "1335 E 103rd St";
            order.Amount = 15;
            order.Shipping = Convert.ToDecimal(1.5);

            List<LineItem> ordlineitems = new List<LineItem>();
            var lineitem = new LineItem();
            lineitem.Id = "1";
            lineitem.Quantity = 1;
            lineitem.ProductTaxCode = "20010";
            lineitem.UnitPrice = 15;
            lineitem.Discount = 0;
            ordlineitems.Add(lineitem);
            order.LineItems = ordlineitems;
            return order;
        }


    }
}
