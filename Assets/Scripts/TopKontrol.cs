using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopKontrol : MonoBehaviour
{
    enum BallState
    {
        Idle = 0,
        InCharacterHand,
        Shooting,
    }

    private BallState ballState;
    public float MoveSpeed = 10;
    public Transform Ball;
    public Transform PosDribble;
    public Transform Target;

    private float timer = 0;
    private float duration = 1f;

    public Transform[] possibleBasketPositions;
    public float basketMaxDistance;
    private Vector3 flyStartPos;
    private Vector3 targetPos;
    private Vector3 ballVelocity;

    void Update()
    {
        // walking
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * (MoveSpeed * Time.deltaTime);

        switch (ballState)
        {
            case BallState.Idle:
                break;
            case BallState.InCharacterHand:
                var previousPos = Ball.position;
                var nextPos = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                ballVelocity = (nextPos - previousPos)  / Time.deltaTime;
                Ball.position = nextPos;
                break;
            case BallState.Shooting:
                timer += Time.deltaTime;
                ShootToPosition(flyStartPos, targetPos, timer);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // throw ball // topu at
        if (Input.GetMouseButton(0) && ballState == BallState.InCharacterHand)
        {
            timer = 0;
            flyStartPos = Ball.position;
            if (CanShootCorrectly())
            {
                targetPos = Target.position;
            }
            else
            {
                targetPos = this.transform.position + ((Vector3.forward * 2) + (Vector3.forward * 4));
            }

            ballState = BallState.Shooting;
        }
    }

    private void ShootToPosition(Vector3 startPos, Vector3 targetPos, float t)
    {
        float normalizedTime = t / duration;

        // move to target
        Vector3 pos = Vector3.Lerp(startPos, targetPos, normalizedTime);

        // move in arc
        Vector3 arc = Vector3.up * (5 * Mathf.Sin(normalizedTime * 3.14f));

        var previousPos = Ball.position;
        var nextPos = pos + arc;
        ballVelocity = (nextPos - previousPos)  / Time.deltaTime;
        Ball.position = nextPos;

        // moment when ball arrives at the target
        if (normalizedTime >= 1)
        {
            var ballRb = Ball.GetComponent<Rigidbody>();
            ballRb.isKinematic = false;
            ballRb.velocity = ballVelocity;
            ballState = BallState.Idle;
        }
    }

    private bool CanShootCorrectly()
    {
        var currentPos = this.transform.position;
        for (int i = 0; i < possibleBasketPositions.Length; i++)
        {
            var distance = Vector3.Distance(currentPos, possibleBasketPositions[i].position);
            if (distance < basketMaxDistance)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ballState == BallState.Idle)
        {
            Ball.GetComponent<Rigidbody>().isKinematic = true;
            ballState = BallState.InCharacterHand;
        }
    }
    
}
