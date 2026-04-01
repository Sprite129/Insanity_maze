using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 20.0f;

    public void takeDamage(float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
            die();
    }

    private void die()
    {
        Destroy(gameObject);
    }
}
