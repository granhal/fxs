using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace interviewFXS.Models
{
    public class RefereeResponseModel : RefereeRequestModel
    {
        public int Id { get; set; }
    }
    public class RefereeRequestModel
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
