using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PersistentService
    {
        public void CleanPersistent<T>() where T : new()
        {
            SaveData(new T());
        }
        
        public void SaveTexture(Texture2D texture2D, string fileName, string folderName = "ProfilePics")
        {
            if (texture2D.format != TextureFormat.RGB24)
            {
                texture2D = new Texture2D(texture2D.width, texture2D.height, TextureFormat.RGB24, false);
            }

            var bytes = texture2D.EncodeToPNG();
            var dirPath = $"{Application.persistentDataPath}/{folderName}/";
            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }
            
            File.WriteAllBytes(dirPath + fileName + ".png", bytes);
        }

        public void SaveData<T>(T data, string fileNameWithExtension = "playerInfo.dat") where T : new()
        {
            //Should be saving a list of commandForAgents, instead of one. Since a list of Formation Command will form one formation. 
            //save commandForAgents here just for the ease. 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + fileNameWithExtension); //Persistent data path. E.g. Window/user/Approaming/etc
        
            //Serialize the file
            bf.Serialize(file, data);
            file.Close();
        }

        public void SaveDataJson<T>(T data, string fileNameWithExtension = "JsonData.txt") where T : new()
        { 
            var jsonData = JsonUtility.ToJson(data, true);
            using var file = File.Create(Application.persistentDataPath + "/" + fileNameWithExtension);
            
            var info = new UTF8Encoding(true).GetBytes(jsonData);
            file.Write(info, 0, info.Length);
            file.Close();
        }

        public T LoadDataJson<T>(string fileNameWithExtension = "JsonData.txt") where T : new()
        {
            //Debug.Log("<color=green>" + Application.persistentDataPath + "/" + "</color>");
            if (File.Exists(Application.persistentDataPath + "/" + fileNameWithExtension))
            {
                var rawStringData = File.ReadAllText(Application.persistentDataPath + "/" + fileNameWithExtension);
                
                var playerInfo = JsonUtility.FromJson<T>(rawStringData);
                
                return playerInfo;
            }
            else
            {
                return new T();
            }
        }


        public T LoadData<T>(string fileNameWithExtension = "playerInfo.dat") where T : new()
        { 
            //Debug.Log("<color=green>" + Application.persistentDataPath + "/" + "</color>");
            if (File.Exists(Application.persistentDataPath + "/" + fileNameWithExtension))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + fileNameWithExtension, FileMode.Open); //Persistent data path. E.g. Window/user/Approaming/etc

                T playerInfo = (T)bf.Deserialize(file);
            
                file.Close();
                return playerInfo;
            }
            else
            {
                return new T();
            }
        }
    }
}