using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GenericEnemy : EnemyAbstract
{
    public override float SpawnChance(float gameTime, int enemiesKilled, int sameEnemiesRemaining, int difficultyRating)
    {
        // the number of enemy that can exist of this type on the field is equal to the current difficult rating / enemy difficult rating
        // so if the difficulty rating is 4, i'm allowed 1 shark or 1 hippo, or 2 frogs, or 4 chicken
        int numberOfTypeAllowed = Mathf.FloorToInt(difficultyRating / this.difficultyRating);
        if (sameEnemiesRemaining >= numberOfTypeAllowed) return 0;  // at cap already

        return 1;
    }


    

}
