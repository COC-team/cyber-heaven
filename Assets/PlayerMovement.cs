using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 10f; // Base movement speed, you can adjust this
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal"); // A (-1) and D (1)
        float vertical = Input.GetAxisRaw("Vertical");     // W (1) and S (-1)

        // Set animator parameters
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        // Create a movement vector
        movement = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        // Apply the movement to the Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}