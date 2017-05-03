using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models
{
    public class Battlegroup
    {
        public List<Minion> frontline;
        public List<Minion> backline;
        public List<Minion> support;

        public double groupSpeed = 0;

        public Battlegroup()
        {

        }

        public Battlegroup(List<Minion> f, List<Minion> b, List<Minion> s)
        {
            this.frontline = f;
            this.backline = b;
            this.support = s;

            CalculateGroupSpeed();
        }

        public void CalculateGroupSpeed()
        {
            double sum = 0;
            sum += this.frontline.Average(x => x.speed) + this.backline.Average(x => x.speed) + this.support.Average(x => x.speed);
            this.groupSpeed = Math.Floor(sum / 3);
        }
    }
}