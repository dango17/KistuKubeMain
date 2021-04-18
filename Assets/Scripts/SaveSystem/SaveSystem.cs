//SAVE SYSTEM SCRIPT
//Use: Classes for storing data in save system
//Created By: Iain Farlow
//Created On: 27/03/2021
//Last Edited: 01/04/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static bool SaveLevelData(int a_level, int a_challenage)
    {
        //get path to save data
        string path = Application.persistentDataPath + "/LevelData.Isave";
        //get binarry formatter 
        BinaryFormatter formatter = new BinaryFormatter();

        GameData gameData;

        //if the files does not exist create a new one
        if (!File.Exists(path))
        {
            Debug.Log("File: '" + path + "' does not exist. Creating file.");
            gameData = new GameData();
            gameData.levels[a_level - 1] = new LevelData(a_challenage);
        }
        else
        {
            //if the save data exists load it to edit
            gameData = LoadLevelData();
            //if the load fails returns null, check it succeeded 
            if (gameData != null)
            {
                //get the current level data
                LevelData tempLevelData = gameData.levels[a_level - 1];
                //check this data exists
                if (tempLevelData != null)
                {
                    //ammend it
                    tempLevelData.challenage[a_challenage - 1] = true;
                }
                else
                {
                    //if it does not exist create it
                    tempLevelData = new LevelData(a_challenage);
                }
                //set level data to ammended data
                gameData.levels[a_level - 1] = tempLevelData;
            }
            else
            {
                //if the file is empty give it some info
                Debug.Log("File: '" + path + "' is empty. Creating context.");
                gameData = new GameData();
                gameData.levels[a_level - 1] = new LevelData(a_challenage);
            }
        }

        //create a filesteam to save file
        FileStream fileStream = new FileStream(path, FileMode.Create);
        //serialise the data as binarry data
        formatter.Serialize(fileStream, gameData);
        //close the stream
        fileStream.Close();
        //check if the file actaully created (only used for first save)
        if (!File.Exists(path))
        {
            Debug.Log("File: '" + path + "' failed to save!");
            return false;
        }
        return true;
    }

    public static GameData LoadLevelData()
    {
        //get the path data
        string path = Application.persistentDataPath + "/LevelData.Isave";
        //if the files does not exist
        if (!File.Exists(path))
        {
            //return null due to no file to load
            Debug.Log("File: '" + path + "' could not be found!");
            return null;
        }
        //if the file does exist but contains no data
        else if (new FileInfo(path).Length == 0)
        {
            //return no due to no data to load
            Debug.Log("File: '" + path + "' is empty!");
            return null;
        }

        //Same as for save jsut with open rather than create
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Open);

        //deserialize the binary data
        GameData gameData = formatter.Deserialize(fileStream) as GameData;

        fileStream.Close();
        //return the deserialized data
        return gameData;
    }

    public static bool DestroyLevelData()
    {
        //NOT CURRENTLY USED
        //Added incase needed

        //Get the file path
        string path = Application.persistentDataPath + "/LevelData.Isave";
        //Check the file exists
        if (!File.Exists(path))
        {
            Debug.Log("File: '" + path + "' could not be found!");
            return false;
        }

        //Delete the file
        File.Delete(path);
        //Check fi the file still exists
        if (File.Exists(path))
        {
            Debug.Log("Failed to delete file: '" + path + "'!");
            return false;
        }
        return true;
    }
}
