using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFill : MonoBehaviour
{
    [SerializeField] Image CurrentImage;
    [SerializeField] Vector2 Target;
    [SerializeField] List<Image> OldImages;
    [SerializeField] float FillPerSecond;
    [SerializeField] bool ShootDart;
    public void SetCurrentImageToFill(Image newImage, Vector2 newTarget)
    {
        Target = newTarget;
        ShootDart = true;
        SetCurrent(newImage);
       
    }

    void SetCurrent(Image newImage)
    {
        if (CurrentImage == newImage)
            return;
        DartSticker.inst.SetVisible(false);
        if (OldImages.Contains(newImage))
            OldImages.Remove(newImage);
        if (CurrentImage != null)
            OldImages.Add(CurrentImage);
        CurrentImage = newImage;
        enabled = true;
    }

    public void SetCurrentImageToFill(Image newImage)
    {
        ShootDart = false;
        SetCurrent(newImage);
    }

    public void ClearImages()
    {
        enabled = false;
        CurrentImage.fillAmount = 0;
        CurrentImage = null;
        for (int i = OldImages.Count - 1; i >= 0; i--)
        {
            OldImages[i].fillAmount = 0;
            OldImages.RemoveAt(i);
        }
    }

    public void Update()
    {
        bool Complete = true;
        float dTime = Time.deltaTime * FillPerSecond;
        if (CurrentImage.fillAmount < 1)
        {
            CurrentImage.fillAmount = Mathf.MoveTowards(CurrentImage.fillAmount, 1, dTime);
            Complete = false;
            if (CurrentImage.fillAmount >= 1)
            {
                if (ShootDart)
                    DartSticker.inst.NewLocation(Target);
                Complete = true;
            }
            
        }
       
        for(int i=OldImages.Count-1;i>=0;i--)
        {
            OldImages[i].fillAmount = Mathf.MoveTowards(OldImages[i].fillAmount, 0, dTime);
            if (OldImages[i].fillAmount == 0)
                OldImages.RemoveAt(i);
        }
        if (OldImages.Count > 0)
            Complete = false;

        if (Complete)
            enabled = false;
    }

}
