using System;
using System.Threading.Tasks;
using interviewFXS.Models;

namespace interviewFXS.Interfaces
{
    public interface IRefereeServices
    {
        Task<object> GetAll();
        Task<string> AddOne(RefereeRequestModel refereeRequestModel);
        Task<object> GetOne(int id);
        Task<object> UpdateOne(int id, RefereeRequestModel RefereeRequestModel);
        Task<string> DeleteOne(int id);

    }
}
