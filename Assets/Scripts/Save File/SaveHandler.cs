using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] public static SaveHandler inst;
    [SerializeField] Schedule Schedule;
    [SerializeField] FileHandler FileHandler;
    [SerializeField] CharacterList Characters;
    [SerializeField] Player Player;
    [SerializeField] EventList Events;
    [SerializeField] int SaveFileVersionNumber;
    readonly string FileName = "SaveFile_{0}";

    public void Start() {
        inst = this;
    }

    public void BeginShowSaveMenu() {

    }

    public void SaveToFile(int file) {
        SaveFile saveFile = new SaveFile();
        int charactersToSendToFile = (int)CharacterNames.Owner+1;

        saveFile.Hour = Schedule.hour;
        saveFile.Minute = Schedule.minutes;
        if (CutsceneHandler.inst != null)
            saveFile.MascFemPortraitIndex = CutsceneHandler.inst.PlayerPortriatIndex();
        else
            saveFile.MascFemPortraitIndex = 0;

        PlayerSaveData tempPlayerData = new PlayerSaveData();
        tempPlayerData.Skill = Player.Skill;
        tempPlayerData.Intoxication = Player.Intoxication;
        tempPlayerData.Luck = Player.Luck;
        tempPlayerData.TotalPoints = Player.TotalPointsScoredAcrossAllDartMatches;
        saveFile.Player = tempPlayerData;

        saveFile.Characters = new PartnerSaveData[charactersToSendToFile];
        for (int i = 0; i < charactersToSendToFile; i++) {
            PartnerSaveData temp = new PartnerSaveData();
            temp.Composure = Characters.list[i].Composure;
            temp.Intoxication = Characters.list[i].Intoxication;
            temp.Love = Characters.list[i].Love;
            int cutscenes = Characters.list[i].RelatedCutScenes.Length;
            temp.CutsceneCompletion = new bool[cutscenes];
            for(int j = 0; j < cutscenes; j++) {
                temp.CutsceneCompletion[j] = Characters.list[i].RelatedCutScenes[j].completed;
            }
            saveFile.Characters[i] = temp;
        }

        saveFile.EventCompletion = new bool[Events.List.Length];
        for(int i = 0; i < Events.List.Length; i++) {
            saveFile.EventCompletion[i] = Events.List[i].done;
        }

        FileHandler.SaveFile(string.Format(FileName,file),SaveFileVersionNumber, saveFile);
        __SaveFile = saveFile;
    }

    public void LoadFromFile(int file) {
        SaveFile saveFile = FileHandler.LoadFile(string.Format(FileName, file));
        if (saveFile == null) {
            Debug.Log("SHIT IS FUCKED YO");
            return;
        }
        __SaveFile = saveFile;

        Schedule.hour = saveFile.Hour;
        Schedule.minutes = saveFile.Minute;
        CutsceneHandler.inst.SetCharacterSprite(saveFile.MascFemPortraitIndex);
        Player.Skill = saveFile.Player.Skill;
        Player.Intoxication = saveFile.Player.Intoxication;
        Player.Luck = saveFile.Player.Luck;
        Player.TotalPointsScoredAcrossAllDartMatches = saveFile.Player.TotalPoints;
        int charactersFromFile = saveFile.Characters.Length;
        for (int i = 0; i < charactersFromFile; i++) {
            Characters.list[i].Composure =saveFile.Characters[i].Composure;
            Characters.list[i].Intoxication = saveFile.Characters[i].Intoxication;
            Characters.list[i].Love = saveFile.Characters[i].Love;
            int cutscenes = Characters.list[i].RelatedCutScenes.Length;
            for (int j = 0; j < cutscenes; j++) {
                Characters.list[i].RelatedCutScenes[j].completed = saveFile.Characters[i].CutsceneCompletion[j];
            }
        }

        for (int i = 0; i < Events.List.Length; i++) {
            Events.List[i].done = saveFile.EventCompletion[i];
        }
    }

#if UNITY_EDITOR
    [SerializeField] SaveFile __SaveFile;
    [SerializeField] bool __test;
    [SerializeField] int __fileNumber;
    private void OnValidate() {
        if (__test) {
            __test = false;
            SaveToFile(__fileNumber);
        }   
    }
#endif

}
