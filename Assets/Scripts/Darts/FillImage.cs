using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillImage : MonoBehaviour
{
    [SerializeField] Image CurrentImage;
    [SerializeField] Vector2 Target;
    [SerializeField] List<Image> OldImages;
    [SerializeField] float FillPerSecond;

    public void SetCurrentImageToFill(Image newImage, Vector2 newTarget)
    {
        if (CurrentImage != null)
            OldImages.Add(CurrentImage);
        CurrentImage = newImage;
        enabled = true;
    }

    public void Update()
    {
        bool Complete = true;
        float dTime = Time.deltaTime * FillPerSecond;
        if (CurrentImage.fillAmount < 1)
        {
            CurrentImage.fillAmount = Mathf.MoveTowards(CurrentImage.fillAmount, 1, dTime);
            if (CurrentImage.fillAmount < 1)
                Complete = false;
        }
       
        for(int i=OldImages.Count-1;i>=0;i--)
        {
            if (OldImages[i].fillAmount > 0)
            {
                OldImages[i].fillAmount = Mathf.MoveTowards(OldImages[i].fillAmount, 0, dTime);
                if (CurrentImage.fillAmount > 0)
                    Complete = false;
                else
                    OldImages.RemoveAt(i);
            }
            
        }
        if (Complete)
            enabled = false;
    }

}
