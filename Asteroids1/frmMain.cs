using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids2
{
    public partial class frmMain : Form
    {
        protected const int WM_KEYDOWN = 0x100;
        protected const int WM_KEYUP = 0x101;
        protected Game game;
        protected Timer tmr;
        public frmMain()
        {
            InitializeComponent();
            game = new Game();
            display.LinkToGame(game);
            tmr = new Timer();
            tmr.Interval = 20;
            tmr.Tick += tmr_Tick;
            tmr.Start();
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            game.Update();
            display.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(msg.Msg == WM_KEYDOWN)
            {
                if(keyData == Keys.W)
                {
                    game.Gas(1);
                }
                else if(keyData == Keys.S)
                {
                    game.Gas(-1);
                }
                else if(keyData == Keys.Space)
                {
                    game.Brake();
                }
                //else if(keyData == Keys.A)
                //{
                //    game.Steer(-4);
                //}
                //else if(keyData == Keys.D)
                //{
                //    game.Steer(4);
                //}

                
                //Text = "KEYDOWN: " + keyData.ToString();
                return true;
            }
            return false;
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                game.ResetShip(new Vector(display.Width / 2, display.Height / 2));
            }
            
            //Text += "  KEYUP: " + e.KeyCode.ToString();

            if(e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show("Exit?", "Quit", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    this.Close();
                }
                
            }
        }

        private void display_SizeChanged(object sender, EventArgs e)
        {
            game.Width = display.Width;
            game.Height = display.Height;
        }

        private void display_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
//            Cursor.Hide();
        }

        private void display_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
//            Cursor.Show();
        }

        private void display_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                game.Fire();
            }

            if(e.Button == MouseButtons.Right)
            {
                game.fireBomb();
            }
        }

        private void display_MouseMove(object sender, MouseEventArgs e)
        {
            if (game.ship != null)
            {
                Vector target = new Vector(e.X, e.Y);
                Vector pointing = target - game.ship.Pos;
                float angle = pointing.Angle;
                game.ship.Angle = (int)angle;
            }
        }
    }
}
