using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    #region Public Static Field

    #endregion

    #region Private Serialized Field

    #endregion

    #region Private Fields

    private SceneManager sceneManager;

    #endregion

    #region Public Fields

    #endregion

    #region Monobehaviour Callbacks

    void Start()
    {

    }

    #endregion

    #region Public Methods

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOver.isGameOver = false;
        Time.timeScale = 1;
    }

    #endregion

    #region Private Methods

    #endregion
}
