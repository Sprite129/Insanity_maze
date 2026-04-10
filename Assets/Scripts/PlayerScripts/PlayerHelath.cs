using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"Player took {damage} damage. HP = {currentHealth}.");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died.");
        // Тут можна додати логіку перезапуску, екран завершення гри тощо.
    }

    // Опціонально: метод для лікування
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"Player healed {amount}. HP = {currentHealth}.");
    }
}