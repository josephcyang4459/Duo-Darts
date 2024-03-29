using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSmoothFill : MonoBehaviour
{
    [SerializeField] Image CurrentImage;
    [SerializeField] float SecondToReach;
    [SerializeField] float TargetFill;
    [SerializeField] float Speed;

    public void FillTo(float newTargetFill)
    {
        TargetFill = newTargetFill;
        Speed = Mathf.Abs(TargetFill - CurrentImage.fillAmount) / SecondToReach;
        enabled = true;
    }

    public void SetCurrentImageToFill(Image newImage)
    {
        CurrentImage = newImage;
        //FillTo(targetFill);
    }

    public void Update()
    {
        CurrentImage.fillAmount = Mathf.MoveTowards(CurrentImage.fillAmount, TargetFill, Time.deltaTime * Speed);
        if (CurrentImage.fillAmount == TargetFill)
        {


            enabled = false;
        }

     
    }
}
