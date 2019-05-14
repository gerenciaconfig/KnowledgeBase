namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using static Arcolabs.Home.Activity;

    public class ActivityButton : MonoBehaviour
    {
        public Activity activity;

        public SpriteRenderer iconSprite;
        public SpriteRenderer spriteBase;

        public bool midItem;
        public List<Sprite> listBases;

        private bool clickedInside;

        public void SetActivity(Activity activity)
        {
            this.activity = activity;
            iconSprite.sprite = activity.activitySprites.iconSprite;
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickedInside = true;
            }

            if (Input.GetMouseButtonUp(0) && clickedInside)
            {
                clickedInside = false;
                HomeIlhasHelper.firstAcess = false;
                HomeIlhasHelper.reEnter = false;
                //SetLastAreaFocus(activity.activityArea);
                //HomeIlhasHelper.previousCamTransform = Camera.main.transform;
                //LoadingScript.nextScene = activity.activtyScene;                
                GetComponent<LoadAssetFileBundle>().LoadGame(false);
                SceneManager.LoadScene(ConstantClass.LOADING);
            }
        }

        private void Start()
        {
            int IndexRange;

            if (midItem)
            {
                IndexRange = Random.Range(1, listBases.Count);
                if (IndexRange >= listBases.Count)
                {
                    IndexRange = 0;
                }
            }
            else
            {
                IndexRange = 0;
                spriteBase.sortingOrder = 2;
            }

            spriteBase.sprite = listBases[IndexRange];

            GetComponent<LoadAssetFileBundle>().SetActivity(activity.levelDTO);
        }

        private void SetLastAreaFocus(ActivityArea area)
        {
            switch (area)
            {
                case ActivityArea.CG:
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IVulcan;
                    break;
                case ActivityArea.EF:
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IBeach;
                    break;
                case ActivityArea.EO:
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IGlacius;
                    break;
                case ActivityArea.ET:
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.ITech;
                    break;
                case ActivityArea.TS:
                    HomeIlhasHelper.previousHomeFocus = HomeIlhasHelper.PreviousHomeFocus.IForest;
                    break;
                default:

                    break;
            }
        }
    }
}