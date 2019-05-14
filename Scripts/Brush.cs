using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcolabs.Brushes
{
    public class Brush : MonoBehaviour {
        /// <summary>
        /// Texture used to apply the strokes
        /// </summary>
        [HideInInspector]
        public Texture2D BrushTexture;
        private Texture2D brushTexture;        

        private int textureWidth;
        private int textureHeight;
        private Toggle toggle;

        // cached calculations
        public int brushSize ;
        private int brushSizeX1 ; // << 1
        private int brushSizeXbrushSize ; // x*x        
        private int brushSizeX4 ; // << 2
        private int brushSizeDiv4; // >> 2 == /4
        public int idealAmount;
        private int minimumAmount;

        [Header("Custom Patterns")]
        public bool useCustomPatterns = false;
        public Texture2D[] customPatterns;
        private byte[] patternBrushBytes;
        private int customPatternWidth;
        private int customPatternHeight;
        public int selectedPattern = 0;

        private Color32 strokeColor;
        //public bool textureNeedsUpdate = false;
        protected BrushApplyier brushApplyierMain;
        public float textureUpdateSpeed = 0.15f; // how often texture should be updated (0 = no delay, 1 = every one seconds)
        //private float nextTextureUpdate = 0;
  
        

        #region MonoBehaviour

        private void Start()
        {
            /*
            brushTexture = Instantiate(BrushTexture) as Texture2D;
            textureWidth = brushTexture.width;
            textureHeight = brushTexture.height;
            */
            // cached calculations
            brushSizeX1 = brushSize << 1;            
            brushSizeXbrushSize = brushSize * brushSize;
            brushSizeX4 = brushSizeXbrushSize << 2;

            minimumAmount = brushSize / 10;
            //pixelUVs = new Vector2[20];
            //pixelUVOlds = new Vector2[20];    

            if (useCustomPatterns)
            {
                ReadCurrentCustomPattern();
            }            
            toggle = this.GetComponent<Toggle>();
        }

        #endregion

        #region Brush Methods

        /// <summary>
        /// Apply brush on Raycast hit target
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="hit"></param>
        /// <param name="applyanceColor"></param>
        /// <param name="brushApplyier"></param>
        public void ApplyWithRaycast(Sprite sprite, RaycastHit hit,Color applyanceColor,BrushApplyier brushApplyier, bool wentOutside)
        {
            brushApplyier.DisableDraggable();
            Vector2 pixelUV = Vector2.zero;
            Vector2 hitPoint = brushApplyier.camMain.WorldToScreenPoint(hit.point);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(hit.collider.GetComponent<RectTransform>(), hitPoint, brushApplyier.camMain, out pixelUV);
            pixelUV.x /= hit.collider.GetComponent<RectTransform>().rect.width;
            pixelUV.y /= hit.collider.GetComponent<RectTransform>().rect.height;
            pixelUV += new Vector2(0.5f, 0.5f);
            //Debug.Log("pixelUV Increased: " + pixelUV);

            /*
            if (pixelUV.x == 0 && pixelUV.y == 0)
            {
                
                Vector2 hitPoint = Camera.main.WorldToScreenPoint(hit.point);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(hit.collider.GetComponent<RectTransform>(), hitPoint, Camera.main, out pixelUV);
                pixelUV.x /= hit.collider.GetComponent<RectTransform>().rect.width;
                pixelUV.y /= hit.collider.GetComponent<RectTransform>().rect.height;
                pixelUV += new Vector2(0.5f, 0.5f);
                Debug.Log("pixelUV Increased: " + pixelUV);
            }           
            */

            pixelUV.x *= sprite.texture.width;
            pixelUV.y *= sprite.texture.height;
            

            ApplyOn(sprite, pixelUV, applyanceColor,brushApplyier, wentOutside);            
        }

        public void ReadCurrentCustomPattern()
        {
            if (customPatterns == null || customPatterns.Length == 0 || customPatterns[selectedPattern] == null) { Debug.LogError("Problem: No custom patterns assigned on " + gameObject.name); return; }

            customPatternWidth = customPatterns[selectedPattern].width;
            customPatternHeight = customPatterns[selectedPattern].height;
            patternBrushBytes = new byte[customPatternWidth * customPatternHeight * 4];

            int pixel = 0;
            Color32[] brushPixel = customPatterns[selectedPattern].GetPixels32();

            for (int x = 0; x < customPatternWidth; x++)
            {
                for (int y = 0; y < customPatternHeight; y++)
                {
                    patternBrushBytes[pixel] = brushPixel[x + y * customPatternWidth].r;
                    patternBrushBytes[pixel + 1] = brushPixel[x + y * customPatternWidth].g;
                    patternBrushBytes[pixel + 2] = brushPixel[x + y * customPatternWidth].b;
                    patternBrushBytes[pixel + 3] = brushPixel[x + y * customPatternWidth].a;

                    pixel += 4;
                }
            }
        }

        public void SetToggleTrue()
        {
            toggle.isOn = true;

        }

        /// <summary>
        /// Changes brush texture to brush apollyier's size
        /// </summary>
        /// <param name="brushApplyier"></param>
        public void ResizeBrushTexture(BrushApplyier brushApplyier)
        {
            //TextureScale.Bilinear(brushTexture, (int)(textureWidth * (brushApplyier.GetBrushSize())), (int)(textureHeight * (brushApplyier.GetBrushSize())));
            brushTexture.Resize((int)(textureWidth * (brushApplyier.GetBrushSize())), (int)(textureHeight * (brushApplyier.GetBrushSize())), TextureFormat.RGBA32, false);
        }

        /// <summary>
        /// apply color with sprite as a brush on target texture
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="offset"></param>
        /// <param name="applyanceColor"></param>
        /// <param name="brushApplyier"></param>
        public virtual void ApplyOn(Sprite sprite, Vector2 offset, Color applyanceColor, BrushApplyier brushApplyier, bool wentOutside)
        {
            if (brushApplyierMain==null) { 
                brushApplyierMain = brushApplyier;
                strokeColor = new Color(brushApplyierMain.ignoreColors[0].r, brushApplyierMain.ignoreColors[0].g, brushApplyierMain.ignoreColors[0].b, brushApplyierMain.ignoreColors[0].a);
            }
            if (wentOutside)
            {
                brushApplyierMain.pixelUVOld = offset;
                brushApplyierMain.wentOutside = false;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                brushApplyierMain.pixelUVOld = offset;
                if (useCustomPatterns)
                {
                    DrawPatternCircle((int)offset.x, (int)offset.y);
                }
                else
                {
                    DrawCircle((int)offset.x, (int)offset.y, applyanceColor);
                }
                UpdateTexture();
            }

            //check distance from previous drawing point and connect them with DrawLine

            //if (Vector2.Distance(offset, brushApplyierMain.pixelUVOld) > brushSize)
            //{
            //if (!brushApplyierMain.pixelUVOld.Equals(offset))
            
            if (Mathf.Abs((int)offset.x - (int)brushApplyierMain.pixelUVOld.x) > 100 || Mathf.Abs((int)offset.y - (int)brushApplyierMain.pixelUVOld.y) > 100)
            {
                
                brushSizeDiv4 = idealAmount;
            }
            else
            {
                brushSizeDiv4 = minimumAmount;
            }

            if (useCustomPatterns)
            {
                DrawLineWithPattern(new Vector2((int)brushApplyierMain.pixelUVOld.x, (int)brushApplyierMain.pixelUVOld.y), new Vector2((int)offset.x, (int)offset.y));
            }
            else
            {
                DrawLine((int)brushApplyierMain.pixelUVOld.x, (int)brushApplyierMain.pixelUVOld.y, (int)offset.x, (int)offset.y, applyanceColor);                
            }
            UpdateTexture();            
            brushApplyier.EnableDraggable();
            brushApplyierMain.pixelUVOld = offset;


            

            //}


        }

        /*
        private void Update()
        {
           
            if (textureNeedsUpdate && (Time.time > nextTextureUpdate))
            {
                nextTextureUpdate = Time.time + textureUpdateSpeed;
                UpdateTexture();
            }

        }
        */                

        public void DrawCircle(int x, int y, Color32 applyanceColor)
        {         
                       
            int pixel = 0;

            for (int i = 0; i < brushSizeX4; i++)
            {
                int tx = (i % brushSizeX1) - brushSize;
                int ty = (i / brushSizeX1) - brushSize;

                if (tx * tx + ty * ty > brushSizeXbrushSize) continue;
                if (x + tx < 0 || y + ty < 0 || x + tx >= brushApplyierMain.texWidth || y + ty >= brushApplyierMain.texHeight) continue; // temporary fix for corner painting

                pixel = (brushApplyierMain.texWidth * (y + ty) + x + tx) << 2;

                if (!(brushApplyierMain.pixels[pixel] == strokeColor.r && brushApplyierMain.pixels[pixel+1] == strokeColor.g && brushApplyierMain.pixels[pixel + 2] == strokeColor.b))//Ignore pixels 2,2,2,x
                {
                    brushApplyierMain.pixels[pixel] = applyanceColor.r;
                    brushApplyierMain.pixels[pixel + 1] = applyanceColor.g;
                    brushApplyierMain.pixels[pixel + 2] = applyanceColor.b;
                    brushApplyierMain.pixels[pixel + 3] = applyanceColor.a;
                }                               
                
            } // for area

            //UpdateTexture();


        } // DrawCircle()

        public void DrawLine(int startX, int startY, int endX, int endY, Color32 applyanceColor)
        {
            int x1 = endX;
            int y1 = endY;
            int tempVal = x1 - startX;
            int dx = (tempVal + (tempVal >> 31)) ^ (tempVal >> 31); // http://stackoverflow.com/questions/6114099/fast-integer-abs-function
            tempVal = y1 - startY;
            int dy = (tempVal + (tempVal >> 31)) ^ (tempVal >> 31);


            int sx = startX < x1 ? 1 : -1;
            int sy = startY < y1 ? 1 : -1;
            int err = dx - dy;
            int pixelCount = 0;
            int e2;
            //for (; ; ) // endless loop
            while (!(startX==endX && startY== endY))
            {
                
                pixelCount++;   

                if (pixelCount > brushSizeDiv4) // might have small gaps if this is used, but its alot(tm) faster to skip few pixels
                {                    
                    pixelCount = 0;
                    DrawCircle(startX, startY, applyanceColor);                    
                }
                

                if (startX == x1 && startY == y1) break;
                e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    startX = startX + sx;
                }
                else if (e2 < dx)
                {
                    err = err + dx;
                    startY = startY + sy;
                }
            }
        } // drawline        

        void UpdateTexture()
        {
            brushApplyierMain.drawingTexture.LoadRawTextureData(brushApplyierMain.pixels);
            brushApplyierMain.drawingTexture.Apply(false);
        }

        #region

        public void DrawPatternCircle(int x, int y)
        {
            int pixel = 0;
            for (int i = 0; i < brushSizeX4; i++)
            {
                int tx = (i % brushSizeX1) - brushSize;
                int ty = (i / brushSizeX1) - brushSize;

                if (tx * tx + ty * ty > brushSizeXbrushSize) continue;
                if (x + tx < 0 || y + ty < 0 || x + tx >= brushApplyierMain.texWidth || y + ty >= brushApplyierMain.texHeight) continue; // temporary fix for corner painting

                pixel = (brushApplyierMain.texWidth * (y + ty) + x + tx) << 2;

                float yy = Mathf.Repeat(y + ty, customPatternWidth);
                float xx = Mathf.Repeat(x + tx, customPatternWidth);
                int pixel2 = (int)Mathf.Repeat((customPatternWidth * xx + yy) * 4, patternBrushBytes.Length);

                if (!(brushApplyierMain.pixels[pixel] == strokeColor.r && brushApplyierMain.pixels[pixel + 1] == strokeColor.g && brushApplyierMain.pixels[pixel + 2] == strokeColor.b))//Ignore pixels 2,2,2,x
                {
                    brushApplyierMain.pixels[pixel] = patternBrushBytes[pixel2];
                    brushApplyierMain.pixels[pixel + 1] = patternBrushBytes[pixel2 + 1];
                    brushApplyierMain.pixels[pixel + 2] = patternBrushBytes[pixel2 + 2];
                    brushApplyierMain.pixels[pixel + 3] = patternBrushBytes[pixel2 + 3];
                }
                
            } // for area
        } // DrawPatternCircle()

        void DrawLineWithPattern(Vector2 start, Vector2 end)
        {
            int x0 = (int)start.x;
            int y0 = (int)start.y;
            int x1 = (int)end.x;
            int y1 = (int)end.y;
            int tempVal = x1 - x0;
            int dx = (tempVal + (tempVal >> 31)) ^ (tempVal >> 31); // http://stackoverflow.com/questions/6114099/fast-integer-abs-function
            tempVal = y1 - y0;
            int dy = (tempVal + (tempVal >> 31)) ^ (tempVal >> 31);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            int pixelCount = 0;
            int e2;
            for (; ; )
            {
                
                pixelCount++;
                if (pixelCount > brushSizeDiv4)
                {
                    pixelCount = 0;
                    DrawPatternCircle(x0, y0);
                }
                

                if ((x0 == x1) && (y0 == y1)) break;
                e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    x0 = x0 + sx;
                }
                else if (e2 < dx)
                {
                    err = err + dx;
                    y0 = y0 + sy;
                }
            }
        }
        #endregion

        #endregion

        /*
        // we have no texture set for canvas, FIXME: this returns true if called initialize again, since texture gets created after this
            if (myRenderer.material.GetTexture(targetTexture) == null && !usingClearingImage) // temporary fix by adding && !usingClearingImage
            {
                // create new texture
                if (drawingTexture != null) Texture2D.DestroyImmediate(drawingTexture, true); // cleanup old texture
                drawingTexture = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        myRenderer.material.SetTexture(targetTexture, drawingTexture);

                // init pixels array
                pixels = new byte[texWidth * texHeight * 4];

            } else { // we have canvas texture, then use that as clearing texture

                usingClearingImage = true;

                if (overrideResolution) Debug.LogWarning("overrideResolution is not used, when canvas texture is assiged to material, we need to use the texture size");

    texWidth = myRenderer.material.GetTexture(targetTexture).width;
                texHeight = myRenderer.material.GetTexture(targetTexture).height;

                // init pixels array
                pixels = new byte[texWidth * texHeight * 4];

                if (drawingTexture != null) Texture2D.DestroyImmediate(drawingTexture, true); // cleanup old texture
                drawingTexture = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);

    // we keep current maintex and read it as "clear pixels array" (so when "clear image" is clicked, original texture is restored
    ReadClearingImage();
    myRenderer.material.SetTexture(targetTexture, drawingTexture);
            }
    */
    }
}
