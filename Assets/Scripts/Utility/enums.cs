
public enum PointValueTarget {
    InnerSingle,
    Triple,
    OuterSingle,
    Double,
}

public enum DartTargetBias
{
    None,
    Bullseye,
    Sixty
}

public enum CharacterNames
{
    Chad,
    Jess,
    Faye,
    Elaine,
    Owner,
    BarGuy,
    CharmingGirl,
    CharmingGuy,
    DanceGirl,
    LoungeGuy,
    Player
}

public enum PlayerSkills
{
     Intoxication,
     Skill,
     Luck
}

public enum Stats
{
    Composure,
    Intoxication,
    Love
}

public enum PartnerCutscenes
{
    DrunkScene =3,
    FinalScene
}

public enum BadEndingIndicies {
    NotEnoughPoints,
    NoLovers,
    DidNotWin
}

public enum SceneNumbers
{
    MainMenu,
    Story,
    Darts,
    Credits,
    NoLovers,
    DidNotWinTheTournament,
    ChadEnding,
    JessEnding,
    FayeEnding,
    ElaineEnding,
}

public enum TimeBlocks
{
    Notification=0,
    Short =5,
    Long = 10
}

public enum Expressions
{
    Nuetral, 
    Positive, 
    Negative, 
    Drunk, 
    ForCutscene
}

public enum AudioClips
{
    Click,
    RandomDart,
}

public enum DartAudioClips {
    Soft,
    Medium,
    Hard
}

public enum Locations
{
     lounge,
     bar,
     dance,
     bathroom,
     darts,
     none
}

public enum ControllerState
{
    UseIfConnected,
    ForceKeyboard,
    ForceController
}

public enum TextboxColorIndex {
    Foreground,
    Background
}
#if UNITY_EDITOR

public enum __CutsceneActions {
    Dialouge,
    Thought,
    Expression,
    BackGround,
    Prompt,
    Answer,
    ChangeStat,
    ChangeBackground,
    ExitScene,
    ResetIntox,
    Fail,
    Success,
    ERROR
}

#endif