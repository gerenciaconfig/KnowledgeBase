using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


    class MathMk
    {

        /// <summary>
        ///  Return full (360 deg) angle between v and OX axe;
        /// </summary>
        public static float GetFullAngleOX(Vector2 v)
        {
            float a = 0f;
            a = Vector2.Angle(v, Vector2.right);
            if (v.y >= 0)
                return a;
            else
            {
                return 360f - a;
            }
        }
    }

 
  