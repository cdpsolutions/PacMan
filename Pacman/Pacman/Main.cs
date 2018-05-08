using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pacman
{
    public partial class Main : Form
    {
        // TODO: Clean code, LoseLife
        Graphics paper;
        SolidBrush color = new SolidBrush(Color.Black);
        int blokSize = 25;
        int score = 0;
        int lives = 3;
        int started = 0;
        int level = 1;
        Random rndAngle;
        Splash splash;
        Muren muren;
        Pacman pacman;
        Monster pinkGhost;
        Monster aquaGhost;
        Monster orangeGhost;

        public Main()
        {
            InitializeComponent();
            paper = pctPlayField.CreateGraphics();
            muren = new Muren();
            rndAngle = new Random();
            splash = new Splash();
            #region initialize pacman & ghosts
            //pacman
            pacman = new Pacman();
            pacman.X = 10;
            pacman.Y = 12;
            pacman.Angle = 0;
            pacman.Wall = 0;
            pacman.PrevX = 10;
            pacman.PrevY = 12;
            pacman.Sprites = pacman.pacmanUp;

            //pink ghost was red
            pinkGhost = new Monster();
            pinkGhost.X = 10;
            pinkGhost.Y = 10;
            pinkGhost.Angle = 3;
            pinkGhost.Wall = 0;
            pinkGhost.PrevX = 10;
            pinkGhost.PrevY = 10;
            pinkGhost.PrevAngle = 0;
            pinkGhost.Sprites = Image.FromFile("../../images/ghostPink.png");

            //aqua ghost was green
            aquaGhost = new Monster();
            aquaGhost.X = 11;
            aquaGhost.Y = 10;
            aquaGhost.Angle = 3;
            aquaGhost.Wall = 0;
            aquaGhost.PrevX = 11;
            aquaGhost.PrevY = 10;
            aquaGhost.PrevAngle = 0;
            aquaGhost.Sprites = Image.FromFile("../../images/ghostAqua.png");

            //orange ghost was yellow
            orangeGhost = new Monster();
            orangeGhost.X = 9;
            orangeGhost.Y = 10;
            orangeGhost.Angle = 3;
            orangeGhost.Wall = 0;
            orangeGhost.PrevX = 9;
            orangeGhost.PrevY = 10;
            orangeGhost.PrevAngle = 0;
            orangeGhost.Sprites = Image.FromFile("../../images/ghostOrange.png");
            #endregion


        }

        public void drawField(int x, int y, int size, SolidBrush kleur, int img)
        {
            if (img == 1)
            {
                Image dot = Image.FromFile("../../images/dot.jpg");
                paper.DrawImage(dot, x, y, size, size);
            }
            else
            {
                if (img == 2)
                {
                    Image wall = Image.FromFile("../../images/wall.png");
                    paper.DrawImage(wall, x, y, size, size);
                }
                else
                {
                    paper.FillRectangle(kleur, x, y, size, size);
                }


            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (started == 1)
            {
                if (e.KeyData == Keys.Left)
                {
                //disallow left key to be pressed if Pacman is exiting to the right
                    if (!(pacman.X <= 0))
                    {
                        pacman.Angle = 1;
                        pacman.Sprites = pacman.pacmanLeft;
                        tmrStep.Enabled = true;
                    }

                }
                //disallow right key to be pressed if Pacman is exiting to the left
                if (!(pacman.X >= 19))
                {
                    if (e.KeyData == Keys.Right)
                    {
                        pacman.Angle = 2;
                        pacman.Sprites = pacman.pacmanRight;
                        tmrStep.Enabled = true;

                    }
                }

                if (e.KeyData == Keys.Up)
                {
                    pacman.Angle = 3;
                    pacman.Sprites = pacman.pacmanUp;
                    tmrStep.Enabled = true;

                }
                if (e.KeyData == Keys.Down)
                {
                    pacman.Angle = 4;
                    pacman.Sprites = pacman.pacmanDown;
                    tmrStep.Enabled = true;

                }



                if (e.KeyData == Keys.P)
                {
                    if (tmrStep.Enabled == true)
                    {
                        tmrStep.Enabled = false;

                        lblPause.Visible = true;
                    }
                    else
                    {
                        lblPause.Visible = false;
                        tmrStep.Enabled = true;
                        drawGrid();

                    }
                }
            }
        }

        private void tmrStep_Tick(object sender, EventArgs e)
        {
            getOldSteps();

            //keep current angle
            pinkGhost.PrevAngle = pinkGhost.Angle;
            aquaGhost.PrevAngle = aquaGhost.Angle;
            orangeGhost.PrevAngle = orangeGhost.Angle;

            //check for walls
            wallDetect(pacman.Angle, pacman.X, pacman.Y, ref pacman.Wall);
            wallDetect(aquaGhost.Angle, aquaGhost.X, aquaGhost.Y, ref aquaGhost.Wall);
            wallDetect(pinkGhost.Angle, pinkGhost.X, pinkGhost.Y, ref pinkGhost.Wall);
            wallDetect(orangeGhost.Angle, orangeGhost.X, orangeGhost.Y, ref orangeGhost.Wall);

            // wall test pacman
            if (pacman.Wall == 1)
            {
                label1.Text = "Wall " + pacman.X + " " + pacman.Y + " " + pacman.Angle;

            }
            else
            {
                moveSprite(pacman.Angle, ref pacman.X, ref pacman.Y);
            }
            loseLife();
            //wall test ghosts
            while (pinkGhost.Wall == 1)
            {
                pinkGhost.Angle = rndAngle.Next(1, 5);
                wallDetect(pinkGhost.Angle, pinkGhost.X, pinkGhost.Y, ref pinkGhost.Wall);
            }

            while (aquaGhost.Wall == 1)
            {
                aquaGhost.Angle = rndAngle.Next(1, 5);
                wallDetect(aquaGhost.Angle, aquaGhost.X, aquaGhost.Y, ref aquaGhost.Wall);
            }

            while (orangeGhost.Wall == 1)
            {
                orangeGhost.Angle = rndAngle.Next(1, 5);
                wallDetect(orangeGhost.Angle, orangeGhost.X, orangeGhost.Y, ref orangeGhost.Wall);
            }

            moveSprite(pinkGhost.Angle, ref pinkGhost.X, ref pinkGhost.Y);
            moveSprite(aquaGhost.Angle, ref aquaGhost.X, ref aquaGhost.Y);
            moveSprite(orangeGhost.Angle, ref orangeGhost.X, ref orangeGhost.Y);

            eraseSteps(aquaGhost.PrevX, aquaGhost.PrevY);
            eraseSteps(pinkGhost.PrevX, pinkGhost.PrevY);
            eraseSteps(orangeGhost.PrevX, orangeGhost.PrevY);
            eraseSteps(pacman.PrevX, pacman.PrevY);

            drawSprites(pacman.X, pacman.Y, pinkGhost.X, pinkGhost.Y, aquaGhost.X, aquaGhost.Y, orangeGhost.X, orangeGhost.Y);
            loseLife();
            scoreCount();
            countDots();
        }
        private void moveSprite(int angle, ref int x, ref int y)
        {
            switch (angle)
            {
                case 1: //Left 
                    x -= 1;
                    break;
                case 2: //right
                    x += 1;
                    break;
                case 3: //up 
                    y -= 1;
                    break;
                case 4: //Down
                    y += 1;
                    break;
                default:
                    break;
            }

            if ((x == 19) && (y == 10))
            {
                x = 0;
            }
            else
            {
                if ((x == 0) && (y == 10))
                {
                    x = 19;
                }
            }
        }

        private void drawGrid()
        {
            int xPos = 0;
            int yPos = 0;
            int row = 0;
            int col = 0;
            for (int i = 0; i < muren.Grid.Length; i++)
            {
                if (i % 20 == 0 && i != 0) // is de rij langer dan 20, nieuwe rij... 
                                            //is the line longer than 20, new row...
                {
                    row += 1;
                    col = 0;
                }
                xPos = col * blokSize;
                yPos = row * blokSize;

                if (muren.Grid[row, col] == 0)
                {
                    color.Color = Color.White;
                    drawField(xPos, yPos, blokSize, color, 1);
                }
                else
                {
                    if (muren.Grid[row, col] == 1)
                    {
                        color.Color = Color.Blue;
                        drawField(xPos, yPos, blokSize, color, 2);
                    }
                    else
                    {
                        color.Color = Color.Black;
                        drawField(xPos, yPos, blokSize, color, 0);
                    }
                }
                col += 1;
            }
        }
        private void drawSprites(int pX, int pY, int grX, int grY, int ggX, int ggY, int gyX, int gyY)
        {
            pX = pX * blokSize;
            pY = pY * blokSize;
            grX = grX * blokSize;
            grY = grY * blokSize;
            ggX = ggX * blokSize;
            ggY = ggY * blokSize;
            gyX = gyX * blokSize;
            gyY = gyY * blokSize;
            // drawGrid();
            paper.DrawImage(pacman.Sprites, pX, pY, blokSize, blokSize);
            paper.DrawImage(pinkGhost.Sprites, grX, grY, blokSize, blokSize);
            paper.DrawImage(aquaGhost.Sprites, ggX, ggY, blokSize, blokSize);
            paper.DrawImage(orangeGhost.Sprites, gyX, gyY, blokSize, blokSize);
        }

        private void wallDetect(int angle, int x, int y, ref int wall) //simulatie maken van een toekomstige stap
            //simulate a future step
        {

            switch (angle)
            {
                case 1: //Left
                    x -= 1;
                    break;
                case 2: //right
                    x += 1;
                    break;
                case 3: //up
                    y -= 1;
                    break;
                case 4: //Down
                    y += 1;
                    break;
                default:
                    break;
            }

            if (muren.Grid[y, x] == 1)
            {
                wall = 1;
            }
            else
            {
                wall = 0;
            }
        }

        private void scoreCount() // Optellen en vervangen van de waarde in de grid ( score) 
//Add and replace the value in the grid(score)
        {
            if (muren.Grid[pacman.Y, pacman.X] == 0)
            {
                score += 5;
                lblScore.Text = Convert.ToString(score);
                muren.Grid[pacman.Y, pacman.X] = 2;
            }
        }

        static void Pause()
        {
            //When you lose a life, pause the game for a couple of seconds so the user realizes something has happened. 
            //The pause should suspend the movement of both pacman and the ghosts.
            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.Enabled = true;
            System.Threading.Thread.Sleep(2000);
            timer.Dispose();
        }

            private void loseLife() // checken of pacman op dezelfde positie als een geest is getekend, indien ja: leven kwijt
                                    //check whether pacman is signed in the same position as a mind, if yes, lost life
        {
            if ((pacman.X == pinkGhost.X) && (pacman.Y == pinkGhost.Y) || (pacman.X == aquaGhost.X) && (pacman.Y == aquaGhost.Y)
                || (pacman.X == orangeGhost.X) && (pacman.Y == orangeGhost.Y))
            {
                lives -= 1;
                Pause();
               



            }
            if (lives == 0)
            {
                level = 1;
                tmrStep.Enabled = false;
                MessageBox.Show("Jou score is: " + score, "Game Over...");
                resetProgress();
                paper.Clear(Color.Black);
                resetField();
            }
            lblLivesCount.Text = Convert.ToString(lives);
        }


        private void countDots() // telt de resterende dots voor het spel is uitgespeeld
                                 //counts the remaining dots before the game is played
        {
            int countDot = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {

                    if (muren.Grid[i, j] == 0)
                    {
                        countDot += 1;
                    }
                }
            }
            if (countDot == 0)
            {
                tmrStep.Enabled = false;
                level += 1;
                resetField();
            }
        }

        private void resetProgress()
        {
            lives = 3;
            score = 0;
            lblLivesCount.Text = Convert.ToString(lives);
            lblScore.Text = Convert.ToString(score);
        }

        private void resetField()
        {
            setGrid();
            pacman.Angle = 1;
            pacman.X = 10;
            pacman.Y = 12;
            pinkGhost.X = 10;
            pinkGhost.Y = 10;
            pinkGhost.Angle = 3;
            aquaGhost.Angle = 3;
            orangeGhost.Angle = 3;
            aquaGhost.X = 11;
            aquaGhost.Y = 10;
            orangeGhost.X = 9;
            orangeGhost.Y = 10;
            drawGrid();
            pacman.Sprites = pacman.pacmanLeft;

            drawSprites(pacman.X, pacman.Y, pinkGhost.X, pinkGhost.Y, aquaGhost.X, aquaGhost.Y, orangeGhost.X, orangeGhost.Y);
        }

        private void getOldSteps() // vervangen van oude met nieuwe positie 
            //Replace old with new position
        {
            pacman.PrevX = pacman.X;
            pacman.PrevY = pacman.Y;
            aquaGhost.PrevX = aquaGhost.X;
            aquaGhost.PrevY = aquaGhost.Y;
            pinkGhost.PrevX = pinkGhost.X;
            pinkGhost.PrevY = pinkGhost.Y;
            orangeGhost.PrevX = orangeGhost.X;
            orangeGhost.PrevY = orangeGhost.Y;
        }

        private void eraseSteps(int x, int y)
        {
            int z = x * blokSize;
            int q = y * blokSize;
            switch (muren.Grid[y, x])
            {
                case 0:
                    Image dot = Image.FromFile("../../images/dot.jpg");
                    paper.DrawImage(dot, z, q, blokSize, blokSize);
                    break;
                case 1:
                    Image wall = Image.FromFile("../../images/wall.png");
                    paper.DrawImage(wall, z, q, blokSize, blokSize);
                    break;
                case 2:
                    color = new SolidBrush(Color.Black);
                    paper.FillRectangle(color, z, q, blokSize, blokSize);
                    break;
                default:
                    break;
            }
        } // vorig vak wordt terug getekend
          //Previous subject is signed back

        private void lblHelp_Click(object sender, EventArgs e)
        {
            Help frm = new Help();
            frm.ShowDialog();
        }  // Helpfunctie




        private void Main_Shown(object sender, EventArgs e) // Splashscreen komt tevoorschijn, Main wordt gehide

            //Splashscreen appears, Main is being hid
        {
            this.Hide();
            tmrSplash.Start();
            tmrSplash.Enabled = true;
            splash.Show();
        }

        private void tmrSplash_Tick_1(object sender, EventArgs e) // Splashscreen wordt gehide en Main komt tevoorschijn
            //Splashscreen is hiding and Main appears

        {
            splash.Close();
            this.Show();
            tmrSplash.Stop();
        }



        private void Main_Load(object sender, EventArgs e)
        {
            setGrid();
        }

        private void lblStart_Click_1(object sender, EventArgs e) // Uittekenen van de grid en objecten  //Shedding of the grid and objects
        {
            //setGrid();
            tmrStep.Enabled = false;
            // drawGrid();
            resetField();
            resetProgress();
            pacman.Sprites = pacman.pacmanLeft;
            drawSprites(pacman.X, pacman.Y, pinkGhost.X, pinkGhost.Y, aquaGhost.X, aquaGhost.Y, orangeGhost.X, orangeGhost.Y);
            started = 1;
        }
        private void setGrid()
        {
            if (level % 2 == 0)
            {
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    { muren.Grid[x, y] = muren.Grid2[x, y]; }
                }
            }
            else
            {
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    { muren.Grid[x, y] = muren.Grid1[x, y]; }
                }
            }

        }
    }
}
