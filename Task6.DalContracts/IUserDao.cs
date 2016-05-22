using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.Entities;

namespace Task6.DalContracts
{
    public interface IUserDao
    {
        bool Create(User User, ImageFile img = null);

        bool Delete(int Id);

        bool GiveAward(int awardId, int userId);

        bool DepriveAward(int awardId, int userId);

        IEnumerable<User> GetAll();

        IEnumerable<User> GetAllWithoutAwards();

        IEnumerable<User> GetOnlyAwardedUsers();
    }
}