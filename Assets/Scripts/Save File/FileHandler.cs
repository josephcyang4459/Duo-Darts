using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileHandler : MonoBehaviour
{
    readonly string FileExtension = "sbs";
    readonly string FileName = "SaveFile_{0}";

    string SaveFilePath(int fileIndex) {
        return Application.persistentDataPath + string.Format("/Saves/{0}.{1}", string.Format(FileName, fileIndex), FileExtension);
    }

    public SaveFile LoadFile(int fileIndex)
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

    public void SaveFile(int fileIndex, SaveFile saveFile) {
        CheckDirectory();

        //Debug.Log(Application.persistentDataPath + string.Format("/Saves/{0}.{1}", fileName, fileExtension));
        //File.Delete()
        BinaryFormatter bf = new BinaryFormatter();
        FileStream caseFileStream = File.Create(SaveFilePath(fileIndex));
        var jsonSaveFile = JsonUtility.ToJson(saveFile);
        bf.Serialize(caseFileStream, jsonSaveFile);
        caseFileStream.Close();
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
