#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene Parser", menuName = "Reference/Cutscene Parser")]
public class __CutsceneParser : ScriptableObject
{
    [SerializeField] CutScene[] Cutscenes;
    [SerializeField] TextAsset Script;

    [SerializeField] bool TryParse;
    [SerializeField] bool SendToCutscenes;
    [SerializeField] List<ParserChunk> ChunkData = new();

    private void OnValidate() {
        if (TryParse) {
            TryParse = false;
            ParseFile();
        }
    }

    void SendToCutscenesFunction(string[] s) {

        for (int chunkIndex = 0; chunkIndex < Cutscenes.Length; chunkIndex++) {

            string[] temp = new string[ChunkData[chunkIndex].ChunkEnd - ChunkData[chunkIndex].ChunkStart+1];
            for (int tempArrayIndex = 0, fullArrayIndex =ChunkData[chunkIndex].ChunkStart; tempArrayIndex < temp.Length; tempArrayIndex++,fullArrayIndex++)
                temp[tempArrayIndex] = s[fullArrayIndex];
            Cutscenes[chunkIndex].__resetCutScene(temp);
        }

    }

    int FindNextChunkStart(int currentEnd, string[] s) {
        for (int i = currentEnd; i < s.Length; i++) {
            if (HasTag(s[i])) {
                return i;
            }
               
        }
        return -1;
    }

    /*int FindNextChunkEnd(int currentStart, string[] s) {
        int i = FindNextPossibleChunkEnd(currentStart, s);
        int i1 = FindNextChunkStart(i, s);
        if (Mathf.Abs(i - i1) > 2)
            return i;
        return FindNextChunkEnd(i1,s);
    }*/

    int FindNextPossibleChunkEnd(int currentStart, string[] s) {
        for (int i = currentStart; i < s.Length; i++) {
            if (!HasTag(s[i]))
                return i;
        }
        return -1;
    }

    bool HasTag(string s) {
        if (!Extant(s))
            return false;
        return s.Contains('[') && s.Contains(']');
    }

    bool Extant(string s) {
        if (s == null)
            return false;
        if (s.Length == 0)
            return false;
        return true;
    }

    string ConcatFromArray(string[] s, int start, int end) {
        string result = "";
        for (int i = start; i <= end; i++)
            if (Extant(s[i]))
                result += s[i];
        return result;
    }

    void GenerateChunks(string[] s) {
        int currentStart = 0;
        int currentEnd = 0;
        ChunkData = new();

        while (currentEnd >= 0) {
            ParserChunk tempChunk = new();
            currentStart = FindNextChunkStart(currentEnd, s);
            if (currentStart < 0)
                return;
            tempChunk.ChunkStart = currentStart;
            tempChunk.FirstLine = s[currentStart];
            currentEnd = FindNextPossibleChunkEnd(currentStart, s);
            if (currentEnd == -1)
                currentEnd = s.Length;
            tempChunk.ChunkEnd = currentEnd-1;
            tempChunk.LastLine = s[tempChunk.ChunkEnd];
            ChunkData.Add(tempChunk);
        }
    }

    string GetPartnerFromHeader(string header) {
        if (header.ToLower().Contains("chad"))
            return "Chad";
        if (header.ToLower().Contains("elaine"))
            return "Elaine";
        if (header.ToLower().Contains("faye"))
            return "Faye";
        if (header.ToLower().Contains("jess"))
            return "Jess";
        return "npc";
    }


    private string GetCharactersNameFromLine(string fullText) {

        string full = fullText.Substring(fullText.IndexOf('['), fullText.IndexOf(']')- fullText.IndexOf('[')).ToLower();

        if (full.Contains("chad"))
            return "Chad";
        if (full.Contains("faye"))
            return "Faye";
        if (full.Contains("jess"))
            return "Jess";
        if (full.Contains("elaine"))
            return "Elaine";
        if (full.Contains("owner"))
            return "Owner";
        if (full.Contains("barguy"))
            return "Bar Guy";
        if (full.Contains("baradvicegirl"))
            return "Charming Girl";
        if (full.Contains("charmadviceguy"))
            return "Charming Advice Guy";
        return null;
    }

    string FirstCharacterExpressionInChunk(int startIndex,int endIndex, string[] s) {
        for (int i = startIndex; i < endIndex; i++)
            if (GetCharactersNameFromLine(s[i]) != null)
                return GetCharactersNameFromLine(s[i]);
        return "IDK";
    }


    void ParseFile() {

        string[] parsedText = Script.text.Split('\n');


        GenerateChunks(parsedText);

        ChunkData[0].Header = ConcatFromArray(parsedText, 0, ChunkData[0].ChunkStart-1);
        ChunkData[0].BestGuess = FirstCharacterExpressionInChunk(ChunkData[0].ChunkStart, ChunkData[0].ChunkEnd, parsedText);
        for (int i = 0; i < ChunkData.Count; i++) {
            ChunkData[i].Header = ConcatFromArray(parsedText, i==0?0:ChunkData[i - 1].ChunkEnd+1, ChunkData[i].ChunkStart-1);
            if (i != 0)
                if (Mathf.Abs(ChunkData[i].ChunkStart - ChunkData[i - 1].ChunkEnd) < 3)
                    Debug.Log("Pretty close there bud check if chunks are correct @" + i + " & @" + (i - 1));
            ChunkData[i].BestGuess = FirstCharacterExpressionInChunk(ChunkData[i].ChunkStart, ChunkData[i].ChunkEnd, parsedText);
        }
        if (SendToCutscenes)
            SendToCutscenesFunction(parsedText);
        System.GC.Collect();
    }

    [System.Serializable]
    class ParserChunk {
        public string BestGuess;
        public string Header;
        public int ChunkStart;
        public int ChunkEnd;
        public string FirstLine;
        public string LastLine;

    }
}
#endif