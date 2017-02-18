using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace Asteroids2
{
    public class Missile : Asteroid
    {
        protected Timer tmr;
        public bool friendlyMissle;

        public Missile(Vector pos, Vector vel, int angle,
            int angularV, Image img, bool type)
            : base(pos, vel, angle, angularV, img)
        {
            friendlyMissle = type;
        }

        virtual public void PlaceMissile(Vector shipPos, int shipAngle,
            int shipHeight)
        {
            Vector facing = new Vector(shipAngle);
            float dist = shipHeight/2 + image.Height/2;
            Vector offset = dist * facing;
            Vector missilePos = shipPos + offset;
            this.Pos = missilePos;
            this.Angle = shipAngle;
            this.Vel = 15 * facing;
        }

        public void PlaceMissile(Vector shipPos, int shipAngle,
            int angleOffset, int shipWidth)
        {
            Vector facing = new Vector(shipAngle+angleOffset);
            float dist = shipWidth / 2 + image.Width / 2;
            Vector offset = dist * facing;
            Vector missilePos = shipPos + offset;
            this.Pos = missilePos;
            this.Angle = shipAngle;
            facing = new Vector(shipAngle);
            this.Vel = 10 * facing;
        }
    }
}
