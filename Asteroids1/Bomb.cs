using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;

namespace Asteroids2
{
    public class Bomb : Missile
    {
        public Timer swap;
        public Timer explode;

        public Bomb(Vector Pos, Vector Vel, int angle, int angularV, Image i, bool f)
            : base(Pos, Vel, angle, angularV, i, f)
        {
            this.type = 0;

            swap = new Timer();
            swap.Interval = 500;
            swap.Elapsed += Flash;
            swap.Start();

            explode = new Timer();
            explode.Interval = 2000;
            explode.Elapsed += pBoom;
            explode.Start();

        }

        private void Flash(object sender, ElapsedEventArgs e)
        {
            if(this.type == 0)
            {
                Image = Properties.Resources.torpedo2;
                type = 1;
            }
            else
            {
                Image = Properties.Resources.torpedo1;
                type = 0;
            }
        }

        public override void PlaceMissile(Vector shipPos, int shipAngle,
            int shipHeight)
        {
            Vector facing = new Vector(shipAngle);
            float dist = shipHeight / 2 + image.Height / 2;
            Vector offset = dist * facing;
            Vector missilePos = shipPos + offset;
            this.Pos = missilePos;
            this.Angle = shipAngle;
            this.Vel = 7 * facing;
        }

        public event photon boom;

        public void pBoom(object sender, EventArgs e)
        {
            if(boom != null)
            {
                boom(this);
            }
        }
    }
}
