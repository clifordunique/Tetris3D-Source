using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on a TPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class TPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    [SerializeField] private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject tPieceTransformHandle;
    [SerializeField] private GameObject tPieceTop;
    [SerializeField] private GameObject tPieceBottom;
    [SerializeField] private GameObject tPieceLeft;
    [SerializeField] private GameObject tPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] tPieceTopBools;
    private bool[] tPieceBottomBools;
    private bool[] tPieceLeftBools;
    private bool[] tPieceRightBools;
    private int orientation = 0;

    #endregion

    #region Public Fields



    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        layerMask = 1 << 9;
        tPieceTopBools = new bool[4];
        tPieceBottomBools = new bool[4];
        tPieceLeftBools = new bool[4];
        tPieceRightBools = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (tPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (tPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (tPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (tPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
            orientation = 4;

        // Collision Checks
        for (int i = 0; i < 4; i++)
        {
            bool[] temporary = new bool[4];
            GameObject currentCube = null;
            switch (i)
            {
                case 0:
                    currentCube = tPieceTop;
                    break;
                case 1:
                    currentCube = tPieceBottom;
                    break;
                case 2:
                    currentCube = tPieceLeft;
                    break;
                case 3:
                    currentCube = tPieceRight;
                    break;
                default:
                    Debug.Log("Error with switch #1 of a CollisionScript!");
                    break;
            }

            // Note!
            // All collisions won't take into account the cubes of the current piece. IE A T-Piece Top won't care about a T-Piece Bottom being below it.
            // Ground checking is taken care of in the Basic RS Raycast Methodology
            // The SRS Raycast Methodology only takes into account special cases for their respective orientations

            #region Basic RS Raycast Methodology

            if (Physics.Raycast(currentCube.transform.position, up, 1, layerMask))
            {
                temporary[0] = true;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(up) * 1, Color.blue);
            }
            else
            {
                temporary[0] = false;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(up) * 1, Color.white);
            }
            if (Physics.Raycast(currentCube.transform.position, down, 1, layerMask))
            {
                temporary[1] = true;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(down) * 1, Color.blue);
                // Here's where ground checking is easily done!
                pieceMovementScript.setIsGrounded(true);
            }
            else
            {
                temporary[1] = false;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(down) * 1, Color.white);
            }
            if (Physics.Raycast(currentCube.transform.position, left, 1, layerMask))
            {
                temporary[2] = true;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(left) * 1, Color.blue);
            }
            else
            {
                temporary[2] = false;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(left) * 1, Color.white);
            }
            if (Physics.Raycast(currentCube.transform.position, right, 1, layerMask))
            {
                temporary[3] = true;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(right) * 1, Color.blue);
            }
            else
            {
                temporary[3] = false;
                Debug.DrawRay(currentCube.transform.position, transform.TransformDirection(right) * 1, Color.white);
            }
            switch(i)
            {
                case 0:
                    tPieceTopBools = temporary;
                    break;
                case 1:
                    tPieceBottomBools = temporary;
                    break;
                case 2:
                    tPieceLeftBools = temporary;
                    break;
                case 3:
                    tPieceRightBools = temporary;
                    break;
                default:
                    Debug.Log("Error with switch #2 of a CollisionScript!");
                    break;
            }

            #endregion

        }

        #region SRS Raycast Methodology

        //// Orientation 1
        bool leftUp2 = false;
        Debug.DrawRay(new Vector3(tPieceLeft.transform.position.x, tPieceLeft.transform.position.y + 1, tPieceLeft.transform.position.z), up * 1, Color.green);
        if (Physics.Raycast(new Vector3 (tPieceLeft.transform.position.x, tPieceLeft.transform.position.y + 1, tPieceLeft.transform.position.z), up, 1, layerMask))
        {
            leftUp2 = true;
        }
        bool leftDown3 = false;
        Debug.DrawRay(new Vector3(tPieceLeft.transform.position.x, tPieceLeft.transform.position.y - 1, tPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(tPieceLeft.transform.position.x, tPieceLeft.transform.position.y - 1, tPieceLeft.transform.position.z), down, 3, layerMask))
        {
            leftDown3 =  true;
        }
        bool leftDown2Right1 = false;
        Debug.DrawRay(new Vector3(tPieceLeft.transform.position.x, tPieceLeft.transform.position.y - 2, tPieceLeft.transform.position.z), right * 1, Color.green);
        if (Physics.Raycast(new Vector3(tPieceLeft.transform.position.x, tPieceLeft.transform.position.y - 2, tPieceLeft.transform.position.z), right, 1, layerMask))
        {
            leftDown2Right1 = true;
        }
        // Reverse
        bool rightUp2 = false;
        Debug.DrawRay(new Vector3(tPieceRight.transform.position.x, tPieceRight.transform.position.y + 1, tPieceRight.transform.position.z), up * 1, Color.green);
        if (Physics.Raycast(new Vector3(tPieceRight.transform.position.x, tPieceRight.transform.position.y + 1, tPieceRight.transform.position.z), up, 1, layerMask))
        {
            rightUp2 = true;
        }
        bool rightDown3 = false;
        Debug.DrawRay(new Vector3(tPieceRight.transform.position.x, tPieceRight.transform.position.y - 1, tPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(tPieceRight.transform.position.x, tPieceRight.transform.position.y - 1, tPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown3 = true;
        }
        bool rightDown2Left1 = false;
        Debug.DrawRay(new Vector3(tPieceRight.transform.position.x, tPieceRight.transform.position.y - 2, tPieceRight.transform.position.z), left * 1, Color.green);
        if (Physics.Raycast(new Vector3(tPieceRight.transform.position.x, tPieceRight.transform.position.y - 2, tPieceRight.transform.position.z), left, 1, layerMask))
        {
            rightDown2Left1 = true;
        }

        //// Orientation 2
        bool rightRight2 = false;
        Debug.DrawRay(new Vector3(tPieceRight.transform.position.x + 1, tPieceRight.transform.position.y, tPieceRight.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(tPieceRight.transform.position.x + 1, tPieceRight.transform.position.y, tPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightRight2 = true;
        }
        bool topDown2 = false;
        Debug.DrawRay(new Vector3(tPieceTop.transform.position.x, tPieceTop.transform.position.y - 1, tPieceTop.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(tPieceTop.transform.position.x, tPieceTop.transform.position.y - 1, tPieceTop.transform.position.z), down, 2, layerMask))
        {
            topDown2 = true;
        }
        //// Orientation 3

        //// Orientation 4

        #endregion

        // This switch statement contains ALL of the checks necessary and sets values accordingly.
        switch (orientation)
        {
            case 1:
                resetMovementBools();
                //// Clockwise Rotation Checks
                // Kick test 0R 1
                if (!tPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                // Kick test 0R 2
                if (tPieceBottomBools[1] && !tPieceTopBools[2] && !tPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test 0R 3
                if (!leftUp2 && tPieceLeftBools[1] && tPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 1));
                }
                // Kick test 0R 4 (The triple twist)
                if (!leftDown3 && !leftDown2Right1 && tPieceBottomBools[1] && tPieceLeftBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -2));
                }
                //// CounterClockwise Rotation Checks
                // Skipping Kick test 0R 1 because it's universal
                // Kick test 0R 2
                if (tPieceBottomBools[1] && !tPieceTopBools[3] && !tPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test 0R 3
                if (!rightUp2 && tPieceRightBools[1] && tPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 1));
                }
                // Kick test 0R 4 (The triple twist)
                if (!rightDown3 && !rightDown2Left1 && tPieceBottomBools[1] && tPieceRightBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -2));
                }
                //// Movement test
                if (tPieceLeftBools[2] || tPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (tPieceRightBools[3] || tPieceTopBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            case 2:
                resetMovementBools();
                if (tPieceLeftBools[2] || tPieceBottomBools[2] || tPieceRightBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                break;
            case 3:
                resetMovementBools();
                if (tPieceRightBools[2] || tPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                break;
            case 4:
                resetMovementBools();
                if (tPieceRightBools[2] || tPieceTopBools[2] || tPieceLeftBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
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
        pieceMovementScript.setCanRotateClockwise(true);
        pieceMovementScript.setCanRotateCounterClockwise(true);
        pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
        pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
    }

    #endregion
}
