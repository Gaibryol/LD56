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
		public EndGame(float finalScore, bool isHighscore, float totalGameTime, Constants.Difficulty difficulty) 
		{
			FinalScore = finalScore;
			IsHighscore = isHighscore;
			TotalGameTime = totalGameTime;
			Difficulty = difficulty;
		}

		public readonly float TotalGameTime;
		public readonly float FinalScore;
		public readonly bool IsHighscore;
		public readonly Constants.Difficulty Difficulty;
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

	public class NotifyAchievementObtained
	{
		public NotifyAchievementObtained((string, float) keyValue, float newValue) 
		{
			Key = keyValue.Item1;
            Threshold = keyValue.Item2;
			NewValue = newValue;
		}

		public readonly string Key;
		public readonly float Threshold;
		public readonly float NewValue;
	}

	public class NotifyAchievementButtonPressed
	{
        public NotifyAchievementButtonPressed(Constants.Difficulty difficulty)
        {
            Difficulty = difficulty;
        }

		public readonly Constants.Difficulty Difficulty;
    }
}