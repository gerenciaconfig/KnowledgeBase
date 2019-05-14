using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcolabs.Brushes
{

    public class BrushApplyier : MonoBehaviour
    {
        #region Editor Parameters
        /// <summary>
        /// Brush that will be applyied
        /// </summary>
        /// 
        //New BrushImplementation
        [HideInInspector]
        public byte[] pixels; // byte array for texture painting, this is the image that we paint into.
        public int texWidth;
        public int texHeight;
        public bool wentOutside = false;

        public Vector2 pixelUVOld;


        public Texture2D drawingTexture;

        public Brush brush;

        public Camera camMain;

        public Brush eraser;

        /// <summary>
        /// Can you drag the brush ?
        /// </summary>
        [SerializeField]
        public bool draggable = true;

        /// <summary>
        /// Color that will be applyied
        /// </summary>
        public Color applyianceColor;

        /// <summary>
        /// How many textures will be bufferized to undo
        /// </summary>
        public int maximumTextureBuffer = 5;

        /// <summary>
        /// Distance from target color to target color for ignoring the painting
        /// </summary>
        public float ColorSensitivity;

        /// <summary>
        /// Colors that will be ignored on the stroke
        /// </summary>
        public List<Color> ignoreColors;

        /// <summary>
        /// Brush scalling
        /// </summary>
        private float brushSize;
        private bool clicked = false;
        #endregion

        #region Undo commands
        /// <summary>
        /// Returns for texture's prior condition
        /// </summary>
        public void Undo()
        {
            int last = imagesBefore.Count - 1;
            if (last == -1) return;
            Graphics.CopyTexture(texturesBefore[last], imagesBefore[last].sprite.texture);
            pixels = imagesBefore[last].sprite.texture.GetRawTextureData();
            texturesBefore.RemoveAt(last);
            imagesBefore.RemoveAt(last);
        }

        public void ResetUndo()
        {
            texturesBefore.Clear();
            imagesBefore.Clear();
        }

        /// <summary>
        /// Textures stored for undo
        /// </summary>
         
        [HideInInspector]
        public List<Texture> texturesBefore;

        /// <summary>
        /// Images stored for undo
        /// </summary>
        private List<Image> imagesBefore;

        #endregion

        #region Brush Apllyier Commands
        /// <summary>
        /// Get Color from a target image and stores as applyiance color.Usefull for color buttons.
        /// </summary>
        /// <param name="image"></param>
        public void GetApplyianceColorFrom(Image image)
        {
            this.applyianceColor = image.color;
        }

        /// <summary>
        /// Sets target brush 
        /// </summary>
        /// <param name="brush"></param>
        public void SetBrush(Brush brush)
        {
            this.brush = brush;
            //brush.ResizeBrushTexture(this);
            //RefreshBrushSize(1);
        }

        public void DisableDraggable()
        {
            draggable = false;
        }
        public void EnableDraggable()
        {
            draggable = true;
        }

        /// <summary>
        /// Refresh brush to slider's value
        /// </summary>
        /// <param name="slider"></param>
        public void RefreshBrushSize(Slider slider)
        {
            RefreshBrushSize(slider.value);
        }

        /// <summary>
        /// Refreshes the Brush's size
        /// </summary>
        public void RefreshBrushSize(float scale)
        {
            brushSize = scale;
            //brush.ResizeBrushTexture(this);
        }

        /// <summary>
        /// Returns the brush's size
        /// </summary>
        /// <returns></returns>
        public float GetBrushSize()
        {
            return brushSize;
        }

        #endregion

        #region MonoBehaviour
        void Start()
        {

            texturesBefore = new List<Texture>();
            imagesBefore = new List<Image>();
        }

        
        


        void Update()
        {
            
            if (!draggable && !Input.GetMouseButtonDown(0)) return;


            Ray ray = camMain.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (!Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                wentOutside = true;
                return;
            }

            
            if (!draggable || draggable && Input.GetMouseButtonDown(0))
            {
                if (texturesBefore.Count == maximumTextureBuffer)
                {
                    texturesBefore.RemoveAt(0);
                    imagesBefore.RemoveAt(0);
                }
                texturesBefore.Add(Instantiate(hit.collider.GetComponent<Image>().sprite.texture));
                imagesBefore.Add(hit.collider.GetComponent<Image>());
                pixels = hit.collider.GetComponent<Image>().sprite.texture.GetRawTextureData();
            }

            if (draggable && Input.GetMouseButton(0))
            {
                
                if (brush is Bucket2)
                {
                    if (clicked)
                    {
                        Sprite sprite = hit.collider.GetComponent<Image>().sprite;

                        brush.ApplyWithRaycast(sprite, hit, applyianceColor, this, wentOutside);

                        clicked = false;
                    }
                }
                else
                {

                    Sprite sprite = hit.collider.GetComponent<Image>().sprite;

                    Color c = (brush == eraser ? Color.white : applyianceColor);

                    brush.ApplyWithRaycast(sprite, hit, c, this, wentOutside);
                }
            }


            if (Input.GetMouseButtonUp(0))
            {
                clicked = false;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                clicked = true;
            }

        }
        #endregion
    }
}
