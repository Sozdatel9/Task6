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
    public class AwardLogic : IAwardLogic
    {
        private const int MaxUserNameLength = 140;
        private readonly IAwardDao awardDao;

        public AwardLogic()
        {
            awardDao = DalProvider.AwardDao;
        }

        public UserLogic uLogic { get; set; }

        public int Add(string awardName, ImageFile img=null)
        {
            if (string.IsNullOrWhiteSpace(awardName))
            {
                throw new ArgumentException("Award name should not be null or empty", nameof(awardName));
            }

            if (awardName.Length > MaxUserNameLength)
            {
                throw new ArgumentException($"Award name cannot be longer than {MaxUserNameLength} chars", nameof(awardName));
            }

            Award Award = new Award
            {
                Title = awardName,
            };

            if (awardDao.Create(Award, img))
            {
                return Award.Id;
            }

            throw new InvalidOperationException("Unknown error on user adding");
        }

        public bool Delete(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0:  {Id} ", nameof(Id));
            }

            if (awardDao.Delete(Id))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on award deleting.");
        }

        public bool GiveAward(int Id, int userId)
        {
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0: {Id} ", nameof(Id));
            }

            if (userId <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0: {userId} ", nameof(userId));
            }

            if (!awardDao.isAwardExist(Id))
            {
                throw new ArgumentException($"This is incorrect Id. Probably, he was deleted or never existed : {Id} ", nameof(Id));
            }

            if (uLogic.uDao.GiveAward(Id, userId))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on award giving.");
        }

        public bool DepriveAward(int Id, int userId)
        {
            if (Id <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0: {Id} ", nameof(Id));
            }

            if (userId <= 0)
            {
                throw new ArgumentException($"This is incorrect Id, because Id can not be less than 0: {userId} ", nameof(userId));
            }

            if (!awardDao.isAwardExist(Id))
            {
                throw new ArgumentException($"This is incorrect Id. Probably, he was deleted or never existed : {Id} ", nameof(Id));
            }

            if (uLogic.uDao.DepriveAward(Id, userId))
            {
                return true;
            }

            throw new InvalidOperationException($"Unknown error on award depriving.");
        }

        public string GetAwardTitle(int Id)
        {
            return awardDao.GetTitle(Id);
        }

        public bool isAwardExist(int Id)
        {
            return awardDao.isAwardExist(Id);
        }

        public IEnumerable<Award> GetAll()
        {
            return awardDao.GetAll().ToList();
        }

        public bool isAwardAlreadyExist(string Title)
        {
            return awardDao.IsAwardAlreadyExist(Title);
        }   
    }
}
