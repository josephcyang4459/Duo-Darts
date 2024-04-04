using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileHandler : MonoBehaviour
{
    readonly string SystemFile = "SystemFile.txt";
    readonly string FileExtension = "sbs";
    readonly int MaxSaves = 3;
    readonly string BlankLine = "|||";

     readonly char[] Numbers = {'0','1','2','3','4','5','6','7','8','9','0'};

    int GetSystemFileLine(string s) {
        if (s.Contains("0"))
            return 0;
        if (s.Contains("1"))
            return 1;
        return 2;
    }

    public string[] CheckSystemFile(string filePath) {


        if (!File.Exists(filePath)) {//create file is not one
            FileStream saveFileStream = File.Create(filePath);
            saveFileStream.Close();
            string[] blankLines = new string[MaxSaves];
            for (int i = 0; i < MaxSaves; i++)
                blankLines[i] = BlankLine;
            return blankLines;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < MaxSaves) {
            Debug.Log("NOT ENOUGH LINES");
            string[] newLines = new string[MaxSaves];
            for (int i = 0; i < MaxSaves; i++) {
                newLines[i] = BlankLine;
            }
            for (int i = 0; i < lines.Length; i++) {
                if (lines[i] != null)
                    if (IsInCorrectFormat(lines[i]) && lines[i].Length > 3)
                        newLines[i] = lines[i];
            }
            return newLines;
        }
        return lines;
    }

    public void WriteToSystemFileLine(int index, int version, SaveFile file) {
        string filePath = SystemFilePath();
        CheckDirectory();
        string[] lines = CheckSystemFile(filePath);
        Debug.Log("ok");
        lines[index] = version + "|" + file.Hour + "|" + file.Minute + "|";
        string newFile="";
        for (int i = 0; i < lines.Length; i++)
            newFile += lines[i].Trim()+"\n";
        File.WriteAllText(filePath, newFile);
    }

    bool IsInCorrectFormat(string line) {
        bool IsNumber(char c) {
            for (int i = 0; i < Numbers.Length; i++) {
                if (Numbers[i] == c)
                    return true;
            }
            return false;
        }

        string[] parse = line.Split('|');
        string temp = parse[(int)SystemFileParse.Hour].Trim();
        for (int i = 0; i < temp.Length; i++) {
            if (!IsNumber(temp[i]))
                return false;
        }
        temp = parse[(int)SystemFileParse.Minute].Trim();
        for (int i = 0; i < temp.Length; i++) {
            if (!IsNumber(temp[i]))
                return false;
        }
        return true;
    }

    public string GetInfoFromSystemFileLine(int index) {
        string filePath = SystemFilePath();
        if (!File.Exists(filePath)) {
            return null;
        }
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length > index) {
            if (lines[index].Trim().CompareTo(BlankLine.Trim()) == 0) {
                return null;
            }
            if(IsInCorrectFormat(lines[index]))
            return lines[index];
        }
        return null;
    }

    public int GetVersionNumberFromFileLine(int index) {
        string filePath = SystemFilePath();
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length > index)
            return int.Parse(lines[index].Split('|')[(int)SystemFileParse.VersionNumber]);
        return -1;
    }

    string SystemFilePath() {
        return Application.persistentDataPath + string.Format("/{0}", SystemFile); ;
    }

    public SaveFile LoadFile(string fileName)
    {
        CheckDirectory();

        if(!File.Exists(Application.persistentDataPath + string.Format("/Saves/{0}.{1}", fileName, FileExtension)))
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Open(Application.persistentDataPath + string.Format("/Saves/{0}.{1}", fileName, FileExtension), FileMode.Open);
        try {
            string jsonSaveFile = (string)bf.Deserialize(caseFileStream);
            caseFileStream.Close();
            return JsonUtility.FromJson<SaveFile>(jsonSaveFile);
        }
        catch {
            Debug.Log("SOMETHING FUCKED UP YO COULD NOT DESERIALIZE");
            caseFileStream.Close();
            return null;
        }
    }

    public void SaveFile(string fileName, int versionNumber, SaveFile saveFile) {
        CheckDirectory();

        //Debug.Log(Application.persistentDataPath + string.Format("/Saves/{0}.{1}", fileName, fileExtension));
        //File.Delete()
        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Create(Application.persistentDataPath + string.Format("/Saves/{0}.{1}", fileName, FileExtension));
        var jsonSaveFile = JsonUtility.ToJson(saveFile);
        bf.Serialize(caseFileStream, jsonSaveFile);
        caseFileStream.Close();
        WriteToSystemFileLine(GetSystemFileLine(fileName), versionNumber, saveFile);
    }

    void CheckDirectory() {
        if (!Directory.Exists(Application.persistentDataPath + string.Format("/Saves"))) {
            Directory.CreateDirectory(Application.persistentDataPath + string.Format("/Saves"));
        }
    }

#if UNITY_EDITOR
    [SerializeField] bool __test;
    private void OnValidate() {
        if (__test) {
            __test = false;
           
            string[] temp =Directory.GetFiles(Application.persistentDataPath + string.Format("/Saves"));
            foreach(string s in temp) {
                Debug.Log(s);
            }
        }
    }
#endif

}
