using UnityEngine;
using UnityEngine.InputSystem;

public class Player_controller : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    private Rigidbody2D playerRigidbody;
    private Vector2 playerInput;
    [SerializeField] private Transform weaponSlot;
    private BasicWeapon weapon;
    
    void Start()
    {
		playerRigidbody = GetComponent<Rigidbody2D>();
        weapon = weaponSlot.GetComponentInChildren<BasicWeapon>();
    }

    void FixedUpdate()
    {
        Vector2 newPosition = playerRigidbody.position + playerInput * playerSpeed * Time.fixedDeltaTime;
		playerRigidbody.MovePosition(newPosition);
    }

    public void Move(InputAction.CallbackContext inputContext)
    {
        playerInput = inputContext.ReadValue<Vector2>();
    }

    public void Attack(InputAction.CallbackContext inputContext)
    {
        if(inputContext.started)
            weapon.Attack();
    }
}
