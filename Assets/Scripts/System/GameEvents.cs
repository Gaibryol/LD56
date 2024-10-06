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
		public EndGame(float finalScore, bool isHighscore, float totalGameTime) 
		{
			FinalScore = finalScore;
			IsHighscore = isHighscore;
			TotalGameTime = totalGameTime;
		}

		public readonly float TotalGameTime;
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