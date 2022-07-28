using UnityEngine;
using UnityEngine.InputSystem;

public class Bathysphere : MonoBehaviour
{
    private Vector3 _horizontalInput;
    private Vector3 _verticalInput;

    private Vector3 _pitchAndYawInput;
    private Vector3 _rollInput;

    private float _rotateSpeed;

    private PlayerControls _playerControls;

    public void Awake()
    {
        _horizontalInput = Vector3.zero;
        _verticalInput = Vector3.zero;

        _pitchAndYawInput = Vector3.zero;
        _rollInput = Vector3.zero;

        _rotateSpeed = 100f;

        _playerControls = new PlayerControls();

        _playerControls.Player.Move.performed += MoveHorizontally;
        _playerControls.Player.Move.canceled += ctx => StopHorizontalMovement();

        _playerControls.Player.Boost.performed += MoveVertically;
        _playerControls.Player.Boost.canceled += ctx => StopVerticalMovement();

        _playerControls.Player.Look.performed += RotateWithPitchAndYaw;
        _playerControls.Player.Look.canceled += ctx => StopPitchAndYawRotation();

        _playerControls.Player.Roll.performed += RotateWithRoll;
        _playerControls.Player.Roll.canceled += ctx => StopRollRotation();
    }
    
    private void StopVerticalMovement()
    {
        _verticalInput = Vector3.zero;
    }

    private void MoveVertically(InputAction.CallbackContext ctx)
    {
        if (ctx.control.shortDisplayName == "RT" && ctx.control.shortDisplayName == "LT")
        {
            return;
        }
        
        _verticalInput = Vector3.up * Time.deltaTime * 5f;
        
        if (ctx.control.shortDisplayName == "RT")
        {
            _verticalInput *= -1;
        }
    }

    private void StopRollRotation()
    {
        _rollInput = Vector3.zero;
    }
    
    private void RotateWithRoll(InputAction.CallbackContext ctx)
    {
        if (ctx.control.shortDisplayName == "RB" && ctx.control.shortDisplayName == "LB")
        {
            return;
        }
        
        _rollInput = Vector3.forward * Time.deltaTime * _rotateSpeed;
        
        if (ctx.control.shortDisplayName == "RB")
        {
            _rollInput *= -1;
        }
    }

    private void StopPitchAndYawRotation()
    {
        _pitchAndYawInput = Vector3.zero;
    }
    
    private void RotateWithPitchAndYaw(InputAction.CallbackContext ctx)
    {
        Vector2 rightStickMovement = ctx.ReadValue<Vector2>();
        _pitchAndYawInput = new Vector3(rightStickMovement.y, rightStickMovement.x, 0) * Time.deltaTime * _rotateSpeed;
    }

    private void StopHorizontalMovement()
    {
        _horizontalInput = Vector3.zero;
    }

    private void MoveHorizontally(InputAction.CallbackContext ctx)
    {
        Vector2 leftStickMovement = ctx.ReadValue<Vector2>();
        _horizontalInput = new Vector3(CalculateMovement(leftStickMovement.x), 0, CalculateMovement(leftStickMovement.y));
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
        if (_horizontalInput != Vector3.zero || _verticalInput != Vector3.zero)
        {
            gameObject.transform.Translate(_horizontalInput+_verticalInput);
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