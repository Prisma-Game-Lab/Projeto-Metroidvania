using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    private static string path = Application.persistentDataPath + "/playerSaveStore1.save";
    public static void SavePlayer(PlayerStatus player)
    {
        // Criando o caminho e o arquivo de salvamento 
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        // De fato salvando o jogador
        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        // verifica se ja existe save 
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;  
            stream.Close();
            return data; 
        }
        else
        {
            Debug.Log("Save file could not be found");
            return null;
        }
    }

    public static void DeleteSave()
    {
        if(File.Exists(path)) File.Delete((path));
    }
}
