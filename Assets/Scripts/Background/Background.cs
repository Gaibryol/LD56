using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Material spriteMaterial; // Reference to the sprite's material
    [SerializeField] private float offsetScale = 0.05f;
    [SerializeField] private GameObject shootingStar;
    void Start()
    {
        spriteMaterial = GetComponent<SpriteRenderer>().material;
        StartCoroutine(SpawnShootingStars());
    }

    void Update()
    {
        SetOffset(transform.position);
    }

    public void SetOffset(Vector2 newOffset)
    {
        spriteMaterial.mainTextureOffset = newOffset * offsetScale;
    }

    private IEnumerator SpawnShootingStars()
    {
        while (true)
        {
            Instantiate(shootingStar, DetermineSpawnLocation(), Quaternion.identity);
            yield return new WaitForSeconds(Random.value * 5);
        }
    }

    private Vector3 DetermineSpawnLocation()
    {
        // Requirements:
        // 1. Spawn out side of player view

        // Determine what the player can see
        Vector3 bottomLeftBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRightBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        // Add an offset because we don't want to spawn on edge of the screen
        bottomLeftBoundPosition -= (Vector3)Constants.EnemyManager.screenSpawnBuffer;
        topRightBoundPosition += (Vector3)Constants.EnemyManager.screenSpawnBuffer;

        // Pick a random spawn edge
        float rand_edge = Random.value;

        Vector3 spawnPosition = new Vector3();

        Vector2 maxSpawnDistanceFromBuffer = Constants.EnemyManager.maxSpawnDistanceFromBuffer;
        float x_spawnOffset = Random.Range(0, maxSpawnDistanceFromBuffer.x);
        float y_spawnOffset = Random.Range(0, maxSpawnDistanceFromBuffer.y);
        float x_randSpawnValue = Random.Range(bottomLeftBoundPosition.x - maxSpawnDistanceFromBuffer.x, topRightBoundPosition.x + maxSpawnDistanceFromBuffer.x);
        float y_randSpawnValue = Random.Range(bottomLeftBoundPosition.y - maxSpawnDistanceFromBuffer.y, topRightBoundPosition.y + maxSpawnDistanceFromBuffer.y);

        if (rand_edge < 0.5)
        {
            // spawn on right edge of screen
            spawnPosition.x = topRightBoundPosition.x + x_spawnOffset;
            spawnPosition.y = y_randSpawnValue;
        }
        else
        {
            // spawn on top edge of screen
            spawnPosition.x = x_randSpawnValue;
            spawnPosition.y = topRightBoundPosition.y + y_spawnOffset;
        }
        return spawnPosition;
    }
}
