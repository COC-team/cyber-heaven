using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Animator animator;
    public float moveDistance = 2f; // Total distance to move in 2 seconds
    public float moveDuration = 1f;  // Time duration for the movement
    public float attackDuration = 1f; // Duration of the attack animation
    public float attackCooldown = 1f; // Cooldown before the next attack
    public float attackRange = 2f; // Range within which to attack the player
    private GameObject player; // Reference to the player

    private void Start()
    {
        // Find the player in the scene by tag
        player = GameObject.FindGameObjectWithTag("Player");
        // Start the movement coroutine
        StartCoroutine(MoveAndAttack());
    }

    private IEnumerator MoveAndAttack()
    {
        while (true)
        {
            // Check if the player is within attack range
            if (player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                // Determine if the player is to the left or right
                if (player.transform.position.x > transform.position.x)
                {
                    // Player is to the right
                    animator.SetTrigger("RightAttack");
                }
                else
                {
                    // Player is to the left
                    animator.SetTrigger("LeftAttack");
                }

                yield return new WaitForSeconds(attackDuration); // Wait for attack animation to complete

                // Cooldown before moving again
                yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                // Get a random direction to move
                animator.SetTrigger("StopAttack");
                Vector2 randomDirection = GetRandomDirection();

                // Set animator parameters based on the random direction
                animator.SetFloat("Horizontal", randomDirection.x);
                animator.SetFloat("Vertical", randomDirection.y);

                // Calculate the target position
                Vector2 targetPosition = (Vector2)transform.position + randomDirection * moveDistance;

                // Store the starting position
                Vector2 startPosition = transform.position;

                // Move towards the target position over the specified duration
                float elapsedTime = 0f;
                while (elapsedTime < moveDuration)
                {
                    transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                // Ensure the NPC ends up at the target position
                transform.position = targetPosition;

                // Reset animator parameters to stop the movement animation
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 0);

                // Wait for a short period before moving again
                yield return new WaitForSeconds(0.1f); // Optional pause between movements
            }
        }
    }

    private Vector2 GetRandomDirection()
    {
        // Randomly choose between four directions
        switch (Random.Range(0, 4))
        {
            case 0: return Vector2.up;    // Move Up
            case 1: return Vector2.down;  // Move Down
            case 2: return Vector2.left;  // Move Left
            case 3: return Vector2.right; // Move Right
            default: return Vector2.zero;  // Should not reach here
        }
    }
}
