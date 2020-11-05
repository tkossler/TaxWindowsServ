using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxjar;
using RestRequest = RestSharp.RestRequest;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaxService
{
    public class TaxJarRequest
    {
        string Url = "";
        string Token = "";

        public TaxJarRequest(string url, string token) {
            Url = url;
            Token = token;
        }

        public IRestResponse<List<RateResponse>> ExecuteRateRequest(string usZipCode)
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

         public IRestResponse<List<TaxResponse>> ExecuteOrderRequest(Order ord)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var timeout = 100000; // Milliseconds

            var rstClient = new RestClient(Url);
            var request = new RestRequest("taxes", Method.POST);
            request.AddHeader("Authorization", "Bearer " + Token);

            request.AddParameter("application/json", JsonConvert.SerializeObject(ord), ParameterType.RequestBody);

            request.Timeout = timeout;
            request.RequestFormat = DataFormat.Json;

            var queryResult = rstClient.Execute<List<TaxResponse>>(request);

            return queryResult;
        }
    }
}