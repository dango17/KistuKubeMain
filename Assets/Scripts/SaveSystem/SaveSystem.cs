using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static bool SaveLevelData(int a_level, int a_levelScore, int a_levelStars)
    {
        string path = Application.persistentDataPath + "/LevelData.Isave";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);

        GameData gameData;

        if (!File.Exists(path))
        {
            Debug.Log("File: '" + path + "' does not exist. Creating file.");
            gameData = new GameData();
            gameData.levels[a_level - 1] = new LevelData(a_levelScore, a_levelStars);
        }
        else
        {
            gameData = LoadLevelData();
            gameData.levels[a_level - 1] = new LevelData(a_levelScore, a_levelStars);

        }

        formatter.Serialize(fileStream, gameData);
        fileStream.Close();
        if (!File.Exists(path))
        {
            Debug.LogError("File: '" + path + "' failed to save!");
            return false;
        }
        return true;
    }

    public static GameData LoadLevelData()
    {
        string path = Application.persistentDataPath + "/LevelData.Isave";
        if (!File.Exists(path))
        {
            Debug.LogError("File: '" + path + "' could not be found!");
            return null;
        }
        else if(new FileInfo(path).Length == 0)
        {
            Debug.LogError("File: '" + path + "' is empty!");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Open);

        GameData gameData = formatter.Deserialize(fileStream) as GameData;

        fileStream.Close();
        return gameData;
    }

    public static bool DestroyLevelData()
    {
        string path = Application.persistentDataPath + "/LevelData.Isave";
        if (!File.Exists(path))
        {
            Debug.LogError("File: '" + path + "' could not be found!");
            return false;
        }

        File.Delete(path);
        if (File.Exists(path))
        {
            Debug.LogError("Failed to delete file: '" + path + "'!");
            return false;
        }
        return true;
    }
}
