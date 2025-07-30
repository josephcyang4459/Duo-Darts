using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageJiggle : MonoBehaviour {
    [SerializeField] RectTransform Transform;
    [SerializeField] Transform OriginalPosition;
    [SerializeField] Vector3 NewPosition;
    [SerializeField] Vector3 AdjustedNewPosition;
    [SerializeField] Vector3 AdjustedOriginalPosition;
    [SerializeField] float speed = .05f;
    [SerializeField] Vector2 Limits;
    [SerializeField] bool Jiggling;

    private void Update() {
        Jiggle();
    }

    void Jiggle() {
        Transform.position = Vector3.MoveTowards(Transform.position, NewPosition, speed * Time.deltaTime);
        if (Vector3.Distance(Transform.position, NewPosition) < .0005f) {
            if (!Jiggling) {
                Transform.position = OriginalPosition.position;
                enabled = false;
                return;
            }
            SetNewPosition();
        }
    }

    void SetNewPosition() {
        NewPosition.x = OriginalPosition.position.x + Random.Range(-Limits.x, Limits.x);
        NewPosition.y = OriginalPosition.position.y + Random.Range(-Limits.y, Limits.y);
    }

    public void StartJiggle() {
        Jiggling = true;
        SetNewPosition();
        enabled = true;
    }

    public void EndJiggle() {
        NewPosition.x = OriginalPosition.position.x;
        NewPosition.y = OriginalPosition.position.y;
        Jiggling = false;
    }

#if UNITY_EDITOR
    [SerializeField] bool __reset;
    private void OnValidate() {
        if (!__reset)
            return;
        __reset = false;
        if (Transform == null)
            Transform = (RectTransform)gameObject.transform;

        NewPosition = OriginalPosition.position;
    }
#endif
}
