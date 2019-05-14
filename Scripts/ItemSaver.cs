namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Sirenix.OdinInspector;
    using System.Runtime.Serialization;
    using System.Xml.Linq;
    using System.Text;
    using Newtonsoft.Json;

    public class ItemSaver : SerializedMonoBehaviour
    {
        [SerializeField]
        public Activity activity;

        public Sprite sprite;

        private int test = 67;

        public AudioClip audioCLip;

        public SpriteRenderer spriteRenderer;

        public AudioSource audioSource;

        public void Start()
        {

            //SaveI();
            LoadI();

            /*
            float[] samples = new float[audioCLip.samples * 2];
            audioCLip.GetData(samples,0);

            AudioClip clip = AudioClip.Create("Teste", samples.Length, 2, 44100, false);
            clip.SetData(samples, 0);
            audioSource.clip = clip;
            audioSource.Play();*/
        }

        private void SaveI()
        {
            SavedItem savedItem = new SavedItem(audioCLip, sprite);

            print(Application.persistentDataPath);
            Save(savedItem);
        }

        private void LoadI()
        {
            SavedItem saveditem = LoadSavedItem(Application.persistentDataPath + "\\" + "Mickey" + ".dat");

            spriteRenderer.sprite = saveditem.GetSprite();
            saveditem.GetAudioCLip(audioSource);
        }
        

        public void Save(SavedItem savedItem)
        {
            
            string destination = Application.persistentDataPath + "\\" + "Mickey" + ".dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

           //string jsonObject =JsonConvert.SerializeObject(savedItem);

            //BinaryWriter bw = new BinaryWriter(file);

            //bw.Write(savedItem);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, savedItem);
            file.Close();
            
            /*
            string destination = Application.persistentDataPath + savedItem.GetActivity().activityName + ".dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            //Serialize to xml
            DataContractSerializer bf = new DataContractSerializer(sprite.GetType());
            MemoryStream streamer = new MemoryStream();

            //Serialize the file
            bf.WriteObject(streamer, sprite);
            streamer.Seek(0, SeekOrigin.Begin);

            //Save to disk
            file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);

            // Close the file to prevent any corruptions
            file.Close();

           /* string result = XElement.Parse(Encoding.ASCII.GetString(streamer.GetBuffer()).Replace("\0", "")).ToString();
            Debug.Log("Serialized Result: " + result);*/
            

        }

        private byte[] ObjectToByteArray(System.Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public SavedItem LoadSavedItem(string destination)
        {
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                Debug.LogError("File not found");
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            SavedItem data = (SavedItem)bf.Deserialize(file);
            file.Close();

            return data;
        }

        public void LoadAllFiles()
        {
            //Folder Location
            string path = Application.persistentDataPath + "/Inventory/ChildId";

            DirectoryInfo dirInfo = new DirectoryInfo(path);

            foreach (var item in dirInfo.GetFiles())
            {

            }


        }
    }
}