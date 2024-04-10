using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlide : MonoBehaviour
{
    [SerializeField] Image CurrentImage;
    [SerializeField] Vector3 StartingLocation;
    [SerializeField] Vector3 EndLocation;
    [SerializeField] AnimationCurve MoveSpeed;
    [SerializeField] float TotalDistance;
    [SerializeField] float AlphaSpeed;

    public void BeginSlide()
    {
        CurrentImage.transform.position = StartingLocation;
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
        CurrentImage.transform.position = StartingLocation;
    }

    public bool IsAtStart() {
        if (enabled)
            return false;
        return Vector3.Distance(CurrentImage.transform.position, StartingLocation) <= .00005f;
    }

    public void Update()
    {
        float distanceRemaining = Vector3.Distance(CurrentImage.transform.position, EndLocation);
        if (distanceRemaining < .005f)
            enabled = false;
        float dTime = Time.deltaTime;
      
        CurrentImage.transform.position = Vector3.MoveTowards(CurrentImage.transform.position, EndLocation, MoveSpeed.Evaluate(distanceRemaining/TotalDistance) * dTime);
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
        StartingLocation = __StartingLocation.position;
        EndLocation = __EndLocation.position;
        TotalDistance = Vector3.Distance(StartingLocation, EndLocation);
        float temp = 1 / __SecondsToTake / __reachFullAlphaAtPercent;
        AlphaSpeed = temp;
    }
#endif
}
