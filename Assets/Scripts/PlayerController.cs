using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State
    {
        LeftCurveEnter,
        LeftCurveExit,
        RightCurveEnter,
        RightCurveExit,
        SlowMovement,
        Obstacle
    }

    private State _state;

    private void ChangeState(State newState)
    {
        _state = newState;

        switch (newState)
        {
            case State.LeftCurveEnter:
                isLeftCurve = true;
                isReverse = false;
                break;

            case State.LeftCurveExit:
                isLeftCurve = false;
                isReverse = true;
                break;

            case State.RightCurveEnter:
                isRightCurve = true;
                isReverse = false;
                break;

            case State.RightCurveExit:
                isRightCurve = false;
                isReverse = true;
                break;

            case State.Obstacle:
                pathFollower.isObstacle = true;
                isLeftCurve = false;
                isRightCurve = false;
                isReverse = true;
                StartCoroutine(BackForce());
                break;
            case State.SlowMovement:
                SlowMovement();
                break;
        }
    }

   

    [SerializeField] Rigidbody _rbPlayer;
    [SerializeField] PathCreation.Examples.PathFollower pathFollower;

    private float lastFingerPos;
    private float moveX;
    private bool isRightCurve, isLeftCurve, isReverse;
    private Vector3 newPos = Vector3.zero;
    public float playerSwerveSpeed;
    public float maxSwerveAmount = 5;
    public float rotateSpeed;
    public float rotateAngle;
  
    void Update()
    {

        PlayerInput();

        SwerveMovement();

        if (isRightCurve)
        {
            PlayerRotate(-rotateAngle);
        }

        if (isLeftCurve)
        {
            PlayerRotate(rotateAngle);
        }

        if (isReverse)
        {
            PlayerRotateReverse();
        }


    }

    private IEnumerator BackForce()
    {
        yield return new WaitForSeconds(1);
        ChangeState(State.SlowMovement);
        
    }
    private void SlowMovement()
    {
        pathFollower.isObstacle = false;
        pathFollower.speed = 10;
        StartCoroutine(FastMovement());
    }

    private IEnumerator FastMovement()
    {
        yield return new WaitForSeconds(2);
        pathFollower.isSpeedIncease = true;
    }

    private void SwerveMovement()
    {
        float newX = Time.deltaTime * playerSwerveSpeed * moveX;
        newPos.x += newX;
        newPos.x = Mathf.Clamp(newPos.x, -maxSwerveAmount, maxSwerveAmount);

        transform.localPosition = newPos;
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastFingerPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            moveX = Input.mousePosition.x - lastFingerPos;
            lastFingerPos = Input.mousePosition.x;


        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveX = 0f;

        }
    }

    private void PlayerRotate(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);//Smooth rotation
    }

    private void PlayerRotateReverse()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);//Smooth rotation
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightCurve"))
        {
            ChangeState(State.RightCurveEnter);
          
        }
        if (other.CompareTag("LeftCurve"))
        {
            ChangeState(State.LeftCurveEnter);

        }
        if (other.CompareTag("Obstacle"))
        {
            ChangeState(State.Obstacle);
         
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightCurve"))
        {
            ChangeState(State.RightCurveExit);
       
        }
        if (other.CompareTag("LeftCurve"))
        {
            ChangeState(State.LeftCurveExit);
          
        }
    }

}
