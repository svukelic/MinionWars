using MinionWarsEntitiesLib.EntityManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class UserDataModel
    {
        public Users userModel;

        public UserDataModel(int id)
        {
            this.userModel = UsersManager.GetUserData(id);
        }
    }
}