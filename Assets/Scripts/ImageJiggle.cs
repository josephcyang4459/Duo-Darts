using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageJiggle : MonoBehaviour
{
    [SerializeField] RectTransform Transform;
    [SerializeField] Vector3 OriginalPosition;
    [SerializeField] Vector3 NewPosition;
    [SerializeField] float speed = .05f;
    [SerializeField] Vector2 Limits;
    [SerializeField] bool Jiggling;

    private void Update()
    {
        Jiggle();
    }

    void Jiggle()
    {
        
        Transform.position = Vector3.MoveTowards(Transform.position, NewPosition, speed * Time.deltaTime);
        if(Vector3.Distance(Transform.position, NewPosition) < .0005f)
        {
            if (!Jiggling)
            {
                Transform.position = OriginalPosition;
                enabled = false;
                return;
            }
            SetNewPosition();
        }
    }
    
    void SetNewPosition()
    {
        NewPosition.x = OriginalPosition.x + Random.Range(-Limits.x, Limits.x);
        NewPosition.y = OriginalPosition.y + Random.Range(-Limits.y, Limits.y);
    }

    public void StartJiggle()
    {
        Jiggling = true;
        SetNewPosition();
        enabled = true;
    }

    public void EndJiggle()
    {
        NewPosition.x = OriginalPosition.x;
        NewPosition.y = OriginalPosition.y;
        Jiggling = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Transform == null)
            Transform = (RectTransform)gameObject.transform;

        OriginalPosition = Transform.position;
        NewPosition = OriginalPosition;
    }
#endif

}
