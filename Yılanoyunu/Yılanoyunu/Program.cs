using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        // Yönler için sabitler
        const int Up = 0;
        const int Down = 1;
        const int Left = 2;
        const int Right = 3;

        // Oyun alanı boyutu
        const int Width = 40;
        const int Height = 20;

        static List<(int, int)> snake = new List<(int, int)>();
        static (int, int) food;
        static int direction = Right;
        static bool gameRunning = true;

        static void Main()
        {
            Console.CursorVisible = false;
            InitializeGame();

            while (gameRunning)
            {
                DrawBoard();
                Input();
                Logic();
                Thread.Sleep(100); // Yılanın hareket hızını ayarlamak için
            }

            Console.Clear();
            Console.WriteLine("Oyun Bitti! Skorunuz: " + (snake.Count - 1));
        }

        static void InitializeGame()
        {
            // Yılanın başlangıç konumu
            snake.Clear();
            snake.Add((Width / 2, Height / 2));
            GenerateFood();
        }

        static void GenerateFood()
        {
            Random random = new Random();
            food = (random.Next(1, Width - 1), random.Next(1, Height - 1));
        }

        static void DrawBoard()
        {
            Console.Clear();

            // Üst duvar
            Console.WriteLine(new string('-', Width + 2));
            
            for (int y = 0; y < Height; y++)
            {
                Console.Write("|"); // Sol duvar

                for (int x = 0; x < Width; x++)
                {
                    if (x == food.Item1 && y == food.Item2)
                    {
                        Console.Write("F"); // Yem
                    }
                    else if (snake.Any(s => s.Item1 == x && s.Item2 == y))
                    {
                        Console.Write("O"); // Yılanın gövdesi
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine("|"); // Sağ duvar
            }

            // Alt duvar
            Console.WriteLine(new string('-', Width + 2));
        }

        static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W when direction != Down:
                        direction = Up;
                        break;
                    case ConsoleKey.S when direction != Up:
                        direction = Down;
                        break;
                    case ConsoleKey.A when direction != Right:
                        direction = Left;
                        break;
                    case ConsoleKey.D when direction != Left:
                        direction = Right;
                        break;
                }
            }
        }

        static void Logic()
        {
            // Yılanın yeni kafasını hareket yönüne göre hesapla
            var head = snake.First();
            (int, int) newHead = head;

            switch (direction)
            {
                case Up:
                    newHead = (head.Item1, head.Item2 - 1);
                    break;
                case Down:
                    newHead = (head.Item1, head.Item2 + 1);
                    break;
                case Left:
                    newHead = (head.Item1 - 1, head.Item2);
                    break;
                case Right:
                    newHead = (head.Item1 + 1, head.Item2);
                    break;
            }

            // Yılanın kendine veya duvara çarpma kontrolü
            if (newHead.Item1 <= 0 || newHead.Item1 >= Width || newHead.Item2 < 0 || newHead.Item2 >= Height || snake.Contains(newHead))
            {
                gameRunning = false;
                return;
            }

            // Yeni başı yılanın başına ekle
            snake.Insert(0, newHead);

            // Yem yeme durumu
            if (newHead == food)
            {
                GenerateFood(); // Yeni yem oluştur
            }
            else
            {
                // Kuyruğu kaldır
                snake.RemoveAt(snake.Count - 1);
            }
        }
    }
}
