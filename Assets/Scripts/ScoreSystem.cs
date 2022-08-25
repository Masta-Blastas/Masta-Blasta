using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public FloatVariable score;
    public FloatVariable highScore;

    private void Start()
    {
        score.Value = 0;
    }

    public void SetHighScore()
    {
        if (score.Value > highScore.Value)
        {
            highScore.SetValue(score);
        }
    }


}
