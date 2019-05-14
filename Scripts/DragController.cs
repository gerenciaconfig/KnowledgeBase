using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcolabs.Brushes;

namespace Arcolabs.Brushes { 
    public class DragController : MonoBehaviour {


        public BrushApplyier brushApplyier;

        private void OnMouseEnter()
        {
            brushApplyier.DisableDraggable();
        }

        private void OnMouseExit()
        {
            brushApplyier.EnableDraggable();
        }
    }
}
