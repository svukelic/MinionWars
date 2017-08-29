using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.EntityManagers
{
    public static class ExperienceManager
    {
        public static void IncreaseExperience(int id, int amount)
        {
            using (var db = new MinionWarsEntities())
            {
                Users user = db.Users.Find(id);
                ModifierCoeficients maxLvl = db.ModifierCoeficients.Find(22);

                if (user.lvl == Convert.ToInt32(maxLvl.value)) return;
                else
                {
                    user.experience += amount;
                    ModifierCoeficients threshold = db.ModifierCoeficients.Find(21);
                    if (user.experience > Convert.ToInt32(threshold.value))
                    {
                        user.experience -= Convert.ToInt32(threshold.value);
                        user.lvl++;
                        if (user.points == null) user.points = 0;
                        else user.points++;
                    }

                    db.Users.Attach(user);
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public static bool IncreaseTrait(int id, int trait)
        {
            using (var db = new MinionWarsEntities())
            {
                Users user = db.Users.Find(id);
                bool success = false;

                switch (trait)
                {
                    case 1:
                        if (user.trait_leadership < 20)
                        {
                            user.trait_leadership++;
                            success = true;
                        }
                        break;
                    case 2:
                        if (user.trait_logistics < 20)
                        {
                            user.trait_logistics++;
                            success = true;
                        }
                        break;
                    case 3:
                        if (user.trait_architecture < 20)
                        {
                            user.trait_architecture++;
                            success = true;
                        }
                        break;
                    case 4:
                        if (user.trait_economics < 20)
                        {
                            user.trait_economics++;
                            success = true;
                        }
                        break;
                }

                if (success) user.points -= 1;

                db.Users.Attach(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return success;
            }
        }
    }
}
