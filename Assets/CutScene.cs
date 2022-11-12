using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CutScene : ScriptableObject
{
    [SerializeReference]
    public block[] blocks;

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
                    Dialouge dTemp = new Dialouge();
                    dTemp.message = overall[i].Substring(1);
                    bl.Add(dTemp);
                    break;

                case '$':
                    Response rTemp = new Response();
                    rTemp.responses = new responseData[3];
                    for (int j = 0; j<3; j++)
                    {
                        responseData r0 = new responseData();
                        //Debug.Log("Answer " + overall[i].Substring(1));
                        r0.adjust = numbPlus(overall[i].Substring(1));
                        r0.answer = removeIndication(overall[i].Substring(1));
                        List<string> rs = new();
                        i++;
                        
                        while (overall[i][0] == '&')
                        {
                            //Debug.Log("aa" + overall[i].Substring(1));
                            rs.Add(overall[i].Substring(1));
                            i++;
                        }
                        r0.responses = rs.ToArray();
                        rTemp.responses[j] = r0;
                    }
                    bl.Add(rTemp);
                    break;

                case '{':
                    Expression eTemp = new Expression();
                    eTemp.expression = overall[i].Substring(1);
                    bl.Add(eTemp);

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
        if (s.IndexOf('+') > 0)
            return 0;

        return s.LastIndexOf('+') - s.IndexOf('+');
    }

#endif
}


[System.Serializable]
public class block
{

    public virtual void action(cutsceneHandler ch) { }
}

public class Dialouge: block
{
    public string name  = "Dialouge";
    public string message;
    public override void action(cutsceneHandler ch)
    {
        ch.dialouge(message);
    }
}

public class Response: block
{
    public string name = "Response";
    public responseData[] responses;

    public override void action(cutsceneHandler ch)
    {
        ch.response(this);
    }
}

public class Expression : block
{
    public string expression;

    public override void action(cutsceneHandler ch)
    {
        ch.changeExpression(expression);
    }

}

[System.Serializable]
public class responseData
{
    public string answer;
    public string[] responses;
    public int adjust;
}

public class cutsceneHandler
{
    public void dialouge(string message)
    {
        Debug.Log(message);
    }

    public void response(Response r)
    {

    }

    public void changeExpression(string b)
    {

    }
}