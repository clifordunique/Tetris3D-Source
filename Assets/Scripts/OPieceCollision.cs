using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on an oPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class OPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject oPieceTransformHandle;
    [SerializeField] private GameObject oPieceTop;
    [SerializeField] private GameObject oPieceBottom;
    [SerializeField] private GameObject oPieceLeft;
    [SerializeField] private GameObject oPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] oPieceTopBools;
    private bool[] oPieceBottomBools;
    private bool[] oPieceLeftBools;
    private bool[] oPieceRightBools;
    private int orientation = 0;

    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        layerMask = 1 << 9;
        oPieceTopBools = new bool[4];
        oPieceBottomBools = new bool[4];
        oPieceLeftBools = new bool[4];
        oPieceRightBools = new bool[4];
        pieceMovementScript = this.GetComponent<PieceMovement>();
    }

    void Update()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (oPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (oPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (oPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (oPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
            orientation = 4;

        PieceMovement.distanceDown = 90;

        // Collision Checks
        for (int i = 0; i < 4; i++)
        {
            bool[] temporary = new bool[4];
            GameObject currentCube = null;
            switch (i)
            {
                case 0:
                    currentCube = oPieceTop;
                    break;
                case 1:
                    currentCube = oPieceBottom;
                    break;
                case 2:
                    currentCube = oPieceLeft;
                    break;
                case 3:
                    currentCube = oPieceRight;
                    break;
                default:
                    Debug.Log("Error with switch #1 of a CollisionScript!");
                    break;
            }

            #region Shortest Distance Down

            RaycastHit hit;
            float distance = 90;
            if (Physics.Raycast(currentCube.transform.position, down, out hit, layerMask))
            {
                distance = hit.distance;
            }
            PieceMovement.distanceDown = Mathf.Min(PieceMovement.distanceDown, distance);

            #endregion

            // Note!
            // All collisions won't take into account the cubes of the current piece. IE A T-Piece Top won't care about a T-Piece Bottom being below it.
            // Ground checking is taken care of in the Basic RS Raycast Methodology
            // The SRS Raycast Methodology only takes into account special cases for their respective orientations

            #region Basic RS Raycast Methodology

            if (Physics.Raycast(currentCube.transform.position, up, 1, layerMask))
            {
                temporary[0] = true;
                Debug.DrawRay(currentCube.transform.position, up * 1, Color.blue);
            }
            else
            {
                temporary[0] = false;
                Debug.DrawRay(currentCube.transform.position, up * 1, Color.white);
            }
            if (Physics.Raycast(currentCube.transform.position, down, 1, layerMask))
            {
                temporary[1] = true;
                Debug.DrawRay(currentCube.transform.position, down * 1, Color.blue);
                // Here's where ground checking is easily done!
                pieceMovementScript.setIsGrounded(true);
            }
            else
            {
                temporary[1] = false;
                Debug.DrawRay(currentCube.transform.position, down * 1, Color.white);
            }
            if (Physics.Raycast(currentCube.transform.position, left, 1, layerMask))
            {
                temporary[2] = true;
                Debug.DrawRay(currentCube.transform.position, left * 1, Color.blue);
            }
            else
            {
                temporary[2] = false;
                Debug.DrawRay(currentCube.transform.position, left * 1, Color.white);
            }
            if (Physics.Raycast(currentCube.transform.position, right, 1, layerMask))
            {
                temporary[3] = true;
                Debug.DrawRay(currentCube.transform.position, right * 1, Color.blue);
            }
            else
            {
                temporary[3] = false;
                Debug.DrawRay(currentCube.transform.position, right * 1, Color.white);
            }
            switch (i)
            {
                case 0:
                    oPieceTopBools = temporary;
                    break;
                case 1:
                    oPieceBottomBools = temporary;
                    break;
                case 2:
                    oPieceLeftBools = temporary;
                    break;
                case 3:
                    oPieceRightBools = temporary;
                    break;
                default:
                    Debug.Log("Error with switch #2 of a CollisionScript!");
                    break;
            }

            #endregion

        }

        // The OPiece sucks and is boring and trash and stupid and and AND AND!

        switch (orientation)
        {
            case 1:
                resetMovementBools();
                //// Movement test
                if (oPieceLeftBools[2] || oPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (oPieceRightBools[3] || oPieceBottomBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            case 2:
                resetMovementBools();
                //// Movement test
                if (oPieceBottomBools[2] || oPieceLeftBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (oPieceRightBools[3] || oPieceTopBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            case 3:
                resetMovementBools();
                //// Movement tests
                if (oPieceRightBools[2] || oPieceBottomBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (oPieceLeftBools[3] || oPieceTopBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }

                break;
            case 4:
                resetMovementBools();
                //// Movement Tests
                if (oPieceRightBools[2] || oPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (oPieceBottomBools[3] || oPieceLeftBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            default:
                resetMovementBools();
                break;
        }
    }

    #endregion

    #region Private Methods

    private void resetMovementBools()
    {
        pieceMovementScript.setCanMoveLeft(true);
        pieceMovementScript.setCanMoveRight(true);
        pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
        pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
    }

    #endregion
}