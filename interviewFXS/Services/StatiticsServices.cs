using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using interviewFXS.Interfaces;
using interviewFXS.Models;
using interviewFXS.Utils;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace interviewFXS.Services
{
    public class StatiticsServices : IStatisticsServices
    {
        private static PlayerServices _playerServices;
        private static ManagerServices _managerServices;
        private static RefereeServices _refereeServices;

        public StatiticsServices(IConfiguration configuration, MongoClient mongoClient)
        {
            _playerServices = new PlayerServices(configuration, mongoClient);
            _managerServices = new ManagerServices(configuration, mongoClient);
            _refereeServices = new RefereeServices(configuration, mongoClient);
        }

        public IList<StatisticsCardsResponseModel> GetRedCards()
        {
            dynamic playerResults = _playerServices.GetAll().Result;
            dynamic managerResult = _managerServices.GetAll().Result;
            List<StatisticsCardsResponseModel> results = new List<StatisticsCardsResponseModel>();
            foreach(var item in playerResults)
            {
                results.Add(new StatisticsCardsResponseModel
                {
                    Id = item.id,
                    Name = item.name,
                    TeamName = item.teamName,
                    Total = item.redCards

                });
            }
            foreach (var item in managerResult)
            {
                results.Add(new StatisticsCardsResponseModel
                {
                    Id = item.id,
                    Name = item.name,
                    TeamName = item.teamName,
                    Total = item.redCards

                });
            }
            return results;

        }

        public IList<StatisticsCardsResponseModel> GetYellowCards()
        {
            dynamic playerResults = _playerServices.GetAll().Result;
            dynamic managerResult = _managerServices.GetAll().Result;
            List<StatisticsCardsResponseModel> results = new List<StatisticsCardsResponseModel>();
            foreach (var item in playerResults)
            {
                results.Add(new StatisticsCardsResponseModel
                {
                    Id = item.id,
                    Name = item.name,
                    TeamName = item.teamName,
                    Total = item.yellowCards

                });
            }
            foreach (var item in managerResult)
            {
                results.Add(new StatisticsCardsResponseModel
                {
                    Id = item.id,
                    Name = item.name,
                    TeamName = item.teamName,
                    Total = item.yellowCards

                });
            }
            return results;
        }

        public IList<StatisticsMinutesResponseModel> GetMinutes()
        {
            dynamic playerResults = _playerServices.GetAll().Result;
            dynamic refereeResult = _refereeServices.GetAll().Result;
            List<StatisticsMinutesResponseModel> results = new List<StatisticsMinutesResponseModel>();
            foreach (var item in playerResults)
            {
                results.Add(new StatisticsMinutesResponseModel
                {
                    Id = item.id,
                    Name = item.name,
                    Total = item.minutesPlayed
                });
            }
            foreach (var item in refereeResult)
            {
                results.Add(new StatisticsMinutesResponseModel
                {
                    Id = item.id,
                    Name = item.name,
                    Total = item.minutesPlayed
                });
            }
            return results;
        }

    }
}
