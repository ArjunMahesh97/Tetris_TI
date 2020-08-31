using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] Tetrominoes;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        int randomNumber = Random.Range(0, Tetrominoes.Length);

        Instantiate(Tetrominoes[randomNumber], transform.position, Quaternion.identity);
    }
}
