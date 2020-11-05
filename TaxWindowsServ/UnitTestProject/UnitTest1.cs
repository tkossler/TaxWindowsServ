using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using RestRequest = RestSharp.RestRequest;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Taxjar;

namespace UnitTestProject
{
    [TestClass]
   public class UnitTest1
    {
        private static string apiToken = "5da2f821eee4035db4771edab942a4cc";
        private static string apiUrl = "https://api.taxjar.com/v2/";


        [TestMethod]
        public void TestMethodGetRateFromWFCServiceZipAsync()
        {
             TaxJarServRef.TaxCalculatorClient c = new TaxJarServRef.TaxCalculatorClient();
            
            var t = Task.Run(() => c.GetRateFromZipAsync("44236"));
            t.Wait();

            Assert.IsNotNull(t.Result.City, "City is null");
            Assert.IsNotNull(t.Result.State, "State is null");

        }


        [TestMethod]
        public void TestMethodGetTaxForOrderFromWCFServiceAsync()
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(1500);

            TaxJarServRef.TaxCalculatorClient c = new TaxJarServRef.TaxCalculatorClient();

            TaxJarServRef.Order order = testorder1();

            var t = Task.Run(() => c.GetTaxForOrderAsync(order));
            t.Wait();

            Assert.AreNotEqual(Convert.ToDouble(0), Convert.ToDouble(t.Result.Rate), Convert.ToDouble(0));

            Assert.AreNotEqual(Convert.ToDouble(0), Convert.ToDouble(t.Result.AmountToCollect), Convert.ToDouble(0));

        }



        private TaxJarServRef.Order testorder1()
        {

            TaxJarServRef.LineItem[] ordlineitems = new TaxJarServRef.LineItem[1];
            var lineitem = new TaxJarServRef.LineItem
            {
                Id = "1",
                Quantity = 1,
                ProductTaxCode = "20010",
                UnitPrice = 15,
                Discount = 0
            };
            ordlineitems[0] = lineitem;

            TaxJarServRef.Order order = new TaxJarServRef.Order
            {
                FromCountry = "US",
                FromZip = "92093",
                FromState = "CA",
                FromCity = "La Jolla",
                FromStreet = "9500 Gilman Drive",
                ToCountry = "US",
                ToZip = "90002",
                ToState = "CA",
                ToCity = "Los Angeles",
                ToStreet = "1335 E 103rd St",
                Amount = 15,
                Shipping = Convert.ToDecimal(1.5)
            };

            order.LineItems = ordlineitems;
            return order;

        }


    }
}
