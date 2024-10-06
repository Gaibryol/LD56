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
		public EndGame(float finalScore, bool isHighscore) 
		{
			FinalScore = finalScore;
			IsHighscore = isHighscore;
		}

		public readonly float FinalScore;
		public readonly bool IsHighscore;
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