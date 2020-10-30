using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on an iPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class IPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject iPieceTransformHandle;
    [SerializeField] private GameObject iPieceTop;
    [SerializeField] private GameObject iPieceBottom;
    [SerializeField] private GameObject iPieceLeft;
    [SerializeField] private GameObject iPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] iPieceTopBools;
    private bool[] iPieceBottomBools;
    private bool[] iPieceLeftBools;
    private bool[] iPieceRightBools;
    private int orientation = 0;

    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        layerMask = 1 << 9;
        iPieceTopBools = new bool[4];
        iPieceBottomBools = new bool[4];
        iPieceLeftBools = new bool[4];
        iPieceRightBools = new bool[4];
        pieceMovementScript = this.GetComponent<PieceMovement>();
    }

    void Update()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (iPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (iPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (iPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (iPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
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
                    currentCube = iPieceTop;
                    break;
                case 1:
                    currentCube = iPieceBottom;
                    break;
                case 2:
                    currentCube = iPieceLeft;
                    break;
                case 3:
                    currentCube = iPieceRight;
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
                    iPieceTopBools = temporary;
                    break;
                case 1:
                    iPieceBottomBools = temporary;
                    break;
                case 2:
                    iPieceLeftBools = temporary;
                    break;
                case 3:
                    iPieceRightBools = temporary;
                    break;
                default:
                    Debug.Log("Error with switch #2 of a CollisionScript!");
                    break;
            }

            #endregion

        }

        // Warning: This is SRS Raycast Methodoloy is extremely confusing if you're not Lucas
        #region SRS Raycast Methodology

        // The IPiece uses a private cast() method to raycast instead of a ton of raycasts
        // with tons and tons of booleans. This is because the IPiece is a funny little
        // thing with no offsets in its SRS Kick Tests. Very cool!

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
                // Kick test 0R 5
                if (!cast(iPieceRight, up, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 2));
                }
                // Kick test 0R 4
                if (!cast(iPieceLeft, down, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-2, -1));
                }
                // Kick test 0R 3
                if (!cast(iPieceRight, down, 2) && !cast(iPieceRight, up, 1))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test 0R 2
                if (!cast(iPieceLeft, down, 2) && !cast(iPieceLeft, up, 1))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-2, 0));
                }
                // Kick test 0R 1
                if (!cast(iPieceTop, down, 2) && !cast(iPieceTop, up, 1))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test L0 5 MIRRORED
                if (!cast(iPieceRight, down, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(2, -1));
                }
                // Kick test L0 4 MIRRORED
                if (!cast(iPieceLeft, up, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 2));
                }
                // Kick test L0 3 MIRRORED
                if (!cast(iPieceRight, down, 2) && !cast(iPieceRight, up, 1))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(2, 0));
                }
                // Kick test L0 2 MIRRORED
                if (!cast(iPieceLeft, down, 2) && !cast(iPieceLeft, up, 1))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test L0 1 MIRRORED
                if (!cast(iPieceBottom, down, 2) && !cast(iPieceBottom, up, 1))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (iPieceLeftBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (iPieceRightBools[3])
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
                if (!cast(iPieceRight, right, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(2, -1));
                }
                // Kick test R2 4
                if (!cast(iPieceLeft, left, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 2));
                }
                // Kick test R2 3
                if (!cast(iPieceTop, right, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(2, 0));
                }
                // Kick test R2 2
                if (!cast(iPieceTop, left, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test R2 1
                if (!cast(iPieceTop, left, 2) && !cast(iPieceTop, right, 1))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 0R 5 MIRRORED
                if (!cast(iPieceRight, left, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -2));
                }
                // Kick test 0R 4 MIRRORED
                if (!cast(iPieceLeft, right, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(2, 1));
                }
                // Kick test 0R 3 MIRRORED
                if (!cast(iPieceBottom, left, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test 0R 2 MIRRORED
                if (!cast(iPieceBottom, right, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(2, 0));
                }
                // Kick test 0R 1 MIRRORED
                if (!cast(iPieceBottom, left, 2) && !cast(iPieceBottom, right, 1))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (iPieceBottomBools[2] || iPieceLeftBools[2] || iPieceRightBools[2] || iPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (iPieceBottomBools[3] || iPieceLeftBools[3] || iPieceRightBools[3] || iPieceTopBools[3])
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
                if (!cast(iPieceRight, down, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -2));
                }
                // Kick test 2L 4
                if (!cast(iPieceLeft, up, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(2, 1));
                }
                // Kick test 2L 3
                if (!cast(iPieceRight, down, 1) && !cast(iPieceRight, up, 2))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test 2L 2
                if (!cast(iPieceLeft, down, 1) && !cast(iPieceLeft, up, 2))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(2, 0));
                }
                // Kick test 2L 1
                if (!cast(iPieceTop, right, 2) && !cast(iPieceTop, down, 1))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test R2 5 MIRRORED
                if (!cast(iPieceRight, up, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-2, 1));
                }
                // Kick test R2 4 MIRRORED
                if (!cast(iPieceLeft, down, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -2));
                }
                // Kick test R2 3 MIRRORED
                if (!cast(iPieceRight, down, 1) && !cast(iPieceRight, up, 2))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-2, 0));
                }
                // Kick test R2 2 MIRRORED
                if (!cast(iPieceLeft, down, 1) && !cast(iPieceLeft, up, 2))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test R2 1 MIRRORED
                if (!cast(iPieceBottom, up, 2) && !cast(iPieceBottom, down, 1))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement tests
                if (iPieceRightBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (iPieceLeftBools[3])
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
                if (!cast(iPieceRight, left, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-2, 1));
                }
                // Kick test L0 4
                if (!cast(iPieceLeft, right, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -2));
                }
                // Kick test L0 3
                if (!cast(iPieceTop, left, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-2, 0));
                }
                // Kick test L0 2
                if (!cast(iPieceTop, right, 3))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test L0 1
                if (!cast(iPieceTop, right, 2) && !cast(iPieceTop, left, 1))
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 2L 5 MIRRORED
                if (!cast(iPieceRight, right, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 2));
                }
                // Kick test 2L 4 MIRRORED
                if (!cast(iPieceLeft, left, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-2, -1));
                }
                // Kick test 2L 3 MIRRORED
                if (!cast(iPieceBottom, right, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test 2L 2 MIRRORED
                if (!cast(iPieceBottom, left, 3))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-2, 0));
                }
                // Kick test 2L 1 MIRRORED
                if (!cast(iPieceBottom, right, 2) && !cast(iPieceBottom, left, 1))
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement Tests
                if (iPieceBottomBools[2] || iPieceLeftBools[2] || iPieceRightBools[2] || iPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (iPieceBottomBools[3] || iPieceLeftBools[3] || iPieceRightBools[3] || iPieceTopBools[3])
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
        PieceMovement.isSpinning = false;
        PieceMovement.isKicking = false;
        pieceMovementScript.setCanMoveLeft(true);
        pieceMovementScript.setCanMoveRight(true);
        pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
        pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
    }

    private bool cast(GameObject iPieceCube, Vector3 direction, float distance)
    {
        if (Physics.Raycast(iPieceCube.transform.position, direction, distance, layerMask))
        {
            Debug.DrawRay(iPieceCube.transform.position, direction * distance, Color.blue);
            return true;
        }
        else
        {
            Debug.DrawRay(iPieceCube.transform.position, direction * distance, Color.white);
            return false;
        }
    }

    #endregion
}