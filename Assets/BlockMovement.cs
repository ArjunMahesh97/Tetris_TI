using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    float prevTime;
    float fallTimeGapFactor;

    [SerializeField] float fallTimeGap = 1f;
    [SerializeField] float downKeySpeedFactor=0.2f;
    [SerializeField] float rotationAngle = -90f;

    [SerializeField] static int w = 10;
    [SerializeField] static int h = 20;

    [SerializeField] Vector3 rotationPoint;

    Spawner spawner;
    ScoreManager scoreManager;

    static Transform[,] stopPositions = new Transform[w, h];

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        scoreManager = FindObjectOfType<ScoreManager>();
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
            else
            {
                updateStopPos();
                LineCheck();

                this.enabled = false;
                spawner.Spawn();
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

            if (stopPositions[xNextMove, yNextMove] != null)
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

            if (stopPositions[roundX, roundY] != null)
            {
                return false;
            }
        }
        return true;
    }

    void updateStopPos()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            stopPositions[roundX, roundY] = child;
        }
    }

    void LineCheck()
    {
        for(int i = h - 1; i >= 0; i--)
        {
            if (IsLine(i))
            {
                DeleteLine(i);
                RowMove(i);
                scoreManager.ScoreAdd();
            }
        }
    }

    bool IsLine(int i)
    {
        for(int j = 0; j < w; j++)
        {
            if (stopPositions[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for(int j = 0; j < w; j++)
        {
            Destroy(stopPositions[j, i].gameObject);
            stopPositions[j, i] = null;
        }
    }

    void RowMove(int i)
    {
        for(int y = i; y < h; y++)
        {
            for(int j = 0; j < w; j++)
            {
                if (stopPositions[j, y] != null)
                {
                    stopPositions[j, y - 1] = stopPositions[j, y];
                    stopPositions[j, y] = null;
                    stopPositions[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

}
