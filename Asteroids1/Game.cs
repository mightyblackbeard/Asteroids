using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;

namespace Asteroids2
{
    public delegate void fireaway(EShip e);
    public delegate void spew(Mother m);
    public delegate void photon(Bomb b);


    public class Game
    {
        #region Properties
        protected GameState state;
        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        protected int width;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        protected int height;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        protected int ak;
        public int AK
        {
            get
            {
                return ak;
            }
        }
        protected int livesLeft;
        public int LivesLeft
        {
            get
            {
                return livesLeft;
            }
            set
            {
                livesLeft = value;
            }
        }
        protected Image shipImage;
        public Image ShipImage
        {
            get
            {
                return shipImage;
            }
        }
        protected int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
        #endregion

        protected int numMissiles;
        public Ship ship;
        public Asteroid blackHole;
        public List<Asteroid> asteroids;
        public List<Missile> missiles;
        public List<Explosion> explosions;
        public List<BombExp> b_explosions;
        protected List<EShip> enemiesToAdd;
        protected int shipMissles;
        protected int Roids;
        protected List<int> explosionsToDelete;
        protected List<Missile> newMissles;
        protected List<BombExp> newBombExplosion;
        public List<Bomb> bombs;
        public List<EShip> enemies;
        protected List<int> bombexpToDelete;
        protected Random rand;
        public int BombsLeft;
        protected Asteroid p;
        protected System.Timers.Timer shipSpawnTmr;
        public int Wave;
        protected int bonusScore;


        public Game()
        {
            state = GameState.Running;
            shipMissles = 0;
            ak = 0;
            Score = 0;
            bonusScore = 0;
            numMissiles = 16;
            livesLeft = 5;
            BombsLeft = 3;
            rand = new Random();
            shipImage = Properties.Resources.ship;
            asteroids = new List<Asteroid>();
            missiles = new List<Missile>();
            explosions = new List<Explosion>();

            explosionsToDelete = new List<int>();
            enemiesToAdd = new List<EShip>();

            bombs = new List<Bomb>();
            b_explosions = new List<BombExp>();
            bombexpToDelete = new List<int>();
            newBombExplosion = new List<BombExp>();

            newMissles = new List<Missile>();
            enemies = new List<EShip>();

            shipSpawnTmr = new System.Timers.Timer();
            shipSpawnTmr.Interval = 4000;
            ResetShip(new Vector(600, 300));
            Wave = 8;
            //            blackHole = new Asteroid(new Vector(600, 300), new Vector(0.0f, 0),
  //              0, 1, Properties.Resources.blackhole);
        }

        public void fireBomb()
        {
            if(BombsLeft > 0)
            {
                Bomb b = new Bomb(new Vector(0, 0), new Vector(0, 0), 0, 0,
                        Properties.Resources.torpedo1, true);
                b.PlaceMissile(ship.Pos, ship.Angle,
                            ship.Image.Height);
                b.boom += B_boom;

                bombs.Add(b);

                BombsLeft--;
            }
        }

        private void B_boom(Bomb b)
        {
            Image img = Properties.Resources.bombpart1;
            BombExp ex = new BombExp(new Vector(b.Pos),
            img, rand.Next(0, 360), 8, 2, 128, 128);
            ex.bFinished += Ex_bFinished;
            newBombExplosion.Add(ex);
            b.swap.Stop();
            b.explode.Stop();

            bombs.Remove(b);
        }
        
        private void Ex_bFinished(BombExp b)
        {
            bombexpToDelete.Insert(0, b_explosions.IndexOf(b));
        }

        public void AddAsteroid()
        {
            float velX =0, velY = 0;
            while (velX == 0 || velY == 0)
            {
                velX = rand.Next(-3, 4);
                velY = rand.Next(-3, 4);
            }
            int angV = 0;
            while (angV == 0)
            {
                angV = rand.Next(-8, 9);
            }

            int type = rand.Next(0, 2);
            // decide which asteroid size...
            int size = rand.Next(1, 4);
            Image img;
            if (size == 1)
            {
                if (type == 0)
                    img = Properties.Resources.AsteroidSmall;
                else
                    img = Properties.Resources.Asteroid2Small;
            }
            else if (size == 2)
            {
                if (type == 0)
                    img = Properties.Resources.AsteroidMed;
                else
                    img = Properties.Resources.Asteroid2Med;
            }
            else
            {
                if (type == 0)
                    img = Properties.Resources.AsteroidLg;
                else
                    img = Properties.Resources.Asteroid2Lg;
            }

            Vector location = new Vector(rand.Next(0, width),
                rand.Next(0, height));
            
            
            

            Asteroid a = new Asteroid(location,new Vector(velX, velY),
                0, angV, img);
            a.Size = size;
            a.Type = type;

            Roids += size;

            if (ship != null)
            {
                float sumOfWidths = ship.Image.Width / 2 + a.Image.Width / 2;
                Vector pointing = ship.Pos - a.Pos;
                float dist = pointing.Magnitude;

                while (dist < sumOfWidths + 150)
                {
                    location.X = rand.Next(0, width);
                    location.Y = rand.Next(0, height);
                    a.Pos = location;

                    pointing = ship.Pos - a.Pos;
                    dist = pointing.Magnitude;
                }
            }

            asteroids.Add(a);
        }

        public void AddEnemies(Mother m)
        {

            int imagevariation = rand.Next(0, 2);
            Image i;

            if (imagevariation == 0)
            {
                i = Properties.Resources.Emeny;
            }
            else
            {
                i = Properties.Resources.Emeny2;
            }


            EShip e = new EShip(new Vector(m.Pos), new Vector(rand.Next(-5, 5), rand.Next(-5, 5)), rand.Next(0, 360), rand.Next(-7, 7), i, imagevariation);
            e.fire += EnemyFire;
            enemiesToAdd.Add(e);

        }

        public void MotherShip()
        {
            int imagevariation = rand.Next(0, 2);
            Image i;

            if (imagevariation == 0)
            {
                i = Properties.Resources.Mothership;
            }
            else
            {
                i = Properties.Resources.Mothership2;
            }


            Mother e = new Mother(new Vector(rand.Next(0, 800), rand.Next(0, 800)),
                new Vector(1,1), rand.Next(0, 360), rand.Next(-7, 7), i,
                imagevariation, rand.Next(3,6));
            e.spawn += Spawn;
            Spawn(e);
            enemiesToAdd.Add(e);
        }

        private void Spawn(Mother es)
        {
            for (int i = 0; i < 2; i++)
            {
                AddEnemies(es);
                        
            }

            es.Vel = new Vector(rand.Next(-5, 5), rand.Next(-5, 5));
            es.AngularV = rand.Next(-10, 10);
            es.shoot.Interval = rand.Next(2, 5) * 1000;
             
        }

        private void EnemyFire(EShip es)
        {
                if (es.canShoot)
                {
                    Image i;

                    if (es.Type == 0)
                    {
                        i = Properties.Resources.lazer;
                    }
                    else
                    {
                        i = Properties.Resources.plasma;
                    }

                    if (ship != null)
                    {
                        Vector pointing = new Vector(ship.Pos - es.Pos);
                        int height = Properties.Resources.Emeny.Height;

                        Missile m = new Missile(new Vector(0, 0),
                                    new Vector(0, 0), 0, 0,
                                    i, false);

                        m.PlaceMissile(new Vector(es.Pos), (int)pointing.Angle, height);

                        newMissles.Add(m);

                    }
                    es.Vel = new Vector(rand.Next(-5, 5), rand.Next(-5, 5));
                    es.AngularV = rand.Next(-10, 10);
                    es.shoot.Interval = rand.Next(1, 4) * 1000;
                }           
        }

        public void ResetShip(Vector pos)
        {
            if (livesLeft > 0)
            {
                ship = new Ship(pos, new Vector(0, 0),
                    0, 0, shipImage);
                if (blackHole != null)
                {
                    shipSpawnTmr.Start();
                    ship.Image = Properties.Resources.shipshielded;
                    ship.Shielded = true;
                }
            }
            ak = 0; // Reset asteroids killed to zero
            numMissiles = 10; // Bonus round ended, no more double shots of Tequila
        }

        public void Gas(int amount)
        {
            if(ship != null)
                ship.Gas(amount);
        }

        public void Steer(int amount)
        {
            if (ship != null)
                ship.Steer(amount);
        }

        public void Fire()
        {
            if (ship != null && shipMissles < numMissiles)
            {
                if (ak < 40)
                {
                    Missile m = new Missile(new Vector(0, 0),
                        new Vector(0, 0), 0, 0,
                        Properties.Resources.missile, true);

                    m.PlaceMissile(ship.Pos, ship.Angle,
                        ship.Image.Height);

                    newMissles.Add(m);
                    shipMissles++;

                }
                else if (ak>=40 && ak < 100)
                {
                    // Bonus round: 2 missiles fired at a time!
                    Missile m1 = new Missile(new Vector(0, 0),
                        new Vector(0, 0), 0, 0,
                        Properties.Resources.missile, true);

                    m1.PlaceMissile(ship.Pos, ship.Angle,
                        -90, ship.Image.Width);

                    newMissles.Add(m1);

                    Missile m2 = new Missile(new Vector(0, 0),
                        new Vector(0, 0), 0, 0,
                        Properties.Resources.missile, true);

                    m2.PlaceMissile(ship.Pos, ship.Angle,
                        90, ship.Image.Width);

                    newMissles.Add(m2);
                    shipMissles+=2;
                }
                else
                {
                    Missile m = new Missile(new Vector(0, 0),
                        new Vector(0, 0), 0, 0,
                        Properties.Resources.missile, true);

                    m.PlaceMissile(ship.Pos, ship.Angle,
                        ship.Image.Height);

                    newMissles.Add(m);

                    Missile m1 = new Missile(new Vector(0, 0),
                        new Vector(0, 0), 0, 0,
                        Properties.Resources.missile, true);

                    m1.PlaceMissile(ship.Pos, ship.Angle,
                        -90, ship.Image.Width);

                    newMissles.Add(m1);

                    Missile m2 = new Missile(new Vector(0, 0),
                        new Vector(0, 0), 0, 0,
                        Properties.Resources.missile, true);

                    m2.PlaceMissile(ship.Pos, ship.Angle,
                        90, ship.Image.Width);

                    newMissles.Add(m2);
                    shipMissles +=3;
                }
            }
        }

        protected Vector CalcGravityPull(Vector shipPos, Vector blackHolePos)
        {
            Vector pull;
            Vector pointing = blackHolePos - shipPos;
            float dist = pointing.Magnitude;
            if (dist > 0.1)
            {
                Vector facing = pointing.Unitized;
                pull = 2500 * (facing / (dist * dist));
            }
            else
            {
                pull = new Vector(5000, 5000);
            }
            return pull;
        }

        public void Update()
        {
            if (blackHole != null)
            {
                blackHole.Move();
                if ((blackHole.Pos.X < 0) || (blackHole.Pos.X > Width)
                    || (blackHole.Pos.Y < 0) || (blackHole.Pos.Y > Width))
                {
                    blackHole = null;
                }
            }

            if (ship != null)
            {
                ship.Move();
                // Only allow blackHole to affect ship if not Resetting...
                if (blackHole != null && state == GameState.Running)
                {
                    Vector pull = CalcGravityPull(ship.Pos, blackHole.Pos);
                    ship.Move(pull);
                }
                Wrap(ship.Pos);
            }

            foreach (Asteroid a in asteroids)
            {
                a.Move();
                if (blackHole != null)
                {       // NOTE: the -1 below is to create a "REPULSION"
                    Vector pull = -1* CalcGravityPull(a.Pos, blackHole.Pos);
                    a.Move(pull);
                }
                Wrap(a.Pos);
            }

            foreach (Missile m in newMissles)
            {
                missiles.Add(m);
            }

            foreach (BombExp b in newBombExplosion)
            {
                b_explosions.Add(b);
            }

            foreach(EShip e in enemiesToAdd)
            {
                enemies.Add(e);
            }

            enemiesToAdd.Clear();
            newBombExplosion.Clear();
            newMissles.Clear();

            foreach (Missile m in missiles)
            {
                if (m != null)
                {
                    m.Move();
                    if (blackHole != null && state == GameState.Running)
                    {
                        Vector pull = CalcGravityPull(m.Pos, blackHole.Pos);
                        m.Move(pull);
                    }
                }
            }

            foreach(Bomb b in bombs)
            {
                b.Move();
                Wrap(b.Pos);
            }

            foreach(EShip e in enemies)
            {
                e.Move();
                Wrap(e.Pos);
            }

            KillMissilesOutOfRange();
            KillExplosionsWhenTheyreDone();
            DetectCollisions();

            if(asteroids.Count==0)
            {
                Wave++;

                Roids = 0;

                if(Wave%5 == 0)
                {
                    for(int i = 0; i < Wave/5; i++)
                    {
                        MotherShip();
                    }

                    AddAsteroid();
                }
                else
                {
                    while (Roids < 10 * (double)(Wave * .5))
                    {
                        AddAsteroid();
                    }
                }
            }

            if(bonusScore > 10000)
            {
                LivesLeft++;
                BombsLeft++;
                bonusScore = 0;
            }
        }

        protected void KillMissilesOutOfRange()
        {
            List<int> missilesToDelete = new List<int>();
            foreach (Missile m in missiles)
            {   // Search every missile to see if out of range
                if ((m.Pos.X < 0) || (m.Pos.X > Width)
                    || (m.Pos.Y < 0) || (m.Pos.Y > Height))
                {   // this missile was outside at least one boundary
                    int idxMissile = missiles.IndexOf(m);
                    if (!missilesToDelete.Contains(idxMissile))
                    {
                        missilesToDelete.Insert(0, missiles.IndexOf(m));
                    }
                }
            }
            foreach (int idx in missilesToDelete)
            {   // Now delete all bad missiles found...
                if(missiles[idx].friendlyMissle)
                {
                    shipMissles--;
                }
                missiles.RemoveAt(idx);
            }
        }

        protected void DetectCollisions()
        {
            List<int> asteroidsToDelete = new List<int>();
            List<int> missilesToDelete = new List<int>();
            List<int> enemiesToDelete = new List<int>();

            
            foreach (Asteroid a in asteroids)
            {

                foreach (EShip Enemy in enemies)
                {
                    int sumOfWidths = Enemy.Image.Width / 2 + a.Image.Width / 2;
                    Vector pointing = new Vector(Enemy.Pos - a.Pos);
                    float dist = pointing.Magnitude;

                    if (dist - 10 < sumOfWidths)
                    {
                        pointing = new Vector(a.Pos - Enemy.Pos);
                        Enemy.Vel = new Vector(pointing.Unitized * -5f);
                    }

                    if(dist < sumOfWidths)
                    {
                        if(!enemiesToDelete.Contains(enemies.IndexOf(Enemy)))
                        {
                            enemiesToDelete.Insert(0, enemies.IndexOf(Enemy));
                        }
                    }

                    foreach(BombExp b in b_explosions)
                    {
                        if (b.img != null)
                        {
                            sumOfWidths = Enemy.Image.Width / 2 + b.img.Width / 2;
                            pointing = new Vector(Enemy.Pos - b.Pos);
                            dist = pointing.Magnitude;

                            if (dist < sumOfWidths)
                            {
                                if (!enemiesToDelete.Contains(enemies.IndexOf(Enemy)))
                                {
                                    enemiesToDelete.Insert(0, enemies.IndexOf(Enemy));
                                }
                            }
                        }

                    }

                }

                foreach (BombExp b in b_explosions)
                {
                    if (b.img != null)
                    {
                        int sumOfWidths = a.Image.Width / 2 + b.img.Width / 2;
                        Vector pointing = new Vector(a.Pos - b.Pos);
                        float dist = pointing.Magnitude;

                        if (dist < sumOfWidths)
                        {
                            int idxAsteroid = asteroids.IndexOf(a);
                            if (!asteroidsToDelete.Contains(idxAsteroid))
                                asteroidsToDelete.Insert(0, idxAsteroid);
                        }
                    }

                }

                foreach (Missile m in missiles)
                {
                    float sumOfWidths = m.Image.Width / 2 + a.Image.Width / 2;
                    Vector pointing = m.Pos - a.Pos;
                    float dist = pointing.Magnitude;
                    if ((dist < sumOfWidths) && (m.friendlyMissle == true))
                    {
                        // Collision detected! Store index for later removal

                        int idxAsteroid = asteroids.IndexOf(a);
                        int idxMissile = missiles.IndexOf(m);
                        if(!asteroidsToDelete.Contains(idxAsteroid))
                            asteroidsToDelete.Insert(0, idxAsteroid);
                        if(!missilesToDelete.Contains(idxMissile))
                            missilesToDelete.Insert(0, idxMissile);
                    }

                    if (ship != null)
                    {
                        sumOfWidths = m.Image.Width / 2 + ship.Image.Width / 2;
                        pointing = m.Pos - ship.Pos;
                        dist = pointing.Magnitude;

                        if ((dist < sumOfWidths && !m.friendlyMissle))
                        {       // Ship was shot by enemy laser
                            if (!ship.Shielded)
                            {
                                ship.Crashed = true;
                                Explosion ex = new Explosion(new Vector(ship.Pos),
                                    Properties.Resources.explosionShip, 0,
                                    8, 4, 256, 256);
                                ex.Finished += ex_Finished;
                                explosions.Add(ex);
                                ship = null;
                                livesLeft--;
                            }
                            if (!missilesToDelete.Contains(missiles.IndexOf(m)))
                                missilesToDelete.Insert(0, missiles.IndexOf(m));
                            // Create a "white hole" for repulsion...
                            blackHole = new Asteroid(new Vector(width / 2, height / 2),
                                new Vector(0, 0), 0, 0, Properties.Resources.blackhole);

                            shipSpawnTmr.Elapsed += shipSpawnTmr_Elapsed;
                            state = GameState.Resetting;
                        }
                    }

                    if(m.friendlyMissle)
                    {//killing the alien ships

                        foreach (EShip enemy in enemies)
                        {
                            sumOfWidths = m.Image.Width / 2 + enemy.Image.Width / 2;
                            pointing = m.Pos - enemy.Pos;
                            dist = pointing.Magnitude;

                            if(dist < sumOfWidths)
                            {
                                int idxofenemy = enemies.IndexOf(enemy);
                                int idxMissile = missiles.IndexOf(m);
                                if (!enemiesToDelete.Contains(idxofenemy))
                                    enemiesToDelete.Insert(0, idxofenemy);
                                if (!missilesToDelete.Contains(idxMissile))
                                    missilesToDelete.Insert(0, idxMissile);
                            }
                        }
                    }
                }

                if (ship != null)
                {
                    float sumOfWidths2 = ship.Image.Width / 2 + a.Image.Width / 2;
                    Vector pointing2 = ship.Pos - a.Pos;
                    float dist2 = pointing2.Magnitude;
                    if (dist2 < sumOfWidths2)
                    {       // Ship has collided with an asteroid
                        if (!ship.Shielded)
                        {
                            ship.Crashed = true;
                            Explosion ex = new Explosion(new Vector(ship.Pos),
                                Properties.Resources.explosionShip, 0,
                                8, 4, 256, 256);
                            ex.Finished += ex_Finished;
                            explosions.Add(ex);
                            ship = null;
                            livesLeft--;
                        }
                        if (!asteroidsToDelete.Contains(asteroids.IndexOf(a)))
                            asteroidsToDelete.Insert(0, asteroids.IndexOf(a));
                        // Create a "white hole" for repulsion...
                        blackHole = new Asteroid(new Vector(width/2, height/2),
                            new Vector(0, 0), 0, 0, Properties.Resources.blackhole);

                        shipSpawnTmr.Elapsed += shipSpawnTmr_Elapsed;
                        state = GameState.Resetting;
                    }
                }

                foreach (int idx in enemiesToDelete)
                {
                    Image img = Properties.Resources.explosion3;
                    Explosion ex = new Explosion(new Vector(enemies[idx].Pos),
                    img, rand.Next(0, 360),
                    5, 5, 80, 80);
                    ex.Finished += ex_Finished;
                    explosions.Add(ex);

                    Score += 1000;
                    bonusScore += 1000;
                    ak += 5;
                    enemies[idx].shoot.Stop();
                    enemies.RemoveAt(idx);
                }

                foreach (int idx in missilesToDelete)
                {   // Remove missiles which had been detected to collide
                    if(missiles[idx].friendlyMissle)
                    {
                        shipMissles--;
                    }
                    missiles.RemoveAt(idx);
                }

                
                
                missilesToDelete.Clear();
                enemiesToDelete.Clear();
            }

            foreach (int idx in asteroidsToDelete)
            {   // Remove asteroids which had been detected to collide
                Image img;
                int numAcross, numDown, spriteWidth, spriteHeight;
                Asteroid a = asteroids[idx];
                if (a.Type == 0)
                {
                    img = Properties.Resources.Explosion;
                    numAcross = 9; numDown = 9; spriteWidth = 100; spriteHeight = 100;
                }
                else
                {
                    img = Properties.Resources.explosion2;
                    numAcross = 3; numDown = 4; spriteWidth = 256; spriteHeight = 128; 
                }
                Explosion ex = new Explosion(new Vector(asteroids[idx].Pos),
                    img, rand.Next(0, 360),
                    numAcross, numDown, spriteWidth, spriteHeight);
                ex.Finished += ex_Finished;
                explosions.Add(ex);
                // Update the score...
                Score += 50 * (4-asteroids[idx].Size);
                bonusScore += 50 * (4 - asteroids[idx].Size);

                //Split the asteroids up

                if(asteroids[idx].Size == 3)
                {
                    if (asteroids[idx].Type == 0)
                    {
                        Asteroid A1 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                            asteroids[idx].Angle + 45, rand.Next(-10, 10), Properties.Resources.AsteroidMed);
                        A1.Size = asteroids[idx].Size - 1;
                        Asteroid A2 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                            asteroids[idx].Angle - 45, rand.Next(-10, 10), Properties.Resources.AsteroidMed);
                        A2.Size = asteroids[idx].Size - 1;

                        asteroids.Add(A1);
                        asteroids.Add(A2);
                    }
                    else
                    {
                        Asteroid A1 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                           asteroids[idx].Angle + 45, rand.Next(-10, 10), Properties.Resources.Asteroid2Med);
                        A1.Size = asteroids[idx].Size - 1;
                        Asteroid A2 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                            asteroids[idx].Angle - 45, rand.Next(-10, 10), Properties.Resources.Asteroid2Med);
                        A2.Size = asteroids[idx].Size - 1;

                        asteroids.Add(A1);
                        asteroids.Add(A2);
                    }                    
                }
                else if(asteroids[idx].Size == 2)
                {
                    if (asteroids[idx].Type == 0)
                    {
                        Asteroid A1 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                            asteroids[idx].Angle + 45, rand.Next(-10, 10), Properties.Resources.AsteroidSmall);
                        A1.Size = asteroids[idx].Size - 1;
                        Asteroid A2 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                            asteroids[idx].Angle - 45, rand.Next(-10, 10), Properties.Resources.AsteroidSmall);
                        A2.Size = asteroids[idx].Size - 1;

                        asteroids.Add(A1);
                        asteroids.Add(A2);
                    }
                    else
                    {
                        Asteroid A1 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                           asteroids[idx].Angle + 45, rand.Next(-10, 10), Properties.Resources.Asteroid2Small);
                        A1.Size = asteroids[idx].Size - 1;
                        Asteroid A2 = new Asteroid(new Vector(asteroids[idx].Pos), new Vector(rand.Next(-10, 10), rand.Next(-10, 10)),
                            asteroids[idx].Angle - 45, rand.Next(-10, 10), Properties.Resources.Asteroid2Small);
                        A2.Size = asteroids[idx].Size - 1;

                        asteroids.Add(A1);
                        asteroids.Add(A2);
                    }
                }

                asteroids.RemoveAt(idx);
                ak++;

            }
            if (ak >= 20 && ak <40)
            {
                numMissiles = 20;
            }
            else if(ak>=40)
            {
                numMissiles = 30;
            }

            

        }

        void shipSpawnTmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            shipSpawnTmr.Stop();
            shipSpawnTmr.Elapsed -= shipSpawnTmr_Elapsed;
            ship.Shielded = false;
            ship.Image = Properties.Resources.ship;
            blackHole = null;
            state = GameState.Running;
        }

        void ex_Finished(Explosion ex)
        {
            explosionsToDelete.Insert(0, explosions.IndexOf(ex));
        }

        protected void KillExplosionsWhenTheyreDone()
        {
            foreach (int idx in explosionsToDelete)
            {
                explosions.RemoveAt(idx);
            }
            explosionsToDelete.Clear();

            foreach(int idx in bombexpToDelete)
            {
                b_explosions.RemoveAt(idx);
            }

            bombexpToDelete.Clear();
        }

        protected void Wrap(Vector pos)
        {
            if (pos.X > width)
            {
                pos.X = 0;
            }
            else if (pos.X < 0)
            {
                pos.X = width;
            }
            else if (pos.Y > height)
            {
                pos.Y = 0;
            }
            else if (pos.Y < 0)
            {
                pos.Y = height;
            }
        }

        public void Brake()
        {
            if(ship!=null)
            {
                ship.Brake();
            }
            
        }

    }
}
