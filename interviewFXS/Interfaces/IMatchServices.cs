using System;
using System.Threading.Tasks;
using interviewFXS.Models;

namespace interviewFXS.Interfaces
{
    public interface IMatchServices
    {

        Task<object> GetAll();
        Task<string> AddOne(MatchRequestModel matchRequestModel);
        Task<object> GetOne(int id);
        Task<object> UpdateOne(int id, MatchRequestModel MatchRequestModel);
        Task<string> DeleteOne(int id);

    }
}
