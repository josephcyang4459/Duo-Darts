using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileHandler
{
    readonly static string FileExtension = "sbs";
    readonly static string FileName = "SaveFile_{0}";
    readonly static string CompletionFile = "Completion";
    static string SaveFilePath(int fileIndex) {
        return Application.persistentDataPath + string.Format("/Saves/{0}.{1}", string.Format(FileName, fileIndex), FileExtension);
    }
    static string CompletionFilePath() {
        return Application.persistentDataPath + string.Format("/{0}.{1}",CompletionFile, FileExtension);
    }

    public static SaveFile LoadSaveFile(int fileIndex)
    {
        CheckDirectory();
        string filePath = SaveFilePath(fileIndex);
        if (!File.Exists(filePath))
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Open(filePath, FileMode.Open);
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

    public static void SaveSaveFile(int fileIndex, SaveFile saveFile) {
        CheckDirectory();

        //Debug.Log(Application.persistentDataPath + string.Format("/Saves/{0}.{1}", fileName, fileExtension));
        //File.Delete()
        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Create(SaveFilePath(fileIndex));
        var jsonSaveFile = JsonUtility.ToJson(saveFile);
        bf.Serialize(caseFileStream, jsonSaveFile);
        caseFileStream.Close();
    }

    public static void SaveCompletion(CompletionData data) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Create(CompletionFilePath());
        var jsonSaveFile = JsonUtility.ToJson(data);
        bf.Serialize(caseFileStream, jsonSaveFile);
        caseFileStream.Close();
    }

    public static CompletionData LoadCompletion() {
        string filePath = CompletionFilePath();
        if (!File.Exists(filePath)) {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Open(filePath, FileMode.Open);
        try {
            string jsonSaveFile = (string)bf.Deserialize(caseFileStream);
            caseFileStream.Close();
            return JsonUtility.FromJson<CompletionData>(jsonSaveFile);
        }
        catch {
            Debug.Log("SOMETHING FUCKED UP YO COULD NOT DESERIALIZE");
            caseFileStream.Close();
            return null;
        }
    }

    static void CheckDirectory() {
        if (!Directory.Exists(Application.persistentDataPath + string.Format("/Saves"))) {
            Directory.CreateDirectory(Application.persistentDataPath + string.Format("/Saves"));
        }
    }

}
