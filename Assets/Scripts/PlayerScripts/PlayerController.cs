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

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Move
        Vector2 moveInput = Vector2.zero;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) moveInput.y += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) moveInput.y -= 1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput.x -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput.x += 1f;

        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        playerInput = moveInput;
        animationInput = new Vector2(Mathf.Round(playerInput.x), Mathf.Round(playerInput.y));

        if (moveInput.sqrMagnitude > 0.01f)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("X", animationInput.x);
            animator.SetFloat("Y", animationInput.y);
            weapon.setAttackDirection(animationInput);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastX", animationInput.x);
            animator.SetFloat("LastY", animationInput.y);
        }

        // Attack
        if (kb.spaceKey.wasPressedThisFrame || Mouse.current?.leftButton.wasPressedThisFrame == true)
            weapon.Attack();

        // Interact
        if (kb.eKey.wasPressedThisFrame && nearestWeapon != null)
        {
            EquipWeapon(nearestWeapon.GetWeaponPrefab());
            Destroy(nearestWeapon.gameObject);
            nearestWeapon = null;
        }
    }

    void FixedUpdate()
    {
        playerRigidbody.linearVelocity = playerInput * playerSpeed;
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
}
