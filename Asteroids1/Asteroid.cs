using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids2
{
    public class Asteroid
    {
        public bool isBlackHole = false;
        #region Properties
        protected Vector pos;
        public Vector Pos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }

        protected Vector vel;
        public Vector Vel
        {
            get
            {
                return vel;
            }
            set
            {
                vel = value;
            }
        }

        protected int angle;
        public int Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }

        protected int angularV;
        public int AngularV
        {
            get
            {
                return angularV;
            }
            set
            {
                angularV = value;
            }
        }

        protected Image image;
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        protected int size;
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        protected int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion

        #region Methods
        public Asteroid(Vector pos, Vector vel, int angle,
            int angularV, Image img)
        {
            Pos = pos;
            Vel = vel;
            Angle = angle;
            AngularV = angularV;
            Image = img;
        }

        public virtual void Move()
        {
            Pos.X += Vel.X;
            Pos.Y += Vel.Y;
            Angle += AngularV;
        }

        public void Move(Vector pull)
        {
            Vel = Vel + pull;
        }
        #endregion

    }
}
