using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBehaviour : MonoBehaviour
{
    [SerializeField] private float timeBetweenAttacks;
    private float timeOfLastAttack = -1;
    protected virtual void Start()
    {
        timeOfLastAttack = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public virtual bool Attack() 
    {
        if (!CanAttack()) return false;
        timeOfLastAttack = Time.time;
        return true;
    }
    protected virtual bool CanAttack() 
    {
        // 1. If player is within certain bounds from the player
        // 2. Certain time has passed before last attack

        if (timeOfLastAttack + timeBetweenAttacks > Time.time) return false;

        return EnemyUtilities.WithinBounds(transform.position, Constants.EnemyAttack.maxValidAttackDistanceAwayFromScreen);
    }
}
