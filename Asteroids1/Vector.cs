using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids2
{
    public class Vector
    {
        #region Properties
        protected float x;
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        protected float y;
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }

        public Vector Unitized
        {
            get
            {
                return this / Magnitude;
            }
        }

        public float Angle
        {
            get
            {
                if (y <= 0)
                    return (float)(Math.Asin(X / Magnitude)
                        * 180 / Math.PI);
                else
                    return (float)(180 - Math.Asin(X / Magnitude)
                        * 180 / Math.PI);
            }
        }
        #endregion

        #region Operators
        public static Vector operator + (Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator - (Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.X / f, v.Y / f);
        }

        public static Vector operator * (float f, Vector v)
        {
            return new Vector(f * v.X, f * v.Y);
        }

        public static Vector operator * (Vector v, float f)
        {
            return new Vector(f * v.X, f * v.Y);
        }
        #endregion

        #region Constructors
        public Vector()
        {
            x = 0;
            y = 0;
        }
        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(Vector v)
        {
            x = v.X;
            y = v.Y;
        }

        public Vector(float angle)
        {
            x = (float)Math.Sin(angle*Math.PI/180);
            y = -1*(float)Math.Cos(angle*Math.PI/180);
        }
        #endregion
    }
}
