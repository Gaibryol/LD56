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
            Shark
        }


    }

    public static class EnemyManager
    {
        public static Vector2 screenSpawnBuffer = new Vector2(5, 5); // Offset from the edge of the screen to start spawning
        public static Vector2 maxSpawnDistanceFromBuffer = new Vector2(10, 10);
        public static Vector2 maxValidDistanceAwayFromScreen = new Vector2(15, 15);
    }

    public static class EnemyAttack
    {
        public static Vector2 maxValidDistanceAwayFromScreen = new Vector2(15, 15); // Distance from screen that an object an live
        public static Vector2 maxValidAttackDistanceAwayFromScreen = new Vector2(1, 1); // Distance from from screen that an enemy can attack
    }
}
