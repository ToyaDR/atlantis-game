using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour {
    Vector3 _moveInput;

    public void Awake() {
        _moveInput = Vector3.zero;
    }

    public void Move(InputAction.CallbackContext context) {
        if (Gamepad.current.leftStick.CheckStateIsAtDefault())
        {
            _moveInput = Vector3.zero;
            return;
        }

        Vector2 leftStickMovement = Gamepad.current.leftStick.ReadValue();

        float forwardVelocity = 5*leftStickMovement.x;
        float rightVelocity = 5*leftStickMovement.y;
        
        Vector3 moveForward = new Vector3(forwardVelocity * Time.fixedDeltaTime, 0, 0);
        Vector3 moveRight = new Vector3(0, 0 , rightVelocity * Time.fixedDeltaTime);
        
        _moveInput = moveForward+moveRight;
    }

    public void FixedUpdate() {
        if (_moveInput != Vector3.zero) {
            gameObject.transform.Translate(_moveInput);
        }
    }
}
