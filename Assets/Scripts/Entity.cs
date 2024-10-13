using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Entity : MonoBehaviour
{
    public bool isBot = true;
    public GameObject aim;
    public bool isAlive = true;
    public bool isActive = true;
    private GameObject player;
    private Rigidbody2D rb;
    public Animator animator;
    public Slider healthBar;
    public int maxHealth = 50;
    private int currentHealth;


    private bool isAttacking = false;
    
    private bool isKnockedBack = false;
    public float knockbackForce = 20f;
    private static float knockbackDuration = 0.2f;
    
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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackPoint = new GameObject().transform;
        aim = GameObject.FindGameObjectWithTag("aim");
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        
        if (isBot)
        { 
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive || !isActive)
        {
            return;
        }
        
        if (isBot)
        {
            if (player != null)
            {
                Rigidbody2D rbPlayer = player.GetComponent<Rigidbody2D>();
                attackDirection = new Vector3(rbPlayer.position.x, rbPlayer.position.y);
                
                if (Vector2.Distance(rb.position, rbPlayer.position) <= attackRange + attackMarginFromEntity)
                {
                    
                    movement = new Vector2(0, 0);
                    animator.SetFloat("Horizontal", movement.x);
                    animator.SetFloat("Vertical", movement.y);
                    if (rbPlayer.position.x > rb.position.x)
                    {
                        // Player is to the right
                        animator.SetTrigger("RightAttack");
                    }
                    else
                    {
                        // Player is to the left
                        animator.SetTrigger("LeftAttack");
                    }
                    Attack();
                } 
                else 
                {
                    // Move towards the player
                    isAttacking = false;
                    animator.SetTrigger("StopAttack");
                    Vector2 direction = (rbPlayer.position - rb.position).normalized;

                    // Add random offset to the movement direction
                    Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    Vector2 randomizedDirection = (direction + randomOffset).normalized;
                    movement = randomizedDirection;
                }
            }
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
    
    void Attack()
    {
                    // && !animator.GetBool("RightAttack") && !animator.GetBool("LeftAttack")
        if (!isAttacking)
        {
            if (lastAttackFinish + attackCooldown <= Time.time)
            {
                Debug.Log("ATTACK");
                if (!isBot)
                    animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }
    }

    void ApplyDamage()
    {
        if (isBot && isAttacking)
        {
            Rigidbody2D rbPlayer = player.GetComponent<Rigidbody2D>();
            if (Vector2.Distance(rb.position, rbPlayer.position) <= attackRange + attackMarginFromEntity)
            {
                Debug.Log("BOT HIT");
                player.GetComponent<Entity>().TakeDamage(attackDamage);
                Vector2 knockbackDirection = player.GetComponent<Rigidbody2D>().position - rb.position;
                player.GetComponent<Entity>().GetKnockback(knockbackForce, knockbackDirection);
            }
        }
        else
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("HIT");
                enemy.GetComponent<Entity>().TakeDamage(attackDamage);
                Vector2 knockbackDirection = enemy.GetComponent<Rigidbody2D>().position - rb.position;
                enemy.GetComponent<Entity>().GetKnockback(knockbackForce, knockbackDirection);
            }
        }
        isAttacking = false;
        lastAttackFinish = Time.time;
    }

    public void GetKnockback(float force, Vector2 knockbackDirection)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            rb.velocity = Vector2.zero; // Reset velocity before applying knockback

            // Apply the force in the opposite direction of the hit
            rb.AddForce(knockbackDirection.normalized * force, ForceMode2D.Impulse);

            // Optionally disable controls/movement during knockback
            StartCoroutine(KnockbackCoroutine());
        }
    }
    
    // Coroutine to handle the knockback duration
    private IEnumerator KnockbackCoroutine()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false; // Allow movement again after knockback duration
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
        
        Debug.Log((isBot ? "Bot" : "Player") + " took damage: " + damage);
        
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
        animator.SetTrigger("Death");
        movement = Vector2.zero;
        if (isBot)
        {
            Destroy(gameObject);
        }
        else
        {
            isAlive = false;
        }

        // Можно добавить анимацию смерти или эффект
        // Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!isAlive || !isActive)
        {
            return;
        }
        if (!isAttacking)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
        }
        Vector3 rb3 = new Vector3(rb.position.x, rb.position.y);
        Vector3 direction = (attackDirection - rb3).normalized;
        attackPoint.position = rb3 + direction * attackMarginFromEntity;
        if (!isBot)
        {
            aim.GetComponent<Transform>().position = attackPoint.position;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive || isBot)
        {
            return;
        }
        
        Debug.Log("Collision with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Boss"))
        {
            int damageAmount = 1; // Set your damage amount here
            TakeDamage(damageAmount);
        }
    }
    
    public void SetActive(bool result)
    {
        isActive = result;
    }
}
