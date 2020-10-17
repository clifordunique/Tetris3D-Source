using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on a tetromino/piece
 * and reports if they are colliding with layer 9 (Collider Layer) below.
*/

public class PieceGroundCheck : MonoBehaviour
{

    /*
    #region Private Serialized Field

    [SerializeField] private PieceMovement pieceMovementScript;

    // WE'LL WORRY ABOUT SETTING THE SIZE TO 4 AND GETTING THE PIECES LATER LMAO
    [SerializeField] private GameObject[] cubePart;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] isGrounded;

    #endregion

    #region Public Fields



    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        layerMask = 1 << 9;
        isGrounded = new bool[4];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Ground Collision Check
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawRay(cubePart[i].transform.position, transform.TransformDirection(down) * 1, Color.white);
            if (Physics.Raycast(cubePart[i].transform.position, down, 1, layerMask))
                isGrounded[i] = true;
            else
                isGrounded[i] = false;
        }
        if (isGrounded[0] || isGrounded[1] || isGrounded[2] || isGrounded[3])
        {
            pieceMovementScript.setIsGrounded(true);
        }
        else
        {
            pieceMovementScript.setIsGrounded(false);
        }

    }

    #endregion

    #region Private Methods



    #endregion
    */
}
