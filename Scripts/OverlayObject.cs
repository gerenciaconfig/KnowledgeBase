using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class OverlayObject : GridObject, IEquatable<OverlayObject>
    {
        public OverlayObjectData OOData { get; private set; }
        public int ID
        {
            get { return (OOData != null) ? OOData.ID : Int32.MinValue; }
        }
        public bool WithScore
        {
            get { return (OOData != null && OOData.WithScore); }
        }

        internal virtual void SetData(OverlayObjectData mData)
        {
            OOData = mData;
            SetToFront(false);
        }

        /// <summary>
        /// Return true if object IDs is Equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(OverlayObject other)
        {
            return (OOData.ID == other.OOData.ID);
        }

        /// <summary>
        /// Create new OverlayObject for gridcell
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="oData"></param>
        /// <param name="addCollider"></param>
        /// <param name="radius"></param>
        /// <param name="isTrigger"></param>
        /// <returns></returns>
        public static OverlayObject Create(GridCell parent, OverlayObjectData oData)
        {
            if (!parent || oData == null) return null;
            GameObject gO =null;
            SpriteRenderer sR =null;
            OverlayObject overlayObject = null;
            if (oData.behaviorPrefab)
            {
                Behaviour bH = Instantiate(oData.behaviorPrefab);
                gO = bH.gameObject;
                gO.transform.position = parent.transform.position;
                gO.transform.localScale = parent.transform.lossyScale;
                gO.transform.parent = parent.transform;
                sR = gO.GetOrAddComponent<SpriteRenderer>();
                sR.sprite = oData.ObjectImage;
            }
            else
            {
                sR = Creator.CreateSpriteAtPosition(parent.transform, oData.ObjectImage, parent.transform.position);
                gO = sR.gameObject;
            }


            overlayObject = gO.GetOrAddComponent<OverlayObject>();
#if UNITY_EDITOR
            gO.name = "overlay " + parent.ToString();
#endif
            overlayObject.Init(parent.Row, parent.Column, parent);
            overlayObject.SetData(oData);

            return overlayObject;
        }

        #region override
        public override void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            ObjectBehavior oB = GetComponent<ObjectBehavior>();
            if (oB) // use private behavior
            {
                oB.ShootAreaCollect(completeCallBack, showPrivateScore, addPrivateScore, decProtection, privateScore);
                return;
            }

            //else
            Debug.Log("collect " + ToString());
            collectSequence = new TweenSeq();
            SetToFront(true);

            collectSequence.Add((callBack) => // play preexplode animation
            {
                SimpleTween.Value(gameObject, 0, 1, 0.050f).AddCompleteCallBack(() =>
                {
                    if (callBack != null) callBack();
                });
            });

            collectSequence.Add((callBack) =>
            {
                SoundMasterController.Instance.SoundPlayClipAtPos(0, OOData.privateClip, null, transform.position, 1.0f);
                SetToFront(true);
                GameObject aP = OOData.hitAnimPrefab;

               // SimpleTween.Move(gameObject, transform.position, new Vector3(transform.position.x + 10, transform.position.y, transform.position.z),2).AddCompleteCallBack(callBack);
            });

            collectSequence.Add((callBack) =>
            {
                if (completeCallBack != null) completeCallBack();
                DestroyImmediate(gameObject);
                callBack();
            });

            collectSequence.Start();
        }

        public override void FallDownCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
            ObjectBehavior oB = GetComponent<ObjectBehavior>();
            if (oB) // use private behavior
            {
                oB.FallDownCollect(completeCallBack, showPrivateScore, addPrivateScore, privateScore);
                return;
            }

            FallDown // else
               (
               () =>
               {
                   if (completeCallBack != null) completeCallBack();
               }
               );
            SetToFront(true);

        }

        public override void ShootHit( Action completeCallBack)
        {
            if (completeCallBack != null) completeCallBack();
        }

        public override void CancellTweensAndSequences()
        {
            base.CancellTweensAndSequences();
        }

        public override void SetToFront(bool set)
        {
            if (set)
                sRenderer.sortingOrder = SortingOrder.OverToFront;
            else
                sRenderer.sortingOrder = SortingOrder.Over;
        }

        public override string ToString()
        {
            return "Overlay: " + ID;
        }
        #endregion override
    }
}
