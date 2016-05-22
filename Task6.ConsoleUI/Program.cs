using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Task6.Entities;
using Task6.Logic;

namespace Task6.ConsoleUI
{
    class Program
    {
        private static UserLogic userLogic;
        private static AwardLogic awardLogic;

        static void Main(string[] args)
        {
            userLogic = new UserLogic();
            awardLogic = new AwardLogic();
            awardLogic.uLogic = userLogic;
            while (true)
            {
                Console.WriteLine("1 - Add user");
                Console.WriteLine("2 - Delete user");
                Console.WriteLine("3 - Show userlist");
                Console.WriteLine("4 - Add award");
                Console.WriteLine("5 - Delete award");
                Console.WriteLine("6 - View awards list");
                Console.WriteLine("7 - Give award to selected user(s)");
                Console.WriteLine("8 - Deprive award selected user(s)");
                Console.WriteLine("0 - Exit");

                string selectedItem = Console.ReadLine();

                switch (selectedItem)
                {
                    case "1":
                        AddUser();
                        break;

                    case "2":
                        DeleteUser();
                        break;

                    case "3":
                        ShowUsers();
                        break;

                    case "4":
                        AddAward();
                        break;

                    case "5":
                        DeleteAward();
                        break;

                    case "6":
                        ShowAwards();
                        break;

                    case "7":
                        GiveAward();
                        break;

                    case "8":
                        DepriveAward();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Incorrect input");
                        break;
                }
            }
        }

        private static void AddUser()
        {
            Console.Write("Enter the name of user: ");
            string userName = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Enter the date of birth: ");
            string strbirthDate = Console.ReadLine();
            DateTime birthDate = DateTime.Parse(strbirthDate, new CultureInfo("ru-RU", true));
            int Id = userLogic.Add(userName, birthDate);
            Console.WriteLine($"User with Id {Id} was successfully added !");
        }

        private static void DeleteUser()
        {
            Console.Write("Enter the Id of user, which you want to delete: ");
            int Id = int.Parse(Console.ReadLine());
            userLogic.Delete(Id);
            Console.WriteLine($"User with Id {Id} was successfully deleted !");
        }

        private static void ShowUsers(string Parameter = "")
        {
            IEnumerable<User> Users = userLogic.GetAll();
            Console.WriteLine("======= List of users: =======");
            Console.WriteLine();
            if (Parameter == "brief")
            {
                foreach (var User in Users)
                {
                    Console.WriteLine($"{User.Id}. {User.Name}, {userLogic.FindAge(User)} yrs.  Award: {User.Award}");
                }
            }
            else
            {
                foreach (var User in Users)
                {
                    Console.WriteLine($"{User.Id}. {User.Name} {User.BirthDate:dd.MM.yyyy}, {userLogic.FindAge(User)} years, award: {User.Award}");
                }
            }
            Console.WriteLine();
        }

        private static void AddAward()
        {
            Console.Write("Enter the title of award: ");
            string awardTitle = Console.ReadLine();
            Console.WriteLine();
            int awardId = awardLogic.Add(awardTitle);
            Console.WriteLine($"Award with Id {awardId} was successfully added !");
        }

        private static void DeleteAward()
        {
            Console.WriteLine("List of availabile awards: ");
            ShowAwards();
            Console.WriteLine();
            Console.Write("Enter the Id of award which you want to delete: ");
            int idToDelete = int.Parse(Console.ReadLine());
            awardLogic.Delete(idToDelete);
            Console.WriteLine($"Award with Id {idToDelete} was successfully deleted !");
        }

        private static void ShowAwards()
        {
            IEnumerable<Award> Awards = awardLogic.GetAll();
            Console.WriteLine("======= List of available awards: =======");
            Console.WriteLine();
            foreach (var award in Awards)
            {
                Console.WriteLine($"{award.Id}.  {award.Title}");
            }
            Console.WriteLine();
        }

        private static void GiveAward()
        {
            ShowUsers("brief");
            Console.Write("Enter the Id of user, which you want to give award: ");
            int userIdToGive = int.Parse(Console.ReadLine());
            Console.WriteLine();
            ShowAwards();
            Console.Write("Enter the Id of award which you want to give for selected user: ");
            int awardId = int.Parse(Console.ReadLine());
            awardLogic.GiveAward(awardId, userIdToGive);
            Console.WriteLine($"User with ID {userIdToGive} was successfully awarded by {awardLogic.GetAwardTitle(awardId)} !");
        }

        private static void DepriveAward()
        {
            ShowUsers("brief");
            Console.Write("Enter the Id of user, which you want to deprive award: ");
            int userIdToDeprive = int.Parse(Console.ReadLine());
            Console.WriteLine();
            ShowAwards();
            Console.Write("Enter the Id of award which you deprive for selected user: ");
            int awardId = int.Parse(Console.ReadLine());
            awardLogic.DepriveAward(awardId, userIdToDeprive);
            Console.WriteLine($"User with Id {userIdToDeprive} was successfully deprived the {awardLogic.GetAwardTitle(awardId)} award !");
        }
    }
}
