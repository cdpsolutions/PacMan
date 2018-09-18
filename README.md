# PacMan
/*CIS269

Debug / Edit a broken PacMan game

1) Fixed the expired certificate problem which kept the program from running

2) Translate relevant comments from Dutch to English

3)	When you lose a life, pause the game for a couple of seconds so the user realizes something has happened. The pause should suspend the movement of both pacman and the ghosts.*/

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
        {
            if ((pacman.X == redGhost.X) && (pacman.Y == redGhost.Y) || (pacman.X == greenGhost.X) && (pacman.Y == greenGhost.Y)
                || (pacman.X == yellowGhost.X) && (pacman.Y == yellowGhost.Y))
            {
                lives -= 1;
                Pause();

//4)	When you go through the left/right portal and push the opposite arrow key while travelling through, the game crashed.

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
                
//Week 6 Team Project Christine Cullen 5/13/2018
//The assignment was to make at least two improvements to the PacMan Game.  The changes made were improving the quality/image resolution //of the ghosts, changing the backgrounds and adding levels to the game, and adding custom sounds based on the classic game.

namespace Pacman
{
    public partial class Main : Form
    {
        // TODO: Clean code, LoseLife
        
        //for this project changed the colors of the ghosts from green, blue and yellow
        Monster pinkGhost;
        Monster aquaGhost;
        Monster orangeGhost;
        //for this project added sound when Pacman dies and when the game plays
        // This SoundPlayer plays a sound whenever PacMan dies but comes back to life.
        //**Special Thanks to HB Radke for creating all the sounds/music especially for this game**
        System.Media.SoundPlayer dieSoundPlayer = new System.Media.SoundPlayer(@"../../sounds/pacdies.wav");
        // This SoundPlayer plays a sound whenever PacMan dies at the end of the game.
        System.Media.SoundPlayer endSoundPlayer = new System.Media.SoundPlayer(@"../../sounds/death.wav");
        // This SoundPlayer plays music when the start button is pressed.
        System.Media.SoundPlayer backgroundSoundPlayer = new System.Media.SoundPlayer(@"../../sounds/the wall 8bit.wav");

        public Main()
        {

            //pink ghost was red our changes include a new image
            pinkGhost = new Monster();
            pinkGhost.X = 10;
            pinkGhost.Y = 10;
            pinkGhost.Angle = 3;
            pinkGhost.Wall = 0;
            pinkGhost.PrevX = 10;
            pinkGhost.PrevY = 10;
            pinkGhost.PrevAngle = 0;
            pinkGhost.Sprites = Image.FromFile("../../images/ghostPink.png");

            //aqua ghost was green our changes include a new image
            aquaGhost = new Monster();
            aquaGhost.X = 11;
            aquaGhost.Y = 10;
            aquaGhost.Angle = 3;
            aquaGhost.Wall = 0;
            aquaGhost.PrevX = 11;
            aquaGhost.PrevY = 10;
            aquaGhost.PrevAngle = 0;
            aquaGhost.Sprites = Image.FromFile("../../images/ghostAqua.png");

            //orange ghost was yellow our changes include a new image
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
                {// made changes to the wall image by level and added a new level to the game
                    if (level == 1)
                    {
                        Image wall = Image.FromFile("../../images/greece.png");//new image
                        paper.DrawImage(wall, x, y, size, size);
                    }
                    else if (level == 2)
                    {
                        Image wall = Image.FromFile("../../images/bolivia.jpg");//new image
                        paper.DrawImage(wall, x, y, size, size);
                    }
                    else
                    {
                        Image wall = Image.FromFile("../../images/southafrica.png");//new image
                        paper.DrawImage(wall, x, y, size, size);
                    }
                }
                else
                {
                    paper.FillRectangle(kleur, x, y, size, size);
                }


            }
        }


        private void loseLife() // checken of pacman op dezelfde positie als een geest is getekend, indien ja: leven kwijt
                                //check whether pacman is signed in the same position as a mind, if yes, lost life
        {
            if ((pacman.X == pinkGhost.X) && (pacman.Y == pinkGhost.Y) || (pacman.X == aquaGhost.X) && (pacman.Y == aquaGhost.Y)
                || (pacman.X == orangeGhost.X) && (pacman.Y == orangeGhost.Y))
            {
                dieSoundPlayer.Play();//plays the sound effect when Pacman dies, but can come back to life
                lives -= 1;
                Pause();
                backgroundSoundPlayer.PlayLooping();//restarts the game music and loops it after it ends

            }
            if (lives == 0)
            {
                endSoundPlayer.Play();//plays the sound effect when Pacman dies at the end of the game
                level = 1;
                tmrStep.Enabled = false;
                MessageBox.Show("Your score is: " + score, "Game Over...");//changed the text to English
                Pause();
                resetProgress();
                paper.Clear(Color.Black);
                resetField();
                backgroundSoundPlayer.PlayLooping();//restarts the game music and loops it after it ends

            }
            lblLivesCount.Text = Convert.ToString(lives);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            setGrid();
            backgroundSoundPlayer.PlayLooping();//restarts the game music and loops it after it ends

        }

        //New grid for each level of the game
        private void setGrid()
        {

            if (level == 1)
            {
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    { muren.Grid[x, y] = muren.Grid1[x, y]; }
                }
            }
            if (level == 2)
            {
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    { muren.Grid[x, y] = muren.Grid2[x, y]; }
                }
            }
            if (level == 3)
            {
                for (int x = 0; x < 20; x++)
                {
                    for (int y = 0; y < 20; y++)
                    { muren.Grid[x, y] = muren.Grid3[x, y]; }
                }
                //restarts from the beginning
                level = 0;
            }
        }
    }
}


