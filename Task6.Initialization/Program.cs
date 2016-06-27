using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.Entities;
using Task6.Logic;

namespace Task6.Initialization
{
    static class Program
    {
        UserLogic userLogic = new UserLogic();
        AwardLogic awardLogic = new AwardLogic();
        awardLogic.uLogic = userLogic;           
        IEnumerable<Award> Awards = awardLogic.GetAll();
        IEnumerable<User> Users = awardLogic.uLogic.GetAllWithoutAwards();
        IEnumerable<User> Users2 = awardLogic.uLogic.GetOnlyAwardedUsers();
        AccountLogic accountLogic = new AccountLogic();
        IEnumerable<Account> Accounts = accountLogic.GetAll();
    }
}
