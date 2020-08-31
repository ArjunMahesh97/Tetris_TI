using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static int score = 0;
    [SerializeField] int scorePerLine = 10;

    [SerializeField] GameObject gameOverUI;

    Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
    }

    public void ScoreAdd()
    {
        score += scorePerLine;
    }
    public void OpenGameOverScreen()
    {
        gameOverUI.SetActive(true);
    }
}
