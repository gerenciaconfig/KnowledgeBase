using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mkey
{
    public enum WinAnimType {AlphaFade, Move, Scale}

    public enum Position {LeftMiddleOut, RightMiddleOut, MiddleBottomOut, MiddleTopOut, LeftMiddleIn, RightMiddleIn, MiddleBottomIn, MiddleTopIn, CustomPosition, AsIs, Center}

    public enum ScaleType {CenterXY, CenterX, CenterY, Top, Bottom, Left, Right}

    [RequireComponent(typeof(GuiFader_v2))]
    public class PopUpsController : MonoBehaviour
    {

        bool showed;

        public void CloseWindow()
        {
            SetControlActivity(false);
            GetComponent<GuiFader_v2>().FadeOut(0, () => { CloseHandler(); });
        }

        public void CloseButton_click()
        {
            SetControlActivity(false);
            if (SoundMasterController.Instance) SoundMasterController.Instance.SoundPlayPopUp(0.2f, null);
            GetComponent<GuiFader_v2>().FadeOut(0, () => { CloseHandler(); });
        }

        /// <summary>
        /// Run after closing windows as completeCallBack.
        /// </summary>
        private void CloseHandler()
        {
            showed = false;
            if (closeDelegate != null) closeDelegate(gameObject);
        }

        /// <summary>
        /// Run after creating windows before it visible. Refresh window. Play open sound. FadeIn. Run openD - openDelegate. Set window control activity.
        /// </summary>
        public void ShowWindow()
        {
            RefreshWindow();
            if (SoundMasterController.Instance) SoundMasterController.Instance.SoundPlayPopUp(0.2f, null);

            GetComponent<GuiFader_v2>().FadeIn(0, () =>
            {
                SetControlActivity(true);
                showed = true;
                if (openDelelegate != null) openDelelegate(gameObject);
            });
        }

        public void SetControlActivity(bool activity)
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        public bool IsVisible()
        {
            return showed;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        Action<GameObject> openDelelegate;
        Action<GameObject> closeDelegate;
        /// <summary>
        /// Set delegates openDel(started afetr opening), cleseDel started after closing
        /// </summary>
        /// <param name="openDel"></param>
        /// <param name="closeDel"></param>
        public void PopUpInit(Action<GameObject> openDel, Action<GameObject> closeDel)
        {
            if (openDel != null) openDelelegate = openDel;
            else
            {
                openDelelegate = new Action<GameObject>((gameObject) => { });
            }

            if (closeDel != null) closeDelegate = closeDel;
            else
            {
                closeDelegate = new Action<GameObject>((gameObject) => { });
            }
        }

        /// <summary>
        /// Refresh window data before it visible
        /// </summary>
        public virtual void RefreshWindow() { }
    }

    [Serializable]
    public class WindowOpions
    {
        public WinAnimType inAnim;
        public WinAnimType outAnim;

        public MoveAnim inMoveAnim;
        public MoveAnim outMoveAnim;

        public ScaleAnim inScaleAnim;
        public ScaleAnim outScaleAnim;

        public FadeAnim inFadeAnim;
        public FadeAnim outFadeAnim;

        public Position instantiatePosition;
        public Vector2 position;

        public WindowOpions()
        {
            inAnim = WinAnimType.AlphaFade;
            outAnim = WinAnimType.AlphaFade;
            inFadeAnim = new FadeAnim();
            outFadeAnim = new FadeAnim();
        }

        public WindowOpions(MoveAnim inMoveAnim, MoveAnim outMoveAnim)
        {
            inAnim = WinAnimType.Move;
            outAnim = WinAnimType.Move;
            this.inMoveAnim = inMoveAnim;
            this.outMoveAnim = outMoveAnim;
        }

        public WindowOpions(ScaleAnim inScaleAnim, ScaleAnim outScaleAnim)
        {
            inAnim = WinAnimType.Scale;
            outAnim = WinAnimType.Scale;
            this.inScaleAnim = inScaleAnim;
            this.outScaleAnim = outScaleAnim;
        }

        public WindowOpions(FadeAnim inFadeAnim, FadeAnim outFadeAnim)
        {
            inAnim = WinAnimType.AlphaFade;
            outAnim = WinAnimType.AlphaFade;
            this.inFadeAnim = inFadeAnim;
            this.outFadeAnim = outFadeAnim;
        }
    }

    [Serializable]
    public class FadeAnim 
    {
        public float time;
        public FadeAnim()
        {
            time = 0.2f;
        }
    }

    [Serializable]
    public class MoveAnim 
    {
        public Position toPosition;
        public float time;
        public Vector3 customPosition;
        public bool useMask;

        public MoveAnim()
        {
            time = 0.2f;
            toPosition = Position.AsIs;
        }
    }

    [Serializable]
    public class ScaleAnim 
    {
        public ScaleType scaleType;
        public float time;

        public ScaleAnim()
        {
            time = 0.2f;
            scaleType = ScaleType.CenterXY;
        }
    }

}

