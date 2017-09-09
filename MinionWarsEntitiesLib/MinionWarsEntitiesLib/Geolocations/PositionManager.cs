using MinionWarsEntitiesLib.AiManagers;
using MinionWarsEntitiesLib.Models;
using MinionWarsEntitiesLib.Structures;
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

        public static Caravan UpdateCaravanPosition(Caravan car)
        {
            Camp destination;
            using (var db = new MinionWarsEntities())
            {
                destination = db.Camp.Find(car.destination_id);
            }

            if(car.directions == null) Geolocations.GetCaravanDirections(car.location, destination.location);

            if (car.location.Distance(destination.location) <= 50)
            {
                car = CampManager.CaravanArrival(car);
            }
            else
            {
                if (car.current_step == null)
                {
                    car.current_step = Geolocations.GetCaravanDirectionMovement(car, destination.location);
                    if (car.current_step == null)
                    {
                        car.directions = Geolocations.GetCaravanDirections(car.location, destination.location);
                    }
                    else
                    {
                        using (var db = new MinionWarsEntities())
                        {
                            db.Caravan.Attach(car);
                            db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                if (car.location.Distance(car.current_step) <= 10)
                {
                    car.current_step = Geolocations.GetCaravanDirectionMovement(car, destination.location);
                    if (car.current_step == null)
                    {
                        car.directions = Geolocations.GetCaravanDirections(car.location, destination.location);
                    }
                    else
                    {
                        using (var db = new MinionWarsEntities())
                        {
                            db.Caravan.Attach(car);
                            db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                car.location = Geolocations.PerformDirectionMovement(car.location, car.current_step, car.last_movement.Value, 2);
                car.last_movement = DateTime.Now;

                using (var db = new MinionWarsEntities())
                {
                    db.Caravan.Attach(car);
                    db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return car;
        }
    }
}
