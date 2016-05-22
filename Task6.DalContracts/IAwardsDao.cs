using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.Entities;

namespace Task6.DalContracts
{
    public interface IAwardDao
    {
        bool Create(Award Award, ImageFile img = null);

        bool Delete(int Id);

        bool isAwardExist(int awardId);

        IEnumerable<Award> GetAll();

        string GetTitle(int awardId);

        bool IsAwardAlreadyExist(string title);
    }
}
