using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    private EnemyMoveBehaviour moveBehaviour;
    void Start()
    {
        moveBehaviour = GetComponent<EnemyMoveBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        moveBehaviour.Move();
        if (EnemyUtilities.OutOfBounds(transform.position, new Vector2(1, 1)))
        {
            Destroy(gameObject);
        }
    }
}
