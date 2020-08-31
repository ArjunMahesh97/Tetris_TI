using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    float prevTime;
    [SerializeField] float fallTimeGap = 1f;
    float fallTimeGapFactor;
    

    [SerializeField] float downKeySpeedFactor=0.2f;
    [SerializeField] float rotationAngle = -90f;

    [SerializeField] static int w = 10;
    [SerializeField] static int h = 20;

    [SerializeField] Vector3 rotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (canMove(-1, 0))
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (canMove(1, 0))
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            fallTimeGapFactor = fallTimeGap * downKeySpeedFactor;
        }
        else
        {
            fallTimeGapFactor = fallTimeGap;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {

            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), rotationAngle);
            if (!canRotate()) 
            { 
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -rotationAngle); 
            } 
        }

        if(Time.time-prevTime>fallTimeGapFactor)
        {
            if (canMove(0, -1))
            {
                transform.position += new Vector3(0, -1, 0);
            }
            prevTime = Time.time;
        }
    }

    bool canMove(int xMove, int yMove)
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            int xNextMove = roundX + xMove;
            int yNextMove = roundY + yMove;

            if (xNextMove < 0 || xNextMove >= w)
            {
                return false;
            }

            if (yNextMove < 0 || yNextMove >= h)
            {
                return false;
            }
        }

        return true;
    }

    bool canRotate()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            if (roundX < 0 || roundX >= w)
            {
                return false;
            }

            if (roundY < 0 || roundY >= h)
            {
                return false;
            }
        }
        return true;
    }
}
