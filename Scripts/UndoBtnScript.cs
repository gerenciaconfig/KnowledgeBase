using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Arcolabs.Brushes { 
    public class UndoBtnScript : MonoBehaviour {

        public BrushApplyier brushApplyier;
        public Toggle UndoToggle;
        private Color PreviousColor;
        public float waitTime;

        void Start()
        {
            //Add listener for when the state of the Toggle changes, and output the state
            UndoToggle.onValueChanged.AddListener(delegate {
                ToggleValueChanged(UndoToggle);
            });

            PreviousColor = UndoToggle.targetGraphic.color;
        }
        

        //Output the new state of the Toggle into Text when the user uses the Toggle
        void ToggleValueChanged(Toggle change)
        {
            if (UndoToggle.isOn == true) { 
                brushApplyier.Undo();

                StartCoroutine(WaitToNextBrush(waitTime));                
            }
        
        }
    

        public IEnumerator WaitToNextBrush(float waitTime)
        {
           yield return new WaitForSeconds(waitTime);
            brushApplyier.brush.SetToggleTrue();            
        }

    }
}
