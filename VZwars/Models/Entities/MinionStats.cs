using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VZwars.Models.Entities
{
    public class MinionStats
    {
        public int str;
        public int vit;
        public int dex;

        public MinionStats(int type, int build)
        {
            CalculateStr(type, build);
            CalculateVit(type, build);
            CalculateDex(type, build);
        }

        private void CalculateStr(int type, int build)
        {

        }

        private void CalculateVit(int type, int build)
        {

        }

        private void CalculateDex(int type, int build)
        {

        }
    }
}