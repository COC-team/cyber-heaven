using System.Collections; // Add this line for IEnumerator and coroutines
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public GameObject objectToSpawn; // Reference to the object you want to spawn
    public Transform spawnPoint;      // Position where the object should be spawned

    // Additional offset to shift the spawn position to the right
    public Vector2 rightOffset = new Vector2(3, -2); // Adjust this value as needed
	public Vector2 leftOffset = new Vector2(-3, -2);

    // This method will be called by the animation event
public void SpawnObject(string offsetSide)
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
        new Vector2(0f, 1.5f),     // Up
        new Vector2(0f, -1.5f)     // Down
    };

    // Spawn multiple objects
    for (int i = 0; i < 8; i++)
    {
        // Use the defined offsets for each object
        Vector2 offset = offsets[i];

		Vector2 sideOffset = offsetSide == "right" ? rightOffset : leftOffset;

        // Calculate the spawn position with the additional right offset
        Vector3 spawnPosition = spawnPoint.position + new Vector3(sideOffset.x, sideOffset.y, 0) + new Vector3(offset.x, offset.y, 0); // Use both offsets

        // Instantiate the object at the calculated position
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Reset the animation to the first frame if the object has an Animator component
        Animator animator = spawnedObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind(); // Reset to default state
            animator.Update(0f); // Force animator to update to ensure it starts from frame 0
        }

        // Start moving the object
        StartCoroutine(MoveObject(spawnedObject, offset)); // Start moving each object
    }
}

private IEnumerator MoveObject(GameObject obj, Vector2 direction)
{
    float duration = 2f; // Reduced duration for faster movement
    float elapsedTime = 0f;

    // Calculate the target position
    Vector3 targetPosition = obj.transform.position + new Vector3(direction.x, direction.y, 0);

    while (elapsedTime < duration)
    {
        // Calculate the fraction of the movement (0 to 1)
        float t = elapsedTime / duration;

        // Smoothly move the object to the target position
        obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, t);

        elapsedTime += Time.deltaTime;
        yield return null; // Wait for the next frame
    }

    // Ensure the object ends at the target position
    obj.transform.position = targetPosition;

    // Optional: You can remove or reduce this wait time for faster destruction
    // yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds before destroying (very short wait)

    // Destroy the object
    Debug.Log($"Destroying object: {obj.name}"); // Debug log before destruction
    Destroy(obj);
}
}
