using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Material
    {
        public virtual bool Scatter(Ray r, RayHit hit, out Vector3 attenuation, out Ray scattered)
        {
            attenuation = Vector3.Zero;
            scattered = new Ray(Vector3.Zero, Vector3.Zero);

            return false;
        }
    }
}
