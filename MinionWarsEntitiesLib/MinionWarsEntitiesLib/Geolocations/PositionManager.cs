using MinionWarsEntitiesLib.AiManagers;
using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.Geolocations
{
    public static class PositionManager
    {
        public static Battlegroup UpdatePosition(Battlegroup bg)
        {
            Orders orders = null;
            using (var db = new MinionWarsEntities())
            {
                orders = db.Orders.Find(bg.orders_id);
            }

            bool arrived = false;

            if (bg.location.Distance(orders.location).Value <= 10)
            {
                arrived = true;
                orders = OrdersManager.ContinueOrders(bg, orders);
            }

            //bg.location = Geolocations.Geolocations.PerformMovement(bg.location, bg.lastMovement.Value, orders, bg.group_speed);
            if(orders.current_step == null)
            {
                orders.current_step = Geolocations.GetDirectionMovement(bg.location, orders);
                if(orders.current_step == null)
                {
                    orders = OrdersManager.ContinueOrders(bg, orders);
                }
                else
                {
                    using (var db = new MinionWarsEntities())
                    {
                        db.Orders.Attach(orders);
                        db.Entry(orders).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            if (bg.location.Distance(orders.current_step) <= 10)
            {
                orders.current_step = Geolocations.GetDirectionMovement(bg.location, orders);
                if (orders.current_step == null)
                {
                    orders = OrdersManager.ContinueOrders(bg, orders);
                }
                else
                {
                    using (var db = new MinionWarsEntities())
                    {
                        db.Orders.Attach(orders);
                        db.Entry(orders).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            bg.location = Geolocations.PerformDirectionMovement(bg.location, orders.current_step, bg.lastMovement.Value, bg.group_speed);

            bg.lastMovement = DateTime.Now;

            using (var db = new MinionWarsEntities())
            {
                db.Battlegroup.Attach(bg);
                db.Entry(bg).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return bg;
        }
    }
}
