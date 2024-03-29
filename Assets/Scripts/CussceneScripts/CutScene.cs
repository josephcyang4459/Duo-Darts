using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CutScene : ScriptableObject
{
  
    [SerializeReference]
    public block[] blocks;
    public string defaultCharacter;

    public bool exception = false;

    public bool AnotherFuckingException = false;

    public virtual CutScene GetDefaultScene(int index)
    {
        return null;
    }

#if UNITY_EDITOR
    public const int FAIL_VALUE = -10000000;
    public TextAsset aa;
    public bool reset;
    public void OnValidate()
    {
        if (!reset)
            return;
        string currentCharacter = defaultCharacter;
        reset = false;
        string[] overall = aa.text.Split('\n');
        List<block> blockList = new List<block>();

        string[] forTags;
        
        for (int i = 0; i < overall.Length; i++)
            if (overall[i] != null || overall[i].Length > 0)
                overall[i] = __removeBrokenChars(overall[i].Trim());

        for (int currentLine = 0; currentLine < overall.Length; currentLine++)
        {
            if (overall[currentLine] == null)
                break;
            if (overall[currentLine].Length <= 0)
                break;
            var h = __getAction(overall[currentLine]);
            //Debug.Log(h);

            switch (h)
            {
                case __CutsceneActions.Dialouge:
                    DialougeBlock tempD = new DialougeBlock();
                    tempD.message = __fix(overall[currentLine]);
                    blockList.Add(tempD);

                    forTags = __getAllTags(overall[currentLine]);
                    if (forTags.Length > 1)
                    {
                        for (int i = 0; i < forTags.Length; i++)
                        {
                            block tempted = __getDialougeOrThoughtAttachment(forTags[i]);
                            
                            if (tempted != null)
                                blockList.Add(tempted);
                            else Debug.Log("Found " + forTags[i]);
                        }
                    }

                    break;
                case __CutsceneActions.Thought:
                    Thought tempT = new Thought();
                    tempT.thoughtMessage = __fix(overall[currentLine]);
                    blockList.Add(tempT);

                    forTags = __getAllTags(overall[currentLine]);
                    if (forTags.Length > 1)
                    {
                        for (int i = 0; i < forTags.Length; i++)
                        {
                           
                            block tempted = __getDialougeOrThoughtAttachment(forTags[i]);

                            if (tempted != null)
                                blockList.Add(tempted);
                            else Debug.Log("Found "+forTags[i]);
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
                        PRD.responses = responses.ToArray();
                        tempR.responses[currentResponse] = PRD;
                    }// for 3 responses
                    blockList.Add(tempR);
                    break;
                case __CutsceneActions.Expression:
                    if (__getCharactersNameFrom(overall[currentLine]).CompareTo(currentCharacter) != 0){
                        currentCharacter = overall[currentLine];
                        SwapCharacterBlock tempSC = new SwapCharacterBlock();
                        tempSC.character = __getCharactersNameFrom(overall[currentLine]);
                        tempSC.Expression = (Expressions)(__getNumberFrom(overall[currentLine]) - 1);
                        blockList.Add(tempSC);
                    }
                    else
                    {
                        ExpressionBlock tempE = new ExpressionBlock();
                        //Debug.Log(overall[currentLine]);
                        tempE.expression = (Expressions)(__getNumberFrom(overall[currentLine]) - 1);
                        blockList.Add(tempE);
                    }
                    
                    
                    break;
                default:
                    Debug.Log(__getAction(overall[currentLine]) + "-> Not implemented ->" +overall[currentLine]);
                    break;

            }
        }
        blocks = blockList.ToArray();
        
        Debug.Log("Done");
        
    }

    Stats __GetSkill(string s)
    {
       
        if (s.Contains("Intox"))
            return Stats.Intoxication;

        if (s.Contains("Love"))
            return Stats.Love;

        if (s.Contains("Composure"))
            return Stats.Composure;

        return Stats.Composure;
    }

    PlayerSkills __GetPSkill(string s)
    {
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
        var g = __getAction(ss);
        //Debug.Log(g);
        switch (g)
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
            case __CutsceneActions.ResetIntox:
                ChangeStat PCSee = new ChangeStat();
                PCSee.Character = __getCharactersFrom(ss);
                PCSee.Stat = __GetSkill(ss);
                PCSee.Adjust = -1000000;
                return PCSee;

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
                    NPCR.Expression = (Expressions)(__getNumberFrom(s[currentTag])-1);
                    break;
            }
        }
        return NPCR;
    }

    int __getNumberFrom(string fullText)
    {
        char[] aca = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', };
        List<char> chars = new(aca);

        string temp = "";
        for(int i = 0; i < fullText.Length; i++)
        {
            if (chars.Contains(fullText[i]))
            {
                temp += fullText[i];
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
            return Characters.Jess;
        if (fullText.Contains("Elaine"))
            return Characters.Elaine;
        if (fullText.Contains("Owner"))
            return Characters.Owner;
        if (fullText.Contains("BarGuy"))
            return Characters.BarGuy;
        if (fullText.Contains("BarAdviceGirl"))
            return Characters.CharmingGirl;
        if (fullText.Contains("BarGuy"))
            return Characters.BarGuy;
        return Characters.Player;
    }

    private string __getCharactersNameFrom(string fullText)
    {
        if (fullText.Contains("Chad"))
            return "Chad";
        if (fullText.Contains("Faye"))
            return "Faye";
        if (fullText.Contains("Jess"))
            return "Jess";
        if (fullText.Contains("Elaine"))
            return "Elaine";
        if (fullText.Contains("Owner"))
            return "Owner";
        if (fullText.Contains("BarGuy"))
            return "Bar Guy";
        if (fullText.Contains("BarAdviceGirl"))
            return "Charming Girl";
        if (fullText.Contains("BarGuy"))
            return "Bar Guy";
        return "";
    }

    private string[] __getAllTags(string s)
    {
        List<string> h = new(s.Split(']'));

        for(int i = h.Count - 1; i >= 0; i--)
        {
            if (h[i].Length > 0)
            {
                if (!h[i].Contains ('['))
                    h.RemoveAt(i);
            }
            else
                h.RemoveAt(i);
        }
        return h.ToArray();
    }

    private string __fix(string s)
    {
        string ss = __removeBrokenChars(s.Substring(s.IndexOf(']')+1).Trim());
        string[] sa;
        if (ss.Contains('[') || ss.Contains(']'))
        {
            sa = ss.Split('[');
        }
        else return ss;
        for (int i = 0; i < sa.Length; i++)
            if (!sa[i].Contains(']'))
                return sa[i];

        return null;
    }

    private string __firstTag(string s)
    { int firstindex = s.IndexOf('[');
        int nextIndex;
        for( nextIndex=firstindex+1;nextIndex<s.Length; nextIndex++)
        {
            if (s[nextIndex] == ']')
                break;
        }
        if (nextIndex >= s.Length)
            nextIndex = s.Length - 1;
        return s.Substring(firstindex, nextIndex);
    }

    private __CutsceneActions __getAction(string s)
    {
        if (s.Length <= 0)
            return __CutsceneActions.ERROR;
        if(s ==null)
            return __CutsceneActions.ERROR;

        string tempActionName = ((s.Contains('[')&&s.Contains(']'))?__firstTag(s):s);
        //Debug.Log(tempActionName);
        if (tempActionName.Contains("Newln"))
        {
            if (s.Contains('*'))
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

        if (tempActionName.Contains("Chad") || tempActionName.Contains("Faye") || tempActionName.Contains("Elaine") || tempActionName.Contains("Jess") || tempActionName.Contains("Owner")
            || tempActionName.Contains("BarAdviceGuy") || tempActionName.Contains("BarAdviceGirl") || tempActionName.Contains("CharmAdviceGuy")
            || tempActionName.Contains("DanceAdviceGirl") || tempActionName.Contains("LoungeAdviceGuy"))
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
        ch.Schedule.characters.list[(int)Character].stateChange((int)Stat, Adjust);
        ch.nextBlock();
    }
}

public class PlayerChangeStat : block
{
    public PlayerSkills Stat;
    public int Adjust;

    public override void action(CutsceneHandler ch)
    {
        AttributeUpdate.inst.UpdateAttribute(Stat, Adjust);
        //ch.p.UpdateAttribute(Stat, Adjust);
        ch.nextBlock();
    }
}

public class ExpressionBlock : block
{
    public Expressions expression;

    public override void action(CutsceneHandler ch)
    {
        ch.ChangeExpression((int)expression);
    }

}

public class SwapCharacterBlock : block
{

    public string character;
    public Expressions Expression;
    public override void action(CutsceneHandler ch)
    {
        ch.ChangeCharacter(character, Expression);
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
            handler.ChangeExpression((int)Expression);
        if (AdjustValue != 0)
        {
            handler.Schedule.characters.list[(int)Character].stateChange((int)Stat, AdjustValue);
        }
            
        if (AdjustValue < -100)
        {
            handler.EndCutscene();
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