using Task6.DalContracts;
using Task6.Entities;
using Task6.DBDal;
using System;
using System.Collections.Generic;

namespace Task6.BllContracts
{
    public interface IUserLogic
    {
        int Add(string userName, DateTime birthDate, ImageFile img);

        void AssertBirthdate(DateTime birthDate);

        bool Delete(int Id);

        int FindAge(User User);

        IEnumerable<User> GetAll();

        IEnumerable<User> GetAllWithoutAwards();

        IEnumerable<User> GetOnlyAwardedUsers();
    }
}
