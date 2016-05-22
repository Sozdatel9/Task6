using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.Entities;

namespace Task6.DalContracts
{
    public interface IAccountDao
    {
        bool Create(Account account);

        bool Delete(int Id);

        bool Update(Account account);

        IEnumerable<Account> GetAll();

        bool CanLogin(string userName, string password);

        bool isUserAlreadyExist(string userName);

        string getIdByUserName(string userName);

        bool DeprivePermissions(int id);

        bool GivePermissions(int id);
    }
}
