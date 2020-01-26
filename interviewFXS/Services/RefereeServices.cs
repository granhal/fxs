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
    public class RefereeServices : IRefereeServices
    {
        readonly IConfiguration _configuration;
        readonly IMongoDatabase _mongoDB;
        readonly IMongoCollection<RefereeRequestModel> _mongoCollection;

        public RefereeServices(IConfiguration configuration, MongoClient _mongoClient)
        {
            _configuration = configuration;
            _mongoDB = _mongoClient.GetDatabase(ConnectionUtils.MongoDatabase(_configuration));
            _mongoCollection = _mongoDB.GetCollection<RefereeRequestModel>(ConnectionUtils.MongoCollection(configuration, "MONGODB_COLLECTION_REFEREES"));
           
        }

        public async Task<object> GetAll()
        {
            try
            {
                var pipeLine = new List<BsonDocument> { };
                pipeLine.Add(new BsonDocument { { "$match", new BsonDocument { { "_id", new BsonDocument { { "$exists", true } } } } } });
                pipeLine.Add(
                    new BsonDocument{ { "$addFields", new BsonDocument {
                        { "id", new BsonDocument { { "$toInt", "$uid" } } }
                        }}
                    });
                pipeLine.Add(
                        new BsonDocument{ { "$project", new BsonDocument {
                        { "_id", 0 }, { "uid", 0 }, { "created", 0 }, { "modified", 0 }
                        }}
                    });
                return _mongoCollection.Aggregate<object>(pipeLine).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> AddOne(RefereeRequestModel refereeRequestModel)
        {
            try
            {
                refereeRequestModel._id = ObjectId.GenerateNewId();
                refereeRequestModel.Created = DateTime.UtcNow;
                refereeRequestModel.Modified = DateTime.UtcNow;
                refereeRequestModel.Uid = GenerateIdUtils.GenerateId();
                await _mongoCollection.InsertOneAsync(refereeRequestModel);
                return refereeRequestModel.Uid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> GetOne(int id)
        {
            try
            {
                var result = await _mongoCollection.FindAsync(Builders<RefereeRequestModel>.Filter.Eq("uid", id.ToString()));
                var response = result.FirstOrDefault();
                return new RefereeResponseModel
                {
                    Id = id,
                    Name = response.Name,
                    MinutesPlayed = response.MinutesPlayed
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> UpdateOne(int id, RefereeRequestModel RefereeRequestModel)
        {
            try
            {
                var update = Builders<RefereeRequestModel>.Update.Set("created", DateTime.UtcNow);
                foreach (BsonElement item in RefereeRequestModel.ToBsonDocument())
                {
                    if (item.Name != "modified" && item.Name != "created" && item.Name != "_id" && item.Value.GetType() != typeof(BsonNull))
                    {
                        update = update.Set(item.Name, item.Value);
                    }
                }
                update = update.Set("modified", DateTime.UtcNow);
                await _mongoCollection.UpdateOneAsync(Builders<RefereeRequestModel>.Filter.Eq("uid", id.ToString()), update);
                return string.Format("{0} update success", id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteOne(int id)
        {
            try
            { 
                await _mongoCollection.DeleteOneAsync(Builders<RefereeRequestModel>.Filter.Eq("uid", id.ToString()));
                return string.Format("{0} delete success", id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
