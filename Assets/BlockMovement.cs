using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    float prevTime;
    [SerializeField] float fallTimeGap = 1f;
    float fallTimeGapFactor;
    

    [SerializeField] float downKeySpeedFactor=0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
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
            transform.position += new Vector3(0, -1, 0);
            prevTime = Time.time;
        }
    }
}
