using UnityEngine;
using UnityEngine.InputSystem;

public class Bathysphere : MonoBehaviour
{
    private Vector3 _moveInput;
    private Vector3 _pitchAndYawInput;
    private Vector3 _rollInput;

    private float _rotateSpeed;

    private PlayerControls _playerControls;

    public void Awake()
    {
        _moveInput = Vector3.zero;
        _pitchAndYawInput = Vector3.zero;
        _rollInput = Vector3.zero;

        _rotateSpeed = 100f;

        _playerControls = new PlayerControls();

        _playerControls.Player.Move.performed += Move;
        _playerControls.Player.Move.canceled += ctx => StopMovement();

        _playerControls.Player.Look.performed += Look;
        _playerControls.Player.Look.canceled += ctx => StopLooking();

        _playerControls.Player.Roll.performed += Roll;
        _playerControls.Player.Roll.canceled += ctx => StopRolling();
    }

    private void StopRolling()
    {
        _rollInput = Vector3.zero;
    }
    
    private void Roll(InputAction.CallbackContext ctx)
    {
        _rollInput = Vector3.forward * Time.deltaTime * _rotateSpeed;
        
        if (ctx.control.shortDisplayName == "RB")
        {
            _rollInput *= -1;
        }
    }

    private void StopLooking()
    {
        _pitchAndYawInput = Vector3.zero;
    }
    
    private void Look(InputAction.CallbackContext ctx)
    {
        Vector2 rightStickMovement = ctx.ReadValue<Vector2>();
        _pitchAndYawInput = new Vector3(rightStickMovement.y, rightStickMovement.x, 0) * Time.deltaTime * _rotateSpeed;
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

        if (_pitchAndYawInput != Vector3.zero || _rollInput != Vector3.zero)
        {
            gameObject.transform.Rotate(_pitchAndYawInput+_rollInput);
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