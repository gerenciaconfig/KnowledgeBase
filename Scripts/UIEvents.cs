using UnityEngine;
using System.Collections;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com
namespace Letra
{
    public class UIEvents : MonoBehaviour
    {
        /// <summary>
        /// Static instance of this class.
        /// </summary>
        public static UIEvents instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void AlbumShapeEvent(TableShape tableShape)
        {
            if (tableShape == null)
            {
                return;
            }

            if (ShapesManager.instance.shapes[tableShape.ID].isLocked)
            {
                return;
            }

            ShapesManager.Shape.selectedShapeID = tableShape.ID;
            LoadGameScene();
        }

        public void PointerButtonEvent(Pointer pointer)
        {
            if (pointer == null)
            {
                return;
            }
            if (pointer.group != null)
            {
                ScrollSlider.instance.DisableCurrentPointer();
                ScrollSlider.instance.currentGroupIndex = pointer.group.Index;
                ScrollSlider.instance.GoToCurrentGroup();
            }
        }

        public void LoadGameScene()
        {
            StartCoroutine(SceneLoader.LoadSceneAsync("Game"));
        }

        public void LoadAlbumScene()
        {
            StartCoroutine(SceneLoader.LoadSceneAsync("Album"));
        }

        public void NextClickEvent()
        {
            AlphabetGameManager.instance.NextShape();
        }

        public void PreviousClickEvent()
        {
            AlphabetGameManager.instance.PreviousShape();
        }

        public void SpeechClickEvent()
        {
            AlphabetGameManager.instance.Spell();
        }

        public void ResetShape()
        {
            if (!AlphabetGameManager.instance.shape.completed)
            {
                AlphabetGameManager.instance.DisableGameManager();
                GameObject.Find("ResetConfirmDialog").GetComponent<Dialog>().Show();
            }
            else
            {
                AlphabetGameManager.instance.ResetShape();
            }
        }

        public void PencilClickEvent(Pencil pencil)
        {
            if (pencil == null)
            {
                return;
            }

            if (AlphabetGameManager.instance.currentPencil != null)
            {
                AlphabetGameManager.instance.currentPencil.DisableSelection();
                AlphabetGameManager.instance.currentPencil = pencil;
            }
            AlphabetGameManager.instance.SetShapeOrderColor();
            pencil.EnableSelection();
        }

        public void ResetConfirmDialogEvent(GameObject value)
        {
            if (value == null)
            {
                return;
            }

            if (value.name.Equals("YesButton"))
            {
                Debug.Log("Reset Confirm Dialog : Yes button clicked");
                AlphabetGameManager.instance.ResetShape();

            }
            else if (value.name.Equals("NoButton"))
            {
                Debug.Log("Reset Confirm Dialog : No button clicked");
            }
            value.GetComponentInParent<Dialog>().Hide();
            AlphabetGameManager.instance.EnableGameManager();

        }

        public void ResetGame()
        {
            AlphabetDataManager.ResetGame();
        }

        public void LeaveApp()
        {
            Debug.Log("Leaving Application");
            Application.Quit();
        }
    }
}