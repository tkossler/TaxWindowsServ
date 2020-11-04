using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using TaxJarServRef;

namespace UnitTestProject
{
    [TestClass]
   public class UnitTest1
    {
        private RateResponseAttributes rr = null;

        [TestMethod]
        public void TestMethodGetRateFromZipAsync()
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(1500);

            TaxCalculatorClient c = new TaxCalculatorClient();
            
            var t = Task.Run(() => c.GetRateFromZipAsync("44236"));
            t.Wait();

            Assert.IsNotNull(t.Result.City, "City is null");
            Assert.IsNotNull(t.Result.State, "State is null");
            var s = "";


        }


        [TestMethod]
        public void TestMethodGetTaxForOrderAsync()
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(1500);

            TaxCalculatorClient c = new TaxCalculatorClient();

            Order order = testorder1();

            var t = Task.Run(() => c.GetTaxForOrderAsync(order));
            t.Wait();

            Assert.AreNotEqual(Convert.ToDouble(0), Convert.ToDouble(t.Result.Rate), Convert.ToDouble(0));



        }

        private Order testorder1()
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

            LineItem [] ordlineitems = new LineItem[1];
            var lineitem = new LineItem();
            lineitem.Id = "1";
            lineitem.Quantity = 1;
            lineitem.ProductTaxCode = "20010";
            lineitem.UnitPrice = 15;
            lineitem.Discount = 0;
            ordlineitems[0] = lineitem;
            order.LineItems = ordlineitems;
            return order;

        }
        //public async Task callService()
        //{
        //    taxsvc.TaxCalculatorClient c = new taxsvc.TaxCalculatorClient();

        //     rr = await c.GetTaxRateFromZipAsync("44236");

        // }

    }
}
