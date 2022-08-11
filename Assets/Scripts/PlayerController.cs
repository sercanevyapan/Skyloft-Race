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
                StartCoroutine(StopBackForce());
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

    private IEnumerator StopBackForce()
    {
        yield return new WaitForSeconds(1);

        pathFollower.isObstacle = false;
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
