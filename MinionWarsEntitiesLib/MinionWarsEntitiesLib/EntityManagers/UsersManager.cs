using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinionWarsEntitiesLib.Models;

namespace MinionWarsEntitiesLib.EntityManagers
{
    public static class UsersManager
    {
        static MinionWarsEntities db = new MinionWarsEntities();

        public static void UpdateUserPosition(int user_id, float longitude, float latitude)
        {
            MinionWarsEntitiesLib.Models.Users user = null;
            user = db.Users.Find(user_id);
            
            if(user != null)
            {
                Location checkLoc = null;
                checkLoc = db.Location.Where(x => x.latitude == latitude && x.longitude == longitude).ToList().First();
                if (checkLoc == null)
                {
                    Location newLoc = new Location();
                    newLoc.latitude = latitude;
                    newLoc.longitude = longitude;

                    db.Location.Add(newLoc);
                    db.SaveChanges();

                    UserMovementHistory newMovement = new UserMovementHistory();
                    newMovement.location_id = newLoc.id;
                    newMovement.users_id = user_id;
                    newMovement.occurence = DateTime.Now;
                }
                else
                {
                    UserMovementHistory newMovement = new UserMovementHistory();
                    newMovement.location_id = checkLoc.id;
                    newMovement.users_id = user_id;
                    newMovement.occurence = DateTime.Now;
                }
            }
        }
    }
}
