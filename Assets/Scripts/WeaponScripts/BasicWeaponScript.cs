using UnityEngine;
using UnityEngine.InputSystem;

public class BasicWeapon : MonoBehaviour
{
	private Animator weaponAnimator;
	private Collider2D damageCollider;

	[SerializeField] private float damage = 20.0f;
	[SerializeField] private float attackSpeed = 1f;
	[SerializeField] private float inputBufferTime = 0.2f;

	private Vector2 attackDirection;
	private float attackBufferTimer = 0f;
	private bool canStrike = true;

	private void Start()
	{
		weaponAnimator = GetComponent<Animator>();
		weaponAnimator.speed = attackSpeed;
		damageCollider = GetComponent<Collider2D>();
		damageCollider.enabled = false;
	}

	private void Update()
	{
		attackBufferTimer -= Time.deltaTime;

		if (attackBufferTimer > 0f && canStrike) {
			attackBufferTimer = 0f;
			weaponAnimator.SetFloat("X", attackDirection.x);
			weaponAnimator.SetFloat("Y", attackDirection.y);
			weaponAnimator.SetTrigger("Attack");
		}
	}

	public void Attack()
	{
		attackBufferTimer = inputBufferTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<BasicEnemy>().takeDamage(damage);
		}
	}

	public void turnOnCollision()
	{
		damageCollider.enabled = true;
	}

	public void turnOffCollision()
	{
		damageCollider.enabled = false;
	}

	public void disableStrike()
	{
		this.canStrike = false;
	}

	public void enableStrike()
	{
		this.canStrike = true;
	}

	public void setAttackDirection(Vector2 attackDirection)
	{
		this.attackDirection = attackDirection;
	}
}
