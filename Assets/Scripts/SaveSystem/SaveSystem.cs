using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static bool SaveGameData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameData.save";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData();

        formatter.Serialize(fileStream, gameData);
        fileStream.Close();
        if (!File.Exists(path))
        {
            Debug.LogError("File: '" + path + "' failed to save!");
            return false;
        }
        return true;
    }

    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/GameData.save";
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

    public static bool DestroyGameData()
    {
        string path = Application.persistentDataPath + "/GameData.save";
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
