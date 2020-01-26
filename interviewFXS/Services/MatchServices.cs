using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using interviewFXS.Models;
using interviewFXS.Utils;
using interviewFXS.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace interviewFXS.Services
{
    public class MatchServices : IMatchServices
    {
        readonly IConfiguration _configuration;
        readonly IMongoDatabase _mongoDB;
        readonly IMongoCollection<MatchRequestMongoModel> _mongoCollection;

        public MatchServices(IConfiguration configuration, MongoClient _mongoClient)
        {
            _configuration = configuration;
            _mongoDB = _mongoClient.GetDatabase(ConnectionUtils.MongoDatabase(_configuration));
            _mongoCollection = _mongoDB.GetCollection<MatchRequestMongoModel>(ConnectionUtils.MongoCollection(configuration, "MONGODB_COLLECTION_MATCHES"));
           
        }

        public async Task<object> GetAll()
        {
            try
            {
                var pipeLine = new List<BsonDocument> { };
                pipeLine.Add(new BsonDocument { { "$match", new BsonDocument { { "_id", new BsonDocument { { "$exists", true } } } } } });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "players" },
                        { "let", new BsonDocument{ { "houseTeam", "$houseTeam" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$in",
                                                new BsonArray {  "$uid", "$$houseTeam"  }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "houseTeam" }
                    }}
                });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "players" },
                        { "let", new BsonDocument{ { "awayTeam", "$awayTeam" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$in",
                                                new BsonArray {  "$uid", "$$awayTeam"  }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "awayTeam" }
                    }}
                });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "managers" },
                        { "let", new BsonDocument{ { "houseManager", "$houseManager" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$eq",
                                                new BsonArray {  "$uid", "$$houseManager" }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "houseManager" }
                    }}
                });
                pipeLine.Add(new BsonDocument { { "$unwind", "$houseManager" } });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "managers" },
                        { "let", new BsonDocument{ { "awayManger", "$awayManger" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$eq",
                                                new BsonArray {  "$uid", "$$awayManger" }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "awayManger" }
                    }}
                });
                pipeLine.Add(new BsonDocument { { "$unwind", "$awayManger" } });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "referees" },
                        { "let", new BsonDocument{ { "referee", "$referee" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$eq",
                                                new BsonArray {  "$uid", "$$referee" }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "referee" }
                    }}
                });
                pipeLine.Add(new BsonDocument { { "$unwind", "$referee" } });
                pipeLine.Add(new BsonDocument{ { "$addFields", new BsonDocument {
                        { "id", new BsonDocument { { "$toInt", "$uid" } } }
                        }}
                    });
                pipeLine.Add(new BsonDocument{ { "$project", new BsonDocument {
                        { "_id", 0 }, { "created", 0 }, { "modified", 0 },{ "uid",0 }
                        }}
                    });
                return _mongoCollection.Aggregate<object>(pipeLine).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> AddOne(MatchRequestModel matchRequestModel)
        {
            try
            {
                MatchRequestMongoModel matchRequestMongoModel = new MatchRequestMongoModel
                {
                    _id = ObjectId.GenerateNewId(),
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    Uid = GenerateIdUtils.GenerateId(),
                    Name = matchRequestModel.Name,
                    Referee = matchRequestModel.Referee.ToString(),
                    AwayManger = matchRequestModel.AwayManger.ToString(),
                    Date = matchRequestModel.Date,
                    HouseManager = matchRequestModel.HouseManager.ToString(),
                    HouseTeam = matchRequestModel.HouseTeam.ConvertAll<string>(delegate (int i)
                    {
                        return i.ToString();
                    }),
                    AwayTeam = matchRequestModel.AwayTeam.ConvertAll<string>(delegate (int i)
                    {
                        return i.ToString();
                    }),
                };
                await _mongoCollection.InsertOneAsync(matchRequestMongoModel);
                return matchRequestMongoModel.Uid;
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

                var pipeLine = new List<BsonDocument> { };
                pipeLine.Add(new BsonDocument { { "$match", new BsonDocument { { "uid", id.ToString() } } } });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "players" },
                        { "let", new BsonDocument{ { "houseTeam", "$houseTeam" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$in",
                                                new BsonArray {  "$uid", "$$houseTeam"  }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "houseTeam" }
                    }}
                });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "players" },
                        { "let", new BsonDocument{ { "awayTeam", "$awayTeam" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$in",
                                                new BsonArray {  "$uid", "$$awayTeam"  }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "awayTeam" }
                    }}
                });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "managers" },
                        { "let", new BsonDocument{ { "houseManager", "$houseManager" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$eq",
                                                new BsonArray {  "$uid", "$$houseManager" }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "houseManager" }
                    }}
                });
                pipeLine.Add(new BsonDocument{ { "$unwind", "$houseManager"}});
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "managers" },
                        { "let", new BsonDocument{ { "awayManger", "$awayManger" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$eq",
                                                new BsonArray {  "$uid", "$$awayManger" }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "awayManger" }
                    }}
                });
                pipeLine.Add(new BsonDocument { { "$unwind", "$awayManger" } });
                pipeLine.Add(new BsonDocument{ { "$lookup", new BsonDocument {
                        { "from", "referees" },
                        { "let", new BsonDocument{ { "referee", "$referee" } } },
                        { "pipeline",
                            new BsonArray {
                                new BsonDocument { { "$match",
                                    new BsonDocument { { "$expr",
                                        new BsonDocument {{ "$eq",
                                                new BsonArray {  "$uid", "$$referee" }
                                        }}
                                    } }
                                } },
                                new BsonDocument {{ "$addFields", new BsonDocument { { "id", new BsonDocument { { "$toInt", "$uid" } } } } }},
                                new BsonDocument {{ "$project", new BsonDocument { { "_id", 0 }, { "uid",0 },{ "created", 0 }, { "modified", 0 } }}}
                            }
                        },
                        { "as", "referee" }
                    }}
                });
                pipeLine.Add(new BsonDocument { { "$unwind", "$referee" } });
                pipeLine.Add(new BsonDocument{ { "$addFields", new BsonDocument {
                        { "id", new BsonDocument { { "$toInt", "$uid" } } }
                        }}
                    });
                pipeLine.Add(new BsonDocument{ { "$project", new BsonDocument {
                        { "_id", 0 }, { "created", 0 }, { "modified", 0 },{ "uid",0 }
                        }}
                    });
                return _mongoCollection.Aggregate<object>(pipeLine).FirstOrDefault();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> UpdateOne(int id, MatchRequestModel MatchRequestModel)
        {
            try
            {
                var update = Builders<MatchRequestMongoModel>.Update.Set("created", DateTime.UtcNow);
                foreach (BsonElement item in MatchRequestModel.ToBsonDocument())
                {
                    if (item.Name != "modified" && item.Name != "created" && item.Name != "_id" && item.Value.GetType() != typeof(BsonNull))
                    {
                        update = update.Set(item.Name, item.Value);
                    }
                    if(item.Name == "referee" || item.Name == "awayManger" || item.Name == "houseManager")
                    {
                        update = update.Set(item.Name, item.Value.ToString());
                    }
                    if (item.Name == "houseTeam" || item.Name == "awayTeam")
                    {
                        List<int> values = BsonSerializer.Deserialize<List<int>>(item.Value.ToJson());
                        update = update.Set(item.Name, values.ConvertAll<string>(delegate (int i)
                        {
                            return i.ToString();
                        }));
                    }
                }
                update = update.Set("modified", DateTime.UtcNow);
                await _mongoCollection.UpdateOneAsync(Builders<MatchRequestMongoModel>.Filter.Eq("uid", id.ToString()), update);
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
                await _mongoCollection.DeleteOneAsync(Builders<MatchRequestMongoModel>.Filter.Eq("uid", id.ToString()));
                return string.Format("{0} delete success", id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
