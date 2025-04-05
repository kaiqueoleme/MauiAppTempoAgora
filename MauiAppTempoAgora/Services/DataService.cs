using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MauiAppTempoAgora.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "9204518293d38234adab1c3334c9b93e";
            string lang = "pt";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&lang={lang}&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                string statusCod = resp.StatusCode.ToString();

                if (statusCod == "NotFound")
                {
                    throw new Exception(statusCod);
                }

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double?)rascunho["main"]["temp_min"],
                        temp_max = (double?)rascunho["main"]["temp_max"],
                        speed = (double?)rascunho["wind"]["speed"],
                        visibility = (int?)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; //Fecha obj. do Tempo.
                } // Fecha if se os status do servidor foi de sucesso
            } // Fecha laço using



            return t;
        }
    }
}
