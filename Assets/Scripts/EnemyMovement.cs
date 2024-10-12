using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Animator animator;
    public float moveDistance = 2f; // Total distance to move in 2 seconds
    public float moveDuration = 1f;  // Time duration for the movement
    public float moveSpeed = 2f; // Speed at which the NPC moves
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
                // Reset animator parameters to stop the movement animation
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 0);
                
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

                // // Cooldown before moving again
                // yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                // Move towards the player
                animator.SetTrigger("StopAttack");
                Vector2 direction = (player.transform.position - transform.position).normalized;

                // Set animator parameters to reflect movement
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                
                // Ensure the NPC ends up at the target position
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

                // Wait for a short period before moving again
                yield return null; // Continue until the next frame
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
