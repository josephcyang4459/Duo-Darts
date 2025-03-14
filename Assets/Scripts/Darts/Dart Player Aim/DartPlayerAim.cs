using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DartPlayerAim : MonoBehaviour
{
    [SerializeField] DartsSettings Settings;
    [SerializeField] Player Player;
    [SerializeField] SpriteRenderer AimSprite;
    [SerializeField] Transform AimPosition;
    public Vector3 CurrentLocation = new Vector3(0, 0, -.2f);
    [SerializeField] Vector3 StartingLocation = new Vector3(0, 0, -.2f);
    [SerializeField] Transform[] StartingLocations;
    [SerializeField] InputActionReference fire;
    [SerializeField] DartPlayerAim_Drift Drift;
    [SerializeField] DartPlayerAim_Control Control;
    [SerializeField] DartPlayerAim_Bloom Bloom;
    [SerializeField] DartScript Dart;
    [SerializeField] Vector2 XScreenClamp;
    [SerializeField] Vector2 YScreenClamp;
    [SerializeField] DartBoard Board;
#if UNITY_EDITOR
    [SerializeField] InputActionReference __SIxty;
    [SerializeField] Transform __sixtyLocation;
#endif
    public void ChangeLocation(float x, float y) {
        CurrentLocation.x += x;
        CurrentLocation.y += y;
    }

    public void SetUpDependants() {
        Drift.SetUp(Player.Intoxication, Player.Skill);
        Bloom.SetUp();
    }

#if UNITY_EDITOR
    public void __setup() {
        __SIxty.action.Enable();
        __SIxty.action.performed += __sixty;
    }

    public void __unsetup() {
        __SIxty.action.Disable();
        __SIxty.action.performed -= __sixty;
    }

    public void __sixty(InputAction.CallbackContext c) {
        ShootDart(__sixtyLocation.position-Vector3.forward);
    }
#endif

    public void BeginPlayerAim() {
        AimSprite.enabled = true;
        Vector3 position= StartingLocations[Random.Range(0, StartingLocations.Length)].position;
        CurrentLocation.x = position.x;
        CurrentLocation.y = position.y;
        AimPosition.position = CurrentLocation;
        Control.BeginMovement();
        fire.action.Enable();
        fire.action.performed += Shoot;
        enabled = true;
#if UNITY_EDITOR
        __setup();
#endif
    }

    public void Shoot(InputAction.CallbackContext c) {
        if (PauseMenu.inst.CurrentState)
            return;
        Vector3 temp = CurrentLocation;
        float bloomRange = Bloom.GetCurrentBloom()/2;
        float xOffset = Random.Range(-bloomRange, bloomRange);
        float yOffsetMax = Mathf.Sqrt((bloomRange * bloomRange) - (xOffset * xOffset));
        float yOffset = Random.Range(-yOffsetMax, yOffsetMax);
        temp.x += xOffset;
        temp.y += yOffset;
        ShootDart(temp);
    }

    public void ShootDart(Vector3 location) {
        EndPlayerAim();
        Dart.ShootDart(location, Board.GetScoreFromLocation(location.x, location.y));
        enabled = false;
        /*
        if (Physics.Raycast(location, Vector3.forward, out RaycastHit hit,12, Settings.DartsLayerMask)) {
            hit.collider.gameObject.GetComponent<BoardCollider>().hit(location);
        }
        else {
            Dart.ShootDart(location, 0);
        }
        */

    }

    public void EndPlayerAim() {
#if UNITY_EDITOR
        __unsetup();
#endif
        Control.End();
        fire.action.Disable();
        fire.action.performed -= Shoot;
        AimSprite.enabled = false;
        enabled = false;
    }

    public void Update() {
        Drift.UpdateDrift(Time.deltaTime);
        Control.UpdateMove(Time.deltaTime);
        Bloom.UpdateBloom(Time.deltaTime);
        CurrentLocation.x = Mathf.Clamp(CurrentLocation.x, XScreenClamp.x, XScreenClamp.y);
        CurrentLocation.y = Mathf.Clamp(CurrentLocation.y, YScreenClamp.x, YScreenClamp.y);
        AimPosition.position = CurrentLocation;
    }

    public void OnDestroy() {
        EndPlayerAim();
    }
}
