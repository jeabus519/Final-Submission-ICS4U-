using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Final_Submission__ICS4U_
{
    public class Hero
    {
        public Point loc;
        public int width, height, speed, life, stepTimer, atkTimer, invTimer;
        public string direction, atkDir;
        public bool moving, attacking, step;
        public Rectangle hitbox;

        public Hero(Point p, int _width, int _height, int _speed, int _life, string _direction, bool _moving)
        {
            loc.X = p.X;
            loc.Y = p.Y;
            width = _width;
            height = _height;
            speed = _speed;
            life = _life;
            direction = _direction;
            moving = _moving;

            stepTimer = 0;
            step = false;
            hitbox = new Rectangle(loc.X, loc.Y, width, height);
            attacking = false;
        }

        public void HitboxUpdate()
        {
            hitbox.X = loc.X;
            hitbox.Y = loc.Y;
        }

        public void Move(List<Rectangle> recList)
        {
            #region 2-frame animation timing
            stepTimer++;
            if(stepTimer > 5)
            {
                step = true;
            }
            if(stepTimer >= 10)
            {
                step = false;
                stepTimer = 0;
            }
            #endregion

            if (moving)
            {
                switch (direction)
                {
                    case "up":
                        loc.Y = loc.Y - speed;
                        break;
                    case "down":
                        loc.Y = loc.Y + speed;
                        break;
                    case "right":
                        loc.X = loc.X + speed;
                        break;
                    case "left":
                        loc.X = loc.X - speed;
                        break;
                }
                HitboxUpdate();
            }
            else
            {
                step = false;
                stepTimer = 0;
            }

            #region Check if movement was valid
            foreach(Rectangle r in recList)
            {
                if (hitbox.IntersectsWith(r))
                {
                    switch (direction)
                    {
                        case "up":
                            loc.Y = loc.Y + speed;
                            break;
                        case "down":
                            loc.Y = loc.Y - speed;
                            break;
                        case "right":
                            loc.X = loc.X - speed;
                            break;
                        case "left":
                            loc.X = loc.X + speed;
                            break;
                    }
                    HitboxUpdate();
                }
            }
            #endregion
        }

        public Rectangle Attack()
        {
            Rectangle sword = new Rectangle();
            moving = false;
            attacking = true;
            atkDir = direction;

            switch (atkDir)
            {
                case "up":
                    sword = new Rectangle(loc.X + 12, loc.Y - 40, 8, 32);
                    break;
                case "down":
                    sword = new Rectangle(loc.X + 18, loc.Y + 45, 8, 32);
                    break;
                case "right":
                    sword = new Rectangle(loc.X + 54, loc.Y + 15, 34, 9);
                    break;
                case "left":
                    sword = new Rectangle(loc.X - 47, loc.Y + 15, 34, 9);
                    break;
            }
            atkTimer = 10;
            return sword;
        }
    }
}
