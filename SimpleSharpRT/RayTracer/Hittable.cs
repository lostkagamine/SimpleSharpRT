using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public struct RayHit
    {
        public Vector3 P;
        public Vector3 Normal;
        public float T;
        public bool FrontFace;
        public Material Material;

        public void SetFaceNormal(Ray r, Vector3 outwardNormal)
        {
            FrontFace = Vector3.Dot(r.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }

    public abstract class Hittable
    {
        public abstract bool Hit(Ray r,
                                 float tMin,
                                 float tMax,
                                 out RayHit hit);
    }
}
