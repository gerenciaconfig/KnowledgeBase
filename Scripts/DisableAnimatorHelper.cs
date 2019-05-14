using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Home { 

    public class DisableAnimatorHelper : MonoBehaviour
    {

        public bool destroy = true;

        public void DisableAnimator()
        {
            gameObject.GetComponent<Animator>().enabled = false;
            WorldManager.curInstances--;
            if (destroy)
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
