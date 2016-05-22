using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Task6.DalContracts;
using Task6.BllContracts;
using Task6.Entities;
using Task6.DBDal;

namespace Task6.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private const int MaxUserNameLength = 99;

        private readonly IAccountDao accountDao;

        public AccountLogic()
        {
            accountDao = DalProvider.AccountDao;
        }

        public int Add(string userName, string password, string permissions)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Account's name should not be null or empty", nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Account's password should not be null or empty", nameof(password));
            }

            if (userName.Length > MaxUserNameLength)
            {
                throw new ArgumentException($"Account's name cannot be longer than {MaxUserNameLength} chars", nameof(userName));
            }

            Account Account = new Account
            {
                Name = userName,
                Password = password,
                Permissions = permissions
            };

            if (accountDao.Create(Account))
            {
                return Account.Id;
            }

            return 0;
            throw new InvalidOperationException("Unknown error on account adding");
        }

        public bool Delete(int Id)
        { 
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0:  {Id} ", nameof(Id));
            }

            if (accountDao.Delete(Id))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on account deleting.");
        }

        public bool Update(string Id, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Account's name should not be null or empty", nameof(userName));
            }
             
            if (userName.Length > MaxUserNameLength)
            {
                throw new ArgumentException($"Account's name cannot be longer than {MaxUserNameLength} chars", nameof(userName));
            }

            Account Account = new Account
            {
                Id = int.Parse(Id),
                Name = userName,
                Password = password,
            };
            if (accountDao.Update(Account))
            {
                return true;
            }

            throw new InvalidOperationException("Unknown error on account update");
        }

        public string getIdByUserName(string userName)
        {
            return accountDao.getIdByUserName(userName);
        }

        public bool CanLogin(string userName, string password)
        {
            if (accountDao.CanLogin(userName, password))
            {
                return true;
            }

            else return false;
        }

        public bool isUserAlreadyExist(string userName)
        {
            if (accountDao.isUserAlreadyExist(userName))
            {
                return true;
            }

            else return false;
        }

        public IEnumerable<Account> GetAll()
        {
            return accountDao.GetAll().ToList();
        }
        
        public bool GivePermissions(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0:  {Id} ", nameof(Id));
            }

            if (accountDao.GivePermissions(Id))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on giving admin permissions to selected account.");
        }

        public bool DeprivePermissions(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0:  {Id} ", nameof(Id));
            }

            if (accountDao.DeprivePermissions(Id))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on depriving admin permissions a selected account.");
        }
    }
}
