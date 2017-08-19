using MinionWarsEntitiesLib.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class UserDataModel
    {
        public UserEntity userModel;

        public UserDataModel(int id)
        {
            this.userModel = UserDataManager.GetUserData(id);
        }
    }
}