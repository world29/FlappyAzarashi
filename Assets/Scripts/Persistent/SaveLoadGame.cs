using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveLoadGame
{
    public static SaveData CreateSaveDataObject()
    {
        SaveData saveData = new SaveData();

        saveData.HighScore = GameDataAccessor.HighScore;

        return saveData;
    }

    public static void SaveGame()
    {
        var saveData = CreateSaveDataObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(fileStream, saveData);
        fileStream.Close();

        Debug.Log("Game saved");
    }

    public static SaveData LoadGame()
    {
        SaveData saveData = null;

        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            saveData = (SaveData)bf.Deserialize(fileStream);
            fileStream.Close();

            Debug.Log("Game loaded");
        }
        else
        {
            Debug.Log("Game savedata not exists");
        }

        return saveData;
    }
}
