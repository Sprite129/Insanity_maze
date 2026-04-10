using UnityEngine;
using UnityEngine.InputSystem;

public class Player_controller : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    private float animationSpeed;

    private Vector2 playerInput;
    private Vector2 animationInput;

    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Animator animator;
    private Rigidbody2D playerRigidbody;

    private BasicWeapon weapon;
	private WeaponPickup nearestWeapon;

	void Start()
    {
        animationSpeed = playerSpeed / 5.0f;

		playerRigidbody = GetComponent<Rigidbody2D>();
        weapon = weaponSlot.GetComponentInChildren<BasicWeapon>();
        animator = GetComponent<Animator>();

        animator.speed = animationSpeed;
    }

    void FixedUpdate()
    {
        Vector2 newPosition = playerRigidbody.position + playerInput * playerSpeed * Time.fixedDeltaTime;
		playerRigidbody.MovePosition(newPosition);
    }

    public void Move(InputAction.CallbackContext inputContext)
    {

		animator.SetBool("isWalking", true);

        if(inputContext.canceled)
        {
			animator.SetBool("isWalking", false);
			animator.SetFloat("LastX", animationInput.x);
			animator.SetFloat("LastY", animationInput.y);
            weapon.setAttackDirection(animationInput);
		}
		playerInput = inputContext.ReadValue<Vector2>();
		animationInput = new Vector2(Mathf.Round(playerInput.x), Mathf.Round(playerInput.y));

		animator.SetFloat("X", animationInput.x);
        animator.SetFloat("Y", animationInput.y);


        if(animationInput != Vector2.zero)
			weapon.setAttackDirection(animationInput);
	}

    public void Attack(InputAction.CallbackContext inputContext)
    {
        if(inputContext.started)
            weapon.Attack();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<WeaponPickup>(out WeaponPickup pickup))
		{
			nearestWeapon = pickup;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.TryGetComponent<WeaponPickup>(out WeaponPickup pickup))
		{
			if (nearestWeapon == pickup)
			{
				nearestWeapon = null;
			}
		}
	}

	private void EquipWeapon(GameObject newPrefab)
	{
		if (weaponSlot == null)
		{
			weaponSlot = transform.Find("WeaponHolder");
		}

		foreach (Transform child in weaponSlot)
		{
			Destroy(child.gameObject);
		}

		GameObject newWeaponObj = Instantiate(newPrefab, weaponSlot);

		newWeaponObj.transform.localPosition = Vector3.zero;
		newWeaponObj.transform.localRotation = Quaternion.identity;
		newWeaponObj.transform.localScale = Vector3.one;

		weapon = newWeaponObj.GetComponent<BasicWeapon>();

		if (weapon != null)
		{
			weapon.setAttackDirection(animationInput);
		}
	}

	public void Interact(InputAction.CallbackContext inputContext)
	{
		if (inputContext.started && nearestWeapon != null)
		{
			Debug.Log(nearestWeapon);
			EquipWeapon(nearestWeapon.GetWeaponPrefab());

			Destroy(nearestWeapon.gameObject);
			nearestWeapon = null;
		}
	}
}
