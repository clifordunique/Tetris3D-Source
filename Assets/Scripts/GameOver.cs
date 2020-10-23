using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    #region Public Static Field

    public static bool isGameOver = false;

    #endregion

    #region Private Serialized Field

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pieceManager;

    #endregion

    #region Private Fields

    #endregion

    #region Public Fields

    #endregion

    #region Monobehaviour Callbacks

    private void Update()
    {
        if (isGameOver)
        {
            gameOverUI.SetActive(true);
            pieceManager.SetActive(false);
        }
        else
        {
            pieceManager.SetActive(true);
            gameOverUI.SetActive(false);
        }
    }

    #endregion

    #region Private Methods

    #endregion
}
