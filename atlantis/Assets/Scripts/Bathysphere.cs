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
        _playerControls.Player.Move.canceled += ctx => Stop();
    }

    /* CalculateMovement
     * Always document math even if its high school algebra
     * movement = time * velocity ^ 2 + velocity
     */
    private static float CalculateMovement(float velocity)
    {
        float movement = Time.fixedDeltaTime * Mathf.Pow(velocity, 2) + velocity;
        return (velocity < 0 && movement > 0) ? movement * -1f : movement;
    }

    private void Stop()
    {
        _moveInput = Vector2.zero;
    }
    
    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 leftStickMovement = ctx.ReadValue<Vector2>();
        
        /* NOTE FOR PROGRAMMER SANITY.
         *  Apparently Unity InputSystem considers x the forward/backward movement on the joystick
         *  and y the left to right
         *      x
         *      ^
         *      |
         * < - - - - > y 
         *     |
         *     v
         */
        
        float velocityMult = 0.5f;
        
        Vector3 moveForward = new Vector3(CalculateMovement(velocityMult * leftStickMovement.x), 0, 0);
        Vector3 moveRight = new Vector3(0, 0, CalculateMovement(velocityMult * leftStickMovement.y)); 
        
        // Vector3 moveForward = new Vector3(velocityMult * leftStickMovement.x, 0, 0);
        // Vector3 moveRight = new Vector3(0, 0, velocityMult * leftStickMovement.y);
        
        _moveInput = moveForward + moveRight;
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
}