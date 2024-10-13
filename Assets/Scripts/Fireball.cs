using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
