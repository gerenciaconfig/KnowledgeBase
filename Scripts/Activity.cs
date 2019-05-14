namespace Arcolabs.Home
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.Serialization;

    [System.Serializable]
    public class Activity
    {
        public enum ActivityArea { TS, EO, ET, CG, EF };

        public ActivityArea activityArea;

        public LevelDTO levelDTO;

        public string activtyScene;

        public bool downloaded;

        [OdinSerialize]
        public ActivitySprites activitySprites = new ActivitySprites();

        public Activity(LevelDTO levelDTO)
        {
            this.levelDTO = levelDTO;

            foreach (var item in CurrentStatsInfo.ActivityImagesList)
            {
                if (item.name == levelDTO.code + ConstantClass.IMAGE_TYPE_ICON)
                {
                    activitySprites.iconSprite = item;
                    break;
                }
            }

            foreach (var item in CurrentStatsInfo.ActivityImagesList)
            {
                if (item.name == levelDTO.code + ConstantClass.IMAGE_TYPE_THUMB)
                {
                    activitySprites.thumbSprite = item;
                    break;
                }
            }

            if (activitySprites.iconSprite == null)
            {
                //Debug.Log("ICON SPRITE NOT FOUND: " + levelDTO.code + ConstantClass.IMAGE_TYPE_ICON);
            }

            if (activitySprites.thumbSprite == null)
            {
                //Debug.Log("THUMBNAIL SPRITE NOT FOUND: " + levelDTO.code + ConstantClass.IMAGE_TYPE_THUMB);
            }
        }
    }
}