using System;
using System.Threading.Tasks;
using interviewFXS.Models;

namespace interviewFXS.Interfaces
{
    public interface IManagerServices
    {
        Task<object> GetAll();
        Task<string> AddOne(ManagerRequestModel managerRequestModel);
        Task<object> GetOne(int id);
        Task<object> UpdateOne(int id, ManagerRequestModel managerRequestModel);
        Task<string> DeleteOne(int id);
    }
}
