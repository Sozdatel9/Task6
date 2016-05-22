using Task6.DalContracts;
using Task6.Entities;
using Task6.DBDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task6.BllContracts
{
    public interface IAccountLogic
    {
        int Add(string userName, string password, string permissions);

        bool Delete(int Id);

        bool Update(string Id, string userName, string password);

        bool CanLogin(string userName, string password);

        bool isUserAlreadyExist(string userName);

        bool GivePermissions(int Id);

        bool DeprivePermissions(int Id);

        string getIdByUserName(string userName);

        IEnumerable<Account> GetAll();
    }
}