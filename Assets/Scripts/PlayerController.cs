using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float lastFingerPos;
    private float moveX;
    public float playerSwerveSpeed = 0.5f;
    public float maxSwerveAmount=5;

    private Vector3 newPos = Vector3.zero;
  
    void Update()
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

        float newX = Time.deltaTime * playerSwerveSpeed * moveX;
        newPos.x += newX;
        newPos.x = Mathf.Clamp(newPos.x, -maxSwerveAmount, maxSwerveAmount);

        transform.localPosition = newPos;
        //float swerveAmount = Time.deltaTime * playerSwerveSpeed * moveX;
        //swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
        //transform.Translate(swerveAmount, 0, 0);


    }
}
