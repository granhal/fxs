using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using interviewFXS.Models;

namespace interviewFXS.Interfaces
{
    public interface IPlayerServices
    {
        Task<IList<object>> GetAll();
        Task<string> AddOne(PlayerRequestModel playerRequestModel);
        Task<object> GetOne(int id);
        Task<object> UpdateOne(int id, PlayerRequestModel PlayerRequestModel);
        Task<string> DeleteOne(int id);
    }
}
