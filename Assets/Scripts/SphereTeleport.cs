using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTeleport : MonoBehaviour
{
    // Define six predefined points for teleportation in 2D space (assuming z=0 for 2D).
    public Vector2[] teleportPoints = new Vector2[6];
    public float teleportInterval = 10f; // Interval in seconds for teleportation
    private bool isActivate = false;
    private Animator animator;
    public int damageAmount = 10; // Amount of damage dealt to the player on collision

    // Start is called before the first frame update
    void Start()
    {
        // Initialize predefined teleport points (example coordinates)
        teleportPoints[0] = new Vector2(32f, 10f);
        teleportPoints[1] = new Vector2(40f, 5f);
        teleportPoints[2] = new Vector2(33f, 0f);
        teleportPoints[3] = new Vector2(50f, 0f);
        teleportPoints[4] = new Vector2(48f, 10f);
        teleportPoints[5] = new Vector2(40f, 2f);

        // Initialize the Animator component
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Activate teleportation when the 'T' key is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            ActivateTeleportation();
        }
    }

    void ActivateTeleportation()
    {
        isActivate = true;

        // Trigger the teleport animation
        animator.SetTrigger("Activate");

        // Start teleporting every teleportInterval seconds
        InvokeRepeating("TeleportToRandomPoint", teleportInterval, teleportInterval);
    }

    // Teleport the object to a random point
    void TeleportToRandomPoint()
    {
        if (!isActivate)
        {
            return;
        }

        // Get a random index from the teleportPoints array
        int randomIndex = Random.Range(0, teleportPoints.Length);

        // Teleport the object by setting its position to the random point
        transform.position = teleportPoints[randomIndex];
    }

    // Detect collision with player and deal damage
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Sphere Collision with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            int damageAmount = 20; // Set your damage amount here
            collision.gameObject.GetComponent<Entity>().TakeDamage(damageAmount);
        }
    }
}
