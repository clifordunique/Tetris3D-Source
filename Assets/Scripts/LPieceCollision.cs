using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on an lPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class LPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject lPieceTransformHandle;
    [SerializeField] private GameObject lPieceTop;
    [SerializeField] private GameObject lPieceBottom;
    [SerializeField] private GameObject lPieceLeft;
    [SerializeField] private GameObject lPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] lPieceTopBools;
    private bool[] lPieceBottomBools;
    private bool[] lPieceLeftBools;
    private bool[] lPieceRightBools;
    private int orientation = 0;

    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        layerMask = 1 << 9;
        lPieceTopBools = new bool[4];
        lPieceBottomBools = new bool[4];
        lPieceLeftBools = new bool[4];
        lPieceRightBools = new bool[4];
        pieceMovementScript = this.GetComponent<PieceMovement>();
    }

    void Update()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (lPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (lPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (lPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (lPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
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
                    currentCube = lPieceTop;
                    break;
                case 1:
                    currentCube = lPieceBottom;
                    break;
                case 2:
                    currentCube = lPieceLeft;
                    break;
                case 3:
                    currentCube = lPieceRight;
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
                    lPieceTopBools = temporary;
                    break;
                case 1:
                    lPieceBottomBools = temporary;
                    break;
                case 2:
                    lPieceLeftBools = temporary;
                    break;
                case 3:
                    lPieceRightBools = temporary;
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
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp2 = true;
        }
        bool leftDown3 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), down, 3, layerMask))
        {
            leftDown3 = true;
        }
        bool leftDown2Right1 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y - 2, lPieceLeft.transform.position.z), right * 1, Color.green);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y - 2, lPieceLeft.transform.position.z), right, 1, layerMask))
        {
            leftDown2Right1 = true;
        }
        bool leftDown2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2 = true;
        }
        bool bottomDown1Down2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y - 1, lPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y - 1, lPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown1Down2 = true;
        }
        bool bottomDown2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown2 = true;
        }
        bool rightDown2Down2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y - 2, lPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y - 2, lPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2Down2 = true;
        }
        bool rightDown2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2 = true;
        }
        bool leftDown2Down2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y - 2, lPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y - 2, lPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2Down2 = true;
        }
        bool bottomDown3 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), down, 3, layerMask))
        {
            bottomDown3 = true;
        }
        bool topUp1Right2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y + 1, lPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y + 1, lPieceTop.transform.position.z), right, 2, layerMask))
        {
            topUp1Right2 = true;
        }
        // Reverse Orientation 1
        bool rightUp2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2 = true;
        }
        bool rightDown3 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), down, 3, layerMask))
        {
            rightDown3 = true;
        }
        bool rightDown2Left1 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y - 2, lPieceRight.transform.position.z), left * 1, Color.green);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y - 2, lPieceRight.transform.position.z), left, 1, layerMask))
        {
            rightDown2Left1 = true;
        }
        bool rightDown3Left2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y - 3, lPieceRight.transform.position.z), left * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y - 3, lPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightDown3Left2 = true;
        }
        bool bottomDown3Left2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x + 1, lPieceBottom.transform.position.y - 3, lPieceBottom.transform.position.z), left * 2, Color.green);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x + 1, lPieceBottom.transform.position.y - 3, lPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomDown3Left2 = true;
        }
        //// Orientation 2
        bool rightRight2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightRight2 = true;
        }
        bool topDown2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y, lPieceTop.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y, lPieceTop.transform.position.z), down, 2, layerMask))
        {
            topDown2 = true;
        }
        bool top3 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right, 3, layerMask))
        {
            top3 = true;
        }
        bool topRight13 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right, 3, layerMask))
        {
            topRight13 = true;
        }
        bool topUp2Right2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), right, 2, layerMask))
        {
            topUp2Right2 = true;
        }
        bool leftUp1Right2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftUp1Right2 = true;
        }
        bool rightDown1Left2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y - 1, lPieceRight.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y - 1, lPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightDown1Left2 = true;
        }
        bool topDown2Right2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y - 2, lPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y - 2, lPieceTop.transform.position.z), right, 2, layerMask))
        {
            topDown2Right2 = true;
        }
        bool rightRight2Down2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 2, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 2, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightRight2Down2 = true;
        }
        bool bottomRight2Down2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x + 2, lPieceBottom.transform.position.y + 1, lPieceBottom.transform.position.z), down * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x + 2, lPieceBottom.transform.position.y + 1, lPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomRight2Down2 = true;
        }
        bool leftUp1Right3 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right, 3, layerMask))
        {
            leftUp1Right3 = true;
        }
        bool leftLeft1Up1Right3 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right, 3, layerMask))
        {
            leftLeft1Up1Right3 = true;
        }
        // Reverse Orientation 2
        bool leftRight2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftRight2 = true;
        }
        bool topUp2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y, lPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            topUp2 = true;
        }
        bool topUp2Up1 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), up * 1, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), up, 1, layerMask))
        {
            topUp2Up1 = true;
        }
        bool leftUp1Left2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x + 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x + 1, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftUp1Left2 = true;
        }
        bool leftUp2Right2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 2, lPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y + 2, lPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftUp2Right2 = true;
        }
        bool leftUp2Left2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x + 1, lPieceLeft.transform.position.y + 2, lPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x + 1, lPieceLeft.transform.position.y + 2, lPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftUp1Left2 = true;
        }
        bool bottomUp3Left2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x + 1, lPieceBottom.transform.position.y + 3, lPieceBottom.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x + 1, lPieceBottom.transform.position.y + 3, lPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomUp3Left2 = true;
        }
        bool leftLeft1Up1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftLeft1Up1Up2 = true;
        }
        bool bottomRight2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomRight2 = true;
        }
        bool topRight1Up3Up2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            topRight1Up3Up2 = true;
        }
        bool leftLeft1Up1Right2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftLeft1Up1Right2 = true;
        }
        bool topUp3Up2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            topUp3Up2 = true;
        }
        bool topRight1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y - 1, lPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y - 1, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            topRight1Up2 = true;
        }
        bool bottomRight2Up2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x + 2, lPieceTop.transform.position.y - 1, lPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x + 2, lPieceTop.transform.position.y - 1, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            bottomRight2Up2 = true;
        }
        //// Orientation 3
        bool bottomDown2Right1 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y - 2, lPieceBottom.transform.position.z), right * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y - 2, lPieceBottom.transform.position.z), right, 1, layerMask))
        {
            bottomDown2Right1 = true;
        }
        bool leftDown1Down2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y - 1, lPieceLeft.transform.position.z), down * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y - 1, lPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown1Down2 = true;
        }
        bool bottomUp2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), up * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), up, 2, layerMask))
        {
            bottomUp2 = true;
        }
        bool rightDown1Down2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y - 1, lPieceRight.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y - 1, lPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown1Down2 = true;
        }
        bool leftUp1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y + 1, lPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp1Up2 = true;
        }
        bool leftDown3Left2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x + 1, lPieceLeft.transform.position.y - 3, lPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x + 1, lPieceLeft.transform.position.y - 3, lPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftDown3Left2 = true;
        }
        // Reverse Orientation 3
        bool bottomDown2Left1 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y - 2, lPieceBottom.transform.position.z), left * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y - 2, lPieceBottom.transform.position.z), left, 1, layerMask))
        {
            bottomDown2Left1 = true;
        }
        bool rightUp2Up2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2Up2 = true;
        }
        bool bottomDown3Right2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x - 1, lPieceBottom.transform.position.y - 3, lPieceBottom.transform.position.z), right * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x - 1, lPieceBottom.transform.position.y - 3, lPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomDown3Right2 = true;
        }
        //// Orientation 4
        bool leftLeft2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x, lPieceLeft.transform.position.y, lPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftLeft2 = true;
        }
        bool rightTop3 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 2, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 2, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTop3 = true;
        }
        bool rightTopLeft13 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTopLeft13 = true;
        }
        bool rightUp1Left2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightUp1Left2 = true;
        }
        bool rightUp2Right2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x - 1, lPieceRight.transform.position.y + 2, lPieceRight.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x - 1, lPieceRight.transform.position.y + 2, lPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightUp2Right2 = true;
        }
        bool bottomUp2Left2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x + 1, lPieceBottom.transform.position.y + 2, lPieceBottom.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x + 1, lPieceBottom.transform.position.y + 2, lPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomUp2Left2 = true;
        }
        bool bottomUp3Right2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x - 1, lPieceBottom.transform.position.y + 3, lPieceBottom.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x - 1, lPieceBottom.transform.position.y + 3, lPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomUp3Right2 = true;
        }
        bool rightUp1Left3 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightUp1Left3 = true;
        }
        bool rightLeft2Up1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x - 2, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x - 2, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightLeft2Up1Up2 = true;
        }
        bool rightLeft1Up1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x - 1, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x - 1, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightLeft1Up1Up2 = true;
        }
        bool topLeft1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y - 1, lPieceTop.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y - 1, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            topLeft1Up2 = true;
        }
        bool topLeft1Up1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y, lPieceTop.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y, lPieceTop.transform.position.z), up, 2, layerMask))
        {
            topLeft1Up1Up2 = true;
        }
        bool rightRight1Up1Up2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y, lPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightRight1Up1Up2 = true;
        }
        // Reverse Orientation 4
        bool rightLeft2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x, lPieceRight.transform.position.y, lPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightLeft2 = true;
        }
        bool rightUp1Right2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x - 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x - 1, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightUp1Right2 = true;
        }
        bool bottomUp2Right2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x - 1, lPieceBottom.transform.position.y + 2, lPieceBottom.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x - 1, lPieceBottom.transform.position.y + 2, lPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomUp2Right2 = true;
        }
        bool topDown2Left2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y - 2, lPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y - 2, lPieceTop.transform.position.z), left, 2, layerMask))
        {
            topDown2Left2 = true;
        }
        bool topUp3Left2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y + 3, lPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y + 3, lPieceTop.transform.position.z), left, 2, layerMask))
        {
            topUp3Left2 = true;
        }
        bool rightUp2Left2 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 2, lPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 1, lPieceRight.transform.position.y + 2, lPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightUp2Left2 = true;
        }
        bool topUp2Left2 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x + 1, lPieceTop.transform.position.y + 2, lPieceTop.transform.position.z), left, 2, layerMask))
        {
            topUp2Left2 = true;
        }
        bool leftDown1Right2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y - 1, lPieceLeft.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 1, lPieceLeft.transform.position.y - 1, lPieceLeft.transform.position.z), right, 2, layerMask))
        {
            topUp2Left2 = true;
        }
        bool rightUp1Right1Left3 = false;
        Debug.DrawRay(new Vector3(lPieceRight.transform.position.x + 2, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(lPieceRight.transform.position.x + 2, lPieceRight.transform.position.y + 1, lPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightUp1Right1Left3 = true;
        }
        bool bottomLeft2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x, lPieceBottom.transform.position.y, lPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomLeft2 = true;
        }
        bool topUp1Right3 = false;
        Debug.DrawRay(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y + 1, lPieceTop.transform.position.z), right * 3, Color.red);
        if (Physics.Raycast(new Vector3(lPieceTop.transform.position.x - 1, lPieceTop.transform.position.y + 1, lPieceTop.transform.position.z), right, 3, layerMask))
        {
            topUp1Right3 = true;
        }
        bool leftLeft2Down2 = false;
        Debug.DrawRay(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y - 1, lPieceLeft.transform.position.z), down * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceLeft.transform.position.x - 2, lPieceLeft.transform.position.y - 1, lPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftLeft2Down2 = true;
        }
        bool bottomLeft2Down2 = false;
        Debug.DrawRay(new Vector3(lPieceBottom.transform.position.x - 2, lPieceBottom.transform.position.y + 1, lPieceBottom.transform.position.z), down * 2, Color.red);
        if (Physics.Raycast(new Vector3(lPieceBottom.transform.position.x - 2, lPieceBottom.transform.position.y + 1, lPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomLeft2Down2 = true;
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
                // Kick test 0R 5
                if (!leftDown3 && !bottomDown3Left2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -2));
                }
                // Kick test 0R 4
                if (!bottomDown3 && !rightDown3Left2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 0R 3
                if (!leftUp2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 1));
                }
                // Kick test 0R 2
                if (!lPieceLeftBools[1] && !lPieceLeftBools[0] && !lPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test 0R 1
                if (!lPieceBottomBools[1] && !lPieceBottomBools[0] && !lPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test L0 5 MIRRORED
                if (!rightDown3 && !lPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -2));
                }
                // Kick test L0 4 MIRRORED
                if (!bottomDown3 && !lPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, -2));
                }
                // Kick test L0 3 MIRRORED
                if (!bottomUp2Right2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 1));
                }
                // Kick test L0 2 MIRRORED
                if (!lPieceBottomBools[0] && !lPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test L0 1 MIRRORED
                if (!lPieceLeftBools[0] && !lPieceBottomBools[1] && !lPieceBottomBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (lPieceLeftBools[2] || lPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (lPieceRightBools[3] || lPieceTopBools[3])
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
                if (!leftUp1Right3)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 2));
                }
                // Kick test R2 4
                if (!leftLeft1Up1Right3)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 2));
                }
                // Kick test R2 3
                if (!lPieceTopBools[3] && !lPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -1));
                }
                // Kick test R2 2
                if (!bottomRight2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test R2 1
                if (!lPieceBottomBools[2] && !lPieceBottomBools[3] && !lPieceRightBools[2])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 0R 5 MIRRORED
                if (!leftUp1Right2 && !topRight1Up3Up2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 2));
                }
                // Kick test 0R 4 MIRRORED
                if (!leftLeft1Up1Right2 && !topUp3Up2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 2));
                }
                // Kick test 0R 3 MIRRORED
                if (!topRight1Up2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -1));
                }
                // Kick test 0R 2 MIRRORED
                if (!bottomRight2 && !bottomRight2Up2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test 0R 1 MIRRORED
                if (!lPieceLeftBools[3] && !lPieceBottomBools[2] && !lPieceBottomBools[3])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (lPieceRightBools[2] || lPieceLeftBools[2] || lPieceBottomBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (lPieceTopBools[3] || lPieceLeftBools[3] || lPieceBottomBools[3])
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
                if (!leftDown3 && !lPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -2));
                }
                // Kick test 2L 4
                if (!bottomDown3)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 2L 3
                if (!leftUp2Left2 && !lPieceLeftBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 1));
                }
                // Kick test 2L 2
                if (!lPieceBottomBools[0] && !lPieceLeftBools[1] && !lPieceLeftBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test 2L 1
                if (!lPieceRightBools[0] && !lPieceBottomBools[1] && !lPieceBottomBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test R2 5 MIRRORED
                if (!topDown2Right2 && !lPieceTopBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -2));
                }
                // Kick test R2 4 MIRRORED
                if (!bottomDown2 && !bottomDown3Right2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, -2));
                }
                // Kick test R2 3 MIRRORED
                if (!rightUp2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 1));
                }
                // Kick test R2 2 MIRRORED
                if (!lPieceRightBools[0] && !lPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test R2 1 MIRRORED
                if (!lPieceBottomBools[0] && !lPieceBottomBools[1] && !lPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement tests
                if (lPieceRightBools[2] || lPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (lPieceLeftBools[3] || lPieceTopBools[3])
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
                if (!rightUp1Left3 && !rightUp2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 2));
                }
                // Kick test L0 4
                if (!rightUp1Left2 && !rightRight1Up1Up2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 2));
                }
                // Kick test L0 3
                if (!leftLeft2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -1));
                }
                // Kick test L0 2
                if (!bottomLeft2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test L0 1
                if (!lPieceBottomBools[2] && !lPieceBottomBools[3] && !lPieceRightBools[3])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 2L 5 MIRRORED
                if (!rightUp1Left3 && !lPieceTopBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 2));
                }
                // Kick test 2L 4 MIRRORED
                if (!topUp1Right3)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 2));
                }
                // Kick test 2L 3 MIRRORED
                if (!leftLeft2 && !leftLeft2Down2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -1));
                }
                // Kick test 2L 2 MIRRORED
                if (!bottomLeft2Down2 && !lPieceBottomBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test 2L 1 MIRRORED
                if (!lPieceBottomBools[2] && !lPieceBottomBools[3] && !lPieceLeftBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement Tests
                if (lPieceBottomBools[2] || lPieceTopBools[2] || lPieceLeftBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (lPieceLeftBools[3] || lPieceBottomBools[3] || lPieceRightBools[3])
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