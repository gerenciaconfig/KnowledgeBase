using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arcolabs.Brushes
{
    public class BrushHelper : MonoBehaviour
    {
        public BrushApplyier Main;
        public bool ShowDrag;


        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            ShowDrag = Main.draggable;
        }
    }
}
