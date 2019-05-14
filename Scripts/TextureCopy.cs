using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyExtensions;

namespace Arcolabs.Brushes
{

    public class TextureCopy : MonoBehaviour
    {

        public BrushApplyier brushApplyier;
        public GameObject quadro;
        public GameObject imgline;
        Texture2D mockupTexture;

        //Texture2D previousImage;
        //Texture2D convertedImage;

        Camera cam;

        Sprite mockupSprite;
        public GameObject openedImage;
        public GameObject originalImage;        

        public GameObject FinalImage;
        public Image finalLine;

        public FilterMode filterMode = FilterMode.Point;

        // Use this for initialization
        void Start()
        {
            
        }


        public void SaveTexture(GameObject currentImage)
        {
            
            openedImage = currentImage;


            //Caso o desenho não esteja pintado, guarda o sprite original No primeiro filho
            if (openedImage.transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                openedImage.transform.GetChild(0).GetComponent<Image>().sprite = openedImage.GetComponent<Image>().sprite;
                              
            }
            
            brushApplyier.texWidth = openedImage.transform.GetChild(0).GetComponent<Image>().sprite.texture.width;
            brushApplyier.texHeight = openedImage.transform.GetChild(0).GetComponent<Image>().sprite.texture.height;

            brushApplyier.pixels = openedImage.GetComponent<Image>().sprite.texture.GetRawTextureData();

            if (openedImage.transform.childCount>1)
            {
                imgline.gameObject.SetActive(true);
                imgline.GetComponent<Image>().sprite = openedImage.transform.GetChild(1).GetComponent<Image>().sprite;
            }
            else
            {
                imgline.gameObject.SetActive(false);
            }

            originalImage = openedImage.transform.GetChild(0).gameObject;            
            mockupTexture = openedImage.GetComponent<Image>().sprite.texture;
            mockupSprite = openedImage.GetComponent<Image>().sprite;
            
            SetImageToQuadro();
            UpdateopenedImage();                 


        }

        public void SaveFinalImage()
        {
            brushApplyier.ResetUndo();
            FinalImage.transform.GetComponent<Image>().sprite = openedImage.transform.GetComponent<Image>().sprite; 
            finalLine.sprite = imgline.GetComponent<Image>().sprite;      
        }


        public void SetImageToQuadro()
        {
            cam = Camera.main;
            BoxCollider quadroCollider = this.GetComponent<BoxCollider>();
            quadroCollider.size = new Vector3(1080, 1538, 1);
            
            quadro.GetComponent<Image>().sprite = Sprite.Create(Instantiate(mockupTexture) as Texture2D, mockupSprite.textureRect, mockupSprite.textureRectOffset);
            if (cam.aspect < 0.56f)
            { 
                quadroCollider.size = new Vector3(quadroCollider.size.x/ (0.56f/cam.aspect), quadroCollider.size.y / (0.56f/cam.aspect), quadroCollider.size.z);
            }            
            
            //quadroCollider.size = new Vector3(Screen.width, Screen.height, 1);

            openedImage.GetComponent<Image>().sprite = Sprite.Create(Instantiate(mockupTexture) as Texture2D, mockupSprite.textureRect, mockupSprite.textureRectOffset);
            
        }

        
        public void ReturnImageToMockup()
        {

            //ChangeFormat(originalImage.GetComponent<Image>().sprite.texture, TextureFormat.RGBA32);

            quadro.GetComponent<Image>().sprite = Sprite.Create(Instantiate (originalImage.GetComponent<Image>().sprite.texture) as Texture2D, originalImage.GetComponent<Image>().sprite.textureRect, originalImage.GetComponent<Image>().sprite.textureRectOffset);
            openedImage.GetComponent<Image>().sprite = quadro.GetComponent<Image>().sprite;
            
            brushApplyier.drawingTexture = openedImage.GetComponent<Image>().sprite.texture;
            brushApplyier.drawingTexture.filterMode = filterMode;
            brushApplyier.drawingTexture.wrapMode = TextureWrapMode.Clamp;
            brushApplyier.pixels = openedImage.transform.GetChild(0).GetComponent<Image>().sprite.texture.GetRawTextureData();
        }
        

        public void UpdateopenedImage()
        {
            openedImage.GetComponent<Image>().sprite = quadro.GetComponent<Image>().sprite;
            brushApplyier.drawingTexture = openedImage.GetComponent<Image>().sprite.texture;

            // set texture modes
            brushApplyier.drawingTexture.filterMode = filterMode;
            brushApplyier.drawingTexture.wrapMode = TextureWrapMode.Clamp;
        }

        public void ChangeFormat(Texture2D oldTexture, TextureFormat newFormat)
        {
            //Create new empty Texture
            Texture2D newTex = new Texture2D(oldTexture.width, oldTexture.height, newFormat, false);
            //Copy old texture pixels into new one
            newTex.SetPixels(oldTexture.GetPixels());
            //Apply
            newTex.Apply();

            oldTexture = newTex;
        }

    }

}
