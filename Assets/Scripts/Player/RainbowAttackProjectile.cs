using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAttackProjectile : MonoBehaviour
{
	private float timer;

    // Start is called before the first frame update
    void Start()
    {
		timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
		if (EnemyUtilities.OutOfBounds(transform.position, Constants.RainbowAttack.maxValidDistanceAwayFromScreen))
		{
			Destroy(gameObject);
			return;
		}

		timer += Time.deltaTime;
		if (timer >= 5f)
		{
			Destroy(gameObject);
			return;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyAbstract>() != null || collision.GetComponent<EnemyAttackObject>() != null)
		{
			// Collided with enemy
			Destroy(collision.gameObject);
		}
		else if (collision.GetComponentInParent<EnemyAbstract>() != null || collision.GetComponentInParent<EnemyAttackObject>() != null)
		{
			// Collided with child enemy
			Destroy(collision.transform.parent.gameObject);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyAbstract>() != null || collision.GetComponent<EnemyAttackObject>() != null)
		{
			// Collided with enemy
			Destroy(collision.gameObject);
		}
		else if (collision.GetComponentInParent<EnemyAbstract>() != null || collision.GetComponentInParent<EnemyAttackObject>() != null)
		{
			// Collided with child enemy
			Destroy(collision.transform.parent.gameObject);
		}
	}
}
