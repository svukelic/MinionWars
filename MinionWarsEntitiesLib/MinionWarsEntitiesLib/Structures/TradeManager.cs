using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Resources;
using MinionWarsEntitiesLib.RewardManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Structures
{
    public static class TradeManager
    {
        public static List<Trading> CheckTradingPost(int camp_id)
        {
            using (var db = new MinionWarsEntities())
            {
                List<Trading> tradingList = null;
                bool exists = false;
                List<UtilityBuilding> ubl = db.UtilityBuilding.Where(x => x.camp_id == camp_id).ToList();
                foreach(UtilityBuilding ub in ubl)
                {
                    if (ub.type == 15) exists = true;
                }

                if (exists)
                {
                    tradingList = new List<Trading>();
                    db.Configuration.LazyLoadingEnabled = false;
                    tradingList = db.Trading.Where(x => x.camp_id == camp_id).ToList();
                    foreach(Trading t in tradingList)
                    {
                        t.Users = db.Users.Find(t.owner_id);
                        t.Minion = db.Minion.Find(t.minion_id);
                        t.Minion.MinionType = db.MinionType.Find(t.Minion.mtype_id);
                    }
                }

                return tradingList;
            }
        }

        public static bool AddToTradingPost(int mo_id, int amount, int camp_id)
        {
            using (var db = new MinionWarsEntities())
            {
                MinionOwnership mo = db.MinionOwnership.Find(mo_id);
                if (mo.available < amount) return false;
                else
                {
                    Trading trade = new Trading();
                    trade.camp_id = camp_id;
                    trade.owner_id = mo.owner_id;
                    trade.amount = amount;
                    trade.minion_id = mo.minion_id;

                    mo.group_count -= amount;
                    mo.available -= amount;

                    db.Trading.Add(trade);
                    db.MinionOwnership.Attach(mo);
                    db.Entry(mo).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
            }

            return true;
        }

        public static bool BuyMinions(int trade_id, int user_id)
        {
            using (var db = new MinionWarsEntities())
            {
                Trading trade = db.Trading.Find(trade_id);
                bool check = CostManager.ApplyMinionCosts(user_id, trade.amount);
                if (check)
                {
                    RewardGenerator.AwardMinions(user_id, trade.minion_id, trade.amount);

                    db.Trading.Remove(trade);

                    db.SaveChanges();
                }
            }

            return true;
        }
    }
}
