using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_FrogTongue : EnemyAttackBehaviour
{
    [SerializeField] private AO_FrogTongue frogTongue;

    public override bool Attack()
    {
        if(!base.Attack()) return false;
        frogTongue.Begin();
        return true;
    }
}
