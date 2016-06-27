using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task6.Entities;
using Task6.Logic;

namespace Task6.StaticClasses
{
    public class staticClasses
    {
        public static UserLogic userLogic;
        public static AccountLogic accountLogic;
        public static AwardLogic awardLogic;
        public static IEnumerable<Award> Awards;
        public static IEnumerable<User> Users;
        public static IEnumerable<User> Users2;
        public static IEnumerable<Account> Accounts;

        static staticClasses()
        {
            userLogic = new UserLogic();
            accountLogic = new AccountLogic();
            awardLogic = new AwardLogic();
            awardLogic.uLogic = userLogic;
            Awards = awardLogic.GetAll();
            Users = awardLogic.uLogic.GetAllWithoutAwards();
            Users2 = awardLogic.uLogic.GetOnlyAwardedUsers();
            Accounts = accountLogic.GetAll();
        }

    }
}