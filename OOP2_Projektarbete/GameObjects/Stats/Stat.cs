﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.GameObjects.Stats
{
    internal class Stat
    {
        public EStats statName { get; private set; }
        private float baseValue;

        private List<float> modifiers;

        // CONSTRUCTOR I
        public Stat(EStats statName, float baseValue)
        {
            this.statName = statName;
            this.baseValue = baseValue;

            modifiers = new List<float>();
        }

        // GET STAT VALUE
        public float GetValue()
        {
            float output = baseValue;
            modifiers.ForEach(x => output += x);
            return output;
        }

        // ADD MODIFIER
        public void AddModifier(float value)
        {
            if (value > 0)
                modifiers.Add(value);
        }

        // REMOVE MODIFIER
        public void RemoveModifier(float value)
        {
            if (value > 0)
                modifiers.Remove(value);
        }
    }

    public enum EStats
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Luck,
        HP,
        BaseDamage
    }
}
