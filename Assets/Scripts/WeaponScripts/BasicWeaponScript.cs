using UnityEngine;
using UnityEngine.InputSystem;

public class BasicWeapon : MonoBehaviour
{
	private Animator weaponAnimator;
	private Collider2D collider;
	[SerializeField] private float damage = 20.0f;

	private void Start()
	{
		weaponAnimator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();
		collider.enabled = false;
	}

	public void Attack()
	{
		weaponAnimator.SetTrigger("Attack");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Enemy"))
		{
			collision.gameObject.GetComponent<BasicEnemy>().takeDamage(this.damage);
		}
	}

	public void turnOnCollision()
	{
		this.collider.enabled = true;
	}

	public void turnOffCollision()
	{
		this.collider.enabled = false;
	}
}
