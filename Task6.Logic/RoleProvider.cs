using System;
using System.Web.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Task6.BllContracts;
using Task6.DalContracts;
using Task6.Entities;
using Task6.DBDal;
using System.Text;

namespace Task6.Logic
{
    public class MyRoleProvider : RoleProvider
    {
        private readonly IRoleDao roleDao;

        public MyRoleProvider()
        {
            roleDao = DalProvider.RoleDao;
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            if (roleDao.IsUserInRole(userName, roleName))
            {
                return true;
            }

            else return false;
        }

        public override string[] GetRolesForUser(string userName)
        {
            return roleDao.GetRolesForUser(userName);
        }

        public override string[] GetAllRoles()
        {
            return roleDao.GetAllRoles();
        }

        #region MyRegion
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }



        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
