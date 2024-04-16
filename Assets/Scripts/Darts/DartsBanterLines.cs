using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Banter Lines", menuName ="Reference/Darts Banter Lines")]
public class DartsBanterLines : ScriptableObject
{
    /*
i figure it would randomly read off lines. though we should not let lines be read off twice within on game of darts.
    also a banter line doesn't need to read off every single time. 
    Only for special ones, like getting 0 or getting 180 should always have a line be read of (unless there are no 'fresh' lines remaining within the case). 
    The other cases can have a chance to read of a line (60/40 favoring reading off a line) 
     */
    [SerializeField] LineGroup[] LineGroups;

    [SerializeField] LineGroup CheckOut;

    public void ResetAllGroups() {
        for (int i = 0; i < LineGroups.Length; i++)
            LineGroups[i].ResetUsedLines();
        CheckOut.ResetUsedLines();
    }

    public string GetLineFromScoreGroup(int score) {
        foreach (LineGroup g in LineGroups)
            if (g.InRange(score))
                return g.GetUnusedLine();
        return null;
    }

    public string GetCheckoutLine() {
        if (CheckOut == null)
            return null;
        return CheckOut.GetUnusedLine();
    }


    [System.Serializable]
    class LineGroup {
        public int MinScoreRangeInclusive;
        public int MaxScoreRangeExclusive;
        [Range(0, 100f)] public float ChanceToUse; 
        public string[] Lines;
        public bool[] AlreadyUsed;
        [SerializeField] int Used;

        public bool InRange(int score) {
            if (score < MinScoreRangeInclusive)
                return false;
            if (score >= MaxScoreRangeExclusive)
                return false;
            return true;
        } 

        public void ResetUsedLines() {
            Used = 0;
            for (int i = 0; i < AlreadyUsed.Length; i++)
                AlreadyUsed[i] = false;
        }

        public string GetUnusedLine() {
            if (ChanceToUse < Random.Range(0, 100))
                return null;

            if (Used == Lines.Length)
                return null;

            int lineIndex = Random.Range(0, Lines.Length);
            while (AlreadyUsed[lineIndex]) {
                lineIndex++;
                if (lineIndex >= Lines.Length)
                    lineIndex = 0;
            }
            AlreadyUsed[lineIndex] = true;
            Used++;
            return Lines[lineIndex];
        }

#if UNITY_EDITOR
        public void __AddToLines(string s) {
            for (int i = 0; i < Lines.Length; i++)
                if (Lines[i] == null) {
                    Lines[i] = s;
                    return;
                }
        }
#endif
    }

#if UNITY_EDITOR
    [SerializeField] List<string> __groups = new();
    string __GetGroupName(string line) {
        return line.Substring(line.IndexOf('['), line.IndexOf(']') - line.IndexOf('['));
    }

    void SetScoreRange(int groupIndex, string name) {
        if (name.ToLower().Contains("check")) {
            return;
        }
        if (name.ToLower().Contains("high")) {
            LineGroups[groupIndex].MinScoreRangeInclusive = 140;
            LineGroups[groupIndex].MaxScoreRangeExclusive = 180;
            LineGroups[groupIndex].ChanceToUse = 60;
            return;
        }
        if (name.ToLower().Contains("70")) {
            LineGroups[groupIndex].MinScoreRangeInclusive = 1;
            LineGroups[groupIndex].MaxScoreRangeExclusive = 70;
            LineGroups[groupIndex].ChanceToUse = 60;
            return;
        }
        if (name.ToLower().Contains("low")) {
            LineGroups[groupIndex].MinScoreRangeInclusive = 100;
            LineGroups[groupIndex].MaxScoreRangeExclusive = 139;
            LineGroups[groupIndex].ChanceToUse = 60;
            return;
        }
        if (name.ToLower().Contains("180")) {
            LineGroups[groupIndex].MinScoreRangeInclusive = 180;
            LineGroups[groupIndex].MaxScoreRangeExclusive = 181;
            LineGroups[groupIndex].ChanceToUse = 100;
            return;
        }
        if (name.ToLower().Contains("0")) {
            LineGroups[groupIndex].MinScoreRangeInclusive = 0;
            LineGroups[groupIndex].MaxScoreRangeExclusive = 1;
            LineGroups[groupIndex].ChanceToUse = 100;
            return;
        }
    }

    public void __ResetLines(string[] lines) {

        __groups = new();
        List<int> linesInGroup = new();
        __groups.Add(__GetGroupName(lines[000]));
        linesInGroup.Add(1);
        for (int i = 1; i < lines.Length; i++) {
            string current = __GetGroupName(lines[i]);
            if (__groups.Contains(current)) {
                linesInGroup[__groups.IndexOf(current)]++;
            }
            else {
                __groups.Add(current);
                linesInGroup.Add(1);
            }
        }

        if (__groups[__groups.Count - 1].ToLower().Contains("check")) {
            LineGroups = new LineGroup[__groups.Count - 1];
            for (int i = 0; i < LineGroups.Length; i++) {
                LineGroup temp = new();
                temp.Lines = new string[linesInGroup[i]];
                temp.AlreadyUsed = new bool[linesInGroup[i]];
                LineGroups[i] = temp;
            }
            CheckOut = new();
            CheckOut.ChanceToUse = 100;
            CheckOut.Lines = new string[linesInGroup[__groups.Count - 1]];
            CheckOut.AlreadyUsed = new bool[linesInGroup[__groups.Count - 1]];
        }
           
        else {
            LineGroups = new LineGroup[__groups.Count];
            for (int i = 0; i < LineGroups.Length; i++) {
                LineGroup temp = new();
                temp.Lines = new string[linesInGroup[i]];
                temp.AlreadyUsed = new bool[linesInGroup[i]];
                LineGroups[i] = temp;
            }
        }
        

        for(int i = 0; i < lines.Length; i++) {
            int index = __groups.IndexOf(__GetGroupName(lines[i]));
                if(!__GetGroupName(lines[i]).ToLower().Contains("check"))
            LineGroups[index].__AddToLines(lines[i].Substring(lines[i].LastIndexOf(']')+1));
            else {
                CheckOut.__AddToLines(lines[i].Substring(lines[i].LastIndexOf(']') + 1));
            }
        }

       

        for (int i = 0; i < __groups.Count; i++) {
            SetScoreRange(i, __groups[i]);
        }

        Debug.Log(name + " RESET ======>");
    }

#endif
}
