using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game_of_Life
{
   public class Game_of_Life
    {

        const int WindowHeight = 35;
        const int WindowWidth = 70;
        const int SizeOfMenuPanel = 5;
        const int BoardSize = WindowHeight - SizeOfMenuPanel;
        const int LifeProcessSpeed = 120;
        public Game_of_Life()
        {
            Console.OutputEncoding = Encoding.Unicode;

            

            SetWindowProperties();

            while (true)
            {
                Console.Clear();

                string menuPanel = StartMenuPanel();
                    Console.WriteLine(menuPanel);

                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.Clear();

                if (key.Key == ConsoleKey.O)
                { 
                    CreateOwnField();
                }
                if (key.Key == ConsoleKey.B)
                {
                    UseBuiltInFields();
                }
            }
        }
        void SetWindowProperties()
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.WindowHeight = Math.Min(WindowHeight, Console.LargestWindowHeight);
            Console.BufferHeight = WindowHeight;
            Console.WindowWidth = Math.Min(WindowWidth, Console.LargestWindowWidth);
            Console.BufferWidth = WindowWidth;
        }

        string StartMenuPanel()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Choose to create your own field or test the built-in fields");

            for (int row = 0; row < BoardSize - 1; row++)
            {
                stringBuilder.AppendLine(new String(' ', WindowWidth));
            }

            stringBuilder.AppendLine(new string('=', WindowWidth));

            stringBuilder.AppendLine("[O] Create own field");
            stringBuilder.AppendLine("[B] Test the build-in fields");

            return stringBuilder.ToString().TrimEnd();


        }

        void CreateOwnField()
        {
            GameOfLifeEditor gameOfLife = new GameOfLifeEditor(BoardSize, WindowWidth);

            Console.CursorVisible = true;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(gameOfLife.Draw(BoardSize, WindowWidth));

            while (true)
            {
                Console.CursorVisible = true;

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                while (key.Key != ConsoleKey.Backspace)
                {
                    string generation = gameOfLife.PlayerMove(key, BoardSize, WindowWidth);

                    if (generation == "")
                    {
                        key = Console.ReadKey(true);
                        continue;
                    }
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(generation);
                    key = Console.ReadKey(true);
                }
                bool keyIsBackspace = key.Key == ConsoleKey.Backspace ? true : false;
                bool gameIsStopped = PlayGame(gameOfLife, keyIsBackspace);

                if (gameIsStopped)
                {
                   break;
                }
            }
            
        }
        bool PlayGame (GameOfLifeBase gameOfLife, bool keyIsBackspace) 
        {
            while (keyIsBackspace)
            {
                ProcessLife(gameOfLife);

                if (Console.KeyAvailable == true)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        return true;
                    }
                    if (key.Key ==  ConsoleKey.Backspace)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        void ProcessLife(GameOfLifeBase gameOfLife)
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(gameOfLife.Draw(BoardSize, WindowWidth));

            gameOfLife.SpawnNextGeneration();

            Thread.Sleep(LifeProcessSpeed);
        }

        void UseBuiltInFields()
        {
            GameOfLifeBuiltIn gameOfLife = new GameOfLifeBuiltIn(BoardSize, WindowWidth);

            Console.WriteLine(gameOfLife.Draw(BoardSize, WindowWidth));

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                if ((key.Key == ConsoleKey.Escape))
                {
                    break;
                }
                if (key.Key == ConsoleKey.F1)
                {
                    gameOfLife.GenerateRandomField();

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(gameOfLife.Draw(BoardSize, WindowWidth));
                    continue;
                }
                if (key.Key == ConsoleKey.F2)
                {
                    string fileName = "pulsar.txt";
                    GenerateField(gameOfLife, fileName);
                    continue;
                }
                if (key.Key == ConsoleKey.F3)
                {
                    string fileName = "gosper-glider-gun.txt";
                    GenerateField(gameOfLife, fileName);
                    continue;
                }
                if (key.Key == ConsoleKey.F4)
                {
                    string fileName = "living-forever.txt";
                    GenerateField(gameOfLife, fileName);
                    continue;
                }

                bool keyIsBackspace = key.Key == ConsoleKey.Backspace ? true : false;
                bool gameIsStopped = PlayGame(gameOfLife, keyIsBackspace);

                if (gameIsStopped)
                {
                    break;
                }
            }
        }

        void GenerateField(GameOfLifeBuiltIn gameOfLife, string fileName)
        {
            gameOfLife.GenerateField(fileName);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(gameOfLife.Draw(BoardSize, WindowWidth));
        }
    }

}
