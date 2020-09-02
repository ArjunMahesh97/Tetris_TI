using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockMovement : MonoBehaviour
{
    float prevTime;
    float fallTimeGapFactor;

    [SerializeField] static int width = 10;
    [SerializeField] static int height = 20;

    [SerializeField] float fallTimeGap = 1f;
    [SerializeField] float downKeySpeedFactor=0.2f;
    [SerializeField] float rotationAngle = -90f;
    [SerializeField] int lossRow = 16;  //row number at which loss is triggered

    [SerializeField] Vector3 rotationPoint; //set manually for different types of tetromino

    Spawner spawner;
    ScoreManager scoreManager;

    static Transform[,] stopPositions = new Transform[width, height];  //grid positions for blocks after dropping

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleVerticalMovement();
    }

    private void HandleInput()
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

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            fallTimeGapFactor = fallTimeGap * downKeySpeedFactor;  //speed up fall
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
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -rotationAngle);  //to prevent rotation outside bounds
            }
        }
    }

    private void HandleVerticalMovement()
    {
        if (Time.timeSinceLevelLoad - prevTime > fallTimeGapFactor)
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

                if (!CheckLoss())
                {
                    spawner.Spawn();
                }
                else
                {
                    //stopPositions = new Transform[w, h];
                    SceneManager.LoadScene("GameOver");
                }
            }
            prevTime = Time.timeSinceLevelLoad;
        }
    }


    bool canMove(int xMove, int yMove) //checks if next move is possible
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            int xNextMove = roundX + xMove;
            int yNextMove = roundY + yMove;

            if (xNextMove < 0 || xNextMove >= width)
            {
                return false;
            }

            if (yNextMove < 0 || yNextMove >= height)
            {
                return false;
            }

            if (stopPositions[xNextMove, yNextMove] != null) //checks for blocks that have already been dropped
            {
                return false;
            }
        }

        return true;
    }

    bool canRotate()  //checks if current rotation is possible
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            if (roundX < 0 || roundX >= width)
            {
                return false;
            }

            if (roundY < 0 || roundY >= height)
            {
                return false;
            }

            if (stopPositions[roundX, roundY] != null)  //checks for blocks that have already been dropped
            {
                return false;
            }
        }
        return true;
    }

    void updateStopPos() //updates grid of stopped positions
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            stopPositions[roundX, roundY] = child;
        }
    }

    void LineCheck()  //checks for horizontal complete lines
    {
        for(int i = height - 1; i >= 0; i--)
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
        for(int j = 0; j < width; j++)
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
        for(int j = 0; j < width; j++)
        {
            Destroy(stopPositions[j, i].gameObject);
            stopPositions[j, i] = null;
        }
    }

    void RowMove(int i) //moves all blocks below after line has been deleted
    {
        for(int y = i; y < height; y++)
        {
            for(int j = 0; j < width; j++)
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

    bool CheckLoss()
    {
        for (int i = 0; i < width; i++)
        {
            if (stopPositions[i, lossRow] != null)
            {
                //Time.timeScale = 0f;
                return true;
            }
        }
        return false;
    }
}
