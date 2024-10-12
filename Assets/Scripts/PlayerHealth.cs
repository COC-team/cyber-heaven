using UnityEngine;
using UnityEngine.UI; // Only needed if you're using UI elements like health bars

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum health
    public int currentHealth;    // Current health

    public Slider healthBar;     // Optional: Assign a UI slider for the health bar

    void Start()
    {
        // Set the player's current health to the maximum health at the start
        currentHealth = maxHealth;

        // Optional: Set the health bar's max value
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Function to handle damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Clamp the health to a minimum of 0 to avoid negative health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Optional: Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to heal the player
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

    // Function that handles the player's death
    private void Die()
    {
        Debug.Log("Player has died!");
        // Add death logic here (e.g., reload the scene, play a death animation, etc.)
        // Example: Destroy(gameObject); // if you want the player object to be removed
    }
}