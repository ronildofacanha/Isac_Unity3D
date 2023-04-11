using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRL : MonoBehaviour
{
    public Transform player;
    public Transform R;
    public Transform L;

    Vector3 newPos;
    void Update()
    {
        newPos = this.transform.position;
        newPos.x = player.position.x;
        newPos.x = Mathf.Clamp(newPos.x, L.position.x, R.position.x);
        this.transform.position = newPos;
    }
}
