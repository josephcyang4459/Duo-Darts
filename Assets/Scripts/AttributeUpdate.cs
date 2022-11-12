using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUpdate : MonoBehaviour {
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private RectTransform AttributeTransform;
    [SerializeField] private GameObject AttributePopUpPosition;
    [SerializeField] private int FadeSpeed = 1;
    [SerializeField] private int StayDuration = 1;
    private bool FadeIn = false;
    private bool FadeOut = false;
    private bool InView = false;
    private float InViewStartTime;
    private float transformMuliplier;

    public void Start()
    {
        transformMuliplier = AttributeTransform.position.y - AttributePopUpPosition.transform.position.y;
    }

    public void ShowUI() {
        FadeIn = true;
    }

    public void HideUI() {
        FadeOut = true;
    }

    private void Update() {
        Debug.Log("X: " + AttributeTransform.position.x + " | Y: " + AttributeTransform.position.y);
        //Debug.Log("InViewStartTime: " + InViewStartTime + " | CurrentTime: " + Time.time);
        if(FadeIn)
            if(CanvasGroup.alpha < 1) {
                CanvasGroup.alpha += Time.deltaTime * FadeSpeed;
                AttributeTransform.position = Vector3.MoveTowards(AttributeTransform.position, AttributePopUpPosition.transform.position, Time.deltaTime * transformMuliplier);
                //AttributeTransform.position = new Vector2(AttributeTransform.position.x, AttributeTransform.position.y + Time.deltaTime * 1000);
                if (CanvasGroup.alpha >= 1) {
                    FadeIn = false;
                    InView = true;
                    InViewStartTime = Time.time;
                }
            }
        if (FadeOut)
            if (CanvasGroup.alpha >= 0) {
                CanvasGroup.alpha -= Time.deltaTime * FadeSpeed;
                Debug.Log("temp: " + transformMuliplier);
                AttributeTransform.position = Vector3.MoveTowards(AttributeTransform.position, AttributePopUpPosition.transform.position, Time.deltaTime * transformMuliplier);
                //AttributeTransform.position = new Vector2(AttributeTransform.position.x, AttributeTransform.position.y - Time.deltaTime * 1000);
                if (CanvasGroup.alpha == 0) {
                    FadeOut = false;
                    InView = false;
                }
            }

        if(InView && Time.time >= InViewStartTime + StayDuration)
            HideUI();
    }

    public void DisplayAttribute() {

    }

    public void UpdateAttribute() {

    }
}
