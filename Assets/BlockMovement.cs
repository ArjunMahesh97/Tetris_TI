using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    float prevTime;
    [SerializeField] float fallTimeGap = 1f;
    float fallTimeGapFactor;
    

    [SerializeField] float downKeySpeedFactor=0.2f;

    [SerializeField] static int w = 10;
    [SerializeField] static int h = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (canMove(-1, 0))
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (canMove(1, 0))
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }

        if(Input.GetKey(KeyCode.S))
        {
            fallTimeGapFactor = fallTimeGap * downKeySpeedFactor;
        }
        else
        {
            fallTimeGapFactor = fallTimeGap;
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
}
