using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // Start is called before the first frame update
    void ActionDir()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 2);
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().AddTorque(100.0f);
        Invoke("Dell", 2.0f);
    }

    void ActionEsq()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(10, 2);
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().AddTorque(-100.0f);
        Invoke("Dell", 2.0f);
    }

    void Dell()
    {
        Destroy(this.gameObject);
    }

}
