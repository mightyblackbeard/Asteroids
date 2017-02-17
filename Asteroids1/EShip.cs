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
    public class EShip : Asteroid
    {
        public Timer shoot;
        public bool canShoot;

        public EShip(Vector Pos, Vector Vel, int angle, int angularV, Image i, int t)
            : base (Pos, Vel, angle, angularV, i)
        {
            canShoot = true;
            type = t;
            shoot = new Timer();
            shoot.Interval = 1500;
            shoot.Elapsed += pew;
            shoot.Start();
        }

        public event fireaway fire;

        virtual public void pew(object sender, EventArgs e)
        {
            Random r = new Random();

            shoot.Interval = r.Next(1, 3) * 1000;

            if(fire != null)
            {
                fire(this);
            }

        }

    }
}
