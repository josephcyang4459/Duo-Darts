using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlide : MonoBehaviour
{
    [SerializeField] Image CurrentImage;
    [SerializeField] Transform StartingLocation;
    [SerializeField] Transform EndLocation;
    [SerializeField] AnimationCurve MoveSpeed;
    [SerializeField] float TotalDistance;
    [SerializeField] float AlphaSpeed;

    public void BeginSlide()
    {
        TotalDistance = Vector3.Distance(StartingLocation.position, EndLocation.position);// * Screen.width / 1920f;
        CurrentImage.transform.position = StartingLocation.position;
        Color c = CurrentImage.color;
        c.a = 0;
        CurrentImage.color = c;
        enabled = true;
    }

    public void SetNewImage(Image newImage)
    {
        CurrentImage = newImage;
    }

    public void SetToStart() {
        enabled = false;
        CurrentImage.transform.position = StartingLocation.position;
    }

    public bool IsAtStart() {
        if (enabled)
            return false;
        return Vector3.Distance(CurrentImage.transform.position, StartingLocation.position) <= .00005f;
    }

    public void Update()
    {
        float distanceRemaining = Vector3.Distance(CurrentImage.transform.position, EndLocation.position);
        if (distanceRemaining < .005f)
            enabled = false;
        float dTime = Time.deltaTime;
        float speed = MoveSpeed.Evaluate(distanceRemaining / TotalDistance) * Screen.width / 1920f * dTime;
        CurrentImage.transform.position = Vector3.MoveTowards(CurrentImage.transform.position, EndLocation.position, speed);
        if (CurrentImage.color.a < 1)
        {
            Color c = CurrentImage.color;
            c.a += AlphaSpeed * dTime;
            CurrentImage.color = c;
        }


    }


#if UNITY_EDITOR
    [SerializeField] Transform __StartingLocation;
    [SerializeField] Transform __EndLocation;
    [SerializeField] bool __reset;
    [SerializeField] [Range(.1f,2f)] float __SecondsToTake =1;
    [SerializeField] [Range(.1f, 1f)] float __reachFullAlphaAtPercent =1;
    private void OnValidate()
    {
        if (!__reset)
            return;
        __reset = false;
        float temp = 1 / __SecondsToTake / __reachFullAlphaAtPercent;
        AlphaSpeed = temp;
    }
#endif
}
