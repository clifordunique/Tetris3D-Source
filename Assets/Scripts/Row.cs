using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    #region Private Serialized Field

    [SerializeField] private bool isEndGameRow = false;

    #endregion

    #region Private Fields

    private Transform[] sensor;
    private bool canClear = false;
    private int layerMask;
    private int total;

    #endregion

    #region Public Fields



    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 9;
        sensor = GetComponentsInChildren<Transform>();
        // Exclude the first child because it's actually the parent object because Unity is stupid and grabs the parent component when I get the child components
        // And I'm not saying this because it's a bug, Unity states that this is on purpose. @Unity What are you homies doing?
        sensor = new Transform[] { sensor[1], sensor[2], sensor[3], sensor[4], sensor[5], sensor[6], sensor[7], sensor[8], sensor[9], sensor[10] };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isEndGameRow)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Physics.Raycast(sensor[i].position, Vector3.back, 1, layerMask))
                {
                    Time.timeScale = 0;
                    GameOver.isGameOver = true;
                }
            }
        }
        scan();
        if (canClear)
        {
            clear();
            canClear = false;
        }
    }

    #endregion

    #region Private Methods

    private void scan()
    {
        total = 0;
        for (int i = 0; i < 10; i++)
        {
            if (Physics.Raycast(sensor[i].position, Vector3.back, 1, layerMask))
            {
                total++;
            }
        }
        if (total >= 10)
        {
            canClear = true;
        } else if (total == 0)
        {
            cascade();
        }
    }

    private void clear()
    {
        RaycastHit hit;
        foreach (Transform transform in sensor)
        {
            if (Physics.Raycast(transform.position, Vector3.back, out hit))
            {
                try
                {
                    GameObject temp = hit.transform.parent.gameObject;
                    hit.transform.parent.DetachChildren();
                    Destroy(temp);
                }
                catch
                {
                    
                }
                try
                {
                    Destroy(hit.transform.gameObject);
                }
                catch
                {
                    Debug.Log("Couldn't destroy a gameobject in the row's clear() method");
                }
            }
        }
    }

    public void cascade()
    {
        foreach (Transform transform in sensor)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.back, out hit))
            {
                hit.transform.Translate(Vector3.down, Space.World);
            }
        }
    }

    #endregion
}
