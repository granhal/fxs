using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace interviewFXS.Models
{
    public class PlayerResponseModel : PlayerRequestModel
    {
        public int Id { get; set; }
    }
    public class PlayerRequestModel
    {
        [JsonIgnore]
        public ObjectId _id
        {
            get;
            internal set;
        }

        [JsonIgnore]
        [BsonElement("uid")]
        [JsonPropertyName("id")]
        public string Uid { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("number")]
        public int Number { get; set; }

        [BsonElement("teamName")]
        public string TeamName { get; set; }

        [BsonElement("yellowCards")]
        public int YellowCards { get; set; }

        [BsonElement("redCards")]
        public int RedCards { get; set; }

        [BsonElement("minutesPlayed")]
        public int MinutesPlayed { get; set; }

        [JsonIgnore]
        [BsonElement("modified")]
        public DateTime Modified { get; set; }

        [JsonIgnore]
        [BsonElement("created")]
        public DateTime Created { get; set; }
    }
}
