using System;
using System.Collections.Generic;
using interviewFXS.Models;

namespace interviewFXS.Interfaces
{
    public interface IStatisticsServices
    {
        IList<StatisticsCardsResponseModel> GetRedCards();
        IList<StatisticsCardsResponseModel> GetYellowCards();
        IList<StatisticsMinutesResponseModel> GetMinutes();
    }
}
