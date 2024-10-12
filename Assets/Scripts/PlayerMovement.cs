using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 3f; // Base movement speed, you can adjust this
    private Rigidbody2D rb;
    private Vector2 movement;

    
    public float attackDuration = 1f; // Duration of the attack animation
    public float attackCooldown = 1f; // Cooldown before the next attack
    private float lastAttackFinish = 0f;
    public float attackRange = 0.5f;
    public float attackMarginFromEntity = 0.5f;
    public int attackDamage = 2;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    
    public int maxHealth = 50;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackPoint = new GameObject().transform;
        currentHealth = maxHealth;
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
        movement = new Vector2(horizontal, vertical);

        // Normalize only when moving diagonally
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }
        
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackFinish)
        {
            Debug.Log("Attack");
            Attack();
        }
    }

    void Attack()
    {
        Vector3 rb3 = new Vector3(rb.position.x, rb.position.y);
        
        // animator.setTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("HIT");
            enemy.GetComponent<NPCMovement>().TakeDamage(attackDamage);
        }

        lastAttackFinish = Time.time + attackDuration + attackCooldown;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage: " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        // Можно добавить анимацию смерти или эффект
        // Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void FixedUpdate()
    {
        // Apply the movement to the Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 rb3 = new Vector3(rb.position.x, rb.position.y);
        Vector3 direction = (mousePosition - rb3).normalized;
        attackPoint.position = rb3 + direction * attackMarginFromEntity;
    }
}
