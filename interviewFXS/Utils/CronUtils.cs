using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using interviewFXS.Models;
using interviewFXS.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace interviewFXS.Utils
{
    public class CronUtils
    {
        private static StatiticsServices _statiticisServices;
        private static MatchServices _matchServices;
        public CronUtils(IConfiguration configuration, MongoClient mongoClient)
        {
            _statiticisServices = new StatiticsServices(configuration, mongoClient);
            _matchServices = new MatchServices(configuration, mongoClient);
        }
        public void Start()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);

            var timer = new Timer(async (e) =>
            {
               await CheckMatches();
            }, null, startTimeSpan, periodTimeSpan);
        }

        public async Task CheckMatches()
        {
            dynamic matchs = await _matchServices.GetAll();
            foreach(dynamic item in matchs)
            {
                DateTime matchDate = item.date;
                matchDate = matchDate.AddMinutes(-5);
                if (matchDate < DateTime.UtcNow)
                {
                    await CheckCardsAsync();
                }
            };
        }

        public async Task CheckCardsAsync()
        {
            IList<StatisticsCardsResponseModel> redCards = _statiticisServices.GetRedCards();
            IList<StatisticsCardsResponseModel> yellowCards = _statiticisServices.GetYellowCards();
            List<int> ids = new List<int>();

            foreach(var item in redCards)
            {
                if (item.Total >= 1)
                {
                    ids.Add(item.Id);
                }
            }
            foreach (var item in yellowCards)
            {
                if (item.Total >= 5)
                {
                    ids.Add(item.Id);
                }
            }
            if (ids.Any())
            {
                Console.WriteLine("yellow cards or red detected!!");
                await SendHttpClient(ids);
            }
            else
            {
                Console.WriteLine("There are not yellow cards or red cards for now...");
            }
        }
        public async Task SendHttpClient(List<int> ids)
        {
            string apiUrl = "http://interview-api.azurewebsites.net/api/IncorrectAlignment";
            dynamic json = ids;
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            using HttpClient client = new HttpClient();
            var response = await client.SendAsync(request, CancellationToken.None);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Send success request to incorrect alignment");
            }
        }

    }
}
