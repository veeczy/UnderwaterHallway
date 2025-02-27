using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Range(0, 200)] public int startHealth = 100, currentHealth;

    public static bool isDead;

    private void Start()
    {
        currentHealth = startHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("the player has dieded");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }


}
