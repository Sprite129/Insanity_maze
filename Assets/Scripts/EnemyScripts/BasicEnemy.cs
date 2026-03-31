using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 20.0f;

    public void takeDamage(float damage)
    {
        this.enemyHealth -= damage;

        if (enemyHealth <= 0)
            this.die();
    }

    private void die()
    {
        Debug.Log("I'm dead, bro");
        Destroy(gameObject);
    }
}
