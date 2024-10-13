using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public bool isBot = true;
    private Rigidbody2D rb;
    public Animator animator;
    public Slider healthBar;
    public int maxHealth = 50;
    private int currentHealth;


    public float attackDuration = 1f; // Duration of the attack animation
    public float attackCooldown = 1f; // Cooldown before the next attack
    private float lastAttackFinish = 0f;
    public float attackRange = 0.5f;
    public float attackMarginFromEntity = 0.5f;
    public int attackDamage = 2;
    private Transform attackPoint;
    public LayerMask enemiesLayer;
	public LayerMask entityLayer; // nado li?
    private Vector3 attackDirection { get; set; } = new Vector3(0, 0);

    public float moveSpeed = 3f;
    private Vector2 movement = Vector2.zero;

    private bool startRecord = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackPoint = new GameObject().transform;
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBot)
        {
            // enemiesLayer
            // if (player != null && Vector2.Distance(transform.position, player.transform.position) 
            //     <= attackRange + attackMarginFromEntity)
            // {
            //     // Reset animator parameters to stop the movement animation
            //     animator.SetFloat("Horizontal", 0);
            //     animator.SetFloat("Vertical", 0);
            //     
            //     // Determine if the player is to the left or right
            //     if (player.transform.position.x > transform.position.x)
            //     {
            //         // Player is to the right
            //         animator.SetTrigger("RightAttack");
            //     }
            //     else
            //     {
            //         // Player is to the left
            //         animator.SetTrigger("LeftAttack");
            //     }
            //     
            //     Vector3 direction = (Vector3) (player.transform.position - transform.position).normalized;
            //
            //     player.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
            //     player.GetComponent<PlayerMovement>().GetKnockback(knockbackForce, direction);
            //     
            //     yield return new WaitForSeconds(attackDuration + attackCooldown); // Wait for attack animation to complete

                // // Cooldown before moving again
                // yield return new WaitForSeconds(attackCooldown);
            // }
            // else
            // {
            //     // Move towards the player
            //     animator.SetTrigger("StopAttack");
            //     Vector2 direction = (player.transform.position - transform.position).normalized;
            //
            //     // Add random offset to the movement direction
            //     Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            //     Vector2 randomizedDirection = (direction + randomOffset).normalized;
            //
            //     // Set animator parameters to reflect movement with randomized direction
            //     animator.SetFloat("Horizontal", randomizedDirection.x);
            //     animator.SetFloat("Vertical", randomizedDirection.y);
            //
            //     // Ensure the NPC moves with the random variation
            //     transform.position += (Vector3)randomizedDirection * moveSpeed * Time.deltaTime;
            //     
            //     // Wait for a short period before moving again
            //     yield return null; // Continue until the next frame
            // }
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            attackDirection = new Vector3(mousePos.x, mousePos.y);
            
            float horizontal = Input.GetAxisRaw("Horizontal"); // A (-1) and D (1)
            float vertical = Input.GetAxisRaw("Vertical");     // W (1) and S (-1)
            
            // Create a movement vector
            movement = new Vector2(horizontal, vertical);

            // Normalize only when moving diagonally
            if (movement.sqrMagnitude > 1)
            {
                movement = movement.normalized;
            }
            
            
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            
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
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Attack()
    {
        if (!animator.GetBool("Attack") && lastAttackFinish + attackCooldown <= Time.time)
        {
            Debug.Log("ATTACK");
            animator.SetTrigger("Attack");
            // Temporarily
            ApplyDamage();
        }
    }

    void ApplyDamage()
    {
        // Vector3 rb3 = new Vector3(rb.position.x, rb.position.y);
        
        // animator.setTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("HIT");
            enemy.GetComponent<Entity>().TakeDamage(attackDamage);
            // enemy.GetComponent<NPCMovement>().GetKnockback(knockbackForce, knockbackDirection);
        }

        lastAttackFinish = Time.time;
    }
    
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Clamp the health to avoid exceeding max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Optional: Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Entity took damage: " + damage);
        
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Entity died!");

        // Можно добавить анимацию смерти или эффект
        // Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!animator.GetBool("Attack"))
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            
            Vector3 rb3 = new Vector3(rb.position.x, rb.position.y);
            Vector3 direction = (attackDirection - rb3).normalized;
            attackPoint.position = rb3 + direction * attackMarginFromEntity;
        }
    }

    bool isPlaying(string animationName)
    {
        if (animator == null)
            return false;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(entityLayer.value); // 0 is the layer index
        return stateInfo.IsName(animationName);
    }
}
