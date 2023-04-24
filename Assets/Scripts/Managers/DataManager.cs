using DataInfo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager
{
    private string dataPath;

    public void Init() {
        Initialize();
    }

    public void Initialize() {
        dataPath = Application.persistentDataPath + "/gameData.dat";

        Debug.Log($"dataPath : {dataPath}");
    }

    public void Save(GameData gameData) {;
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log(dataPath);
        
        FileStream file = File.Create(dataPath);

        GameData data = gameData;


        Managers.GameData = data;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("���̺� �Ϸ�!");
        Debug.Log("gameProgess : " + data.gameProgress);
        Debug.Log("goldAmount : " + data.goldAmount);
    }

    public GameData Load() {
        if (dataPath == null) {
            Initialize();
        }

        if (File.Exists(dataPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            Debug.Log("�ε� ����");
            return data;
        }
        else {
            GameData data = new GameData();
            data.goldAmount = 20;
            Debug.Log("�ε� ������ ���� ���� �� 20");
            return data;
        }
    }

    public void DeleteData() {
        File.Delete(dataPath);
        GameData data = new GameData();
        data.goldAmount = 20;
        data.gameProgress = 0;

        Save(data);
    }
}
