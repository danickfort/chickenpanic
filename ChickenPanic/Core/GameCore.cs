﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows;
using System;
using ChickenPanic.Graphics;
using ChickenPanic.Graphics.Characters;

namespace ChickenPanic.Core
{
    public class GameCore
    {
        private Canvas worldCanvas;

        private Chicken test;

        private GamePhysics gamePhysics;

        private GameGenerator gameGenerator;

        public GameCore(Canvas canvas)
        {
            worldCanvas = canvas;

            InitializeCore();
        }

        private void InitializeCore()
        {
            worldCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(WorldCanvas_MouseLeftButtonDown);

            test = new Chicken(100, 100, 60, 60, 0, 0, 0.1);
            gamePhysics = new GamePhysics();
            gamePhysics.DynamicGraphicsList.Add(test);

            gameGenerator = new GameGenerator(ref worldCanvas, ref gamePhysics);

            worldCanvas.Children.Add(test.GetRepresentation());
        }

        private void WorldCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            test.DoFlap();
        }

        public void UpdateGame(int elapsedMilliseconds)
        {
            gamePhysics.Update(elapsedMilliseconds);
            if (gamePhysics.checkColision(test))
            {
                // TODO : stopper le jeu
                Application.Current.Terminate();
            }
            gameGenerator.UpdateObstacles(elapsedMilliseconds);
        }

        public Canvas WorldCanvas
        {
            get { return worldCanvas; }
        }
    }
}
