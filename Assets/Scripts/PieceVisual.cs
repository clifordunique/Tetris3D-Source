using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceVisual : MonoBehaviour
{
    #region Private Serialized Field

    [SerializeField] private GameObject tPieceVisual; // 0
    [SerializeField] private GameObject sPieceVisual; // 1
    [SerializeField] private GameObject zPieceVisual; // 2
    [SerializeField] private GameObject jPieceVisual; // 3
    [SerializeField] private GameObject lPieceVisual; // 4
    [SerializeField] private GameObject iPieceVisual; // 5
    [SerializeField] private GameObject oPieceVisual; // 6

    #endregion

    #region Private Fields

    #endregion

    #region Public Fields

    public int pieceToShow = 0;

    #endregion

    #region Monobehaviour Callbacks

    // Update is called once per frame
    void LateUpdate()
    {
        hidePieces();
        switch (pieceToShow)
        {
            case 0:
                tPieceVisual.SetActive(true);
                break;
            case 1:
                sPieceVisual.SetActive(true);
                break;
            case 2:
                zPieceVisual.SetActive(true);
                break;
            case 3:
                jPieceVisual.SetActive(true);
                break;
            case 4:
                lPieceVisual.SetActive(true);
                break;
            case 5:
                iPieceVisual.SetActive(true);
                break;
            case 6:
                oPieceVisual.SetActive(true);
                break;
        }
    }

    #endregion

    #region Private Methods

    private void hidePieces()
    {
        tPieceVisual.SetActive(false);
        sPieceVisual.SetActive(false);
        zPieceVisual.SetActive(false);
        jPieceVisual.SetActive(false);
        lPieceVisual.SetActive(false);
        iPieceVisual.SetActive(false);
        oPieceVisual.SetActive(false);
    }

    #endregion
}
