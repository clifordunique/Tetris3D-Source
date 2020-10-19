using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on an SPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class SPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    [SerializeField] private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject sPieceTransformHandle;
    [SerializeField] private GameObject sPieceTop;
    [SerializeField] private GameObject sPieceBottom;
    [SerializeField] private GameObject sPieceLeft;
    [SerializeField] private GameObject sPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] sPieceTopBools;
    private bool[] sPieceBottomBools;
    private bool[] sPieceLeftBools;
    private bool[] sPieceRightBools;
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
        sPieceTopBools = new bool[4];
        sPieceBottomBools = new bool[4];
        sPieceLeftBools = new bool[4];
        sPieceRightBools = new bool[4];
    }

    void FixedUpdate()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (sPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (sPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (sPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (sPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
            orientation = 4;

        // Collision Checks
        for (int i = 0; i < 4; i++)
        {
            bool[] temporary = new bool[4];
            GameObject currentCube = null;
            switch (i)
            {
                case 0:
                    currentCube = sPieceTop;
                    break;
                case 1:
                    currentCube = sPieceBottom;
                    break;
                case 2:
                    currentCube = sPieceLeft;
                    break;
                case 3:
                    currentCube = sPieceRight;
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
                    sPieceTopBools = temporary;
                    break;
                case 1:
                    sPieceBottomBools = temporary;
                    break;
                case 2:
                    sPieceLeftBools = temporary;
                    break;
                case 3:
                    sPieceRightBools = temporary;
                    break;
                default:
                    Debug.Log("Error with switch #2 of a CollisionScript!");
                    break;
            }

            #endregion

        }

        // Warning: This is SRS Raycast Methodoloy is extremely confusing if you're not Lucas
        #region SRS Raycast Methodology

        //// Orientation 1
        bool leftUp2 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp2 = true;
        }
        bool leftDown3 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), down, 3, layerMask))
        {
            leftDown3 = true;
        }
        bool leftDown2Right1 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y - 2, sPieceLeft.transform.position.z), right * 1, Color.green);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y - 2, sPieceLeft.transform.position.z), right, 1, layerMask))
        {
            leftDown2Right1 = true;
        }
        bool leftDown2 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2 = true;
        }
        bool bottomDown1Down2 = false;
        Debug.DrawRay(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y - 1, sPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y - 1, sPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown1Down2 = true;
        }
        bool bottomDown2 = false;
        Debug.DrawRay(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y, sPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y, sPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown2 = true;
        }
        bool rightDown2Down2 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y - 2, sPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y - 2, sPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2Down2 = true;
        }
        bool rightDown2 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2 = true;
        }
        // Reverse
        bool rightUp2 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2 = true;
        }
        bool rightDown3 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), down, 3, layerMask))
        {
            rightDown3 = true;
        }
        bool rightDown2Left1 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y - 2, sPieceRight.transform.position.z), left * 1, Color.green);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y - 2, sPieceRight.transform.position.z), left, 1, layerMask))
        {
            rightDown2Left1 = true;
        }
        //// Orientation 2
        bool rightRight2 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightRight2 = true;
        }
        bool topDown2 = false;
        Debug.DrawRay(new Vector3(sPieceTop.transform.position.x, sPieceTop.transform.position.y, sPieceTop.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceTop.transform.position.x, sPieceTop.transform.position.y, sPieceTop.transform.position.z), down, 2, layerMask))
        {
            topDown2 = true;
        }
        bool top3 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x - 2, sPieceLeft.transform.position.y + 1, sPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x - 2, sPieceLeft.transform.position.y + 1, sPieceLeft.transform.position.z), right, 3, layerMask))
        {
            top3 = true;
        }
        bool topRight13 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x - 1, sPieceLeft.transform.position.y + 1, sPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x - 1, sPieceLeft.transform.position.y + 1, sPieceLeft.transform.position.z), right, 3, layerMask))
        {
            topRight13 = true;
        }
        // Reverse
        bool leftRight2 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftRight2 = true;
        }
        bool topUp2 = false;
        Debug.DrawRay(new Vector3(sPieceTop.transform.position.x, sPieceTop.transform.position.y, sPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceTop.transform.position.x, sPieceTop.transform.position.y, sPieceTop.transform.position.z), up, 2, layerMask))
        {
            topUp2 = true;
        }
        bool topUp2Up1 = false;
        Debug.DrawRay(new Vector3(sPieceTop.transform.position.x, sPieceTop.transform.position.y + 2, sPieceTop.transform.position.z), up * 1, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceTop.transform.position.x, sPieceTop.transform.position.y + 2, sPieceTop.transform.position.z), up, 1, layerMask))
        {
            topUp2Up1 = true;
        }
        //// Orientation 3
        // LeftUp2 is covered earlier
        // TopDown2 is covered earlier
        bool bottomDown2Right1 = false;
        Debug.DrawRay(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y - 2, sPieceBottom.transform.position.z), right * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y - 2, sPieceBottom.transform.position.z), right, 1, layerMask))
        {
            bottomDown2Right1 = true;
        }
        // Reverses are covered
        bool bottomDown2Left1 = false;
        Debug.DrawRay(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y - 2, sPieceBottom.transform.position.z), left * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(sPieceBottom.transform.position.x, sPieceBottom.transform.position.y - 2, sPieceBottom.transform.position.z), left, 1, layerMask))
        {
            bottomDown2Left1 = true;
        }
        //// Orientation 4
        bool leftLeft2 = false;
        Debug.DrawRay(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(sPieceLeft.transform.position.x, sPieceLeft.transform.position.y, sPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftLeft2 = true;
        }
        bool rightTop3 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x + 2, sPieceRight.transform.position.y + 1, sPieceRight.transform.position.z), left * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x + 2, sPieceRight.transform.position.y + 1, sPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTop3 = true;
        }
        bool rightTopLeft13 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x + 1, sPieceRight.transform.position.y + 1, sPieceRight.transform.position.z), left * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x + 1, sPieceRight.transform.position.y + 1, sPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTopLeft13 = true;
        }
        // topUp2Up1 is covered
        // topUp2 is also covered for mirror R2 5
        // topDown2 is also covered for mirror R2 3
        // Reverse
        bool rightLeft2 = false;
        Debug.DrawRay(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(sPieceRight.transform.position.x, sPieceRight.transform.position.y, sPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightLeft2 = true;
        }

        #endregion

        // This switch statement contains ALL of the checks necessary and sets values accordingly.
        // Most movement checks are if statements going from most complex to simple. Simplest maneuvers
        // MUST have highest priority, so they are checked last.
        switch (orientation)
        {
            case 1:
                resetMovementBools();
                pieceMovementScript.setCanRotateClockwise(false);
                pieceMovementScript.setCanRotateCounterClockwise(false);
                //// Clockwise Rotation tests
                // Kick test 0R 5 (The triple twist)
                if (!leftDown2 && !bottomDown1Down2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -2));
                }
                // Kick test 0R 4
                if (!bottomDown2 && !rightDown2Down2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 0R 3
                if (!leftUp2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 1));
                }
                // Kick test 0R 2
                if (!sPieceTopBools[2] && !sPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 1));
                }
                // Kick test 0R 1
                if (!rightDown2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }    
                //// CounterClockwise Rotation tests
                // Kick test 0R 3
                if (!sPieceTopBools[0] && !sPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 1));
                }
                // Kick test 0R 2
                if (!rightDown2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test 0R 1
                if (!sPieceBottomBools[1] && !sPieceTopBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (sPieceLeftBools[2] || sPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (sPieceRightBools[3] || sPieceBottomBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            case 2:
                resetMovementBools();
                pieceMovementScript.setCanRotateClockwise(false);
                pieceMovementScript.setCanRotateCounterClockwise(false);
                //// Clockwise Rotation tests
                // Kick test R2 5

                // Kick test R2 4

                // Kick test R2 3

                // Kick test R2 2

                //// CounterClockwise Rotation tests
                // Kick test 0R 2

                // Kick test L0 5 MIRRORED

                // Kick test L0 3 MIRRORED

                // Kick test L0 2 MIRRORED

                // Kick test R2 1

                //// Movement test
                if (sPieceLeftBools[2] || sPieceBottomBools[2] || sPieceRightBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (sPieceLeftBools[3] || sPieceTopBools[3] || sPieceRightBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            case 3:
                resetMovementBools();
                pieceMovementScript.setCanRotateClockwise(false);
                pieceMovementScript.setCanRotateCounterClockwise(false);
                //// Clockwise Rotation tests
                // Kick test 2L 5

                // Kick test 2L 4

                // Kick test 2L 2

                // Skipping Kick test 2L 1 because it's universal
                //// CounterClockwise Rotation tests
                // Kick test 2L 5

                // Kick test 2L 4

                // Kick test 2L 2

                // Kick test 2L 1

                //// Movement tests
                if (sPieceRightBools[2] || sPieceBottomBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (sPieceLeftBools[3] || sPieceTopBools[3])
                {
                    pieceMovementScript.setCanMoveRight(false);
                }
                break;
            case 4:
                resetMovementBools();
                pieceMovementScript.setCanRotateClockwise(false);
                pieceMovementScript.setCanRotateCounterClockwise(false);
                //// Clockwise Rotation tests
                // Kick test L0 5

                // Kick test L0 3

                // Kick test L0 2

                //// CounterClockwise Rotation tests
                // Kick test R2 5 MIRRORED

                // Kick test R2 4 MIRRORED

                // Kick test R2 3 MIRRORED

                // Kick test L0 1

                //// Movement Tests
                if (sPieceRightBools[2] || sPieceTopBools[2] || sPieceLeftBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (sPieceRightBools[3] || sPieceBottomBools[3] || sPieceLeftBools[3])
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