using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyImporter
{
    class Program
    {
        static string connstr = ConfigurationManager.ConnectionStrings["CurrencyImporterConnectionString"].ConnectionString;

        private static void Main(string[] args)
        {
            using (var rep = new LetterAmazerEntities())
            {
                string serviceselector = ConfigurationManager.AppSettings["ServiceSelector"];
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                // APPSpot service URL
                string appspoturl = "http://rate-exchange.appspot.com/currency?from=";

                //OpenExchange Service URL
                string openexchangeurl =
                    "http://openexchangerates.org/api/latest.json?app_id=fbae42ce9f8a46d89633509b60a45a7f";

                //Variable to store the JSON result
                string json = String.Empty;

                var currencies = rep.DbCurrencies.ToList();

                foreach (var dbCurrency in currencies)
                {

                    json = new WebClient().DownloadString(openexchangeurl);
                    ClassOpenExchangeJsonData jdata = JsonConvert.DeserializeObject<ClassOpenExchangeJsonData>(json);

                    double exchangeratedkk = Math.Round(Convert.ToDouble(jdata.rates["DKK"]), 2);
                    double exchangerateothercurrency =
                        Math.Round(
                            Convert.ToDouble(
                                jdata.rates[dbCurrency.CurrencyCode]), 2);
                    var exchangerate = (Math.Round((exchangeratedkk / exchangerateothercurrency), 2) * 100);


                    rep.DbExchangeRates.Add(new DbExchangeRates()
                    {
                        DateFetched = DateTime.Now,
                        ExchangeRate = (decimal) exchangerate,
                        CurrencyId = dbCurrency.Id
                    });
                    rep.SaveChanges();
                }
            }


        }

    }

}

