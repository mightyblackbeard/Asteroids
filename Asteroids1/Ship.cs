using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids2
{
    public class Ship : Asteroid
    {
        protected bool shielded;
        public bool Shielded
        {
            get
            {
                return shielded;
            }
            set
            {
                shielded = value;
            }
        }

        protected bool crashed;
        public bool Crashed
        {
            get
            {
                return crashed;
            }
            set
            {
                crashed = value;
            }
        }
        public Ship(Vector pos, Vector vel, int angle,
            int angularV, Image img)
            : base(pos, vel, angle, angularV, img)
        {
            crashed = false;
            shielded = false;
        }

        public override void Move()
        {
            base.Move();
            if (AngularV > 0)
            { 
                angularV--;
            }
            else if(AngularV < 0)
            {
                angularV++;
            }

        }

        public void Steer(int amount)
        {
            AngularV += amount;
        }

        public void Gas(int amount)
        {
            Vel.X += (int)Math.Round(Math.Sin(
                Angle / 180.0f * Math.PI) * amount);
            Vel.Y -= (int)Math.Round(Math.Cos(
                Angle / 180.0f * Math.PI) * amount);
        }

        public void Brake()
        {
            if(Vel.X > 1 || Vel.X < -1)
            {
                Vel.X =(int)(Vel.X * 0.9);
            }
            else
            {
                Vel.X = 0;
            }

            if(Vel.Y > 1 || Vel.Y < -1)
            {
                Vel.Y = (int)(Vel.Y * 0.9);

            }
            else
            {
                Vel.Y = 0;
            }
        }

    }
}
