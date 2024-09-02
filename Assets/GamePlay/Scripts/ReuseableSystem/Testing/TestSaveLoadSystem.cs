using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem.SaveLoadSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataTestToBeSave : DataRecord
{
    public string name;
    public int number;

    public CustomSaveData savingData = new CustomSaveData();
}
[Serializable]
public class CustomSaveData
{
    public string dataCustom1;
    public string dataCustom2;
}
public class TestSaveLoadSystem : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Delete();
        }
    }

    public void Load()
    {
        Debug.Log(SaveLoadManager.Instance.GetProfileFolderPath());
        var data = SaveLoadManager.Instance.Get<DataTestToBeSave>();
        if(data != null)
            Debug.Log(data.name + " " + data.number + " " + data.savingData.dataCustom2 + " " + data.savingData.dataCustom1);
    }

    public void Save()
    {
        
        var data = SaveLoadManager.Instance.Get<DataTestToBeSave>();
        data.name = GetRandomString(10);
        data.number = 1;
        data.savingData.dataCustom1 = GetRandomString(5);
        data.savingData.dataCustom2 = GetRandomString(5);
        
        data.Save();
    }

    public void Delete()
    {
        var data = SaveLoadManager.Instance.Get<DataTestToBeSave>();
        data.Delete();
    }

    private string GetRandomString(int length)
    {
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += (char)Random.Range(60, 100);
        }

        return result;
    }
}
