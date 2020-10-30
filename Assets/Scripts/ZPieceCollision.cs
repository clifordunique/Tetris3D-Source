using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on an ZPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class ZPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject zPieceTransformHandle;
    [SerializeField] private GameObject zPieceTop;
    [SerializeField] private GameObject zPieceBottom;
    [SerializeField] private GameObject zPieceLeft;
    [SerializeField] private GameObject zPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] zPieceTopBools;
    private bool[] zPieceBottomBools;
    private bool[] zPieceLeftBools;
    private bool[] zPieceRightBools;
    private int orientation = 0;

    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        layerMask = 1 << 9;
        zPieceTopBools = new bool[4];
        zPieceBottomBools = new bool[4];
        zPieceLeftBools = new bool[4];
        zPieceRightBools = new bool[4];
        pieceMovementScript = this.GetComponent<PieceMovement>();
    }

    void Update()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (zPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (zPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (zPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (zPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
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
                    currentCube = zPieceTop;
                    break;
                case 1:
                    currentCube = zPieceBottom;
                    break;
                case 2:
                    currentCube = zPieceLeft;
                    break;
                case 3:
                    currentCube = zPieceRight;
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
                    zPieceTopBools = temporary;
                    break;
                case 1:
                    zPieceBottomBools = temporary;
                    break;
                case 2:
                    zPieceLeftBools = temporary;
                    break;
                case 3:
                    zPieceRightBools = temporary;
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
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp2 = true;
        }
        bool leftDown3 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), down, 3, layerMask))
        {
            leftDown3 = true;
        }
        bool leftDown2Right1 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y - 2, zPieceLeft.transform.position.z), right * 1, Color.green);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y - 2, zPieceLeft.transform.position.z), right, 1, layerMask))
        {
            leftDown2Right1 = true;
        }
        bool leftDown2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2 = true;
        }
        bool bottomDown1Down2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y - 1, zPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y - 1, zPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown1Down2 = true;
        }
        bool bottomDown2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y, zPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y, zPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown2 = true;
        }
        bool rightDown2Down2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y - 2, zPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y - 2, zPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2Down2 = true;
        }
        bool rightDown2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2 = true;
        }
        bool leftDown2Down2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y - 2 , zPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y - 2, zPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2Down2 = true;
        }
        // Reverse
        bool rightUp2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2 = true;
        }
        bool rightDown3 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), down, 3, layerMask))
        {
            rightDown3 = true;
        }
        bool rightDown2Left1 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y - 2, zPieceRight.transform.position.z), left * 1, Color.green);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y - 2, zPieceRight.transform.position.z), left, 1, layerMask))
        {
            rightDown2Left1 = true;
        }
        //// Orientation 2
        bool rightRight2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightRight2 = true;
        }
        bool topDown2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x, zPieceTop.transform.position.y, zPieceTop.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x, zPieceTop.transform.position.y, zPieceTop.transform.position.z), down, 2, layerMask))
        {
            topDown2 = true;
        }
        bool top3 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x - 2, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x - 2, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), right, 3, layerMask))
        {
            top3 = true;
        }
        bool topRight13 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), right, 3, layerMask))
        {
            topRight13 = true;
        }
        bool topUp2Right2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x - 1, zPieceTop.transform.position.y + 2, zPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x - 1, zPieceTop.transform.position.y + 2, zPieceTop.transform.position.z), right, 2, layerMask))
        {
            topUp2Right2 = true;
        }
        bool leftUp1Right2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftUp1Right2 = true;
        }
        bool rightDown1Left2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y - 1, zPieceRight.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y - 1, zPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightDown1Left2 = true;
        }
        bool topDown2Right2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x - 1, zPieceTop.transform.position.y - 2, zPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x - 1, zPieceTop.transform.position.y - 2, zPieceTop.transform.position.z), right, 2, layerMask))
        {
            topDown2Right2 = true;
        }
        // Reverse
        bool leftRight2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftRight2 = true;
        }
        bool topUp2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x, zPieceTop.transform.position.y, zPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x, zPieceTop.transform.position.y, zPieceTop.transform.position.z), up, 2, layerMask))
        {
            topUp2 = true;
        }
        bool topUp2Up1 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x, zPieceTop.transform.position.y + 2, zPieceTop.transform.position.z), up * 1, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x, zPieceTop.transform.position.y + 2, zPieceTop.transform.position.z), up, 1, layerMask))
        {
            topUp2Up1 = true;
        }
        bool leftUp1Left2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x + 1, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x + 1, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftUp1Left2 = true;
        }
        bool leftUp2Right2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y + 2, zPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y + 2, zPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftUp2Right2 = true;
        }
        bool leftUp2Left2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x + 1, zPieceLeft.transform.position.y + 2, zPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x + 1, zPieceLeft.transform.position.y + 2, zPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftUp2Left2 = true;
        }
        bool bottomUp3Left2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x + 1, zPieceBottom.transform.position.y + 3, zPieceBottom.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x + 1, zPieceBottom.transform.position.y + 3, zPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomUp3Left2 = true;
        }
        //// Orientation 3
        bool bottomDown2Right1 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y - 2, zPieceBottom.transform.position.z), right * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y - 2, zPieceBottom.transform.position.z), right, 1, layerMask))
        {
            bottomDown2Right1 = true;
        }
        bool leftDown1Down2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y - 1, zPieceLeft.transform.position.z), down * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y - 1, zPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown1Down2 = true;
        }
        bool bottomUp2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y, zPieceBottom.transform.position.z), up * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y, zPieceBottom.transform.position.z), up, 2, layerMask))
        {
            bottomUp2 = true;
        }
        bool rightDown1Down2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y - 1, zPieceRight.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y - 1, zPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown1Down2 = true;
        }
        bool leftUp1Up2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y + 1, zPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp1Up2 = true;
        }
        // Reverses
        bool bottomDown2Left1 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y - 2, zPieceBottom.transform.position.z), left * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x, zPieceBottom.transform.position.y - 2, zPieceBottom.transform.position.z), left, 1, layerMask))
        {
            bottomDown2Left1 = true;
        }
        bool rightUp2Up2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2Up2 = true;
        }
        //// Orientation 4
        bool leftLeft2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x, zPieceLeft.transform.position.y, zPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftLeft2 = true;
        }
        bool rightTop3 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x + 2, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x + 2, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTop3 = true;
        }
        bool rightTopLeft13 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTopLeft13 = true;
        }
        bool rightUp1Left2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightUp1Left2 = true;
        }
        bool rightUp2Right2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x - 1, zPieceRight.transform.position.y + 2, zPieceRight.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x - 1, zPieceRight.transform.position.y + 2, zPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightUp2Right2 = true;
        }
        bool bottomUp2Left2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x + 1, zPieceBottom.transform.position.y + 2, zPieceBottom.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x + 1, zPieceBottom.transform.position.y + 2, zPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomUp2Left2 = true;
        }
        bool bottomUp3Right2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x - 1, zPieceBottom.transform.position.y + 3, zPieceBottom.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x - 1, zPieceBottom.transform.position.y + 3, zPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomUp3Right2 = true;
        }
        // Reverse
        bool rightLeft2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x, zPieceRight.transform.position.y, zPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightLeft2 = true;
        }
        bool rightUp1Right2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x - 1, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x - 1, zPieceRight.transform.position.y + 1, zPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightUp1Right2 = true;
        }
        bool bottomUp2Right2 = false;
        Debug.DrawRay(new Vector3(zPieceBottom.transform.position.x - 1, zPieceBottom.transform.position.y + 2, zPieceBottom.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceBottom.transform.position.x - 1, zPieceBottom.transform.position.y + 2, zPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomUp2Right2 = true;
        }
        bool topDown2Left2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x + 1, zPieceTop.transform.position.y - 2, zPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x + 1, zPieceTop.transform.position.y - 2, zPieceTop.transform.position.z), left, 2, layerMask))
        {
            topDown2Left2 = true;
        }
        bool topUp3Left2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x + 1, zPieceTop.transform.position.y + 3, zPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x + 1, zPieceTop.transform.position.y + 3, zPieceTop.transform.position.z), left, 2, layerMask))
        {
            topUp3Left2 = true;
        }
        bool rightUp2Left2 = false;
        Debug.DrawRay(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y + 2, zPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceRight.transform.position.x + 1, zPieceRight.transform.position.y + 2, zPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightUp2Left2 = true;
        }
        bool topUp2Left2 = false;
        Debug.DrawRay(new Vector3(zPieceTop.transform.position.x + 1, zPieceTop.transform.position.y + 2, zPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceTop.transform.position.x + 1, zPieceTop.transform.position.y + 2, zPieceTop.transform.position.z), left, 2, layerMask))
        {
            topUp2Left2 = true;
        }
        bool leftDown1Right2 = false;
        Debug.DrawRay(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y - 1, zPieceLeft.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(zPieceLeft.transform.position.x - 1, zPieceLeft.transform.position.y - 1, zPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftDown1Right2 = true;
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
                if (!bottomDown2 && !leftDown2Down2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -2));
                }
                // Kick test 0R 4
                if (!rightDown2 && !bottomDown1Down2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 0R 3
                if (!zPieceTopBools[1] && !zPieceBottomBools[2])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 1));
                }
                // Kick test 0R 2
                if (!leftDown2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test 0R 1
                if (!zPieceBottomBools[1] && !zPieceTopBools[3])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test L0 3 MIRRORED
                if (!rightUp2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 1));
                }
                // Kick test L0 2 MIRRORED
                if (!zPieceBottomBools[1] && !zPieceTopBools[3])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test L0 1 MIRRORED
                if (!leftDown2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (zPieceLeftBools[2] || zPieceBottomBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (zPieceRightBools[3] || zPieceTopBools[3])
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
                if (!leftUp1Left2 && !zPieceLeftBools[3])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 2));
                }
                // Kick test R2 4
                if (!bottomUp2Left2 && !zPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 2));
                }
                // Kick test R2 3
                if (!topDown2Right2 && !zPieceTopBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -1));
                }
                // Kick test R2 2
                if (!rightRight2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test R2 1
                if (!zPieceBottomBools[2] && !zPieceTopBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 0R 5 MIRRORED
                if (!leftUp1Right2 && !leftUp2Left2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 2));
                }
                // Kick test 0R 4 MIRRORED
                if (!bottomUp2Right2 && !bottomUp3Left2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 2));
                }
                // Kick test 0R 3 MIRRORED
                if (!rightRight2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -1));
                }
                // Kick test 0R 2 MIRRORED
                if (!zPieceBottomBools[0] && !zPieceTopBools[3])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test 0R 1 MIRRORED
                if (!leftLeft2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (zPieceRightBools[2] || zPieceBottomBools[2] || zPieceLeftBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (zPieceRightBools[3] || zPieceTopBools[3] || zPieceLeftBools[3])
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
                if (!topDown2 && !zPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -2));
                }
                // Kick test 2L 4
                if (!rightDown2Down2 && !zPieceTopBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 2L 3
                if (!leftUp1Up2 && !zPieceBottomBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 1));
                }
                // Kick test 2L 2
                if (!leftUp2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test 2L 1
                if (!zPieceBottomBools[0] && !zPieceTopBools[2])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test R2 5 MIRRORED
                if (!rightDown1Down2 && !zPieceTopBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -2));
                }
                // Kick test R2 4 MIRRORED
                if (!topDown2 && !zPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, -2));
                }
                // Kick test R2 3 MIRRORED
                if (!bottomUp2 && !zPieceRightBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 1));
                }
                // Kick test R2 2 MIRRORED
                if (!zPieceBottomBools[0] && !zPieceTopBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test R2 1 MIRRORED
                if (!leftUp2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement tests
                if (zPieceRightBools[2] || zPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (zPieceLeftBools[3] || zPieceBottomBools[3])
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
                if (!topUp2Right2 && !topUp3Left2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 2));
                }
                // Kick test L0 4
                if (!rightUp1Right2 && !rightUp2Left2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 2));
                }
                // Kick test L0 3
                if (!zPieceBottomBools[1] && !zPieceTopBools[2])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -1));
                }
                // Kick test L0 2
                if (!rightLeft2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test L0 1
                if (!zPieceBottomBools[3] && !zPieceTopBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 2L 5 MIRRORED
                if (!topUp2Left2 && !zPieceTopBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 2));
                }
                // Kick test 2L 4 MIRRORED
                if (!topUp2Right2 && !zPieceRightBools[3])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 2));
                }
                // Kick test 2L 3 MIRRORED
                if (!leftDown1Right2 && !zPieceLeftBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -1));
                }
                // Kick test 2L 2 MIRRORED
                if (!zPieceBottomBools[1] && !zPieceTopBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test 2L 1 MIRRORED
                if (!leftRight2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement Tests
                if (zPieceLeftBools[2] || zPieceTopBools[2] || zPieceRightBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (zPieceLeftBools[3] || zPieceBottomBools[3] || zPieceRightBools[3])
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

    #endregion
}