using System;
using System.Collections;
using UnityEngine;
namespace Mkey
{
    public class MainObject : GridObject, IEquatable<MainObject>
    {
        public MainObjectData MOData { get; private set; }
        public int ID
        {
            get { return (MOData!=null) ? MOData.ID : Int32.MinValue; }
        }
        public bool IsExploidable
        {
            get; internal set;
        }
        public bool IsMatchedById
        {
            get { return (MOData != null && MOData.match == Match.ById); }
        }

        public bool IsDestroyable
        {
            get { return (MOData != null && MOData.isDestroyable); }
        }
        public bool WithScore
        {
            get { return (MOData != null && MOData.WithScore); }
        }

        private int hits = 0;

        public int Protection
        {
            get { return (IsDestroyable) ? MOData.protectionStateImages.Length + 1 - hits : 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mData"></param>
        public void SetData(MainObjectData mData)
        {
            MOData = mData;
            baseObjectData = mData;
            SetToFront(false);
        }

        /// <summary>
        /// Return true if object IDs is Equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MainObject other)
        {
            return (MOData.ID == other.MOData.ID);
        }

        /// <summary>
        /// Create new MainObject for gridcell
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mData"></param>
        /// <param name="addCollider"></param>
        /// <param name="radius"></param>
        /// <param name="isTrigger"></param>
        /// <returns></returns>
        public static MainObject Create(GridCell parent, MainObjectData mData, bool addCollider, float radius, bool isTrigger)
        {
            if (!parent || mData == null) return null;
            SpriteRenderer sR = Creator.CreateSpriteAtPosition(parent.transform, mData.ObjectImage, parent.transform.position);
            GameObject gO = sR.gameObject;
            MainObject  Mainobject = gO.AddComponent<MainObject>();

#if UNITY_EDITOR
            gO.name = "main " + parent.ToString();
#endif

            Mainobject.Init(parent.Row, parent.Column, parent);
            if (addCollider)
            {
                CircleCollider2D cC = Mainobject.gameObject.GetOrAddComponent<CircleCollider2D>();
                cC.radius = radius;
                cC.isTrigger = isTrigger;
            }
            Mainobject.SetData(mData);
            return Mainobject;
        }

        #region override
        public override void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            collectSequence = new TweenSeq();
            SetToFront(true);

            collectSequence.Add((callBack) => // play preexplode animation
            {
                SimpleTween.Value(gameObject, 0, 1, 0.050f).AddCompleteCallBack(() =>
                {
                    if (MOData.collectAnimPrefab)
                    {
                        GameObject gA = Instantiate(MOData.collectAnimPrefab);
                        gA.transform.localScale = transform.lossyScale;
                        gA.transform.position = transform.position;
                    }
                    if (callBack != null) callBack();
                });
            });

            collectSequence.Add((callBack) =>
            {
                SoundMasterController.Instance.SoundPlayClipAtPos(0, MOData.privateClip, null, transform.position, 1.0f);
                SetToFront(true);
                GameObject aP = MOData.hitAnimPrefab;
                callBack();
            });

            collectSequence.Add((callBack) =>
            {
                if (showPrivateScore && WithScore && MOData.scoreFlyerPrefab) InstantiateScoreFlyerAtPosition( MOData.scoreFlyerPrefab, privateScore, transform.position);
                if (addPrivateScore && WithScore) BubblesPlayer.Instance.AddScore(privateScore);
                if (completeCallBack != null) completeCallBack();
                Destroy(gameObject);
                callBack();
            });

            collectSequence.Start();
        }

        public override void FallDownCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
            SetToFront(true);
            transform.parent = null; // unparent
            FallDown
                (
                () =>
                {
                    if (MOData.collectAnimPrefab)
                    {
                        SoundMasterController.Instance.SoundPlayClipAtPos(0, MOData.privateClip, null, transform.position, 1.0f);
                        GameObject gA = Instantiate(MOData.collectAnimPrefab);
                        gA.transform.localScale = transform.lossyScale;
                        gA.transform.position = transform.position;
                    }
                    if (showPrivateScore && WithScore && MOData.scoreFlyerPrefab)InstantiateScoreFlyerAtPosition(MOData.scoreFlyerPrefab, privateScore, transform.position);
                    if (addPrivateScore && WithScore) BubblesPlayer.Instance.AddScore(privateScore);
                    if (completeCallBack != null) completeCallBack();
                }
                );
        }
       
        public override void ShootHit(Action completeCallBack)
        {
            if (IsDestroyable)
            {
                hits++;
                if (MOData.protectionStateImages.Length > 0)
                {
                    int i = Mathf.Min(hits - 1, MOData.protectionStateImages.Length - 1);
                    GetComponent<SpriteRenderer>().sprite = MOData.protectionStateImages[i];
                }

                if (MOData.hitAnimPrefab)
                {
                    Creator.InstantiateAnimPrefabAtPosition(MOData.hitAnimPrefab, ParentCell.transform, transform.position, SortingOrder.MainExplode, true, null);
                }

                if (Protection <= 0)
                {
                    hitDestroySeq = new TweenSeq();

                    SetToFront(true);

                    hitDestroySeq.Add((callBack) => // play preexplode animation
                    {
                        SimpleTween.Value(gameObject, 0, 1, 0.050f).AddCompleteCallBack(() =>
                        {
                            if (callBack != null) callBack();
                        });
                    });

                    hitDestroySeq.Add((callBack) =>
                    {
                        SoundMasterController.Instance.SoundPlayClipAtPos(0, MOData.privateClip, null, transform.position, 1.0f);
                        callBack();
                    });

                    hitDestroySeq.Add((callBack) =>
                    {
                        if (completeCallBack != null) completeCallBack();
                        Destroy(gameObject);
                        callBack();
                    });

                    hitDestroySeq.Start();
                }
            }
            else
            {
              if(completeCallBack!=null)  completeCallBack();
            }
        }

        public override void CancellTweensAndSequences()
        {
            base.CancellTweensAndSequences();
        }

        public override void SetToFront(bool set)
        {
            if (set)
                sRenderer.sortingOrder = SortingOrder.MainToFront;
            else
                sRenderer.sortingOrder = SortingOrder.Main;
        }

        public override string ToString()
        {
            return "MainObject: " + ID;
        }

        internal void InstantiateScoreFlyerAtPosition(GameObject scoreFlyerPrefab, int score, Vector3 position)
        {
            GameObject flyer = Instantiate(scoreFlyerPrefab);
            ScoreFlyer sF = flyer.GetComponent<ScoreFlyer>();
            sF.StartFly(score.ToString(), position);
            flyer.transform.localScale = transform.lossyScale;
        }
        #endregion override
    }
}