using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    [SerializeField] float RotationMax;
    [SerializeField] float RotationMin;
    [SerializeField] float Speed;
    [SerializeField] Transform Rotate;
    float currentRotation;
    float targetRotation;
    public void SetRotate(bool b) {
        enabled = b;
        if (b) {
            currentRotation = Rotate.localEulerAngles.z;
        }
    }

    public void Update() {
        currentRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, Speed * Time.deltaTime);
        Rotate.rotation = Quaternion.Euler(0, 0, currentRotation);
        if (Mathf.Approximately(currentRotation, targetRotation)) {
            targetRotation = targetRotation == RotationMax ? RotationMin : RotationMax;
        }
    }
}
