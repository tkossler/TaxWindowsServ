using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Taxjar;
using RestRequest = RestSharp.RestRequest;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaxService
{
    public class TaxCalculator : ITaxCalculator
    {
        //private static string DefaultApiUrl = "https://api.taxjar.com";
        //private static string SandboxApiUrl = "https://api.sandbox.taxjar.com";
        //private static string ApiVersion = "v2";
        private static string apiToken = "5da2f821eee4035db4771edab942a4cc";
        private static string apiUrl = "https://api.taxjar.com/v2/";


        /// <summary>
        /// Returns all the Tax rate Response attributes for a given Zip code 
        /// </summary>
        /// <param name="usZipCode">string for us zip code</param>
        /// <returns>Tax Jar RateResponseAttributes</returns>
        public RateResponseAttributes GetRateFromZip(string usZipCode)
        {
            RateResponse rateResponse = new RateResponse();
            TaxJarRequest req = new TaxJarRequest(apiUrl, apiToken);

             var queryResult = req.ExecuteRateRequest(usZipCode);

            string err = "";
            if (queryResult.StatusCode == HttpStatusCode.OK)
            {

                string s = queryResult.Content;
                if (queryResult.Content != null)
                {
                    rateResponse = JsonConvert.DeserializeObject<RateResponse>(queryResult.Content);
                }
            }
            else {
                //log error
                err = queryResult.ErrorMessage;

            }

            return rateResponse.Rate;

        }

        /// <summary>
        /// Gets all the tax respose attributes for a given tax jar order
        /// </summary>
        /// <param name="order">A tax jar order object</param>
        /// <returns></returns>
        public TaxResponseAttributes GetTaxForOrder(Order order)
        {
          // order = testorder1();

            TaxResponse taxResponse = new TaxResponse();
            TaxJarRequest req = new TaxJarRequest(apiUrl, apiToken);
            var queryResult = req.ExecuteOrderRequest(order);
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

            return taxResponse.Tax;
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



        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}
    }
}
