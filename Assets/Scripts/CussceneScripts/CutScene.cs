using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CutScene : ScriptableObject
{
    [SerializeReference]
    public block[] blocks;
    public string defaultCharacter;

#if UNITY_EDITOR
    public TextAsset aa;
    public bool reset;
    public void OnValidate()
    {
        if (!reset)
            return;

        string[] overall = aa.text.Split('\n');
        List<block> bl = new List<block>();

        for (int i = 0; i < overall.Length; i++)
            if (overall[i] != null || overall[i].Length > 0)
                overall[i] = sanatize(overall[i].Trim());

        for (int i = 0; i < overall.Length; i++)
        {
            if (overall[i] == null || overall[i].Length <= 0)
                break;
            char a = overall[i][0];

            switch (a)
            {

                case '<':

                    if (overall[i][1] == '*')
                    {
                        Thought ttemp = new Thought();
                        ttemp.thoughtMessage = overall[i].Substring(2, overall[i].Length - 3);
                        bl.Add(ttemp);
                        break;
                    }
                    DialougeBlock dTemp = new DialougeBlock();
                    dTemp.message = overall[i].Substring(1);
                    bl.Add(dTemp);
                    break;

                case '$':
                    Response rTemp = new Response();
                    rTemp.responses = new responseData[3];
                    for (int j = 0; j<3; j++)
                    {
                        responseData r0 = new responseData();
                        r0.adjust = numbPlus(overall[i].Substring(1));
                        r0.answer = removeIndication(overall[i].Substring(1));
                        List<string> rs = new();
                        i++;

                        while (overall[i][0] == '&')
                        {
                            rs.Add(overall[i].Substring(1));
                            i++;
                            if (overall[i] == null || overall[i].Length <= 0)
                                break;
                        }
                        
                        r0.responses = rs.ToArray();
                        rTemp.responses[j] = r0;
                    }
                    i--;
                    bl.Add(rTemp);

                    break;

                case '{':
                    ExpressionBlock eTemp = new ExpressionBlock();
                    eTemp.expression = overall[i].Substring(1);
                    bl.Add(eTemp);

                    break;

                case '>':
                    SwapCharacterBlock scb = new SwapCharacterBlock();
                    scb.character = overall[i].Substring(1);
                    bl.Add(scb);
                    break;
                case '[':
                    SwapBackGround sbg = new SwapBackGround();
                    sbg.place = overall[i].Substring(1);
                    bl.Add(sbg);
                    break;

            }
        }
        blocks = bl.ToArray();
        reset = false;
        Debug.Log("Done");
        
    }

    public bool checkChar(char a)
    {
        if (a == '&')
            return true;
        if (a == '$')
            return true;
        if (a == '<')
            return true;
        if (a == '{')
            return true;
        if (a == '[')
            return true;
        if (a == '>')
            return true;
        return false;
    }


    public string sanatize(string s)
    {
        string temp = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '�')
                temp += '\'';
            else
                temp += s[i];
        }

        int index = 0;
        if (s.Length <= 0)
            return null;
        while (!checkChar(s[index]))
        {
            index++;
        }

        return temp.Substring(index);
    }

    public string removeIndication(string s)
    {
        if (s.IndexOf('+') <= 0)
            return s;
            
        return s.Substring(0, s.IndexOf('+')-1);
    }
    public int numbPlus(string s)
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

    public responseData[] responses;

    public override void action(CutsceneHandler ch)
    {
        ch.response(this);
    }
}

public class ExpressionBlock : block
{

    public string expression;

    public override void action(CutsceneHandler ch)
    {
        ch.changeExpression(expression);
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


[System.Serializable]
public class responseData
{
    public string answer;
    public string[] responses;
    public int adjust;
}
