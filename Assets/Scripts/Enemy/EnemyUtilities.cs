using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyUtilities
{
    public static bool OutOfBounds(Vector3 position, Vector2 distanceFromScreen)
    {
        // Determine what the player can see
        Vector3 bottomLeftBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRightBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        bottomLeftBoundPosition -= (Vector3)distanceFromScreen;
        topRightBoundPosition += (Vector3)distanceFromScreen;

        float x = position.x;
        float y = position.y;
        return x < bottomLeftBoundPosition.x || x > topRightBoundPosition.x || y > topRightBoundPosition.y || y < bottomLeftBoundPosition.y;
    }

    public static bool WithinBounds(Vector3 position, Vector2 distanceFromScreen)
    {
        // Determine what the player can see
        Vector3 bottomLeftBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRightBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        bottomLeftBoundPosition -= (Vector3)distanceFromScreen;
        topRightBoundPosition += (Vector3)distanceFromScreen;

        float x = position.x;
        float y = position.y;
        return x > bottomLeftBoundPosition.x && x < topRightBoundPosition.x && y < topRightBoundPosition.y && y > bottomLeftBoundPosition.y;
    }
}
