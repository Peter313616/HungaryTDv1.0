using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace HungaryTDv1
{
    public class Tower
    {
        public Point Location;
        int towerType;
        Rectangle towerRect;
        Canvas cBackground;
        Canvas cObstacles;
        int[] positions;
        Point[] track;
        List<int> targets = new List<int>();
        int range;
        int bSpeed;
        int bPower;
        double shortestDistance = 0;
        double startPosition = 0;
        bool initialPlace = true;
        Point bCurrentLocation;
        Rectangle bullet;
        int counter;
        bool bulletDrawn;
        double xMove;
        double yMove;
        double NumbOfTransforms;
        double xDistance = 0;
        double yDistance = 0;
        public Tower(int tT, Canvas cBack, Canvas cObs, int[] p, Point[] t, Point l)
        {
            towerType = tT;
            towerRect = new Rectangle();
            cBackground = cBack;
            cObstacles = cObs;
            positions = p;
            track = t;
            Location = l;
            if (towerType == 0)//norm
            {
                range = 100;
                bSpeed = 20;


                BitmapImage bi = new BitmapImage(new Uri("normal.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                Canvas.SetTop(towerRect, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(towerRect, Location.X - towerRect.Width / 2);
                cBackground.Children.Add(towerRect);

                Rectangle tempTower = new Rectangle();
                tempTower.Height = towerRect.Height;
                tempTower.Width = towerRect.Width;
                tempTower.Fill = Brushes.Transparent;
                Canvas.SetTop(tempTower, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(tempTower, Location.X - towerRect.Width / 2);
                cObstacles.Children.Add(tempTower);
                //MessageBox.Show(Canvas.GetTop(towerRect).ToString());
            }
            else if (towerType == 1)//popo
            {
                range = 150;
                bSpeed = 20;

                BitmapImage bi = new BitmapImage(new Uri("police.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                Canvas.SetTop(towerRect, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(towerRect, Location.X - towerRect.Width / 2);
                cBackground.Children.Add(towerRect);

                Rectangle tempTower = new Rectangle();
                tempTower.Height = towerRect.Height;
                tempTower.Width = towerRect.Width;
                tempTower.Fill = Brushes.Transparent;
                Canvas.SetTop(tempTower, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(tempTower, Location.X - towerRect.Width / 2);
                cObstacles.Children.Add(tempTower);
            }
            else if (towerType == 2)//fam
            {
                range = 50;
                bSpeed = 20;

                BitmapImage bi = new BitmapImage(new Uri("family.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                Canvas.SetTop(towerRect, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(towerRect, Location.X - towerRect.Width / 2);
                cBackground.Children.Add(towerRect);

                Rectangle tempTower = new Rectangle();
                tempTower.Height = towerRect.Height;
                tempTower.Width = towerRect.Width;
                tempTower.Fill = Brushes.Transparent;
                Canvas.SetTop(tempTower, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(tempTower, Location.X - towerRect.Width / 2);
                cObstacles.Children.Add(tempTower);
            }
            else//thicc
            {
                range = 50;
                bSpeed = 20;

                BitmapImage bi = new BitmapImage(new Uri("tank.png", UriKind.Relative));
                towerRect.Fill = new ImageBrush(bi);
                Canvas.SetTop(towerRect, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(towerRect, Location.X - towerRect.Width / 2);
                cBackground.Children.Add(towerRect);

                Rectangle tempTower = new Rectangle();
                tempTower.Height = towerRect.Height;
                tempTower.Width = towerRect.Width;
                tempTower.Fill = Brushes.Transparent;
                Canvas.SetTop(tempTower, Location.Y - towerRect.Height / 2);
                Canvas.SetLeft(tempTower, Location.X - towerRect.Width / 2);
                cObstacles.Children.Add(tempTower);
            }
        }

        public bool CheckTower()
        {
            Canvas.SetTop(towerRect, Mouse.GetPosition(cBackground).Y - towerRect.Height / 2);
            Canvas.SetLeft(towerRect, Mouse.GetPosition(cBackground).X - towerRect.Width / 2);
            if (towerType < 2)
            {
                towerRect.Height = 35;
                towerRect.Width = 35;
            }
            else if (towerType == 2)
            {
                towerRect.Height = 45;
                towerRect.Width = 70;
            }
            else
            {
                towerRect.Height = 70;
                towerRect.Width = 70;
            }
            bool valid = true;
            double x = Mouse.GetPosition(cBackground).X;
            double y = Mouse.GetPosition(cBackground).Y;
            bool check1 = cObstacles.InputHitTest(new Point(x + this.towerRect.Width / 2 - 5, y + this.towerRect.Height / 2 - 5)) == null;
            bool check2 = cObstacles.InputHitTest(new Point(x - this.towerRect.Width / 2 + 5, y + this.towerRect.Height / 2 - 5)) == null;
            bool check3 = cObstacles.InputHitTest(new Point(x + this.towerRect.Width / 2 - 5, y - this.towerRect.Height / 2 + 5)) == null;
            bool check4 = cObstacles.InputHitTest(new Point(x - this.towerRect.Width / 2 + 5, y - this.towerRect.Height / 2 + 5)) == null;
            bool check5 = cObstacles.InputHitTest(new Point(x, y)) == null;
            if (check1 && check2 && check3 && check4 && check5)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
            return valid;
        }

        public void Shoot()
        {
            if (initialPlace)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    xDistance = track[i].X - Location.X;
                    yDistance = Location.Y - track[i].Y;

                    double TotalDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));

                    if (shortestDistance > TotalDistance || shortestDistance == 0)
                    {
                        shortestDistance = TotalDistance;
                        startPosition = i;
                    }
                }

                for (int i = (int)startPosition + range; i >= startPosition - range; i--)
                {
                    targets.Add(i);
                }
                initialPlace = false;
            }

            Point frontEnemy = new Point(0, 0);

            for (int i = 0; i < targets.Count; i++)
            {
                if (positions[targets[i]] != -1 && bulletDrawn == false)
                {
                    frontEnemy = track[targets[i]];
                    i = targets.Count;
                }
            }
            
            if (bulletDrawn == false && frontEnemy != new Point(0,0))
            {
                /*for (int i = 0; i < targets.Count; i++)
                {
                    if (positions[targets[i]] != -1 && b.bulletDrawn == false)
                    {
                        b.DrawBullet(track[targets[i]]);
                        frontEnemy = track[targets[i]];
                        i = targets.Count;
                    }
                }*/

                xDistance = 0;
                yDistance = 0;

                xDistance = frontEnemy.X - Location.X;
                yDistance = Location.Y - frontEnemy.Y;

                double TotalDistance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                NumbOfTransforms = Math.Ceiling(TotalDistance / bSpeed);
                xMove = xDistance / NumbOfTransforms;
                yMove = yDistance / NumbOfTransforms;

                double temp = Math.Atan(xDistance / yDistance);
                double angle = temp * 180 / Math.PI;

                bullet = new Rectangle();
                bullet.Height = 20;
                bullet.Width = 10;
                BitmapImage bi = new BitmapImage(new Uri("fork.png", UriKind.Relative));
                bullet.Fill = new ImageBrush(bi);
                cBackground.Children.Add(bullet);

                if (frontEnemy.Y > Location.Y)
                {
                    angle += 180;
                    RotateTransform rotate = new RotateTransform(angle);
                    bullet.RenderTransformOrigin = new Point(0.5, 0.5);
                    bullet.RenderTransform = rotate;
                }
                else
                {
                    RotateTransform rotate = new RotateTransform(angle);
                    bullet.RenderTransformOrigin = new Point(0.5, 0.5);
                    bullet.RenderTransform = rotate;
                }
                bulletDrawn = true;
                bCurrentLocation = Location;
            }
            else if (bulletDrawn)
            {
                bCurrentLocation.X = Location.X + (xMove * counter);
                bCurrentLocation.Y = Location.Y - (yMove * counter);
                Canvas.SetLeft(bullet, bCurrentLocation.X);
                Canvas.SetTop(bullet, bCurrentLocation.Y);
                counter++;
                if (bCurrentLocation == frontEnemy || counter == 20)
                {
                    cBackground.Children.Remove(bullet);
                    bulletDrawn = false;
                    counter = 1;
                }
            }
        }
    }
}
