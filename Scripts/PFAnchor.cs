using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mkey
{
    public class PFAnchor : MonoBehaviour
    {
        private bool debug = false;
        [SerializeField]
        private Transform gridParent;

        [SerializeField]
        private Sprite pathSelector;

        [SerializeField]
        private GridCell target;

        [SerializeField]
        private GridCell anchored;

        private List<GameObject> gPathSelectors;
        private Row<GridCell> topRow;
        private BubbleGrid grid;
        private IList<PFCell> path;
        private PathFinder pF;
        private Map map;
        private Vector2 [] pathPoints;

        [SerializeField]
        private float speed = 0.1f;
        private bool moving = false;
        private bool canUpdate = false;
        private bool pathChanged;

        public void DebugPath()
        {
            Debug.Log(pF.DebugPath());
            UpdatePath();
            if (path == null) return;

            if (gPathSelectors != null)
                foreach (var item in gPathSelectors)
                {
                    DestroyImmediate(item);
                }

            gPathSelectors = new List<GameObject>();

            if (pathSelector)
            {
                foreach (var item in path)
                {
                    SpriteRenderer sR = Creator.CreateSpriteAtPosition(null, pathSelector, item.gCell.transform.position, 25);
                    gPathSelectors.Add(sR.gameObject);
                }
            }
           if(debug) Debug.Log(map.ToString());
        }

        #region regular 
        public void InitStart(BubbleGrid grid,  GridCell gCell)
        {
            if (grid == null || !gCell) return;
            map = new Map(grid);
            gameObject.SetActive(true);
            this.grid = grid;
            anchored = gCell;
            target = gCell;
            topRow = grid.Rows[0];
            pF = new PathFinder();
            transform.localScale = gCell.transform.lossyScale;
            SimpleTween.Move(gameObject, transform.position, target.transform.position, 2.5f).AddCompleteCallBack(()=> {
                canUpdate = true;
                moving = false;
                transform.parent = gridParent;
                UpdatePath();
            });
        }

        void Update()
        {
            if (!canUpdate) return;

            if (pathChanged && moving)
            {
                SimpleTween.Cancel(tweenID, true);
                moving = false;

            }
            else if (pathChanged && !moving)
            {
                pathChanged = false;
                SimpleMoveAlongPath(0, EaseAnim.EaseLinear, () => { });
            }
        }

        private void OnDestroy()
        {
            SimpleTween.Cancel(gameObject, false);
        }

        private void OnDisable()
        {
            SimpleTween.Cancel(gameObject, false);
        }
        #endregion regular

        private IList<PFCell> GetPathToTop()
        {
            if (!anchored) return null;
            map.UpdateMap(grid); 
            pF.CreatePathToTop(map, anchored.pfCell);
    
            return pF.FullPath;
        }

        private bool PathEqual(IList<PFCell> newPath)
        {
            if (path == null && newPath == null) return true;
            if(path.Count == newPath.Count)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (path[i] != newPath[i]) return false;
                }
                return true;
            }
            return false;
        }

        public void UpdatePath()
        {
            path = GetPathToTop();
            pathChanged = true;
            if (path != null && path.Count > 0)
                target = path[path.Count - 1].gCell;
            if (debug) Debug.Log("update path: " + path.Count);
        }

        public bool HavePathToTop()
        {
            return (target && target.GRow.IsSeviceRow);
        }

        #region simpletween
        private int tweenID;
        private void SimpleMoveAlongPath(float delay, EaseAnim easeAnim, Action completeCallBack)
        {
            if (path == null || path.Count ==0)
            {
             //   Debug.Log("move start return");
                if (completeCallBack != null) completeCallBack();
                return;
            }

          //  Debug.Log("move start");
            pathPoints = new Vector2[path.Count+1];
            pathPoints[0] = transform.localPosition;
            for (int i = 1; i < pathPoints.Length; i++)
            {
                pathPoints[i] = path[i-1].gCell.transform.localPosition;
            }

            moving = true;
            PolyLine plPath = new PolyLine(pathPoints); 
            float time = plPath.Length / speed;
            int segNumber = 0;
            tweenID = SimpleTween.Value(gameObject, 0, plPath.Length, time).
                   SetOnUpdate((float d) =>
                   {
                       if (this)
                       {
                           gameObject.transform.localPosition = plPath.GetPolyLinePosition(d, out segNumber);
                           if(path!=null && segNumber<path.Count) anchored = path[segNumber].gCell;
                       }

                   }).SetEase(easeAnim).SetDelay(delay).
                   AddCompleteCallBack(() =>
                   {
                      // Debug.Log("tween complete callback");
                       moving = false;
                       if (completeCallBack != null) completeCallBack();
                   }).ID;
        }
        #endregion simpletween
    }
}
