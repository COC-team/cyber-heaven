using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    public Slider healthSlider; // Reference to the Slider
    public Transform enemy; // Reference to the enemy's transform
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Adjust this value if needed

    private void Start()
    {
        // Set the slider's initial value (if needed)
        healthSlider.maxValue = 100; // Set this to the enemy's max health
        healthSlider.value = 100; // Set this to the enemy's current health
    }

    private void Update()
    {
        // Position the slider above the enemy
        if (enemy != null)
        {
            Vector3 enemyPosition = enemy.position + offset;
            healthSlider.transform.position = enemyPosition;
        }
    }
}