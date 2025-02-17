﻿

namespace Skalm.Utilities
{
    internal static class Calculations
    {
        public static float SpawningScalingEquation(float baseMod, float multiplier)
        {
            float calc = baseMod * (1 + 0.01f * multiplier);
            return Math.Clamp(calc, 0.1f, 0.9f);
        }
    }
}
