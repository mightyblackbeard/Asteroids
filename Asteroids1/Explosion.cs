using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Asteroids2
{
    public delegate void FinishedHandler(Explosion ex);

    public class Explosion
    {
        protected int frameIdx;
        protected List<Image> frames;
        protected int angle;
        public int Angle
        {
            get
            {
                return angle;
            }
        }
        protected Vector pos;
        public Vector Pos
        {
            get
            {
                return pos;
            }
        }
        public event FinishedHandler Finished;
        virtual public Image NextFrame
        {
            get
            {
                if (frameIdx == frames.Count)
                {
                    if (Finished != null)
                    {
                        Finished(this);
                    }
                    return frames[frames.Count - 1];
                }
                else
                {
                    return frames[frameIdx++];
                }
            }
        }

        public Explosion(Vector pos, Image spriteSheet, int angle,
            int numAcross, int numDown, int width, int height)
        {
            frameIdx = 0;
            this.pos = pos;
            this.angle = angle;
            frames = new List<Image>();
            Bitmap bmpSheet = new Bitmap(spriteSheet);
            PixelFormat pixFormat = bmpSheet.PixelFormat;
            for (int y = 0; y < numDown * height; y += height)
            {
                for (int x = 0; x < numAcross * width; x += width)
                {
                    Rectangle rect = new Rectangle(x, y, width, height);
                    Image frame = bmpSheet.Clone(rect, pixFormat);
                    frames.Add(frame);
                }
            }
        }

    }
}
