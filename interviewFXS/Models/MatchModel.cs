using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace interviewFXS.Models
{
    public class HouseTeamPlayer
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

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
    }

    public class AwayTeamPlayer
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

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
    }

    public class HouseTeamManager
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("teamName")]
        public string TeamName { get; set; }

        [BsonElement("yellowCards")]
        public int YellowCards { get; set; }

        [BsonElement("redCards")]
        public int RedCards { get; set; }
    }

    public class AwayTeamManager
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("teamName")]
        public string TeamName { get; set; }

        [BsonElement("yellowCards")]
        public int YellowCards { get; set; }

        [BsonElement("redCards")]
        public int RedCards { get; set; }
    }

    public class Referee
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("minutesPlayed")]
        public int MinutesPlayed { get; set; }
    }

    public class MatchResponseModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<HouseTeamPlayer> HouseTeamPlayers { get; set; }
        public IList<AwayTeamPlayer> AwayTeamPlayers { get; set; }
        public HouseTeamManager HouseTeamManager { get; set; }
        public AwayTeamManager AwayTeamManager { get; set; }
        public Referee Referee { get; set; }
        public DateTime Date { get; set; }
    }

    public class MatchRequestModel
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

        [BsonElement("houseTeam")]
        public List<int> HouseTeam { get; set; }

        [BsonElement("houseManager")]
        public int HouseManager { get; set; }

        [BsonElement("awayTeam")]
        public List<int> AwayTeam { get; set; }

        [BsonElement("awayManger")]
        public int AwayManger { get; set; }

        [BsonElement("referee")]
        public int Referee { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [JsonIgnore]
        [BsonElement("modified")]
        public DateTime Modified { get; set; }

        [JsonIgnore]
        [BsonElement("created")]
        public DateTime Created { get; set; }
    }
    public class MatchRequestMongoModel
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

        [BsonElement("houseTeam")]
        public IList<string> HouseTeam { get; set; }

        [BsonElement("houseManager")]
        public string HouseManager { get; set; }

        [BsonElement("awayTeam")]
        public IList<string> AwayTeam { get; set; }

        [BsonElement("awayManger")]
        public string AwayManger { get; set; }

        [BsonElement("referee")]
        public string Referee { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [JsonIgnore]
        [BsonElement("modified")]
        public DateTime Modified { get; set; }

        [JsonIgnore]
        [BsonElement("created")]
        public DateTime Created { get; set; }
    }
}
