using Task6.DalContracts;
using Task6.Entities;
using Task6.DBDal;
using System.Collections.Generic;

namespace Task6.BllContracts
{
    public interface IAwardLogic
    {
        int Add(string awardTitle, ImageFile img);

        bool Delete(int Id);

        bool GiveAward(int awardId, int userId);

        bool DepriveAward(int awardId, int userId);

        bool isAwardExist(int awardId);

        bool isAwardAlreadyExist(string Title);

        string GetAwardTitle(int awardId);

        IEnumerable<Award> GetAll();
    }
}
