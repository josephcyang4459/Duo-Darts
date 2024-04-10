using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Schedule : MonoBehaviour, SceneEntrance {
    public int hour = 4;
    public int minutes = 0;
    [SerializeField] TimeClock Clock;

    public EventList available;

    public Location[] locals;

    public CharacterList characters;

    public int location;

    public Button[] EventButtons;
    public TMP_Text[] btnText;
    public LocationSelecterUI LocationSelector;
    public EventSelectorUI EventSelector;

    public AudioClip song0;

    public GameObject FirstLocationButton;
    public GameObject FirstEventButton;
    public GameObject GenderChoiceButton;

    public DartPartnerStoryUI DartsMenu;
    public ResetStats ResetStats;
    public SaveHandler SaveHandler;
    public Canvas GenderChoiceCanvas;

    public void Start() {
        Audio.inst.PlaySong(song0);
        int fileIndex = TransitionManager.inst.GetFileIndex();
        if (fileIndex > -1) {
            SaveHandler.LoadFromFile(fileIndex);
            GenderChoiceCanvas.enabled = false;
            System.GC.Collect();
        }
        else
            ResetStats.ResetStatsAndCompletionToBase();

        TransitionManager.inst.ReadyToEnterScene(this);

    }

    public void EnterScene() {
        UIState.inst.SetAsSelectedButton(GenderChoiceButton);
        if (TransitionManager.inst.GetFileIndex() > -1) {
            PauseMenu.inst.SetEnabled(true);
            SetTime(0);
        }
        UIState.inst.SetInteractable(true);
        CutsceneHandler.inst.SetUpForMainGame(DartsMenu, this);
    }

    public void ChooseCharacterGender(int i) {
        CutsceneHandler.inst.SetCharacterSprite(i);
        PauseMenu.inst.SetEnabled(true);
        SetTime(0);

    }

    private void SetLocationEventLists() {
        for (int i = 0; i < locals.Length; i++) {
            locals[i].EventsButtonUsed = 0;
            while (locals[i].Events.Count > 0)
                locals[i].Events.RemoveAt(0);
        }

        for (int i = 0; i < (int)CharacterNames.Owner; i++) {
            int availableCutSceneIndex = characters.list[i].GetCutScene();

            if (availableCutSceneIndex >= 0) {
                Locations locationIndex = characters.list[i].RelatedCutScenes[availableCutSceneIndex].CutsceneLocation;

                if (locationIndex != Locations.none) {

                    locals[(int)locationIndex].Events.Add(characters.list[i].RelatedCutScenes[availableCutSceneIndex].CutScene);

                    locals[(int)locationIndex].EventsButtonUsed++;
                }
            }
            else {
                if (characters.list[i].Love >= 0 && !characters.list[i].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed) {
                    int loungeIndex = (int)Locations.lounge;
                    locals[loungeIndex].Events.Add(characters.list[i].DefaultCutScene);
                    locals[loungeIndex].EventsButtonUsed++;
                }
            }
        }
        //events
        for (int i = 0; i < available.List.Length; i++) {
            Locations locationIndex = LocationOf(available.List[i]);

            if (locationIndex != Locations.none) {
                locals[(int)locationIndex].Events.Add(available.List[i].cutScene);
                locals[(int)locationIndex].EventsButtonUsed++;
            }
        }
    }

    public void SetTime(TimeBlocks time) {
        CharacterStatUI.inst.UpdateUI();
        UIState.inst.SetInteractable(true);
        Audio.inst.PlaySong(song0);
        LocationSelector.BeginEntrance();
        //---------------------------------------------------------------------------------------------------------------------SET BUTTON HERE
        //UI_Helper.SetSelectedUIElement(LocationFirstButton);
        IncreaseTimeByMinutes((int)time);

        if (hour >= 9) {
            TransitionManager.inst.GoToScene(SceneNumbers.DidNotWinTheTournament);
        }

        if (hour >= 8) {
            if (minutes >= 30) {
                Debug.Log("Now only duo dates should be available if they are not that is bug ");
                int numberAvailable = 0;
                for (int i = 0; i < 4; i++) {
                    if (characters.list[i].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed && characters.list[i].Love > 0) {
                        numberAvailable++;
                    }
                }
                if (numberAvailable <= 0) {
                    //Debug.Log("GAME OVER please send to end state");
                    TransitionManager.inst.GoToScene(SceneNumbers.NoLovers);
                    return;
                }
            }
        }

        SetLocationEventLists();
    }

    private void SetEventCutsceneComplete(CutScene c) {
        for (int i = 0; i < available.List.Length; i++)
            if (available.List[i].cutScene == c)
                available.List[i].done = true;
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


        SetEventCutsceneComplete(locals[location].Events[eventIndex]);
        EventSelector.HideUI();
        CutsceneHandler.inst.PlayCutScene(locals[location].Events[eventIndex], location);
    }

    public void TurnLocationAndEventSelectorUIOff() {
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
}
[System.Serializable]
public class Location {

    public string Name;

    public int EventsButtonUsed = 0;

    public List<CutScene> Events = new();
}
