using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth = 20.0f;

    private Animator animator;
    private bool isDead = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void takeDamage(float damage)
    {
        if (isDead) return;

        enemyHealth -= damage;

        if (enemyHealth <= 0)
            die();
        else
            animator?.SetTrigger("Hurt");
    }

    private void die()
    {
        isDead = true;

        var dropHandler = GetComponent<EnemyDropHandler>();
        if (dropHandler != null)
            dropHandler.TryDrop();

        animator?.SetTrigger("Death");

        // Disable enemy behavior
        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
            col.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 1f); // Destroy after death animation plays
    }

    public bool IsDead() => isDead;
}
