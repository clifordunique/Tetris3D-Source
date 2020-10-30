using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// Was gonna use this class to clean up line clearing behaviors (cuz they're shit right now)

public class RowManager : MonoBehaviour
{
    #region Static Fields

    #endregion

    #region Private Fields

    private List<Row> rowScripts;
    private bool[] clearableLines;
    private int linesToClear = 0;

    #endregion

    #region Public Fields



    #endregion

    #region Monobehaviour Callbacks

    private void Start()
    {
        rowScripts = new List<Row>();
        // Get the row children
        foreach (Transform child in transform)
        {
            if (child.tag == "Row")
            {
                try
                {
                    rowScripts.Add(child.gameObject.GetComponent<Row>());
                }
                catch
                {
                    Debug.Log("Couldn't find a transform child in RowManager.");
                }
            }
        }
    }

    #endregion

    #region Public Methods

    public int clearLines()
    {
        findClearableLines();
        int returnValue = linesToClear;
        for (int i = 0; i < linesToClear; i++)
        {
            bool clearingALine = false;
            foreach (Row row in rowScripts)
            {
                if (row.getCanClear() && !clearingALine)
                {
                    clearingALine = true;
                    row.clear();
                    row.cascade();
                }
                else if (clearingALine)
                {
                    row.cascade();
                }
            }
        }
        linesToClear = 0;
        return returnValue;
    }

    #endregion

    #region Private Methods

    private void findClearableLines()
    {
        foreach (Row row in rowScripts)
        {
            if(row.getCanClear())
            {
                linesToClear++;
            }
        }
    }

    #endregion
}
