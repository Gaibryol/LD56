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

	public class Die
	{
		public Die() { }
	}
}