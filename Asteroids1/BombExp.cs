using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Asteroids2
{
    public delegate void BombExplosion(BombExp b);

    public class BombExp : Explosion
    {
        public event BombExplosion bFinished;
        public Image img;

        public override Image NextFrame
        {
            get
            {
                if (frameIdx == frames.Count)
                {
                    if (bFinished != null)
                    {
                        bFinished(this);
                    }
                    return frames[frames.Count - 1];
                }
                else
                {
                    img = frames[frameIdx];
                    return frames[frameIdx++];
                }
            }
        }

        public BombExp(Vector pos, Image spriteSheet, int angle, int numAcross, int numDown, int width, int height)
            : base(pos, spriteSheet, angle, numAcross, numDown, width, height)
        {
            AdditionalFrames();
        }

        private void AdditionalFrames()
        {
            Rectangle r = new Rectangle(0, 0, 70, 61);
            Bitmap bmpSheet = new Bitmap(Properties.Resources.bombpart2);
            PixelFormat pixFormat = bmpSheet.PixelFormat;
            Image i = bmpSheet.Clone(r, pixFormat);
            frames.Add(i);

            r = new Rectangle(70, 0, 112, 90);
            i = bmpSheet.Clone(r, pixFormat);
            frames.Add(i);
            frames.Add(i);

            r = new Rectangle(182, 0, 150, 137);
            i = bmpSheet.Clone(r, pixFormat);
            frames.Add(i);
            frames.Add(i);

            r = new Rectangle(332, 0, 190, 173);
            i = bmpSheet.Clone(r, pixFormat);
            frames.Add(i);
            frames.Add(i);

            r = new Rectangle(522, 0, 226, 211);
            i = bmpSheet.Clone(r, pixFormat);
            frames.Add(i);
            frames.Add(i);

            r = new Rectangle(748, 0, 241, 225);
            i = bmpSheet.Clone(r, pixFormat);
            frames.Add(i);
            frames.Add(i);
            frames.Add(i);
        }
    }
}
