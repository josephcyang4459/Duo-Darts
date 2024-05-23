using UnityEngine;
[CreateAssetMenu(fileName ="New Player Notification", menuName ="Cutscene/Player Notification")]
public class PlayerNotification : CutScene
{

#if UNITY_EDITOR
    [SerializeField] TextAsset TextAsset;
    [TextArea(1,5)] [SerializeField] string[] ThoughtStrings;
    [SerializeField] bool UseTextFile;
    public bool reset;

    void UsingTextFile() {
        __resetCutScene(TextAsset.text.Split("\n"));
    }

    void UsingArray() {
        blocks = new block[ThoughtStrings.Length];

        for (int i = 0; i < ThoughtStrings.Length; i++) {
            Thought temp = new();
            temp.thoughtMessage = ThoughtStrings[i];
            blocks[i] = temp;
            }
    }

    private void OnValidate() {
        TimeLength = TimeBlocks.Notification;
        if (reset) {
            reset = false;
            if (UseTextFile)
                UsingTextFile();
            else
                UsingArray();
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
