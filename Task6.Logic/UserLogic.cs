using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.DalContracts;
using Task6.BllContracts;
using Task6.Entities;
using Task6.DBDal;

namespace Task6.Logic
{
    public class UserLogic : IUserLogic
    {
        private const int MaxUserNameLength = 140;
        private readonly IUserDao userDao;

        public UserLogic()
        {
            userDao = DalProvider.UserDao;
            uDao = userDao;
        }

        public int Add(string userName, DateTime birthDate, ImageFile img = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name should not be null or empty", nameof(userName));
            }

            AssertBirthdate(birthDate);

            if (userName.Length > MaxUserNameLength)
            {
                throw new ArgumentException($"User name cannot be longer than {MaxUserNameLength} chars", nameof(userName));
            }

            User User = new User
            {
                Name = userName,
                BirthDate = birthDate,
            };

            if (userDao.Create(User, img))
            {
                return User.Id;
            }

            throw new InvalidOperationException("Unknown error on user adding");
        }

        public void AssertBirthdate(DateTime birthDate)
        {
            int Age;
            DateTime nowDate = DateTime.Today;
            Age = nowDate.Year - birthDate.Year;
            if (birthDate > nowDate.AddYears(-Age))
            {
                Age--;
            }

            if (Age > 150)
            {
                throw new ArgumentException("The distance between birth date and current date should not be more than 150 years", nameof(birthDate));
            }
            else if ((Age < 5) && (Age >= 0))
            {
                throw new ArgumentException("The distance between birth date and current date should not be less than 5 years", nameof(birthDate));
            }
            else if (Age < 0)
            {
                throw new ArgumentException("Birth date can not be earlier than current date", nameof(birthDate));
            }
        }

        public bool Delete(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0:  {Id} ", nameof(Id));
            }

            if (userDao.Delete(Id))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on user deleting.");
        }

        public int FindAge(User User)
        {
            int Age;
            DateTime nowDate = DateTime.Today;
            Age = nowDate.Year - User.BirthDate.Year;
            if (User.BirthDate > nowDate.AddYears(-Age))
            {
                Age--;
            }

            return Age;
        }

        public IUserDao uDao { get; set; }

        public IEnumerable<User> GetAll()
        {
            return userDao.GetAll().ToList();
        }

        public IEnumerable<User> GetAllWithoutAwards()
        {
            return userDao.GetAllWithoutAwards().ToList();
        }

        public IEnumerable<User> GetOnlyAwardedUsers()
        {
            return userDao.GetOnlyAwardedUsers().ToList();
        }
    }
}
