﻿using Skalm.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors
{
    internal interface ICollidable
    {
        void OnCollision(ICollidable collider);
    }
}
