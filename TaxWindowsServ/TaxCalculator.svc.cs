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
             var queryResult = ExecuteGetRequest(usZipCode);

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
            var queryResult = ExecutePostRequest(order);
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


        private IRestResponse<List<RateResponse>> ExecuteGetRequest(string usZipCode)
        {
            RateResponseAttributes rra = new RateResponseAttributes();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var timeout = 100000; // Milliseconds

            var rstClient = new RestClient(apiUrl);
            var request = new RestRequest("rates/" + usZipCode, Method.GET);
            request.AddHeader("Authorization", "Bearer " + apiToken);
            request.Timeout = timeout;
            var queryResult = rstClient.Execute<List<RateResponse>>(request);
            return queryResult;


        }


        private IRestResponse<List<TaxResponse>> ExecutePostRequest(Order ord){

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var timeout = 100000; // Milliseconds

            var rstClient = new RestClient(apiUrl);
            var request = new RestRequest("taxes", Method.POST);
            request.AddHeader("Authorization", "Bearer " + apiToken);

            request.AddParameter("application/json", JsonConvert.SerializeObject(ord), ParameterType.RequestBody);

            request.Timeout = timeout;
            request.RequestFormat = DataFormat.Json;

            var queryResult = rstClient.Execute<List<TaxResponse>>(request);

            return queryResult;
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
