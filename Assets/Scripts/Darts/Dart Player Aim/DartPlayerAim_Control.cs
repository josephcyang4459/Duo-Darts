using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DartPlayerAim_Control : MonoBehaviour
{
    [SerializeField] DartPlayerAim Aim;
    public InputActionReference move;

    public float MoveSpeed;
    public Vector2 cache = Vector2.zero;

    public void BeginMovement() {
        

        move.action.Enable();
        move.action.performed += Move;
        move.action.canceled += StopMovement;
        /*fire.action.Enable();
        fire.action.performed += shoot;*/

    }

    public void End() {

        move.action.Disable();
        move.action.performed -= Move;
        move.action.canceled -= StopMovement;
/*
        fire.action.Disable();
        fire.action.performed -= shoot;*/
    }

    public void StopMovement(InputAction.CallbackContext c) {
        cache = Vector2.zero;
    }

    public void Move(InputAction.CallbackContext c) {
        cache = c.ReadValue<Vector2>();
        cache.x *= MoveSpeed;
        cache.y *= MoveSpeed;
    }

    public void UpdateMove(float dTime) {
        Aim.ChangeLocation(cache.x * dTime, cache.y * dTime);
        
    }
}
