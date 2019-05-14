using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class GuiSlider : MonoBehaviour
    {
        public RectTransform[] sliderTransforms;
        public Image[] navs;
        public Sprite emptyNav;
        public Sprite fullNav;
        public float changeTime = 3.0f;

        private Slide[] slides;
        private RectTransform parent;
        private Vector2[] sliderPositions;
    
        private int current = 0;
        private int length = 0;
        private bool sliderOn = false;

        void Start()
        {
            parent = GetComponent<RectTransform>();
            length = sliderTransforms.Length;
            sliderOn = length > 1;
            slides = new Slide[length];
            sliderPositions = new Vector2[length + 1];
            float parentWidth = parent.sizeDelta.x;
           //s Debug.Log(parentWidth + " pos: " + parent.position);

            for (int i = 0; i < length+1; i++)
            {
                sliderPositions[i] = new Vector2(sliderTransforms[0].anchoredPosition.x + parentWidth*(i-1), sliderTransforms[0].anchoredPosition.y);
            }

            for (int i = 0; i < length; i++)
            {
                slides[i] = new Slide(sliderTransforms[i], i+1, sliderPositions);
            }
            SetNavi();
        }

        float dTime = 0;
        void Update()
        {
            if (sliderOn)
            {
                dTime += Time.deltaTime;
                if (dTime >= changeTime)
                {
                    dTime = 0;
                    MoveLeft(0.2f, ()=> {
                        //Debug.Log("complete");
                    });
                }
            }
        }

        void MoveLeft()
        {
            current++;
            for (int i = 0; i < length; i++)
                slides[i].MoveLeft();
            SetNavi();
        }

        bool moving =  false;
        void MoveLeft(float time, System.Action completeCallBack)
        {
            if (moving) return;
            moving = true;
            current++;
            SetNavi();
            ParallelTween pT = new ParallelTween();

            for (int i = 0; i < length; i++)
            {
                int ii = i;
                pT.Add((callBack) => { slides[ii].MoveLeft(time, callBack); });
            }
            pT.Start(()=> { moving = false; if (completeCallBack != null) completeCallBack(); });
        }

        void SetNavi()
        {
            for (int i = 0; i <navs.Length; i++)
            {
                navs[i].sprite = (i == current%navs.Length) ? fullNav:emptyNav;
            }
        }
    }

    [System.Serializable]
    public class Slide
    {
        public RectTransform rT;
        public int position;
        public Vector2[] positions;

        public Slide(RectTransform rT, int position, Vector2[]positions)
        {
            this.rT = rT;
            this.position = position;
            this.positions = positions;
            if(rT)  rT.anchoredPosition = positions[position];
        }

        public void MoveLeft()
        {
            position--;
            if (position < 0)
            {
                position = positions.Length - 1;
                position--;
            }
           
          if(rT)  rT.anchoredPosition = positions[position];
        }

        public void MoveLeft(float time, System.Action completeCallBack)
        {
            float currPosX = positions[position].x;
            float currPosY = positions[position].y;
            position--;
           
            if (position < 0)
            {
                position = positions.Length - 1;
                currPosX = positions[position].x;
                rT.anchoredPosition = positions[position];
                position--;
            }
            float targetPosX = positions[position].x;
            GameObject gO = (rT) ? rT.gameObject : null;
            SimpleTween.Value(gO, currPosX, targetPosX, time).SetOnUpdate((pos)=> 
            {
              if(this!=null && rT)  rT.anchoredPosition = new Vector2(pos, currPosY);
            }).AddCompleteCallBack(completeCallBack);
        }
    }
}