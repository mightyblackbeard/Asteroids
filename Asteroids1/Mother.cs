using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Asteroids2
{
    public class Mother : EShip
    {
        public int HP;

        public Mother(Vector pos, Vector Vel, int angle, int angularV, Image i, int type,int hp)
            : base(pos,Vel,angle,angularV, i, type)
        {
            canShoot = false;
            this.shoot.Interval = hp * 1000;
            shoot.Elapsed += pew;
            shoot.Start();

            HP = 5 * hp;
        }

        public event spew spawn;

        public override void pew(object sender, EventArgs e)
        {
            if (spawn != null)
            {
                spawn(this);
            }
        }
    }
}
