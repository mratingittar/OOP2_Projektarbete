﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class ActorStatsObject
    {
        public StatsObject stats;

        public string name;
        int HPcurr;

        // CONSTRUCTOR I
        public ActorStatsObject(StatsObject stats, string name)
        {
            this.stats = stats;

            this.name = name;
            HPcurr = stats.statsArr[(int)EStats.HP].GetValue();
        }

        // TAKE DAMAGE
        public void TakeDamage(int damage)
        {
            HPcurr -= damage;
            if (HPcurr <= 0)
                HandleDeath();
        }

        // HEAL DAMAGE
        public void HealDamage(int healAmount)
        {
            HPcurr += healAmount;
            if (HPcurr > stats.statsArr[(int)EStats.HP].GetValue())
                HPcurr = stats.statsArr[(int)EStats.HP].GetValue();
        }

        // HANDLE DEATH
        private void HandleDeath()
        {

        }
    }
}
