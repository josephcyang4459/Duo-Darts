#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene Parser", menuName = "Testing Tools/Cutscene Parser")]
public class __CutsceneParser : ScriptableObject
{
    [SerializeField] CutScene[] Cutscenes;
    [SerializeField] EventList EventList;
    [SerializeField] TextAsset Script;
    [SerializeField] List<CutScene> NotFound;
    [SerializeField] bool TryParse;
    [SerializeField] bool SendToCutscenes;
    [SerializeField] List<ParserChunk> ChunkData = new();
    [SerializeField] int RepeatingHangoutSceneIndex;
    [SerializeField] CharacterList Characters;
    private void OnValidate() {
        if (TryParse) {
            TryParse = false;
            ParseFile();
        }
        if (SendToCutscenes) {
            SendToCutscenes = false;
            SendToCutscenesFunction();
        }
    }

    void SendToCutscenesFunction() {

        string[] parcedText = Script.text.Split('\n');
        for (int chunkIndex = 0; chunkIndex < Cutscenes.Length; chunkIndex++) {

            string[] temp = new string[ChunkData[chunkIndex].ChunkEnd - ChunkData[chunkIndex].ChunkStart + 1];
            for (int tempArrayIndex = 0, fullArrayIndex = ChunkData[chunkIndex].ChunkStart; tempArrayIndex < temp.Length; tempArrayIndex++, fullArrayIndex++)
                temp[tempArrayIndex] = parcedText[fullArrayIndex];
            if (ChunkData[chunkIndex].IsCutscene) {
                if (ChunkData[chunkIndex].AssociatedCutscene != null)
                    ChunkData[chunkIndex].AssociatedCutscene.__resetCutScene(temp);
            }
            else if (ChunkData[chunkIndex].IsBanter) {
                if (ChunkData[chunkIndex].Banter != null)
                    ChunkData[chunkIndex].Banter.__ResetLines(temp);
            }
            else {
                Debug.Log(chunkIndex + " NOT SENT ---------------------------------------------------");
            }
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
        if (full.Contains("baradviceguy"))
            return "Bar Guy";
        if (full.Contains("danceadvicegirl"))
            return "Dance Girl";
        if (full.Contains("baradvicegirl"))
            return "Charming Girl";
        if (full.Contains("loungeadviceguy"))
            return "Lounge Guy";
        if (full.Contains("charmadviceguy"))
            return "Charming Guy";
        return null;
    }

    private bool IsPartner(string full) {

        if (full.Contains("chad"))
            return true;
        if (full.Contains("faye"))
            return true;
        if (full.Contains("jess"))
            return true;
        if (full.Contains("elaine"))
            return true;
        return false;
    }


    private bool IsCutScene(string line) {
        if (line.ToLower().Contains("newln"))
            return true;
        if (line.Contains("Chad") || line.Contains("Faye") || line.Contains("Elaine") || line.Contains("Jess") || line.Contains("Owner")
     || line.Contains("BarAdviceGuy") || line.Contains("BarAdviceGirl") || line.Contains("CharmAdviceGuy")
     || line.Contains("DanceAdviceGirl") || line.Contains("LoungeAdviceGuy"))
            return true;
        return false;
    }

    private bool IsBanter(string line) {
        if (line.ToLower().Contains("newln"))
            return false;
        if (line.Contains("[0"))
            return true;
        return false;
    }

    string MostReferncedCharacterExpressionInChunk(int startIndex,int endIndex, string[] s) {
        List<string> name =new();
        List<int> occurance = new();
        for (int i = startIndex; i < endIndex; i++)
            if (GetCharactersNameFromLine(s[i]) != null) {
                if (name.Contains(GetCharactersNameFromLine(s[i])))
                    occurance[name.IndexOf(GetCharactersNameFromLine(s[i]))]++;
                else {
                    name.Add(GetCharactersNameFromLine(s[i]));
                    occurance.Add(1);
                }
            }
        if(occurance.Count==0)
            return "IDK";
        int most = 0;
        for(int i = 0; i < occurance.Count; i++) {
            if (occurance[i] > occurance[most])
                most = i;
        }
        return name[most];
       
    }

    int FindSceneIndexInHeader(string header) {
        string check = "scene ";
        int index = header.IndexOf(check)+ check.Length;
        int number= GetNumber(header.Substring(index, 6));
        return number;
    }

    int GetNumber(string s) {
        if (s.Contains("one"))
            return 1;
        if (s.Contains("two"))
            return 2;
        if (s.Contains("three"))
            return 3;
        if (s.Contains("four"))
            return 4;
        if (s.Contains("five"))
            return 5;
        if (s.Contains("six"))
            return 6;
        return -1;
    }

    string GetStatFromFinalLine(string lastLine) {
        if (lastLine.ToLower().Contains("playerintox"))
            return "Drinking";
        return "Hangout";
    }

    int GetRepeatIndexFromFinalLine(string s) {
        if (s.ToLower().Contains("playerintox"))
            return RepeatingHangoutSceneIndex+1;
        return RepeatingHangoutSceneIndex;
    }

    int GetPartnerIndex(string full) {
        if (full.ToLower().Contains("chad"))
            return 0;
        if (full.ToLower().Contains("faye"))
            return 1;
        if (full.ToLower().Contains("jess"))
            return 2;
        if (full.ToLower().Contains("elaine"))
            return 3;
        if (full.ToLower().Contains("owner"))
            return 4;
        return -1;
    }

    string PartnerScene(string name, int chunkIndex) {
        if(IsPartner(name.ToLower())) {
            int number= FindSceneIndexInHeader(ChunkData[chunkIndex].Header.ToLower());

            if (number > -1) {
                ChunkData[chunkIndex].CutsceneIndex = number-1;
                ChunkData[chunkIndex].CharacterName = name;
                return name + " Scene " + number;
            }
            else {
                ChunkData[chunkIndex].CharacterName = name;
                ChunkData[chunkIndex].CutsceneIndex = GetRepeatIndexFromFinalLine(ChunkData[chunkIndex].LastLine);
                return name + " " + GetStatFromFinalLine(ChunkData[chunkIndex].LastLine) + " Scene";
            }
               
        }
        if (name.ToLower().Contains("owner")) {
            int number = FindSceneIndexInHeader(ChunkData[chunkIndex].Header.ToLower())-1
                ;
            ChunkData[chunkIndex].CutsceneIndex = number;
            ChunkData[chunkIndex].CharacterName = name;
            return name + " Scene " + number;
        }
        ChunkData[chunkIndex].CharacterName = name;
        ChunkData[chunkIndex].CutsceneIndex = -1;
        return name;
    }

    void SetBestGuessCutscene() {
        for (int chunkIndex = 0; chunkIndex < ChunkData.Count; chunkIndex++) {

            if (ChunkData[chunkIndex].IsCutscene) {
                if (ChunkData[chunkIndex].CutsceneIndex > -1) {
                    int characterCutsceneIndex = 0;
                    for (int cutSceneIndex = 0; cutSceneIndex < Cutscenes.Length; cutSceneIndex++) {
                        if (Cutscenes[cutSceneIndex].defaultCharacter.ToLower().CompareTo(ChunkData[chunkIndex].CharacterName.ToLower()) == 0) {
                            if (characterCutsceneIndex == ChunkData[chunkIndex].CutsceneIndex) {
                                characterCutsceneIndex = -100;
                                ChunkData[chunkIndex].AssociatedCutscene = Cutscenes[cutSceneIndex];
                                NotFound.Remove(Cutscenes[cutSceneIndex]);
                            }
                            else {
                                characterCutsceneIndex++;
                            }
                        }
                    }
                }
                else {
                    for (int i = 0; i < EventList.List.Length; i++) {
                        if (EventList.List[i].cutScene.defaultCharacter.ToLower().CompareTo(ChunkData[chunkIndex].CharacterName.ToLower()) == 0) {
                            ChunkData[chunkIndex].AssociatedCutscene = EventList.List[i].cutScene;
                            NotFound.Remove(EventList.List[i].cutScene);
                        }
                    }
                }
            }
            
        }
    }

    void ParseFile() {

        string[] parsedText = Script.text.Split('\n');

        NotFound = new List<CutScene>(Cutscenes);
        GenerateChunks(parsedText);

        ChunkData[0].Header = ConcatFromArray(parsedText, 0, ChunkData[0].ChunkStart - 1);
        ChunkData[0].BestGuess = MostReferncedCharacterExpressionInChunk(ChunkData[0].ChunkStart, ChunkData[0].ChunkEnd, parsedText);
        for (int chunkIndex = 0; chunkIndex < ChunkData.Count; chunkIndex++) {
            ChunkData[chunkIndex].Header = ConcatFromArray(parsedText, chunkIndex == 0 ? 0 : ChunkData[chunkIndex - 1].ChunkEnd + 1, ChunkData[chunkIndex].ChunkStart - 1);
            if (chunkIndex != 0)
                if (Mathf.Abs(ChunkData[chunkIndex].ChunkStart - ChunkData[chunkIndex - 1].ChunkEnd) < 3)
                    Debug.Log("Pretty close there bud check if chunks are correct @" + chunkIndex + " & @" + (chunkIndex - 1));
            ChunkData[chunkIndex].CutsceneIndex = -1;
            ChunkData[chunkIndex].IsCutscene = IsCutScene(ChunkData[chunkIndex].FirstLine);
            ChunkData[chunkIndex].IsBanter = IsBanter(ChunkData[chunkIndex].FirstLine);
            if (ChunkData[chunkIndex].IsCutscene) {
                string name = MostReferncedCharacterExpressionInChunk(ChunkData[chunkIndex].ChunkStart, ChunkData[chunkIndex].ChunkEnd, parsedText);
                ChunkData[chunkIndex].BestGuess = PartnerScene(name, chunkIndex);
            }
            else {
                
                bool finals = false;
                for(int previousChunk = chunkIndex-1; previousChunk >= 0; previousChunk--) {
                    if(ChunkData[previousChunk].IsBanter) {
                        finals = true;
                    }
                    if (ChunkData[previousChunk].IsCutscene) {
                        int partnerIndex=0;
                        for(int characterIndexer = 0; characterIndexer < (int)CharacterNames.Owner; characterIndexer++) {
                            if (Characters.list[characterIndexer].DefaultCutScene.defaultCharacter.ToLower().CompareTo(ChunkData[previousChunk].CharacterName.ToLower()) == 0) {
                                partnerIndex = characterIndexer;
                            }
                        }
                        ChunkData[chunkIndex].Banter = finals ? Characters.list[partnerIndex].FinalsBanterLines : Characters.list[partnerIndex].RegularBanterLines;
                        ChunkData[chunkIndex].BestGuess = Characters.list[partnerIndex].DefaultCutScene.defaultCharacter + (finals? " Finals ":" ") + " Banter";
                        break;
                    }
                }
               
            }

        }

        SetBestGuessCutscene();
        System.GC.Collect();
    }

    [System.Serializable]
    class ParserChunk {
        public string BestGuess;
        public bool IsCutscene;
        public bool IsBanter;
        public string Header;
        public int ChunkStart;
        public int ChunkEnd;
        public string FirstLine;
        public string LastLine;

        public string CharacterName;
        public int CutsceneIndex;
        public CutScene AssociatedCutscene;
        public DartsBanterLines Banter;
    }
}
#endif