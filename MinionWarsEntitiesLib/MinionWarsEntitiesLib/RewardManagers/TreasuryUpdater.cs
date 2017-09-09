using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.RewardManagers
{
    public static class TreasuryUpdater
    {
        public static void UpdateUserTreasuries()
        {
            using (var db = new MinionWarsEntities())
            {
                List<Users> userList = db.Users.Where(x => x.online != -1).ToList();
                foreach(Users u in userList)
                {
                    List<UserTreasury> utl = db.UserTreasury.Where(x => x.user_id == u.id).ToList();
                    foreach(UserTreasury ut in utl)
                    {
                        ut.amount += ut.generation;
                        db.UserTreasury.Attach(ut);
                        db.Entry(ut).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
