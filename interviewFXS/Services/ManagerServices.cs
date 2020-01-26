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
    public class ManagerServices : IManagerServices
    {
        readonly IConfiguration _configuration;
        readonly IMongoDatabase _mongoDB;
        readonly IMongoCollection<ManagerRequestModel> _mongoCollection;

        public ManagerServices(IConfiguration configuration, MongoClient _mongoClient)
        {
            _configuration = configuration;
            _mongoDB = _mongoClient.GetDatabase(ConnectionUtils.MongoDatabase(_configuration));
            _mongoCollection = _mongoDB.GetCollection<ManagerRequestModel>(ConnectionUtils.MongoCollection(configuration, "MONGODB_COLLECTION_MANAGERS"));
           
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

        public async Task<string> AddOne(ManagerRequestModel managerRequestModel)
        {
            try
            {
                managerRequestModel._id = ObjectId.GenerateNewId();
                managerRequestModel.Created = DateTime.UtcNow;
                managerRequestModel.Modified = DateTime.UtcNow;
                managerRequestModel.Uid = GenerateIdUtils.GenerateId();
                await _mongoCollection.InsertOneAsync(managerRequestModel);
                return managerRequestModel.Uid;
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
                var result = await _mongoCollection.FindAsync(Builders<ManagerRequestModel>.Filter.Eq("uid", id.ToString()));
                var response = result.FirstOrDefault();
                return new ManagerResponseModel
                {
                    Id = id,
                    Name = response.Name,
                    RedCards = response.RedCards,
                    YellowCards = response.YellowCards,
                    TeamName = response.TeamName
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> UpdateOne(int id, ManagerRequestModel managerRequestModel)
        {
            try
            {
                var update = Builders<ManagerRequestModel>.Update.Set("created", DateTime.UtcNow);
                foreach (BsonElement item in managerRequestModel.ToBsonDocument())
                {
                    if (item.Name != "modified" && item.Name != "created" && item.Name != "_id" && item.Value.GetType() != typeof(BsonNull))
                    {
                        update = update.Set(item.Name, item.Value);
                    }
                }
                update = update.Set("modified", DateTime.UtcNow);
                await _mongoCollection.UpdateOneAsync(Builders<ManagerRequestModel>.Filter.Eq("uid", id.ToString()), update);
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
                await _mongoCollection.DeleteOneAsync(Builders<ManagerRequestModel>.Filter.Eq("uid", id.ToString()));
                return string.Format("{0} delete success", id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
