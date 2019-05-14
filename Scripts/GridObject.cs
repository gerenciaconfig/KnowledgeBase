using System;
using System.Collections;
using UnityEngine;

namespace Mkey
{
    public class GridObject : MonoBehaviour
    {
        private static PhysicsMaterial2D physMat;
        public int Row { get; private set; }
        public int Column { get; private set; }
        public GridCell ParentCell { get; private set; }
        public SpriteRenderer sRenderer { get; private set; }
        public BaseObjectData baseObjectData { get; protected set; }

        protected TweenSeq collectSequence;
        protected TweenSeq hitDestroySeq;


        #region regular
        void OnDestroy()
        {
           // Debug.Log("destroy " + ToString());
            CancellTweensAndSequences();
        }
        #endregion regular


        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="parentCell"></param>
        internal void Init(int row, int column, GridCell parentCell)
        {
            this.Row = row;
            this.Column = column;
            this.ParentCell = parentCell;
            sRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Return true if is the same object (the same reference)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal bool ReferenceEquals(GridObject other)
        {
            return System.Object.ReferenceEquals(this, other);//Determines whether the specified Object instances are the same instance.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        internal void SetLocalScale(float scale)
        {
            transform.localScale = ParentCell.transform.localScale * scale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        internal void SetLocalScaleX(float scale)
        {
            float ns = ParentCell.transform.localScale.x * scale;
            transform.localScale = new Vector3(ns, ParentCell.transform.localScale.y, ParentCell.transform.localScale.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        internal void SetLocalScaleY(float scale)
        {
            float ns = ParentCell.transform.localScale.y * scale;
            transform.localScale = new Vector3(ParentCell.transform.localScale.x, ns, ParentCell.transform.localScale.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        internal void SetAlpha(float alpha)
        {
            Color c = sRenderer.color;
            Color newColor = new Color(c.r, c.g, c.b, alpha);
            sRenderer.color = newColor;
        }

        protected void FallDown(Action completeCallBack)
        {
            Rigidbody2D rB = gameObject.AddComponent<Rigidbody2D>();
            rB.mass = UnityEngine.Random.Range(1, 2);
            rB.isKinematic = false;
            rB.bodyType = RigidbodyType2D.Dynamic;
            if (!physMat)
            {
                physMat = new PhysicsMaterial2D();
                physMat.bounciness = 1f;
            }
            rB.sharedMaterial = physMat;
            rB.AddForce(UnityEngine.Random.onUnitSphere * 10f, ForceMode2D.Impulse);
            CircleCollider2D cC = gameObject.GetComponent<CircleCollider2D>();
            if (cC) cC.isTrigger = false;
            // rB.drag = 1;
            StartCoroutine(FallDownDestrtoy(completeCallBack));
        }

        private IEnumerator FallDownDestrtoy(Action completeCallBack)
        {
            while (transform.position.y > -10)
            {
                yield return new WaitForEndOfFrame();
            }
            if (completeCallBack != null) completeCallBack();
            Destroy(gameObject);
        }

        #region virtual
        /// <summary>
        /// Collect object with shootarea
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <param name="showPrivateScore"></param>
        /// <param name="addPrivateScore"></param>
        /// <param name="decProtection"></param>
        /// <param name="privateScore"></param>
        public virtual void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {

        }

        /// <summary>
        /// Collect as fall down
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <param name="showPrivateScore"></param>
        /// <param name="addPrivateScore"></param>
        /// <param name="privateScore"></param>
        public virtual void FallDownCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
          
        }

        /// <summary>
        /// Hit object from shoot bubble
        /// </summary>
        /// <param name="completeCallBack"></param>
        public virtual void ShootHit( Action completeCallBack)
        {

        }

        /// <summary>
        /// Cancel all tweens and sequences
        /// </summary>
        public virtual void CancellTweensAndSequences()
        {
            if (collectSequence != null) collectSequence.Break();
            if (hitDestroySeq != null) hitDestroySeq.Break();
            SimpleTween.Cancel(gameObject, false);
        }

        public virtual void SetToFront(bool set)
        {

        }
        #endregion virtual
    }
}
