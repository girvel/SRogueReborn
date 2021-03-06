﻿using SRogue.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRogue.Core.Modules
{
    public class Display
    {
        public enum Destination
        {
            Overlay,
            Screen
        }

        public readonly int Width = 79;
        public readonly int Height = 25;
        public readonly int FieldWidth = 59;
        public readonly int FieldHeight = 25;
        private char[,] OverlaySaver;
        private char[,] Overlay;
        private char[,] Screen;
        private string Buffer;
        private readonly char Fog = '▒';

        public Display()
        {
            Overlay = new char[Height, Width];
            Screen = new char[Height, Width];
        }

        public void SaveOverlay()
        {
            OverlaySaver = (char[,])Overlay.Clone();
        }

        public void LoadOverlay()
        {
            Overlay = (char[,])OverlaySaver.Clone();
        }


        public void ResetOverlay()
        {
            for (int x = 0; x < FieldWidth; x++)
            {
                for (int y = 0; y < FieldHeight; y++)
                {
                    Put(Fog, x, y, Destination.Overlay);
                }
            }
        }

        public void Put(char c, int x, int y, Destination destination)
        {
            switch (destination)
            {
                case Destination.Overlay:
                    Overlay[y, x] = c;
                    break;
                case Destination.Screen:
                    Screen[y, x] = c;
                    break;
                default:
                    break;
            }
        }

        public void ShowEndScreen()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Put(' ', x, y, Destination.Overlay);
                }
            }

            Put("YOU DIED", 36, 12, Destination.Overlay);

            Console.Out.Write(Render(false));
        }

        public void Put(string s, int x, int y, Destination destination)
        {
            for (int i = 0; i < s.Length; i++)
            {
                switch (destination)
                {
                    case Destination.Overlay:
                        Overlay[y, x + i] = s[i];
                        break;
                    case Destination.Screen:
                        Screen[y, x + i] = s[i];
                        break;
                    default:
                        break;
                }
            }
        }

        protected void MakeOverlay()
        {
            var ui = UiManager.Current.Render();

            for (int x = UI.MarginWidth; x < Width; x++)
            {
                for (int y = UI.MarginHeight; y < Height; y++)
                {
                    Put(ui[y, x - UI.MarginWidth], x, y, Destination.Overlay);
                }
            }

            for (int x = Math.Max(GameManager.Current.Player.X - 3, 0);
                x < Math.Min(GameManager.Current.Player.X + 3, FieldWidth); x++)
            {
                for (int y = Math.Max(GameManager.Current.Player.Y - 3, 0);
                y < Math.Min(GameManager.Current.Player.Y + 3, FieldHeight); y++)
                {
                    Put('\0', x, y, Destination.Overlay);
                }
            }

            if (GameManager.Current.InventoryOpened)
            {
                var inventory = UiManager.Current.RenderInventory();

                for (int x = 1; x < UI.InventoryWidth + 1; x++)
                {
                    for (int y = 1; y < UI.InventoryHeight + 1; y++)
                    {
                        Put(inventory[y - 1, x - 1], x, y, Destination.Overlay);
                    }
                }
            }
        }

        protected void MakeScreen()
        {
            foreach (var entity in GameManager.Current.Tiles)
            {
                Put(entity.Texture, entity.X, entity.Y, Destination.Screen);
            }

            foreach (var entity in GameManager.Current.Entities)
            {
                Put(entity.Texture, entity.X, entity.Y, Destination.Screen);
            }
        }
        
        public string Render(bool makeScreens = true)
        {
            var result = new StringBuilder();

            if (makeScreens)
            {
                MakeScreen();
                MakeOverlay();
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var valueToDisplay = (Overlay[y, x] != '\0') ? Overlay[y, x] : Screen[y, x];
                    result.Append((valueToDisplay == '\0') ? ' ' : valueToDisplay);
                }
            }

            return result.ToString();
        }

        public void Draw()
        {
            //Drawing new screen, replacing updated cells
            var newScreen = DisplayManager.Current.Render();
            var length = newScreen.Length;

            for (int i = 0; i < length; i++)
            {
                if (Buffer == null || newScreen[i] != Buffer[i])
                {
                    var y = i / Width;
                    var x = i - (y * Width);
                    Console.SetCursorPosition(x, y);
                    Console.Out.Write(newScreen[i]);
                }
            }
            //Clearing actions string
            for (int i = 0; i <= Width; i++)
            {
                Console.SetCursorPosition(i, Height);
                Console.Out.Write(" ");
            }

            Console.SetCursorPosition(0, Height);
            Console.Out.Write(UiManager.Current.MakeActionsLine());
            Console.SetCursorPosition(0, Height + 1);
            Buffer = newScreen;
        }
    }
}
