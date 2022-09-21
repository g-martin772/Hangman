using System;
using System.IO;
using System.Text;

namespace Hangman;

internal class Program
{
	static Random rnd = new Random();

	static string[]? gallows;

	static int lives;

	static StringBuilder guess = new StringBuilder();
	static List<char> guesses = new List<char>();

	static void Main(string[] args)
	{
		Init();

		while (true)
		{
			string word = Reset();

			while (lives > 0)
			{
				GameLoop(word);

				if(guess.ToString().ToLower() == word.ToLower())
				{
					GameWin();
					break;
				}
			}

			if(lives <= 0)
				EndScreen();

			GameEnd();
		}
	}

	private static void GameWin()
	{
		Console.WriteLine("-------------------------");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Congratulations! You win!");
		Console.ResetColor();
		Console.WriteLine("-------------------------");
		Console.WriteLine();
		Thread.Sleep(1000);
	}

	private static void Init()
	{
		ReadFileToStringArr("gallow.txt", ',', ref gallows);
		gallows = gallows.Reverse().ToArray();
	}

	private static void GameEnd()
	{
		Console.Write("Press any key to start again.....");
		Console.ReadKey();
		Console.Clear();
	}

	private static void GameLoop(string word)
	{
		Console.WriteLine($"{guess}\n");
		Console.WriteLine();
		Console.WriteLine(gallows[lives - 1]);
		Console.WriteLine();
		Console.Write("Enter your guess: ");
		ProcessUserInput(word);
		Console.WriteLine($"Remaining lives: {lives}\n");
		Console.WriteLine();
		Console.WriteLine($"Your guesses: {guesses}");

		Thread.Sleep(1000);
		Console.Clear();
	}

	private static void EndScreen()
	{
		Console.WriteLine("---------------");
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("Sorry you lost!");
		Console.ResetColor();
		Console.WriteLine("---------------");
		Console.WriteLine();
		Thread.Sleep(1000);
	}

	private static void ProcessUserInput(string word)
	{
		char c = GetUserInput().ToString().ToLower().ToCharArray()[0];
		if (word.IndexOf(c) == -1)
		{
			Console.WriteLine("sorry, this was wrong!\n");
			lives--;
		}
		else
		{
			Console.WriteLine("Great, you got i right!\n");
			for (int i = word.ToLower().IndexOf(c); i > -1; i = word.ToLower().IndexOf(c, i + 1))
			{
				guess[i] = word[i];
			}
		}
	}

	private static string Reset()
	{
		string word = File.ReadLines("dict.txt").Skip(rnd.Next(0 ,189590)).Take(1).First();
		guess = new StringBuilder(new String('_', word.Length - 1));
		guess.Append('\r');
		lives = 7;
		return word;
	}

	private static char GetUserInput()
	{
		char c = (char)GetKeyCode();
		Console.WriteLine();
		if (c >= 65 && c <= 90 && !guesses.Contains(c))
			guesses.Add(c);
		return c;
	}

	private static void ReadFileToStringArr(string filepath, char seperator, ref string[]? arr)
	{
		string fileStr = File.ReadAllText(filepath);
		arr = fileStr.Split(seperator);
	}

	private static ConsoleKey GetKeyCode()
	{
		return Console.ReadKey().Key;
	}
}
