using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Schedule : MonoBehaviour, SceneEntrance, TransitionCaller, Caller {
    [SerializeField] Player Player;
    [SerializeField] CharacterList characters;
    [SerializeField] EventList MiscNPCEvents;
    [SerializeField] EventList PlayerNotifications;
    [SerializeField] TimeClock Clock;
    public InSceneTransition Transition;
    [SerializeField] LocationSelecterUI LocationSelector;
    [SerializeField] EventSelectorUI EventSelector;
    [SerializeField] FinalRoundPartnerSelector FinalRoundSelector;
    [SerializeField] int PointsNeededToPlayFinalRound;
    public int hour = 4;
    public int minutes = 0;
    [SerializeField] Location[] locals;

    [SerializeField] int EventSelected;
    public int location;

    [SerializeField] Button[] EventButtons;
    [SerializeField] TMP_Text[] btnText;
    [SerializeField] EventList BadEndings;

    public GameObject GenderChoiceButton;
    [SerializeField] ImageFill GenderFill;

    public DartPartnerStoryUI DartsMenu;
    public ResetStats ResetStats;
    public SaveHandler SaveHandler;
    public Canvas GenderChoiceCanvas;
    [SerializeField] Achievement MeetAll;

    public void Start() {
        Audio.inst.PlaySong(MusicTrack.LocationSelect);
        
        int fileIndex = TransitionManager.inst.GetFileIndex();
        if (fileIndex > -1)
            LoadFromFile(fileIndex);
        else {
            hour = 5;
            minutes = 0;
            ResetStats.ResetStatsAndCompletionToBase();
        }
            
        Clock.SetVisible(true);
        Clock.SetTime(hour, minutes);
        TransitionManager.inst.ReadyToEnterScene(this);
    }

    void LoadFromFile(int fileIndex) {
        SaveHandler.LoadFromFile(fileIndex);
        GenderChoiceCanvas.enabled = false;
        System.GC.Collect();
        foreach (EventStart e in BadEndings.List)
            e.done = false;
        for (int i = 0; i < PlayerNotifications.List.Length; i++) {
            if (CheckIfValidTime(PlayerNotifications.List[i]))
                PlayerNotifications.List[i].done = true;
            else
                PlayerNotifications.List[i].done = false;
        }
    }

    public void EnterScene() {
        DartSticker.inst.SetVisible(false);
        UIState.inst.SetAsSelectedButton(GenderChoiceButton);
        if (TransitionManager.inst.GetFileIndex() > -1) {
            PauseMenu.inst.SetEnabled(true);
            SetTime(0);
        }
        UIState.inst.SetInteractable(true);
        CutsceneHandler.Instance.SetUpForMainGame(DartsMenu, this);
    }

    public void ChooseCharacterGender(int i) {
        GenderFill.enabled = false;
        CutsceneHandler.Instance.SetCharacterSprite(i);
        DartSticker.inst.SetVisible(false);
        TutorialHandler.inst.EnableStoryTutorial(true, this);

    }

    public void Ping() {
        PauseMenu.inst.SetEnabled(true);
        SetTime(0);
    }

    private void SetLocationEventLists() {
        for (int i = 0; i < locals.Length; i++) {
            locals[i].EventsButtonUsed = 0;
            while (locals[i].Events.Count > 0)
                locals[i].Events.RemoveAt(0);
        }
        // include owner
        for (int characterIndex = 0; characterIndex <= (int)CharacterNames.Owner; characterIndex++) {
            int availableCutSceneIndex = characters.list[characterIndex].GetCutScene();

            if (availableCutSceneIndex >= 0) {
                Locations locationIndex = characters.list[characterIndex].RelatedCutScenes[availableCutSceneIndex].CutsceneLocation;

                if (locationIndex != Locations.none) {

                    locals[(int)locationIndex].Events.Add(characters.list[characterIndex].RelatedCutScenes[availableCutSceneIndex].CutScene);

                    locals[(int)locationIndex].EventsButtonUsed++;
                }
            }
            else {
                if (characterIndex != (int)CharacterNames.Owner)//exclude owner
                    if (characters.list[characterIndex].Love >= 0 && !characters.list[characterIndex].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed) {

                        int loungeIndex = (characterIndex == (int)CharacterNames.Elaine) ? (int)Locations.darts : (int)Locations.lounge;
                        locals[loungeIndex].Events.Add(characters.list[characterIndex].DefaultCutScene);
                        locals[loungeIndex].EventsButtonUsed++;
                    }
            }
        }
        //events
        for (int i = 0; i < MiscNPCEvents.List.Length; i++) {
            Locations locationIndex = LocationOf(MiscNPCEvents.List[i]);

            if (locationIndex != Locations.none) {
                locals[(int)locationIndex].Events.Add(MiscNPCEvents.List[i].cutScene);
                locals[(int)locationIndex].EventsButtonUsed++;
            }
        }
    }

    bool CheckForNotification() {
        for (int i = 0; i < PlayerNotifications.List.Length; i++) {
            if(!PlayerNotifications.List[i].done)
                if (CheckIfValidTime(PlayerNotifications.List[i])) {
                    PlayerNotifications.List[i].done = true;
                    CutsceneHandler.Instance.PlayCutScene(PlayerNotifications.List[i].cutScene, (int)Locations.lounge);
                    return true;
                }
        }
        return false;
    }
    bool CheckMetCharacters() {
        for (int i = 0; i <= (int)CharacterNames.Owner; i++)
            if (!characters.list[i].HasMet())
                return false;
        return true;
    }

    public void SetTime(TimeBlocks time) {

        if (time == TimeBlocks.Long) {// achievement check
            if (!MeetAll.IsComplete())
                if (MiscNPCEvents.AllComplete()) {
                    if (CheckMetCharacters()) {
                        MeetAll.TrySetAchievement(true);
                    }
                }
        }

        CharacterStatUI.inst.UpdateUI();
        UIState.inst.SetInteractable(true);
        Audio.inst.PlaySong(MusicTrack.LocationSelect);
       
        IncreaseTimeByMinutes((int)time);
       

        if (hour >= 9) {
            if (!BadEndings.List[(int)BadEndingIndicies.DidNotWin].done) {
                BadEndings.List[(int)BadEndingIndicies.DidNotWin].done = true;
                CutsceneHandler.Instance.PlayCutScene(BadEndings.List[(int)BadEndingIndicies.DidNotWin].cutScene, (int)Locations.lounge);
                return;
            }
            TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
            return;
        }

        if (hour >= 8) {
            if (minutes >= 30) {
                if (Player.TotalPointsScoredAcrossAllDartMatches <= PointsNeededToPlayFinalRound) {
                    if (!BadEndings.List[(int)BadEndingIndicies.NotEnoughPoints].done) {
                        BadEndings.List[(int)BadEndingIndicies.NotEnoughPoints].done = true;
                        CutsceneHandler.Instance.PlayCutScene(BadEndings.List[(int)BadEndingIndicies.NotEnoughPoints].cutScene, (int)Locations.lounge);
                        return;
                    }
                    TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
                    return;
                }

                int numberAvailable = 0;
                for (int i = 0; i < 4; i++) {
                    if (characters.list[i].FinalRoundEligable()) {
                        numberAvailable++;
                    }
                }
                if (numberAvailable <= 0) {
                    if (!BadEndings.List[(int)BadEndingIndicies.NoLovers].done) {
                        BadEndings.List[(int)BadEndingIndicies.NoLovers].done = true;
                        CutsceneHandler.Instance.PlayCutScene(BadEndings.List[(int)BadEndingIndicies.NoLovers].cutScene, (int)Locations.lounge);
                        return;
                    }

                    TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
                    return;
                }

                FinalRoundSelector.ShowUI();
                return;
            }
        }

        if (CheckForNotification())
            return;


        LocationSelector.BeginEntrance();
        SetLocationEventLists();
    }
    
    /// <summary>
    /// Jank AF Yuck
    /// </summary>
    /// <param name="c"></param>
    private void SetEventCutsceneComplete(CutScene c) {
        for (int i = 0; i < MiscNPCEvents.List.Length; i++)
            if (MiscNPCEvents.List[i].cutScene == c)
                MiscNPCEvents.List[i].done = true;
    }

    private void IncreaseTimeByMinutes(int times) {
        minutes += times;
        if (minutes >= 60) {
            minutes -= 60;
            hour++;
        }

        Clock.SetVisible(true);
        Clock.SetTime(hour, minutes);
    }


    public void SetEventsForLocation(int locationIndex) {
        int starting = 0;
        if (locationIndex == (int)Locations.darts) {
            btnText[0].text = "DARTS";
            EventButtons[0].gameObject.SetActive(true);
            starting = 1;
        }

        for (int eventIndex = 0, buttonIndex = starting; buttonIndex < EventButtons.Length; eventIndex++, buttonIndex++) {
            if (eventIndex < locals[locationIndex].EventsButtonUsed) {
                btnText[buttonIndex].text = locals[locationIndex].Events[eventIndex].defaultCharacter;
                EventButtons[buttonIndex].gameObject.SetActive(true);
            }
            else if (EventButtons[buttonIndex].gameObject.activeSelf)
                EventButtons[buttonIndex].gameObject.SetActive(false);
            else
                break;
        }

        location = locationIndex;
    }

    public void SelectEvent(int eventIndex) {
        UIState.inst.SetInteractable(false);
        if (location == (int)Locations.darts) {
            if (eventIndex == 0) {
                //EventSelector.HideUI();
                DartsMenu.BeginSetUp();
                return;
            }
            eventIndex -= 1;
        }

        EventSelected = eventIndex;
        SetEventCutsceneComplete(locals[location].Events[eventIndex]);
        Transition.BeginTransition(this);
    }

    public void NowHidden() {
        EventSelector.HideUI();
        CutsceneHandler.Instance.PlayCutScene(locals[location].Events[EventSelected], location);
    }

    public void TurnLocationAndEventSelectorUIOff() {
        FinalRoundSelector.HideUI();
        Clock.SetVisible(false);
        EventSelector.HideUI();
        LocationSelector.HideUI();
    }

    private bool CheckIfValidTime(EventStart t) {
        if (hour < t.hour)
            return false;
        if (hour == t.hour)
            if (minutes < t.minutes)
                return false;
        if (hour < t.endHour)
            return true;
        if (hour == t.endHour)
            if (minutes < t.endMinutes)
                return true;
        return false;
    }

    public Locations LocationOf(EventStart eventP) {
        if (eventP.done)
            return Locations.none;

        if (CheckIfValidTime(eventP)) {
            return eventP.Location;
        }
        return Locations.none;
    }
    private void OnDestroy() {
        Achievements.Instance.SaveLocalToDisk();
    }
}
[System.Serializable]
public class Location {

    public string Name;

    public int EventsButtonUsed = 0;

    public List<CutScene> Events = new();
}
