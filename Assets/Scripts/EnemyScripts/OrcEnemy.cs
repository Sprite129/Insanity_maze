using UnityEngine;

public class OrcEnemy : BasicEnemy
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseDistance = 6f;
    [SerializeField] private float attackDistance = 1.2f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int attackDamage = 10;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;
    private float attackTimer;
    private Vector2 movement;

    public bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        attackTimer = 0f;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (IsDead()) return;

        if (player == null)
        {
            animator?.SetBool("isRunning", false);
            return;
        }

        attackTimer -= Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;

            if (distance <= attackDistance)
            {
                movement = Vector2.zero;
                if (attackTimer <= 0f)
                    Attack();
            }
            else
            {
                animator?.SetBool("isRunning", true);
            }

            Flip(direction.x);
        }
        else
        {
            movement = Vector2.zero;
            animator?.SetBool("isRunning", false);
        }
    }

    void FixedUpdate()
    {
        if (IsDead()) return;
        if (movement.sqrMagnitude > 0.001f)
        {
            Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    private void Attack()
    {
        attackTimer = attackCooldown;
        animator?.SetTrigger("Attack");

        if (player != null && player.TryGetComponent<PlayerHealth>(out var playerHealth))
            playerHealth.TakeDamage(attackDamage);
    }

    private void Flip(float moveX)
    {
        if (moveX > 0 && !isFacingRight || moveX < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && attackTimer <= 0f)
            Attack();
    }
}
