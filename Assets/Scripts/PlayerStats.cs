using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 3;
    public int money = 0;

    [Header("Combat Stats")]
    public float attackPower = 1f;
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;

    [Header("Movement Stats")]
    public float playerSpeed = 5f;
    public float rollSpeedMultiplier = 2.5f;
    public float rollDuration = 0.5f;
    public float rollDecayFactor = 2.5f;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // Handle death logic here
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}
