using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids2
{
    public partial class Display : UserControl
    {
        protected Game game;
        public Display()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.DoubleBuffer, true);
            UpdateStyles();
        }

        public void LinkToGame(Game g)
        {
            game = g;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);  // First, paint black bkgrnd
            if (game != null)
            {

                

                if (game.blackHole != null && game.State == GameState.Running)//&& game.blackHole.Image != null)
                {   // Paint blackHole if exists
                    Vector posHole;
                    int angle;
                    posHole = game.blackHole.Pos;
                    angle = game.blackHole.Angle;
                    Image img = game.ShipImage;// game.blackHole.Image;
                    e.Graphics.TranslateTransform(posHole.X - img.Width / 2, posHole.Y - img.Height / 2);// Move to pos
                    e.Graphics.TranslateTransform(img.Width / 2, img.Height / 2);
                    e.Graphics.RotateTransform(angle); // Rotate to angle of blackHole
                    e.Graphics.TranslateTransform(img.Width / -2, img.Height / -2);
                    e.Graphics.DrawImage(img, new Point()); // Paint the blackHole
                }
                

                foreach(EShip a in game.enemies)
                {
                    e.Graphics.ResetTransform();    // Reset picasso to origin
                    e.Graphics.TranslateTransform(a.Pos.X - a.Image.Width / 2, a.Pos.Y - a.Image.Height / 2);
                    e.Graphics.TranslateTransform(a.Image.Width / 2, a.Image.Height / 2);
                    e.Graphics.RotateTransform(a.Angle);
                    e.Graphics.TranslateTransform(a.Image.Width / -2, a.Image.Height / -2);
                    e.Graphics.DrawImage(a.Image, new Point(0, 0));
                }

                foreach (Asteroid a in game.asteroids)
                {
                    e.Graphics.ResetTransform();    // Reset picasso to origin
                    e.Graphics.TranslateTransform(a.Pos.X - a.Image.Width / 2, a.Pos.Y - a.Image.Height / 2);
                    e.Graphics.TranslateTransform(a.Image.Width / 2, a.Image.Height / 2);
                    e.Graphics.RotateTransform(a.Angle);
                    e.Graphics.TranslateTransform(a.Image.Width / -2, a.Image.Height / -2);
                    e.Graphics.DrawImage(a.Image, new Point(0, 0));
                }

                foreach (Missile m in game.missiles)
                {
                    e.Graphics.ResetTransform();    // Reset picasso to origin
                    e.Graphics.TranslateTransform(m.Pos.X - m.Image.Width / 2, m.Pos.Y - m.Image.Height / 2);
                    e.Graphics.TranslateTransform(m.Image.Width / 2, m.Image.Height / 2);
                    e.Graphics.RotateTransform(m.Angle);
                    e.Graphics.TranslateTransform(m.Image.Width / -2, m.Image.Height / -2);
                    e.Graphics.DrawImage(m.Image, new Point(0, 0));
                }

                foreach (Explosion ex in game.explosions)
                {
                    Image frame = ex.NextFrame;
                    e.Graphics.ResetTransform();
                    e.Graphics.TranslateTransform(ex.Pos.X - frame.Width / 2, ex.Pos.Y - frame.Height / 2);
                    e.Graphics.TranslateTransform(frame.Width / 2, frame.Height / 2);
                    e.Graphics.RotateTransform(ex.Angle);
                    e.Graphics.TranslateTransform(frame.Width / -2, frame.Height / -2);
                    e.Graphics.DrawImage(frame, new Point(0, 0));
                }

                if (game.ship != null)
                {
                    e.Graphics.ResetTransform();
                    Vector posShip = game.ship.Pos;
                    int angle = game.ship.Angle;
                    Image img = game.ship.Image;
                    e.Graphics.TranslateTransform(posShip.X - img.Width / 2, posShip.Y - img.Height / 2);// Move to pos
                    e.Graphics.TranslateTransform(img.Width / 2, img.Height / 2);
                    e.Graphics.RotateTransform(angle); // Rotate to angle of ship
                    e.Graphics.TranslateTransform(img.Width / -2, img.Height / -2);
                    e.Graphics.DrawImage(img, new Point()); // Paint the ship
                }

                // Now draw the HUD indicating # of ships remaining...
                e.Graphics.ResetTransform();
                int left = Width - (game.LivesLeft - 1) * (game.ShipImage.Width + 20);
                e.Graphics.TranslateTransform(left, 10);
                for (int i = 0; i < game.LivesLeft - 1; i++)
                {
                    e.Graphics.DrawImage(game.ShipImage, new Point(0, 0));
                    e.Graphics.TranslateTransform(game.ShipImage.Width + 20, 0);
                }
                // Draw the score at the top center of the screen...
                e.Graphics.ResetTransform();
                e.Graphics.TranslateTransform(Width / 2 - 100, 10);
                e.Graphics.DrawString("SCORE: " + game.Score.ToString(),
                    new Font(FontFamily.GenericSerif, 28, FontStyle.Italic),
                    new SolidBrush(Color.Red), 0, 0);

                //Draw Wave Indicator

                e.Graphics.ResetTransform();
                e.Graphics.TranslateTransform(10, 10);
                e.Graphics.DrawString("WAVE: " + game.Wave,
                    new Font(FontFamily.GenericSerif, 28, FontStyle.Regular),
                    new SolidBrush(Color.Red), 0, 0);
            }
            //            base.OnPaint(e);
        }
    }
}
