using UnityEngine;
using System.Collections;

public class TriggerBase2D {


}

public delegate void TriggerDel(GameObject o, GameObject s);

public interface ITrigger2D
{
    void OnTriggerEnter2D(Collider2D other);

    void OnTriggerStay2D(Collider2D other);

    void OnTriggerExit2D(Collider2D other);
}
