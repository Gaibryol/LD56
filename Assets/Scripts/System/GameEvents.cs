using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameEvents
{
	public class StartGame
	{
		public StartGame() { }
	}

	public class EndGame
	{
		public EndGame(int finalScore) 
		{
			FinalScore = finalScore;
		}

		public readonly int FinalScore;
	}

	public class EarnScore
	{
		public EarnScore(int amount)
		{
			Amount = amount;
		}

		public readonly int Amount;
	}
}