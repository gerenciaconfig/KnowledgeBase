using UnityEngine;
using System.Collections;
using System;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com
namespace Letra
{
    [DisallowMultipleComponent]
    public class TableShape : MonoBehaviour
    {
        /// <summary>
        /// Table Shape ID.
        /// </summary>
        public int ID = -1;

        // Use this for initialization
        void Start()
        {
            ///Setting up the ID for Table Shape
            if (ID == -1)
            {
                string[] tokens = gameObject.name.Split('-');
                if (tokens != null)
                {
                    ID = int.Parse(tokens[1]);
                }
            }
        }
    }
}