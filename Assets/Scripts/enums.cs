
public enum PointValueTarget
{
    Double,
    OuterSingle,
    Triple,
    InnerSingle
}

public enum DartTargetBias
{
    None,
    Bullseye,
    Sixty
}

public enum Characters
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
     Charisma,
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

public enum SceneNumbers
{
    Game = 1,
    NoLovers,
    DidNotWinTheTournament,
    ChadEnding,
    ElaineEnding,
    JessEnding,
    FayeEnding


}

public enum Expressions
{
    nuetral, positive, negative, drunk, ForCutscene
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

public enum Controls
{
    Pause,
    DartsGame,
    Cutscene,
}

#if UNITY_EDITOR

public enum __CutsceneActions
{
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