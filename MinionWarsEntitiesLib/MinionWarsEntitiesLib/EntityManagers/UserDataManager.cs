using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.EntityManagers
{
    public static class UserDataManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();
        public static UserEntity GetUserData(int id)
        {
            UserEntity ue = new UserEntity();
            ue.user = db.Users.Find(id);
            ue.traits = db.UserTraits.Find(ue.user.traits_id);

            return ue;
        }
    }
}
