using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public struct Pixel
    {
        public float Red;
        public float Green;
        public float Blue;
        public int Samples;

        //public object Lock;

        public Pixel Normalise()
        {
            var scale = 1f / Samples;
            return new Pixel
            {
                Red = MathF.Sqrt(Red * scale),
                Green = MathF.Sqrt(Green * scale),
                Blue = MathF.Sqrt(Blue * scale),
                Samples = 1
                //Lock = new()
            };
        }

        public static Pixel FromVec(Vector3 v, int s=1)
        {
            return new Pixel
            {
                Red = v.X,
                Green = v.Y,
                Blue = v.Z,
                Samples = s
                //Lock = new()
            };
        }
    }

    public class Image
    {
        public int Width;
        public int Height;
        public int ColourDepth = 255;
        public int DefaultSamples = 100;

        public int CurrentX = 0;
        public int CurrentY = 0;

        public Pixel[,] Data;

        public Image(int w, int h, int cd=255, int samp=100)
        {
            Width = w;
            Height = h;
            ColourDepth = cd;
            DefaultSamples = samp;

            CurrentX = 0;
            CurrentY = Height-1;

            Data = new Pixel[Height, Width];

            for (int y=0; y<h; y++)
            {
                for (int x=0; x<w; x++)
                {
                    Data[y, x] = new Pixel
                    {
                        Red = 0,
                        Blue = 0,
                        Green = 0,
                        Samples = DefaultSamples
                        //Lock = new()
                    };
                }
            }
        }

        public string ProduceHeader()
        {
            return $"P3\n{Width} {Height}\n{ColourDepth}\n";
        }

        public string ProduceData()
        {
            var s = ProduceHeader();

            for (int y=Height-1; y>=0; y--)
            {
                for (int x=0; x<Width; x++)
                {
                    var pix = Data[y, x].Normalise();
                    var cr = (int)(pix.Red * ColourDepth);
                    var cg = (int)(pix.Green * ColourDepth);
                    var cb = (int)(pix.Blue * ColourDepth);

                    s += $"{cr} {cg} {cb}\n";
                }
            }

            return s;
        }

        public void SetPixel(int x, int y, float r, float g, float b)
        {
            var pix = Data[y, x];
            pix.Red = r;
            pix.Green = g;
            pix.Blue = b;
            Data[y, x] = pix;
        }

        public void SetPixel(int x, int y, Vector3 colour)
        {
            var samp = Data[y, x].Samples;
            Data[y, x] = Pixel.FromVec(colour, samp);
        }

        public void SetNextPixel(Vector3 colour)
        {
            var samp = Data[CurrentY, CurrentX].Samples;
            Data[CurrentY, CurrentX] = Pixel.FromVec(colour, samp);
            CurrentX++;
            if (CurrentX >= Width)
            {
                CurrentX = 0;
                if (CurrentY-- < 0)
                {
                    throw new Exception($"Image.SetNextPixel: Tried to go outside bounds.");
                }
            }
        }
    }
}
