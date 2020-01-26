using System;
using MongoDB.Bson.Serialization.Attributes;

namespace interviewFXS.Models
{
    public class StatisticsMinutesResponseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("total")]
        public int Total { get; set; }
    }
    public class StatisticsCardsResponseModel
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("teamName")]
        public string TeamName { get; set; }

        [BsonElement("total")]
        public int Total { get; set; }
    }
}
