namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [System.Serializable]
    public class SavedItem
    {
        public enum ItemType { Image, Audio, Video};

        private ItemType itemType;

        [SerializeField]

        //private AudioClip audioClip;

        private DateTime dateTime;

        private int activityID;

        private Byte[] spriteByteArray;

        private Byte[] audioByteArray;

        public SavedItem(AudioClip clip, Sprite image)
        {
            //this.activity = activity;
            this.itemType = ItemType.Image;
            this.dateTime = DateTime.Now;

            ConvertSpriteToByte(image);
            ConvertAudioToByte(clip);
        }
        
        private void ConvertSpriteToByte(Sprite image)
        {
            spriteByteArray = image.texture.EncodeToPNG();
        }

        private void ConvertAudioToByte(AudioClip clip)
        {

            
            var samples = new float[clip.samples * clip.channels];

            clip.GetData(samples, 0);

            Int16[] intData = new Int16[samples.Length];
            //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

            audioByteArray = new Byte[samples.Length * 2];
            //bytesData array is twice the size of
            //dataSource array because a float converted in Int16 is 2 bytes.

            int rescaleFactor = 32767; //to convert float to Int16

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * rescaleFactor);
                Byte[] byteArr = new Byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(audioByteArray, i * 2);
            }

            /*
            audioByteArray = new float[audioClip.samples * audioClip.channels];
            Debug.Log(audioClip.samples);
            Debug.Log(audioClip.channels);
            audioClip.GetData(audioByteArray, 0);*/
        }
         
        
        public void GetAudioCLip(AudioSource audioSource)
        {
            float [] samples = new float[(audioByteArray.Length / 2)];

            Int16[] intData = new Int16[samples.Length];

            Debug.Log("audioByteArray Lenght" + audioByteArray.Length);
            Debug.Log("samples Lenght" + samples.Length);

            for (int i = 0; i < samples.Length; i++)
            {
                Byte[] byteArr = new Byte[2];
                byteArr[0] = audioByteArray[i * 2];
                byteArr[1] = audioByteArray[(i * 2) + 1];
                intData[i] = BitConverter.ToInt16(byteArr, 0);
                samples[i] = (float)intData[i] / 32767;
            }

            AudioClip clip = AudioClip.Create("Teste", samples.Length,2, 44100, false);
            clip.SetData(samples, 0);
            audioSource.clip = clip;
            audioSource.Play();
        }

        public Sprite GetSprite()
        {
            Texture2D texture = new Texture2D(1,1);
            texture.LoadImage(spriteByteArray);
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            return sprite;
        }

        /*
        public Type GetType()
        {
            return this.type;
        }*/

        public DateTime GetDateTime()
        {
            return dateTime;
        }
    }
}