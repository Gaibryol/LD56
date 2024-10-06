using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerEvents
{
	public class Damage
	{
		public Damage(int amount)
		{
			Amount = amount;
		}

		public readonly int Amount;
	}

	public class Heal
	{
		public Heal(int amount)
		{
			Amount = amount;
		}

		public readonly int Amount;
	}

	public class Die
	{
		public Die(int numBunny, int numChicken, int numCrab, int numFrog, int numShark, int numSquid, int numHippo)
		{
			NumBunny = numBunny;
			NumChicken = numChicken;
			NumCrab = numCrab;
			NumFrog = numFrog;
			NumShark = numShark;
			NumSquid = numSquid;
			NumHippo = numHippo;
		}

		public readonly int NumBunny;
		public readonly int NumChicken;
		public readonly int NumCrab;
		public readonly int NumFrog;
		public readonly int NumShark;
		public readonly int NumSquid;
		public readonly int NumHippo;
	}

	public class UpdateClawState
	{
		public UpdateClawState(Constants.Claw.States clawState, GameObject grabbedObj = null) 
		{
			ClawState = clawState;
			GrabbedObj = grabbedObj;
		}

		public readonly Constants.Claw.States ClawState;
		public readonly GameObject GrabbedObj;
	}

	public class UpdateInvulnerability
	{
		public UpdateInvulnerability(bool newState)
		{
			NewState = newState;
		}

		public readonly bool NewState;
	}

	public class UpgradeBulletBlocks
	{
		public UpgradeBulletBlocks(int amount)
		{
			Amount = amount;
		}

		public readonly int Amount;
	}

	public class Upgrade
	{
		public Upgrade(List<int> indexes, Constants.Enemy.EnemyType type)
		{
			Indexes = indexes;
			Type = type;
		}

		public readonly List<int> Indexes;
		public readonly Constants.Enemy.EnemyType Type;
	}

	public class RainbowAttack
	{
		public RainbowAttack() { }
	}
}