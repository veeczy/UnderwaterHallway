using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Range(0, 200)] public int startHealth = 100, currentHealth;

    public static bool isDead;
    public bool loseCondition = false;

    private void Start()
    {
        currentHealth = startHealth;
        isDead = false;
        loseCondition = false;
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log("the player has died");
        }
        if(isDead) { loseCondition = true; }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }


}
