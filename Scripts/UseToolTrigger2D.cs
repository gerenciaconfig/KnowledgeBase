using UnityEngine;
using UnityEngine.Events;

//MonoBehavior class for trigger handling
public class UseToolTrigger2D : MonoBehaviour, ITrigger2D
{

    public TriggerDel enterDel;
    public TriggerDel stayDel;
    public TriggerDel exitDel;
 
  	public UnityEvent enterCallback;
    public UnityEvent stayCallback;
    public UnityEvent exitCallback;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (enterDel != null)
        {
            enterDel(other.gameObject, gameObject);
        }
        if (enterCallback != null)
        {
            enterCallback.Invoke();
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (exitDel != null)
        {
            exitDel(other.gameObject, gameObject);
        }

        if (exitCallback != null)
        {
            exitCallback.Invoke();
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (stayDel != null)
        {
            stayDel(other.gameObject, gameObject);
        }

        if (stayCallback != null)
        {
            stayCallback.Invoke();
        }
    }

}
