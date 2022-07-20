using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public void Move(InputAction.CallbackContext context) {
        Vector2 leftStickMovement = Gamepad.current.leftStick.ReadValue()* new Vector2(0.10f, 0.10f);

        Vector3 localPos = gameObject.transform.localPosition;
        Vector3 newPos = new Vector3(localPos.x+leftStickMovement.x, localPos.y, localPos.z+leftStickMovement.y);
        Debug.Log("OLD" + localPos + " NEW: " + newPos);

        gameObject.transform.Translate(newPos, Space.World);
    }
}
