using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mkey
{
    public class ScrollPanelController : MonoBehaviour
    {
        public Text textCaption;
        public RectTransform scrollContent;

        private void OnDestroy()
        {
            SimpleTween.Cancel(gameObject, true);
        }

        /// <summary>
        /// Open panel
        /// </summary>
        /// <param name="panel"></param>
        public void OpenScrollPanel(Action completeCallBack )
        {
            RectTransform panel = GetComponent<RectTransform>();
            transform.localScale = new Vector3(0, 1, 1);
            if (!panel) return;
            SetControlActivity( false);
            float startX = 0;
            float endX = 1f;
            SimpleTween.Cancel(gameObject, true);

            SimpleTween.Value(gameObject, startX, endX, 0.2f).SetEase(EaseAnim.EaseInCubic).
                               SetOnUpdate((float val) =>
                               {
                                   transform.localScale = new Vector3(val, 1, 1);
                               }).AddCompleteCallBack(() =>
                               {
                                   SetControlActivity(true);
                                   if (completeCallBack != null) completeCallBack();
                               });
        }

        /// <summary>
        /// Close panel
        /// </summary>
        /// <param name="panel"></param>
        public void CloseScrollPanel(bool destroy, Action completeCallBack)
        {
            RectTransform panel = GetComponent<RectTransform>();
            if (!panel) return;
            SetControlActivity(false);
            float startX = 1;
            float endX = 0f;
            SimpleTween.Cancel(gameObject, true);
            SimpleTween.Value(gameObject, startX, endX, 0.2f).SetEase(EaseAnim.EaseInCubic).
                               SetOnUpdate((float val) =>
                               {
                                   transform.localScale = new Vector3(val, 1, 1);
                               }).AddCompleteCallBack(() =>
                               {
                                   if (destroy && this) DestroyImmediate(gameObject);
                                   if (completeCallBack != null) completeCallBack();
                               });
        }

        private void SetControlActivity(bool activity)
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

    }
}