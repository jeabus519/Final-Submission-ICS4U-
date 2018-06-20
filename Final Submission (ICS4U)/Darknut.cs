using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Final_Submission__ICS4U_
{
    public class Darknut
    {
        public Point loc;
        public int width, height, speed, life, stepTimer, stepCount, pathDist;
        public string direction;
        public bool step;
        public Rectangle hitbox;

        public Darknut(Point p, int _width, int _height, int _speed, int _life, string _direction)
        {
            loc.X = p.X;
            loc.Y = p.Y;
            width = _width;
            height = _height;
            speed = _speed;
            life = _life;
            direction = _direction;

            stepTimer = 0;
            step = false;
            hitbox = new Rectangle(loc.X, loc.Y, width, height);
        }

        public void HitboxUpdate()
        {
            hitbox.X = loc.X;
            hitbox.Y = loc.Y;
        }

        public void GenPath()
        {
            Random rnd = new Random();

            #region choose direction
            switch (direction)
            {
                case "left":
                    switch (rnd.Next(0, 3))
                    {
                        case 0:
                            direction = "right";
                            break;
                        case 1:
                            direction = "up";
                            break;
                        case 2:
                            direction = "down";
                            break;
                    }
                    break;
                case "right":
                    switch (rnd.Next(0, 3))
                    {
                        case 0:
                            direction = "left";
                            break;
                        case 1:
                            direction = "up";
                            break;
                        case 2:
                            direction = "down";
                            break;
                    }
                    break;
                case "up":
                    switch (rnd.Next(0, 3))
                    {
                        case 0:
                            direction = "left";
                            break;
                        case 1:
                            direction = "right";
                            break;
                        case 2:
                            direction = "down";
                            break;
                    }
                    break;
                case "down":
                    switch (rnd.Next(0, 3))
                    {
                        case 0:
                            direction = "left";
                            break;
                        case 1:
                            direction = "right";
                            break;
                        case 2:
                            direction = "up";
                            break;
                    }
                    break;
            }
            #endregion

            pathDist = rnd.Next(2, 8);
            stepCount = 0;
        }

        public void Move(List<Rectangle> recList)
        {
            #region 2-frame animation timing
            stepTimer++;
            if (stepTimer > 5)
            {
                step = true;
            }
            if (stepTimer >= 10)
            {
                step = false;
                stepTimer = 0;
            }
            #endregion

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
            stepCount = stepCount + speed;

            #region Check if movement was valid
            foreach (Rectangle r in recList)
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
                    GenPath(); //gens new path so that DK does not walk into wall for remaining # of steps.
                }
            }
            #endregion

            if(stepCount == pathDist * 48) //if previously gen'd path has been walked, gen new path
            {
                GenPath();
            }
        }
    }
}
