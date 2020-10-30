using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    #region Static Fields

    public static bool isClearing;
    public static bool hasSpun = false;
    public static bool hasKicked = false;
    public static int score = 0;

    #endregion

    #region Private Serialized Field

    [SerializeField] private RowManager rowManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private int scoreLevel = 1;

    #endregion

    #region Private Fields

    private int lineClears = 0;
    private bool isBackToBack = false;

    #endregion

    #region Public Fields

    #endregion

    #region Monobehaviour Callbacks

    private void Start()
    {
        isClearing = false;
    }

    private void Update()
    {
        if (isClearing)
        {
            clearLines();
            Debug.Log("Clearing " + lineClears + " lines.");

            isClearing = false;
        }
        refreshText();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void clearLines()
    {
        int multiplier = 1;
        lineClears = rowManager.clearLines();
        switch (lineClears)
        {
            case 0:
                break;
            case 1:
                if (hasSpun && isBackToBack)
                {
                    multiplier = 1200;
                    hasSpun = false;
                    isBackToBack = true;
                }
                else if (hasSpun)
                {
                    multiplier = 800;
                    isBackToBack = true;
                } else
                {
                    multiplier = 100;
                    isBackToBack = false;
                }
                break;
            case 2:
                if (hasSpun && isBackToBack)
                {
                    multiplier = 1800;
                    hasSpun = false;
                    isBackToBack = true;
                }
                else if (hasSpun)
                {
                    multiplier = 1200;
                    isBackToBack = true;
                } else
                {
                    multiplier = 300;
                    isBackToBack = false;
                }
                break;
            case 3:
                if (hasSpun && isBackToBack)
                {
                    multiplier = 2400;
                    hasSpun = false;
                    isBackToBack = true;
                }
                else if (hasSpun)
                {
                    multiplier = 1600;
                    isBackToBack = true;
                } else
                {
                    multiplier = 500;
                    isBackToBack = false;
                }
                break;
            case 4:
                if (isBackToBack)
                {
                    multiplier = 1200;
                    hasSpun = false;
                } else
                {
                    multiplier = 800;
                }
                isBackToBack = true;
                break;
        }
        score += (scoreLevel * multiplier);
    }

    private void refreshText()
    {
        scoreText.text = "Score " + score;
        comboText.text = "B2B: " + isBackToBack + " Spin: " + hasSpun + " Kick: " + hasKicked;
    }

    #endregion
}
