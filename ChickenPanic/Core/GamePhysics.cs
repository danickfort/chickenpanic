﻿using ChickenPanic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Diagnostics;
using ChickenPanic.Graphics.Surroundings;
namespace ChickenPanic.Core
{
    public class GamePhysics : IUpdatable
    {
        private const double MAX_X_SPEED = 5.0;
        private const double MAX_Y_SPEED = 5.0;

        private double xSpeedFactor;
        private double ySpeedfactor;

        private List<DynamicGraphic> dynamicGraphicsList;

        private Size resolution;

        public GamePhysics()
        {
            xSpeedFactor = 0.0;
            ySpeedfactor = 0.00981*2.5; // 9.81 m/s -> 0.00981 mm/s

            dynamicGraphicsList = new List<DynamicGraphic>();

            resolution = ScreenResolution();
        }

        private static Size ScreenResolution()
        {
            var content = Application.Current.Host.Content;

            return new Size(content.ActualWidth, content.ActualHeight);
        }

        public void Update(int elapsedMilliseconds)
        {
            foreach(DynamicGraphic dynamicGraphic in dynamicGraphicsList)
	        {
                dynamicGraphic.Update(elapsedMilliseconds);

                /* Update position */
                dynamicGraphic.X += dynamicGraphic.XSpeed * elapsedMilliseconds;
                dynamicGraphic.Y += dynamicGraphic.YSpeed * elapsedMilliseconds;

                /* Check collisions */
                if (dynamicGraphic.Y + dynamicGraphic.Height >= resolution.Width && dynamicGraphic.Weight != 0)
                {
                    dynamicGraphic.Y = resolution.Width - dynamicGraphic.Height;
                    dynamicGraphic.YSpeed = 0;
                }
                else if (dynamicGraphic.Y < 0 && dynamicGraphic.Weight != 0)
                {
                    dynamicGraphic.Y = 0;
                    dynamicGraphic.YSpeed = 0;
                }
                else
                {
                    dynamicGraphic.YSpeed += dynamicGraphic.Weight * ySpeedfactor * elapsedMilliseconds;
                }

                /* Update speed */
                dynamicGraphic.XSpeed += dynamicGraphic.Weight * xSpeedFactor * elapsedMilliseconds;

                /* Limit max speed */
                if (dynamicGraphic.XSpeed > MAX_X_SPEED)
                {
                    dynamicGraphic.XSpeed = MAX_X_SPEED;
                }

                if (dynamicGraphic.YSpeed > MAX_Y_SPEED)
                {
                    dynamicGraphic.YSpeed = MAX_Y_SPEED;
                }
	        }
        }

        public bool checkColision(DynamicGraphic obj)
        {
            foreach (DynamicGraphic dg in dynamicGraphicsList)
            {
                if (obj.X + obj.Width - 20 > dg.X && obj.X + 20 < dg.X + dg.Width
                    && obj.Y + obj.Height - 20 > dg.Y && obj.Y + 20 < dg.Y + dg.Height
                    && !obj.Equals(dg))
                {
                    return true;
                }
            }
            return false;
        }

        public double XSpeedFactor
        {
            get { return xSpeedFactor; }
            set { xSpeedFactor = value; }
        }

        public double YSpeedFactor
        {
            get { return ySpeedfactor; }
            set { ySpeedfactor = value; }
        }

        public List<DynamicGraphic> DynamicGraphicsList
        {
            get { return dynamicGraphicsList; }
            set { dynamicGraphicsList = value; }
        }
    }
}
