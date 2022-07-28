using UnityEngine;
using UnityEngine.InputSystem;

public class Bathysphere : MonoBehaviour
{
    private Vector3 _moveInput;

    private PlayerControls _playerControls;

    public void Awake()
    {
        _moveInput = Vector3.zero;
        _playerControls = new PlayerControls();

        _playerControls.Player.Move.performed += Move;
        _playerControls.Player.Move.canceled += ctx => StopMovement();
    }

    private void StopMovement()
    {
        _moveInput = Vector2.zero;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 leftStickMovement = ctx.ReadValue<Vector2>();
        _moveInput = new Vector3(CalculateMovement(leftStickMovement.x), 0, CalculateMovement(leftStickMovement.y));
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void FixedUpdate()
    {
        if (_moveInput != Vector3.zero)
        {
            gameObject.transform.Translate(_moveInput);
        }
    }


    /* CalculateMovement
     * Always document math even if its high school algebra
     * movement = time * velocity ^ 2 + velocity
     */
    private static float CalculateMovement(float velocity)
    {
        float multipliedVelocity = velocity * 0.5f;
        float movement = Time.fixedDeltaTime * Mathf.Pow(multipliedVelocity, 2) + multipliedVelocity;
        return (velocity < 0 && movement > 0) ? movement * -1f : movement;
    }
}