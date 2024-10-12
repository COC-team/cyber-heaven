using System.Collections; // Add this line for IEnumerator and coroutines
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public GameObject objectToSpawn; // Reference to the object you want to spawn
    public Transform spawnPoint;      // Position where the object should be spawned

    // Additional offset to shift the spawn position to the right
    public Vector2 rightOffset = new Vector2(3, -2); // Adjust this value as needed

    // This method will be called by the animation event
    public void SpawnObject()
    {
        Debug.Log("SpawnObject method called");

        // Define the diagonal offsets for the 8 objects (2 on each diagonal)
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(1.5f, 1.5f),   // Top-right
            new Vector2(-1.5f, 1.5f),  // Top-left
            new Vector2(1.5f, -1.5f),  // Bottom-right
            new Vector2(-1.5f, -1.5f), // Bottom-left
            new Vector2(1.5f, 0f),     // Right
            new Vector2(-1.5f, 0f),    // Left
            new Vector2(0f, 1.5f),      // Up
            new Vector2(0f, -1.5f)      // Down
        };

        // Spawn multiple objects
        for (int i = 0; i < 8; i++)
        {
            // Use the defined offsets for each object
            Vector2 offset = offsets[i];

            // Calculate the spawn position with the additional right offset
            Vector3 spawnPosition = spawnPoint.position + new Vector3(rightOffset.x, rightOffset.y, 0) + new Vector3(offset.x, offset.y, 0); // Use both offsets

            // Instantiate the object at the calculated position
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Start moving the object
            StartCoroutine(MoveObject(spawnedObject, offset)); // Start moving each object
        }
    }

    private IEnumerator MoveObject(GameObject obj, Vector2 direction)
    {
        float duration = 20f; // Duration for movement (4 times slower)
        float elapsedTime = 0f;

        // Calculate the target position (50% distance)
        Vector3 targetPosition = obj.transform.position + new Vector3(direction.x, direction.y, 0); // Move diagonally

        while (elapsedTime < duration)
        {
            // Calculate the fraction of the movement
            float t = elapsedTime / duration;

            // Move the object smoothly
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the object ends at the target position
        obj.transform.position = targetPosition;

        // Destroy the object after it has moved
        Debug.Log($"Destroying object: {obj.name}"); // Debug log before destruction
        Destroy(obj);
    }
}
