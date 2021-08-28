using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Direction;

        public Ray(Vector3 origin, Vector3 dir)
        {
            Origin = origin;
            Direction = dir;
        }

        public Vector3 At(float t)
        {
            return Origin + (t * Direction);
        }
    }
}
