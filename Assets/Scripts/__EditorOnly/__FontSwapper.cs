#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class __FontSwapper : MonoBehaviour
{
    [System.Serializable]
    class AssetGroup {
        public GameObject TopParent;
        public List<TMP_Text> Texts;
        public bool replace;
    }
    [SerializeField] TMP_FontAsset Font;
    [SerializeField] List<AssetGroup> AssetGroups;
    [SerializeField] int ParentLayersUp;
    [SerializeField] bool GrabGroups;
    [SerializeField] bool EditGroups;

    Transform GetParentLayer(Transform t) {
        Transform parentTransform = t;
        for (int i = 0; i < ParentLayersUp; i++)
            parentTransform = parentTransform.parent;
        return parentTransform;
    }
   
    public int CheckForGroup(GameObject g) {
        for (int i=0;i<AssetGroups.Count;i++) {
            if (AssetGroups[i].TopParent == g)
                return i;
        }
        return -1;
    }

    void GrabGroupsF() {
        AssetGroups = new();
        TMP_Text[] textAssets = FindObjectsOfType<TMP_Text>();
        if (textAssets.Length == 0)
            return;
        
        for(int i = 0; i < textAssets.Length; i++) {
            GameObject g = GetParentLayer(textAssets[i].transform).gameObject;
            int index = CheckForGroup(g);
            if (index > -1) {
                AssetGroups[index].Texts.Add(textAssets[i]);
            }
            else {
                AssetGroup newAssetGroup = new();
                newAssetGroup.TopParent = g;
                newAssetGroup.Texts = new();
                newAssetGroup.Texts.Add(textAssets[i]);
                AssetGroups.Add(newAssetGroup);
            }
        }
    }

    void EditGroupsF() {
        foreach(AssetGroup group in AssetGroups) {
            if (group.replace) {
                foreach(TMP_Text text in group.Texts) {
                    text.font = Font;
                }
            }
        }
    }


    private void OnValidate() {
        if (GrabGroups) {
            GrabGroups = false;
            GrabGroupsF();
        }

        if (EditGroups) {
            EditGroups = false;
            EditGroupsF();
        }
    }
}
#endif
