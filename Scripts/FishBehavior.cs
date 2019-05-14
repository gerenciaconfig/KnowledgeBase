using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class FishBehavior : ObjectBehavior
    {
        [SerializeField]
        private SceneCurve pathToLeft;
        [SerializeField]
        private SceneCurve pathToRight;
        [SerializeField]
        private float speed = 5f;

        #region overrides
        private int hits;
        /// <summary>
        /// If main object in soot area and collected fish swim to side
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <param name="showPrivateScore"></param>
        /// <param name="addPrivateScore"></param>
        /// <param name="decProtection"></param>
        /// <param name="privateScore"></param>
        public override void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            OverlayObject oO = GetComponent<OverlayObject>();

            if(oO && oO.OOData != null)
            {
                collectSequence = new TweenSeq();
                float locScale = transform.localScale.x;
                oO.sRenderer.sprite = oO.OOData.GuiImage;

                collectSequence.Add((callBack) => // scale out
                {
                    SimpleTween.Value(gameObject, locScale, locScale * 1.2f, 0.20f).SetOnUpdate((float val)=> 
                    {
                        transform.localScale = new Vector3(val, val, val);
                    }).AddCompleteCallBack(callBack);
                });

                collectSequence.Add((callBack) =>  //scale in
                {
                    SimpleTween.Value(gameObject, locScale*1.2f, locScale, 0.20f).SetOnUpdate((float val) =>
                    {
                        transform.localScale = new Vector3(val, val, val);
                    }).AddCompleteCallBack(callBack);
                });

                collectSequence.Add((callBack) =>
                {
                    SoundMasterController.Instance.SoundPlayClipAtPos(0, oO.OOData.privateClip, null, transform.position, 1.0f);
                    GameObject aP = oO.OOData.hitAnimPrefab;
                    Transform rel = GetComponentInParent<GridCell>().transform;
                    SceneCurve path = (UnityEngine.Random.Range(0, 2) == 0) ? pathToLeft : pathToRight; 
                    path.MoveAlongPath(gameObject, rel, path.Length / speed, 0, EaseAnim.EaseInOutSine, callBack);
                });

                collectSequence.Add((callBack) =>
                {
                    //  if (showPrivateScore) EffectsHolder.Instance.InstantiateScoreFlyerAtPosition(privateScore, transform.position, oO.OOData.privateFont);
                    //   if (addPrivateScore) BubblesPlayer.Instance.AddScore(privateScore);

                    if (completeCallBack != null) completeCallBack();
                    DestroyImmediate(gameObject);
                    callBack();
                });

                collectSequence.Start();
            }
            else
            {
                if (completeCallBack != null) completeCallBack();
            }
        }

        /// <summary>
        /// If main object falldown fish swim to side
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <param name="showPrivateScore"></param>
        /// <param name="addPrivateScore"></param>
        /// <param name="privateScore"></param>
        public override void FallDownCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
            OverlayObject oO = GetComponent<OverlayObject>();

            if (oO && oO.OOData != null)
            {
                collectSequence = new TweenSeq();// Debug.Log("Fish collect " + ToString());
                float locScale = transform.localScale.x;
                oO.sRenderer.sprite = oO.OOData.GuiImage;

                collectSequence.Add((callBack) => // scale out
                {
                    SimpleTween.Value(gameObject, locScale, locScale * 1.2f, 0.20f).SetOnUpdate((float val) =>
                    {
                        transform.localScale = new Vector3(val, val, val);
                    }).AddCompleteCallBack(callBack);
                });

                collectSequence.Add((callBack) =>  //scale in
                {
                    SimpleTween.Value(gameObject, locScale * 1.2f, locScale, 0.20f).SetOnUpdate((float val) =>
                    {
                        transform.localScale = new Vector3(val, val, val);
                    }).AddCompleteCallBack(callBack);
                });

                collectSequence.Add((callBack) =>
                {
                    SoundMasterController.Instance.SoundPlayClipAtPos(0, oO.OOData.privateClip, null, transform.position, 1.0f);
                    GameObject aP = oO.OOData.hitAnimPrefab;
                    Transform rel = GetComponentInParent<GridCell>().transform;

                    SceneCurve path = (UnityEngine.Random.Range(0, 2) == 0) ? pathToLeft : pathToRight;
                    path.MoveAlongPath(gameObject, rel, path.Length / speed, 0, EaseAnim.EaseInOutSine, callBack);
                });

                collectSequence.Add((callBack) =>
                {
                    //   if (showPrivateScore) EffectsHolder.Instance.InstantiateScoreFlyerAtPosition(privateScore, transform.position, oO.OOData.privateFont);
                    //  if (addPrivateScore) BubblesPlayer.Instance.AddScore(privateScore);
                    if (completeCallBack != null) completeCallBack();
                    DestroyImmediate(gameObject);
                    callBack();
                });

                collectSequence.Start();
            }
            else
            {
                if (completeCallBack != null) completeCallBack();
            }
        }

        /// <summary>
        /// After hit fish can swim to another cell
        /// </summary>
        /// <param name="completeCallBack"></param>
        public override void ShootHit(Action completeCallBack)
        {
            hits++;
        }
        #endregion overrides
    }
}