using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Constants
{
    public static class Enemy
    {
        public enum EnemyType
        {
            Chicken,
            Squid,
            Frog,
            Crab,
            Bunny,
            Hippo,
            Shark,
			None
        }


    }

    public static class EnemyManager
    {
        public static Vector2 screenSpawnBuffer = new Vector2(0f, 0f); // Offset from the edge of the screen to start spawning
        public static Vector2 maxSpawnDistanceFromBuffer = new Vector2(1, 1);
        public static Vector2 maxValidDistanceAwayFromScreen = new Vector2(4, 4);
    }

    public static class EnemyAttack
    {
        public static Vector2 maxValidDistanceAwayFromScreen = new Vector2(4, 4); // Distance from screen that an object an live
        public static Vector2 maxValidAttackDistanceAwayFromScreen = new Vector2(0f, 0f); // Distance from from screen that an enemy can attack
    }
}
