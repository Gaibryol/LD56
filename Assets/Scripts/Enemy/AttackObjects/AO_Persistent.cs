using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_Persistent : EnemyAttackObject
{
    [SerializeField] protected float lifeTime = 30f;
    protected float timer = 0;
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            OnLifeSpanEnded();
        }
    }

    protected virtual void OnLifeSpanEnded()
    {
        Destroy(gameObject);
    }
}
