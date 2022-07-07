using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraKontrol : MonoBehaviour
{
    public GameObject top;
    public float lerpSpeed = 3;
    Vector3 aradakiMesafe;

    void Start()
    {
        aradakiMesafe = transform.position - top.transform.position;
        aradakiMesafe = new Vector3(aradakiMesafe.x, 0, aradakiMesafe.z);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(top.transform.position.x, transform.position.y, top.transform.position.z) + aradakiMesafe;
    }
}
