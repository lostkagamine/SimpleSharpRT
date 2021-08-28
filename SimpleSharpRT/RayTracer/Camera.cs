using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Camera
    {
        public Vector3 Origin;
        public Vector3 Horizontal;
        public Vector3 Vertical;
        public Vector3 LowerLeft;

        public Camera(Vector3 lookFrom, Vector3 lookAt,
            Vector3 upVector, float vfov, float aspect)
        {
            var theta = Misc.DegRad(vfov);
            var h = MathF.Tan(theta / 2);

            var viewportHeight = 2.0f * h;
            var viewportWidth = aspect * viewportHeight;

            var w = Misc.Unit(lookFrom - lookAt);
            var u = Misc.Unit(Vector3.Cross(upVector, w));
            var v = Vector3.Cross(w, u);

            Origin = lookFrom;
            Horizontal = viewportWidth * u;
            Vertical = viewportHeight * v;
            LowerLeft = Origin - Horizontal / 2 - Vertical / 2 - w;
        }

        public Ray GetRay(float u, float v)
        {
            return new Ray(Origin, LowerLeft + u * Horizontal + v * Vertical - Origin);
        }

        public float Clamp(float x, float a, float b)
        {
            if (x < a) return a;
            if (x > b) return b;
            return x;
        }
    }
}
