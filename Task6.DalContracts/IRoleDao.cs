using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.Entities;

namespace Task6.DalContracts
{
    public interface IRoleDao
    {
        bool IsUserInRole(string userName, string Permissions);

        string[] GetRolesForUser(string userName);

        string[] GetAllRoles();
    }
}
