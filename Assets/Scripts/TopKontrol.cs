using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopKontrol : MonoBehaviour
{

    public float MoveSpeed = 10;
    public Transform Ball;
    public Transform PosDribble;
    public Transform flyStartPos;
    public Transform Target;

    //variables
    private bool IsBallInChar = true;
    private bool IsBallFlying = false;
    private float T = 0;

    void Update()
    {

        // walking
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * MoveSpeed * Time.deltaTime;

        // ball in hands
        if (IsBallInChar)
        {
            Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));

            // throw ball // atýþ topu
            if (Input.GetMouseButton(0))
            {
                IsBallInChar = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        // ball in the air
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // move to target
            Vector3 A = flyStartPos.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // move in arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // moment when ball arrives at the target
            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!IsBallInChar && !IsBallFlying)
        {
            IsBallInChar = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "test")
        {
            Debug.Log("ÝÇÝNDE");
        }

    }
}
