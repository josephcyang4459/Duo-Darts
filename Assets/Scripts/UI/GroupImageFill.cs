using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupImageFill : MonoBehaviour
{
    [SerializeReference] Fillable CurrentImage;
    [SerializeField] Vector2 Target;
    [SerializeReference] List<Fillable> OldImages =new();
    [SerializeField] float FillPerSecond;
    [SerializeField] bool ShootDart;

    public void SetCurrentImageToFill(Fillable newImage, Vector2 newTarget) {
        Target = newTarget;
        ShootDart = true;
        SetCurrent(newImage);
    }

    void SetCurrent(Fillable newImage) {
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

    public void SetCurrentImageToFill(Fillable newImage) {
        ShootDart = false;
        SetCurrent(newImage);
    }

    public void ClearImages() {
        enabled = false;
        if (CurrentImage != null)
            CurrentImage.SetFill(0);
        CurrentImage = null;
        for (int i = OldImages.Count - 1; i >= 0; i--) {
            OldImages[i].SetFill(0);
            OldImages.RemoveAt(i);
        }
    }

    public void Update() {
        bool Complete = true;
        float dTime = Time.deltaTime * FillPerSecond;
        if (CurrentImage.GetFill() < 1) {
            CurrentImage.SetFill(Mathf.MoveTowards(CurrentImage.GetFill(), 1, dTime));
            Complete = false;
            if (CurrentImage.GetFill() >= 1) {
                if (ShootDart)
                    DartSticker.inst.NewLocation(Target);
                Complete = true;
            }
        }

        for (int i = OldImages.Count - 1; i >= 0; i--) {
            OldImages[i].SetFill(Mathf.MoveTowards(OldImages[i].GetFill(), 0, dTime));
            if (OldImages[i].GetFill() == 0)
                OldImages.RemoveAt(i);
        }
        if (OldImages.Count > 0)
            Complete = false;

        if (Complete)
            enabled = false;
    }
}
[System.Serializable]
public abstract class Fillable {
    public abstract float GetFill();
    public abstract void SetFill(float value);
}
[System.Serializable]
public class Fillable_Image : Fillable {
    public Image[] Images;
    public override float GetFill() {
        return Images[0].fillAmount;
    }

    public override void SetFill(float value) {
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = value;
    }
}
[System.Serializable]
public class Fillable_SeeSaw : Fillable {
    public Image[] Images;
    public Image[] Images1;

    public override float GetFill() {
        return Images[0].fillAmount;
    }

    public override void SetFill(float value) {
        for (int i = 0; i < Images.Length; i++)
            Images[i].fillAmount = value;
        for (int i = 0; i < Images.Length; i++)
            Images1[i].fillAmount = 1 - value;
    }
}
