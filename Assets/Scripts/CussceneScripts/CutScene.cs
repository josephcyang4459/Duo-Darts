using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CutScene : ScriptableObject
{
  
    [SerializeReference]
    public block[] blocks;
    public string defaultCharacter;

    public bool exception = false;

    public playerStatChange playerS;
    public PartnerStatChange partnerS;

    public bool AnotherFuckingException = false;

#if UNITY_EDITOR
    public const int FAIL_VALUE = -10000000;
    public TextAsset aa;
    public bool reset;
    public void OnValidate()
    {
        if (!reset)
            return;

        reset = false;
        string[] overall = aa.text.Split('\n');
        List<block> actionBlocks = new List<block>();

        string[] forTags;
        
        for (int i = 0; i < overall.Length; i++)
            if (overall[i] != null || overall[i].Length > 0)
                overall[i] = __removeBrokenChars(overall[i].Trim());

        for (int currentLine = 0; currentLine < overall.Length; currentLine++)
        {
            var h = __getAction(overall[currentLine]);
            Debug.Log(h);

            switch (h)
            {
                case __CutsceneActions.Dialouge:
                    DialougeBlock tempD = new DialougeBlock();
                    tempD.message = __fix(overall[currentLine]);
                    actionBlocks.Add(tempD);

                    forTags = __getAllTags(overall[currentLine]);
                    if (forTags.Length > 1)
                    {
                        for (int i = 0; i < forTags.Length; i++)
                        {
                            __getDialougeOrThoughtAttachment(forTags[i]);
                        }
                    }

                    break;
                case __CutsceneActions.Thought:
                    Thought tempT = new Thought();
                    tempT.thoughtMessage = __fix(overall[currentLine]);
                    actionBlocks.Add(tempT);

                    forTags = __getAllTags(overall[currentLine]);
                    if (forTags.Length > 1)
                    {
                        for (int i = 0; i < forTags.Length; i++)
                        {
                            __getDialougeOrThoughtAttachment(forTags[i]);
                        }
                    }
                    break;

                case __CutsceneActions.Prompt:
                    Response tempR = new Response();
                    tempR.responses = new PlayerResponseData[3];

                    for( int currentResponse = 0; currentResponse < 3; currentResponse++)
                    {
                        if (__getAction(overall[currentLine]) != __CutsceneActions.Prompt)
                            break;
                        PlayerResponseData PRD = new PlayerResponseData();
                        PRD.answer = __fix(overall[currentLine]);
                        currentLine++;
                        List<NPCResponseData> responses = new();
                        while(__getAction(overall[currentLine]) == __CutsceneActions.Answer)
                        {
                            responses.Add(__getNPCResponse(overall[currentLine]));
                            currentLine++;
                        }
                        tempR.responses[currentResponse] = PRD;
                    }// for 3 responses
                    actionBlocks.Add(tempR);
                    break;
                case __CutsceneActions.Expression:
                    ExpressionBlock tempE = new ExpressionBlock();
                    tempE.expression = (Expressions)__getNumberFrom(overall[currentLine]);
                    Debug.Log("EXpression block not done yet");
                    break;
                default:
                    Debug.Log(__getAction(overall[currentLine] + " Not implemented"));
                    break;

            }
        }
        blocks = actionBlocks.ToArray();
        
        Debug.Log("Done");
        
    }

    Stats __GetSkill(string s)
    {

        if (s.Contains("Intoxication"))
            return Stats.Intoxication;

        if (s.Contains("Love"))
            return Stats.Love;

        if (s.Contains("Composure"))
            return Stats.Composure;

        return Stats.Composure;
    }

    PlayerSkills __GetPSkill(string s)
    {
        if (s.Contains("Charisma"))
            return PlayerSkills.Charisma;

        if (s.Contains("Intoxication"))
            return PlayerSkills.Intoxication;

        if (s.Contains("Skill"))
            return PlayerSkills.Skill;

        if (s.Contains("Luck"))
            return PlayerSkills.Luck;

        return PlayerSkills.Luck;
    }

    block __getDialougeOrThoughtAttachment(string ss)
    {
        switch (__getAction(ss))
        {
            case __CutsceneActions.Expression:
                ExpressionBlock tempDe = new ExpressionBlock();
                tempDe.expression = (Expressions)__getNumberFrom(ss);
                return tempDe;

            case __CutsceneActions.ChangeStat:
                if (__getCharactersFrom(ss) == Characters.Player)
                {
                    PlayerChangeStat dPCS = new PlayerChangeStat();
                    dPCS.Stat = __GetPSkill(ss);
                    dPCS.Adjust = __getNumberFrom(ss);
                    return dPCS;
                }
                else
                {
                    ChangeStat PCS = new ChangeStat();
                    PCS.Character = __getCharactersFrom(ss);
                    PCS.Stat = __GetSkill(ss);
                    PCS.Adjust = __getNumberFrom(ss);
                    return PCS;
                }

        }
        return null;
    }

    NPCResponseData __getNPCResponse(string line)
    {
        NPCResponseData NPCR = new();
        NPCR.Message = __fix(line);
        NPCR.Expression = Expressions.ForCutscene;
        NPCR.AdjustValue = 0;
        if (line.Contains('*'))
        {
            NPCR.exemption = true;
        }
        else
            NPCR.exemption = false;

        string[] s = __getAllTags(line);
        for (int currentTag = 0; currentTag < s.Length; currentTag++)
        {
            switch (__getAction(s[currentTag]))
            {
                case __CutsceneActions.ChangeStat:
                    NPCR.Character = __getCharactersFrom(s[currentTag]);
                    NPCR.AdjustValue = __getNumberFrom(s[currentTag]);
                    break;
                case __CutsceneActions.Fail:
                    NPCR.Character = __getCharactersFrom(s[currentTag]);
                    NPCR.AdjustValue = FAIL_VALUE;
                    break;
                case __CutsceneActions.Expression:
                    NPCR.Character = __getCharactersFrom(s[currentTag]);
                    NPCR.Expression = (Expressions)__getNumberFrom(s[currentTag]);
                    break;
            }
        }
        return NPCR;
    }

    int __getNumberFrom(string fullText)
    {
        char[] aca = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '+' };
        List<char> chars = new(aca);

        string temp = "";
        for(int i = 0; i < fullText.Length; i++)
        {
            if (chars.Contains(fullText[i]))
            {
                temp += fullText;
            }
        }

        return int.Parse(temp);
    }

    private Characters __getCharactersFrom(string fullText)
    {
        if (fullText.Contains("Chad"))
            return Characters.Chad;
        if (fullText.Contains("Faye"))
            return Characters.Faye;
        if (fullText.Contains("Jess"))
            return Characters.Chad;
        if (fullText.Contains("Elaine"))
            return Characters.Chad;

        return Characters.Player;
    }

    private string[] __getAllTags(string s)
    {
        List<string> h = new(s.Split(']'));

        for(int i = h.Count - 1; i >= 0; i--)
        {
            if (h[i][0] != '[')
                h.RemoveAt(i);
        }
        return h.ToArray();
    }

    private string __fix(string s)
    {
        return __removeBrokenChars(s.Substring(s.LastIndexOf(']')).Trim());
    }

    private string __firstTag(string s)
    { int firstindex = s.IndexOf('[');
        int nextIndex;
        for( nextIndex=firstindex+1;nextIndex<s.Length; nextIndex++)
        {
            if (s[nextIndex] == ']')
                break;
        }
        return s.Substring(firstindex, nextIndex);
    }

    private __CutsceneActions __getAction(string s)
    {
        string tempActionName = __firstTag(s);
        Debug.Log(tempActionName);
        if (tempActionName.Contains("Newln"))
        {
            if (tempActionName.Contains('*'))
                return __CutsceneActions.Thought;
            else
                return __CutsceneActions.Dialouge;
        }

        if (tempActionName.Contains("Display"))
        {
            return __CutsceneActions.Prompt;
        }
        if (tempActionName.Contains("Answer"))
        {
            return __CutsceneActions.Answer;
        }

        if(tempActionName.Contains("Raise") || tempActionName.Contains("Lower"))
        {
            return __CutsceneActions.ChangeStat;
        }

        if (tempActionName.Contains("Reset"))
            return __CutsceneActions.ResetIntox;

        if (tempActionName.Contains("Fail"))
            return __CutsceneActions.Fail;
        if (tempActionName.Contains("Success"))
            return __CutsceneActions.Success;

        if (tempActionName.Contains("Chad") || tempActionName.Contains("Faye") || tempActionName.Contains("Elaine") || tempActionName.Contains("Jess"))
            return __CutsceneActions.Expression;

        if (tempActionName.Contains("Area"))
            return __CutsceneActions.ChangeBackground;
        if (tempActionName.Contains("Exit"))
        {
            return __CutsceneActions.ExitScene;
        }

        return __CutsceneActions.ERROR;
    }


    public string __removeBrokenChars(string s)
    {
        string temp = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '�')
                temp += '\'';
            else
                temp += s[i];
        }

        if (s.Length <= 0)
            return null;


        return temp;
    }

    public string __removeIndication(string s)
    {
        if (s.IndexOf('+') <= 0)
            return s;
            
        return s.Substring(0, s.IndexOf('+')-1);
    }

    public int __numbPlus(string s)
    {
        if (s.IndexOf('+') <= 0)
            return 0;

        return (s.LastIndexOf('+') - s.IndexOf('+'))+1;
    }

#endif
}


[System.Serializable]
public class block
{

    public virtual void action(CutsceneHandler ch) { }
}

public class DialougeBlock: block
{

    public string message;
    public override void action(CutsceneHandler ch)
    {
        ch.dialouge(message);
    }
}

public class Response: block
{
    //3 responses
    public PlayerResponseData[] responses;

    public override void action(CutsceneHandler ch)
    {
        ch.response(this);
    }
}

public class ChangeStat: block
{
    public Characters Character;
    public Stats Stat;
    public int Adjust;

    public override void action(CutsceneHandler ch)
    {
        ch.sc.partners[(int)Character].stateChange((int)Stat, Adjust);
        ch.nextBlock();
    }
}

public class PlayerChangeStat : block
{
    public PlayerSkills Stat;
    public int Adjust;

    public override void action(CutsceneHandler ch)
    {
        ch.p.UpdateAttribute(Stat, Adjust);
        ch.nextBlock();
    }
}

public class ExpressionBlock : block
{

    public Expressions expression;

    public override void action(CutsceneHandler ch)
    {
        ch.changeExpression((int)expression);
    }

}

public class SwapCharacterBlock : block
{

    public string character;

    public override void action(CutsceneHandler ch)
    {
        ch.changeChar(character);
    }

}

public class SwapBackGround : block
{


    public string place;

    public override void action(CutsceneHandler ch)
    {
        ch.changeBackground(place);
    }

}

public class Thought: block
{
    public string thoughtMessage;

    public override void action(CutsceneHandler ch)
    {
        ch.Thought(thoughtMessage);
    }
}

public class PresentOptions: block
{
    public override void action(CutsceneHandler ch)
    {
        //ch.Thought(thoughtMessage);
        //ch.PresentDefaults(drinkingBlock,DefaultBlock);
        //then in ch make the choice and skip to that block
    }

}

public class EndWithStatChange : block
{
    [SerializeReference]
    public PartnerStatChange[] StatChanges;
    public override void action(CutsceneHandler ch)
    {
        //ch.Thought(thoughtMessage);
        //if (ch.option ==
    }

}


[System.Serializable]
public class PlayerResponseData
{
    public string answer;
    public NPCResponseData[] responses;
}

[System.Serializable]
public class NPCResponseData 
{
    public string Message;
    public Characters Character;
    public Expressions Expression;
    public Stats Stat;
    public int AdjustValue =0;
    public bool exemption;

    public void Adjust(CutsceneHandler handler)
    {
        if (Expression != Expressions.ForCutscene)
            handler.changeExpression((int)Expression);
        if (AdjustValue != 0)
            handler.sc.partners[(int)Character].stateChange((int)Stat, AdjustValue);
        if (AdjustValue < -100)
        {
            handler.off();
        }
    }
}



[System.Serializable]
public class playerStatChange
{
    public string[] stats;
    public int[] values;
}

[System.Serializable]
public class PartnerStatChange
{
    public int[] stats;
    public int[] values;
}