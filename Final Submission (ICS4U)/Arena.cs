/// created by : Michael Peterman
/// date       : June 20, 2018
/// description: A simple game which is created as if it were a room in the NES game The Legend of Zelda.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Final_Submission__ICS4U_
{
    public partial class Arena : UserControl
    {
        Hero link;
        Rectangle sword;
        bool down, up, left, right; //player input

        List<Darknut> dkList;

        //sets rectangles around illegal areas
        Rectangle leftObstacle = new Rectangle(168, 168, 96, 144);
        Rectangle rightObstacle = new Rectangle(456, 168, 96, 144);
        Rectangle nWall = new Rectangle(0, 40, 720, 31);
        Rectangle eWall = new Rectangle(648, 0, 31, 480);
        Rectangle sWall = new Rectangle(0, 408, 720, 31);
        Rectangle wWall = new Rectangle(40, 0, 31, 480);
        List<Rectangle> recList;

        //for ending animation
        int opacityVal;
        bool soundPlayed, gameOver;
        SoundPlayer win = new SoundPlayer(Properties.Resources._06_triforce);
        SoundPlayer lose = new SoundPlayer(Properties.Resources._07_game_over);

        //background music
        SoundPlayer labyrinth = new SoundPlayer(Properties.Resources._04_labyrinth);

        public Arena()
        {
            InitializeComponent();
            OnStart();
        }

        public void OnStart()
        {
            link = new Hero(new Point(336, 216), 48, 48, 5, 3, "down", false);
            Darknut dk1 = new Darknut(new Point(72, 72), 48, 48, 3, 1, "right");
            Darknut dk2 = new Darknut(new Point(599, 72), 48, 48, 3, 1, "left");
            Darknut dk3 = new Darknut(new Point(72, 359), 48, 48, 3, 1, "right");
            Darknut dk4 = new Darknut(new Point(599, 359), 48, 48, 3, 1, "left");

            recList = new List<Rectangle>(new Rectangle[] { leftObstacle, rightObstacle, nWall, eWall, sWall, wWall });
            dkList = new List<Darknut>(new Darknut[] { dk1, dk2, dk3, dk4 });
            foreach(Darknut dk in dkList)
            {
                dk.GenPath();
            }
            labyrinth.PlayLooping();
            opacityVal = 0;
            soundPlayed = false;
            gameOver = false;
            label2.Text = "escape to quit";
        }

        public void CheckMoving() //if attacking, do nothing; else if anykeydown, moving = true;
        {
            if (link.attacking)
            {
                return;
            }
            if (up || down || left || right)
            {
                link.moving = true;
            }
            else
            {
                link.moving = false;
            }
        }

        public void CheckAttacking() //if attacking: if duration < 10 ticks, wait; if duration = 10 ticks, attacking = false;
        {
            if (link.attacking)
            {
                if(link.atkTimer > 0)
                {
                    foreach(Darknut dk in dkList.AsEnumerable().Reverse()) //check if darknuts got killed
                    {
                        if (sword.IntersectsWith(dk.hitbox))
                        {
                            dkList.Remove(dk);
                        }
                    }
                    --link.atkTimer;
                }
                else if (link.atkTimer == 0)
                {
                    --link.atkTimer;
                    link.attacking = false;
                    sword.Size = new Size(0, 0);
                }
            }
        }

        private void Arena_Paint(object sender, PaintEventArgs e)
        {
            if (link.life <= 0 || dkList.Count() <= 0)
            {
                gameOver = true;
                #region ending animation
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                drawBrush.Color = Color.FromArgb(opacityVal, 0, 0, 0);
                e.Graphics.FillRectangle(drawBrush, 0, 0, 720, 480);

                if (opacityVal <= 255)
                {
                    opacityVal = opacityVal + 2;
                }
                if (opacityVal > 255)
                {
                    opacityVal = 255;
                }

                if (opacityVal == 255)
                {
                    Font drawFont = new Font("The Legend of Zelda NES", 32, FontStyle.Bold);
                    //SolidBrush drawBrush = new SolidBrush(Color.White);
                    drawBrush.Color = Color.White;
                    if (link.life > 0)
                    {
                        e.Graphics.DrawImage(Properties.Resources.celebration, link.loc.X + 5, link.loc.Y - 4, 39, 48);
                        e.Graphics.DrawString("You win!", drawFont, drawBrush, 50, 250);
                        if (!soundPlayed)
                        {
                            win.Play();
                            soundPlayed = true;
                            label2.Text += ", N to restart";
                        }
                    }
                    else
                    {
                        e.Graphics.DrawString("You lose!", drawFont, drawBrush, 50, 250);
                        if (!soundPlayed)
                        {
                            lose.Play();
                            soundPlayed = true;
                            label2.Text += ", N to restart";
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region player life display
                switch (link.life)
                {
                    case 3:
                        e.Graphics.DrawImage(Properties.Resources.full_heart, 741, 120, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.full_heart, 741, 216, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.full_heart, 741, 312, 42, 48);
                        break;
                    case 2:
                        e.Graphics.DrawImage(Properties.Resources.full_heart, 741, 120, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.full_heart, 741, 216, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.empty_heart, 741, 312, 42, 48);
                        break;
                    case 1:
                        e.Graphics.DrawImage(Properties.Resources.full_heart, 741, 120, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.empty_heart, 741, 216, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.empty_heart, 741, 312, 42, 48);
                        break;
                    default:
                        e.Graphics.DrawImage(Properties.Resources.empty_heart, 741, 120, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.empty_heart, 741, 216, 42, 48);
                        e.Graphics.DrawImage(Properties.Resources.empty_heart, 741, 312, 42, 48);
                        break;
                }
                #endregion

                #region Hero
                if (!link.attacking)
                {
                    string value;
                    if (link.step) { value = "2"; } else { value = "1"; }
                    switch (link.direction)
                    {
                        case "left":
                            var img = (Image)Properties.Resources.ResourceManager.GetObject("hl" + value);
                            e.Graphics.DrawImage(img, link.loc.X, link.loc.Y, 48, 48);
                            break;
                        case "right":
                            img = (Image)Properties.Resources.ResourceManager.GetObject("hr" + value);
                            e.Graphics.DrawImage(img, link.loc.X, link.loc.Y, 48, 48);
                            break;
                        case "up":
                            img = (Image)Properties.Resources.ResourceManager.GetObject("hu" + value);
                            e.Graphics.DrawImage(img, link.loc.X, link.loc.Y, 36, 48);
                            break;
                        case "down":
                            img = (Image)Properties.Resources.ResourceManager.GetObject("hd" + value);
                            e.Graphics.DrawImage(img, link.loc.X, link.loc.Y, 44, 48);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (link.atkDir)
                    {
                        case "left":
                            e.Graphics.DrawImage(Properties.Resources.hla, link.loc.X - 47, link.loc.Y - 8, 86, 48);
                            break;
                        case "right":
                            e.Graphics.DrawImage(Properties.Resources.hra, link.loc.X + 2, link.loc.Y - 8, 86, 48);
                            break;
                        case "up":
                            e.Graphics.DrawImage(Properties.Resources.hua, link.loc.X - 4, link.loc.Y - 40, 48, 84);
                            break;
                        case "down":
                            e.Graphics.DrawImage(Properties.Resources.hda, link.loc.X - 4, link.loc.Y - 4, 48, 81);
                            break;
                    }
                }
                #endregion

                #region Darknuts
                foreach (Darknut dk in dkList)
                {
                    string value;
                    if (dk.step) { value = "2"; } else { value = "1"; }
                    switch (dk.direction)
                    {
                        case "left":
                            var img = (Image)Properties.Resources.ResourceManager.GetObject("dkl" + value);
                            e.Graphics.DrawImage(img, dk.loc.X, dk.loc.Y, 48, 48);
                            break;
                        case "right":
                            img = (Image)Properties.Resources.ResourceManager.GetObject("dkr" + value);
                            e.Graphics.DrawImage(img, dk.loc.X, dk.loc.Y, 48, 48);
                            break;
                        case "up":
                            img = (Image)Properties.Resources.ResourceManager.GetObject("dku" + value);
                            e.Graphics.DrawImage(img, dk.loc.X, dk.loc.Y, 36, 48);
                            break;
                        case "down":
                            img = (Image)Properties.Resources.ResourceManager.GetObject("dkd" + value);
                            e.Graphics.DrawImage(img, dk.loc.X, dk.loc.Y, 44, 48);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
        }

        private void Arena_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    left = true;
                    link.direction = "left";
                    break;
                case Keys.Down:
                    down = true;
                    link.direction = "down";
                    break;
                case Keys.Right:
                    right = true;
                    link.direction = "right";
                    break;
                case Keys.Up:
                    up = true;
                    link.direction = "up";
                    break;
                case Keys.Space:
                    sword = link.Attack();
                    break;
                case Keys.N:
                    if (gameOver)
                    {
                        OnStart();
                    }
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        private void Arena_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    left = false;
                    break;
                case Keys.Down:
                    down = false;
                    break;
                case Keys.Right:
                    right = false;
                    break;
                case Keys.Up:
                    up = false;
                    break;
                default:
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (link.life > 0 && dkList.Count > 0) //if game has not been won/lost
            {
                if (link.attacking)
                {
                    CheckAttacking();
                }
                CheckMoving();
                link.Move(recList);

                if (link.invTimer > 0)
                {
                    link.invTimer--;
                }

                foreach (Darknut dk in dkList)
                {
                    dk.Move(recList);
                    if (dk.hitbox.IntersectsWith(link.hitbox) && link.invTimer <= 0)
                    {
                        link.life--;
                        link.invTimer = 35;
                    }

                    #region stops overlapped movement
                    //creates a list of all darknuts that are not dk
                    List<Darknut> tempList = new List<Darknut>(dkList);
                    tempList.Remove(dk);

                    foreach (Darknut subject in tempList)
                    {
                        int area = dk.width * dk.height;
                        Rectangle intersection = new Rectangle();
                        intersection = Rectangle.Intersect(dk.hitbox, subject.hitbox);
                        int intArea = intersection.Width * intersection.Height;

                        if (dk.direction == subject.direction && intArea >= area / 2) //if both darknuts are moving in the same direction and their hitboxes share 50% of their pixels
                        {
                            subject.GenPath();
                        }
                    }
                    #endregion

                    dk.speed = 7 - dkList.Count();
                } 
            }
            Refresh();
        }
    }
}
