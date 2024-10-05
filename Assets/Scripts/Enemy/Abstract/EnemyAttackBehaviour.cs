using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBehaviour : MonoBehaviour
{
    [SerializeField] private float timeBetweenAttacks;
    private float timeOfLastAttack = -1;
    public bool CurrentlyAttacking = false;

    protected virtual void Start()
    {
        timeOfLastAttack = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public delegate float TriggerAnimation();

    public virtual void QueueAttack(TriggerAnimation triggerAnimation)
    {
        if (!CanAttack()) return;
        CurrentlyAttacking = true;

        timeOfLastAttack = Time.time;

        StartCoroutine(Attack(triggerAnimation));
    }

    public virtual IEnumerator Attack(TriggerAnimation triggerAnimation) 
    {
        float delay = triggerAnimation();
        yield return new WaitForSeconds(delay);
    }
    protected virtual bool CanAttack() 
    {
        // 1. If player is within certain bounds from the player
        // 2. Certain time has passed before last attack
        if (CurrentlyAttacking) return false;
        if (timeOfLastAttack + timeBetweenAttacks > Time.time) return false;

        return EnemyUtilities.WithinBounds(transform.position, Constants.EnemyAttack.maxValidAttackDistanceAwayFromScreen);
    }
}
