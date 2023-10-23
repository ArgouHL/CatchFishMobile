using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerDataControl : MonoBehaviour
{
    public static PlayerDataControl instance; 
    public PlayerData playerData;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadPlayer();
    }

    [ContextMenu("NewPlayerData")]
    public void NewSave()
    {
        NewSave("Test");
        Debug.LogWarning("player");
    }
    public void NewSave(string name)
    {
        playerData.NewData(name);
        Save();
        LoadPlayer();
        Debug.LogWarning("playerSaveCreated:"+name);
    }

    public bool LoadPlayer()
    {
        var _ac = Load();
        if (_ac == null)
        {
            Debug.LogWarning("No save");
            return false;
        }         
        playerData.Reverse(_ac);
        Debug.Log("Load:" + playerData.name);
        return true;
    }


    [ContextMenu("DeleteSave")]
    public void DeleteSave()
    {
     
        
        File.Delete(Application.persistentDataPath + "/Save.ept");
        
        Debug.Log("Save Delected:" + Application.dataPath + "/Save.ept");
    }

    [ContextMenu("Save")]
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/Save.ept", FileMode.Create);
        Account account = new Account(playerData);
        bf.Serialize(stream, account);
        stream.Close();
        Debug.Log("Save Complete"+ Application.dataPath + "/Save.ept");
    }

    [ContextMenu("Load")]
    public Account Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.ept"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/Save.ept", FileMode.Open);
            Account account = bf.Deserialize(stream) as Account;
            stream.Close();
            Debug.Log("Load");
            return account;
        }
        else
        {
            Debug.Log("LoadFail");
            return null;
        }
    }


}
