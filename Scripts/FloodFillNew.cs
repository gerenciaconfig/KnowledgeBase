using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com
/// 
/// Note :
/// If you would like to implement a color picker rather than the built-in brushes and to cover more colors,
/// you can contact us and we will be happy to help you.
/// </summary>
namespace Arcolabs.Brushes
{

    [DisallowMultipleComponent]
    public class FloodFillNew : MonoBehaviour
    {
        public static FloodFillNew instance;

        public BrushApplyier brushApplyier;
        /// <summary>
        /// The allowed colors, you can fill them.
        /// </summary>
        public List<Color32> allowedColors = new List<Color32>();

        /// <summary>
        /// The default normal color.
        /// </summary>
        public Color defaultColor = new Color(1, 1, 1, 1);

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                if (!allowedColors.Contains(defaultColor))
                    allowedColors.Add(defaultColor);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Flood Fill Functionality
        ///Developed by Indie Studio
        ///https://www.assetstore.unity3d.com/en/#!/publisher/9268
        ///www.indiestd.com
        ///info@indiestd.com
        /// </summary>
        /// <param name="texture2D">Texture2d reference.</param>
        /// <param name="clickPoint">Click point.</param>
        /// <param name="newColor">New color.</param>
        /// <param name="oldColor">Old color.</param>
        public void Fill(Texture2D texture2D, Vector2 clickPoint, Color32 newColor, Color32 oldColor)
        {
            if (oldColor.Equals(newColor))
            {
                brushApplyier.EnableDraggable();
                return;
            }
            /*
            if (!CommonUtil.EqualsToOneOf(oldColor, allowedColors))
            {
                return;
            }
            */
            StartCoroutine(FillCoroutine(texture2D, clickPoint, newColor, oldColor));
        }

        /// <summary>
        /// Flood Fill Functionality without checking the allowed colors
        ///Developed by Indie Studio
        ///https://www.assetstore.unity3d.com/en/#!/publisher/9268
        ///www.indiestd.com
        ///info@indiestd.com
        /// </summary>
        /// <param name="texture2D">Texture2d reference.</param>
        /// <param name="clickPoint">Click point.</param>
        /// <param name="newColor">New color.</param>
        /// <param name="oldColor">Old color.</param>
        public void Fill2(Texture2D texture2D, Vector2 clickPoint, Color32 newColor, Color32 oldColor)
        {

            if (oldColor.Equals(newColor))
            {
                brushApplyier.EnableDraggable();
                return;
            }

            //brushApplyier.pixels = texture2D.GetRawTextureData();
            StartCoroutine(FillCoroutine(texture2D, clickPoint, newColor, oldColor));

        }

        /// <summary>
        /// Flood Fill Coroutine
        ///Developed by Indie Studio
        ///https://www.assetstore.unity3d.com/en/#!/publisher/9268
        ///www.indiestd.com
        ///info@indiestd.com
        /// </summary>
        /// <param name="texture2D">Texture2d reference.</param>
        /// <param name="clickPoint">Click point.</param>
        /// <param name="newColor">New color.</param>
        /// <param name="oldColor">Old color.</param>
        public IEnumerator FillCoroutine(Texture2D texture2D, Vector3 clickPoint, Color32 newColor, Color32 oldColor)
        {

            yield return 0;
            
            int x = (int)clickPoint.x, y = (int)clickPoint.y;

            Color[] colors = texture2D.GetPixels();
            Queue<Vector2> queue = new Queue<Vector2>();
            int textureWidth = texture2D.width;
            int textureHeight = texture2D.height;
            int xValue;
            bool spanAbove, spanBelow;

            queue.Enqueue(new Vector2(x, y));

            Vector3 temp;
            while (queue.Count != 0)
            {

                temp = queue.Dequeue();
                x = (int)temp.x;
                y = (int)temp.y;
                xValue = x;


                while (xValue >= 0 && colors[y * textureWidth + xValue].Equals(oldColor))
                {
                    xValue--;
                }

                xValue++;

                spanAbove = spanBelow = false;

                while (xValue < textureWidth && colors[y * textureWidth + xValue].Equals(oldColor))
                {
                    colors[y * textureWidth + xValue] = newColor;


                    if (!spanAbove && y > 0 && colors[(y - 1) * textureWidth + xValue].Equals(oldColor))
                    {
                        queue.Enqueue(new Vector2(xValue, y - 1));
                        spanAbove = true;
                    }
                    else if (spanAbove && y > 0 && !colors[(y - 1) * textureWidth + xValue].Equals(oldColor))
                    {
                        spanAbove = false;
                    }

                    if (!spanBelow && y < textureHeight - 1 && colors[(y + 1) * textureWidth + xValue].Equals(oldColor))
                    {
                        queue.Enqueue(new Vector2(xValue, y + 1));
                        spanBelow = true;
                    }
                    else if (spanBelow && y < textureHeight - 1 && !colors[(y + 1) * textureWidth + xValue].Equals(oldColor))
                    {
                        spanBelow = false;
                    }

                    xValue++;
                }
            }
            texture2D.SetPixels(colors);
            texture2D.Apply();
            brushApplyier.pixels = texture2D.GetRawTextureData();
            brushApplyier.EnableDraggable();
        }
    }
}
