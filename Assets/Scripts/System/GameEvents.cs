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
		public EndGame(float finalScore) 
		{
			FinalScore = finalScore;
		}

		public readonly float FinalScore;
	}

	public class EarnScore
	{
		public EarnScore(float amount)
		{
			Amount = amount;
		}

		public readonly float Amount;
	}

	public class EarnScoreMultiplier
	{
		public EarnScoreMultiplier(float amount)
		{
			Amount = amount;
		}

		public readonly float Amount;
	}
}