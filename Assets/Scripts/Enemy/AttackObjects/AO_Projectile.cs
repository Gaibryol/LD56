using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_Projectile : EnemyAttackObject
{
    [SerializeField] private float projectileSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyUtilities.OutOfBounds(transform.position, Constants.EnemyAttack.maxValidDistanceAwayFromScreen))
        {
            Destroy(gameObject);
            return;
        }
        transform.position += transform.right * projectileSpeed * Time.deltaTime;
    }
}
