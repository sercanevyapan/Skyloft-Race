using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float lastFingerPos;
    private float moveX;
    public float playerSwerveSpeed;
    public float maxSwerveAmount=5;
    public float rotateSpeed;
    public bool isRightCurve, isLeftCurve, isReverse,isCrash;


    public float rotateAngle;
    private Vector3 newPos = Vector3.zero;
    [SerializeField] Rigidbody _rbPathFollower;
    public float m_Thrust = 20f;
    [SerializeField] PathCreation.Examples.PathFollower pathFollower;

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

    private void FixedUpdate()
    {
        if (isCrash)
        {
            _rbPathFollower.AddForce(0, 0, -1f);
            print("crash");
        }
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
            isRightCurve = true;
            isReverse = false;
        }
        if (other.CompareTag("LeftCurve"))
        {
            isLeftCurve = true;
            isReverse = false;
        }

        if (other.CompareTag("Obstacle"))
        {

            isCrash = true;
            pathFollower.isObstacleCrash = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightCurve"))
        {
            isRightCurve = false;
            isReverse = true;
        }
        if (other.CompareTag("LeftCurve"))
        {
            isLeftCurve = false;
            isReverse = true;
        }
    }


  
}
