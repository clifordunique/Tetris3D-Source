using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class makes raycasts from each of the cubes on an jPiece and
 * reports if they are colliding with layer 9 (Collider Layer) accordingly.
*/

public class JPieceCollision : MonoBehaviour
{
    #region Private Serialized Field

    private PieceMovement pieceMovementScript;
    [SerializeField] private GameObject jPieceTransformHandle;
    [SerializeField] private GameObject jPieceTop;
    [SerializeField] private GameObject jPieceBottom;
    [SerializeField] private GameObject jPieceLeft;
    [SerializeField] private GameObject jPieceRight;

    #endregion

    #region Private Fields

    private int layerMask;
    private bool[] jPieceTopBools;
    private bool[] jPieceBottomBools;
    private bool[] jPieceLeftBools;
    private bool[] jPieceRightBools;
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
        jPieceTopBools = new bool[4];
        jPieceBottomBools = new bool[4];
        jPieceLeftBools = new bool[4];
        jPieceRightBools = new bool[4];
        pieceMovementScript = this.GetComponent<PieceMovement>();
    }

    void Update()
    {
        pieceMovementScript.setIsGrounded(false);
        Vector3 down = transform.TransformDirection(Vector3.down);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (jPieceTransformHandle.transform.rotation.eulerAngles.z == 0)
            orientation = 1;
        if (jPieceTransformHandle.transform.rotation.eulerAngles.z == 270)
            orientation = 2;
        if (jPieceTransformHandle.transform.rotation.eulerAngles.z == 180)
            orientation = 3;
        if (jPieceTransformHandle.transform.rotation.eulerAngles.z == 90)
            orientation = 4;

        // Collision Checks
        for (int i = 0; i < 4; i++)
        {
            bool[] temporary = new bool[4];
            GameObject currentCube = null;
            switch (i)
            {
                case 0:
                    currentCube = jPieceTop;
                    break;
                case 1:
                    currentCube = jPieceBottom;
                    break;
                case 2:
                    currentCube = jPieceLeft;
                    break;
                case 3:
                    currentCube = jPieceRight;
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
                    jPieceTopBools = temporary;
                    break;
                case 1:
                    jPieceBottomBools = temporary;
                    break;
                case 2:
                    jPieceLeftBools = temporary;
                    break;
                case 3:
                    jPieceRightBools = temporary;
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
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp2 = true;
        }
        bool leftDown3 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), down, 3, layerMask))
        {
            leftDown3 = true;
        }
        bool leftDown2Right1 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y - 2, jPieceLeft.transform.position.z), right * 1, Color.green);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y - 2, jPieceLeft.transform.position.z), right, 1, layerMask))
        {
            leftDown2Right1 = true;
        }
        bool leftDown2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2 = true;
        }
        bool bottomDown1Down2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y - 1, jPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y - 1, jPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown1Down2 = true;
        }
        bool bottomDown2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomDown2 = true;
        }
        bool rightDown2Down2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y - 2, jPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y - 2, jPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2Down2 = true;
        }
        bool rightDown2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown2 = true;
        }
        bool leftDown2Down2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y - 2, jPieceLeft.transform.position.z), down * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y - 2, jPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown2Down2 = true;
        }
        bool bottomDown3 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), down, 3, layerMask))
        {
            bottomDown3 = true;
        }
        bool topUp1Right2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y + 1, jPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y + 1, jPieceTop.transform.position.z), right, 2, layerMask))
        {
            topUp1Right2 = true;
        }
        // Reverse Orientation 1
        bool rightUp2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), up * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2 = true;
        }
        bool rightDown3 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), down * 3, Color.green);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), down, 3, layerMask))
        {
            rightDown3 = true;
        }
        bool rightDown2Left1 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y - 2, jPieceRight.transform.position.z), left * 1, Color.green);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y - 2, jPieceRight.transform.position.z), left, 1, layerMask))
        {
            rightDown2Left1 = true;
        }
        bool rightDown3Left2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y - 3, jPieceRight.transform.position.z), left * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y - 3, jPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightDown3Left2 = true;
        }
        bool bottomDown3Left2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x + 1, jPieceBottom.transform.position.y - 3, jPieceBottom.transform.position.z), left * 2, Color.green);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x + 1, jPieceBottom.transform.position.y - 3, jPieceBottom.transform.position.z), left , 2, layerMask))
        {
            bottomDown3Left2 = true;
        }
        //// Orientation 2
        bool rightRight2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightRight2 = true;
        }
        bool topDown2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x, jPieceTop.transform.position.y, jPieceTop.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x, jPieceTop.transform.position.y, jPieceTop.transform.position.z), down, 2, layerMask))
        {
            topDown2 = true;
        }
        bool top3 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x - 2, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x - 2, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), right, 3, layerMask))
        {
            top3 = true;
        }
        bool topRight13 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), right * 3, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), right, 3, layerMask))
        {
            topRight13 = true;
        }
        bool topUp2Right2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y + 2, jPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y + 2, jPieceTop.transform.position.z), right, 2, layerMask))
        {
            topUp2Right2 = true;
        }
        bool leftUp1Right2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftUp1Right2 = true;
        }
        bool rightDown1Left2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y - 1, jPieceRight.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y - 1, jPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightDown1Left2 = true;
        }
        bool topDown2Right2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y - 2, jPieceTop.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y - 2, jPieceTop.transform.position.z), right, 2, layerMask))
        {
            topDown2Right2 = true;
        }
        bool rightRight2Down2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 2, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 2, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightRight2Down2 = true;
        }
        bool bottomRight2Down2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x + 2, jPieceBottom.transform.position.y + 1, jPieceBottom.transform.position.z), down * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x + 2, jPieceBottom.transform.position.y + 1, jPieceBottom.transform.position.z), down, 2, layerMask))
        {
            bottomRight2Down2 = true;
        }
        // Reverse Orientation 2
        bool leftRight2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftRight2 = true;
        }
        bool topUp2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x, jPieceTop.transform.position.y, jPieceTop.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x, jPieceTop.transform.position.y, jPieceTop.transform.position.z), up, 2, layerMask))
        {
            topUp2 = true;
        }
        bool topUp2Up1 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x, jPieceTop.transform.position.y + 2, jPieceTop.transform.position.z), up * 1, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x, jPieceTop.transform.position.y + 2, jPieceTop.transform.position.z), up, 1, layerMask))
        {
            topUp2Up1 = true;
        }
        bool leftUp1Left2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x + 1, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x + 1, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftUp1Left2 = true;
        }
        bool leftUp2Right2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y + 2, jPieceLeft.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y + 2, jPieceLeft.transform.position.z), right, 2, layerMask))
        {
            leftUp2Right2 = true;
        }
        bool leftUp2Left2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x + 1, jPieceLeft.transform.position.y + 2, jPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x + 1, jPieceLeft.transform.position.y + 2, jPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftUp1Left2 = true;
        }
        bool bottomUp3Left2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x + 1, jPieceBottom.transform.position.y + 3, jPieceBottom.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x + 1, jPieceBottom.transform.position.y + 3, jPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomUp3Left2 = true;
        }
        bool leftLeft1Up1Up2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftLeft1Up1Up2 = true;
        }
        bool bottomRight2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), right * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomRight2 = true;
        }
        //// Orientation 3
        bool bottomDown2Right1 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y - 2, jPieceBottom.transform.position.z), right * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y - 2, jPieceBottom.transform.position.z), right, 1, layerMask))
        {
            bottomDown2Right1 = true;
        }
        bool leftDown1Down2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y - 1, jPieceLeft.transform.position.z), down * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y - 1, jPieceLeft.transform.position.z), down, 2, layerMask))
        {
            leftDown1Down2 = true;
        }
        bool bottomUp2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), up * 2, Color.cyan);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), up, 2, layerMask))
        {
            bottomUp2 = true;
        }
        bool rightDown1Down2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y - 1, jPieceRight.transform.position.z), down * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y - 1, jPieceRight.transform.position.z), down, 2, layerMask))
        {
            rightDown1Down2 = true;
        }
        bool leftUp1Up2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y + 1, jPieceLeft.transform.position.z), up, 2, layerMask))
        {
            leftUp1Up2 = true;
        }
        bool leftDown3Left2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x + 1, jPieceLeft.transform.position.y - 3, jPieceLeft.transform.position.z), left * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x + 1, jPieceLeft.transform.position.y - 3, jPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftDown3Left2 = true;
        }
        // Reverse Orientation 3
        bool bottomDown2Left1 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y - 2, jPieceBottom.transform.position.z), left * 1, Color.cyan);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y - 2, jPieceBottom.transform.position.z), left, 1, layerMask))
        {
            bottomDown2Left1 = true;
        }
        bool rightUp2Up2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), up * 2, Color.magenta);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightUp2Up2 = true;
        }
        //// Orientation 4
        bool leftLeft2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x, jPieceLeft.transform.position.y, jPieceLeft.transform.position.z), left, 2, layerMask))
        {
            leftLeft2 = true;
        }
        bool rightTop3 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 2, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 2, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTop3 = true;
        }
        bool rightTopLeft13 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightTopLeft13 = true;
        }
        bool rightUp1Left2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightUp1Left2 = true;
        }
        bool rightUp2Right2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x - 1, jPieceRight.transform.position.y + 2, jPieceRight.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x - 1, jPieceRight.transform.position.y + 2, jPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightUp2Right2 = true;
        }
        bool bottomUp2Left2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x + 1, jPieceBottom.transform.position.y + 2, jPieceBottom.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x + 1, jPieceBottom.transform.position.y + 2, jPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomUp2Left2 = true;
        }
        bool bottomUp3Right2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x - 1, jPieceBottom.transform.position.y + 3, jPieceBottom.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x - 1, jPieceBottom.transform.position.y + 3, jPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomUp3Right2 = true;
        }
        bool rightUp1Left3 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightUp1Left3 = true;
        }
        bool rightLeft2Up1Up2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x - 2, jPieceRight.transform.position.y, jPieceRight.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x - 2, jPieceRight.transform.position.y, jPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightLeft2Up1Up2 = true;
        }
        bool rightLeft1Up1Up2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x - 1, jPieceRight.transform.position.y, jPieceRight.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x - 1, jPieceRight.transform.position.y, jPieceRight.transform.position.z), up, 2, layerMask))
        {
            rightLeft1Up1Up2 = true;
        }
        bool topLeft1Up2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y - 1, jPieceTop.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y - 1, jPieceTop.transform.position.z), up, 2, layerMask))
        {
            topLeft1Up2 = true;
        }
        bool topLeft1Up1Up2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y, jPieceTop.transform.position.z), up * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x - 1, jPieceTop.transform.position.y, jPieceTop.transform.position.z), up, 2, layerMask))
        {
            topLeft1Up1Up2 = true;
        }
        // Reverse Orientation 4
        bool rightLeft2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x, jPieceRight.transform.position.y, jPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightLeft2 = true;
        }
        bool rightUp1Right2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x - 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x - 1, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), right, 2, layerMask))
        {
            rightUp1Right2 = true;
        }
        bool bottomUp2Right2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x - 1, jPieceBottom.transform.position.y + 2, jPieceBottom.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x - 1, jPieceBottom.transform.position.y + 2, jPieceBottom.transform.position.z), right, 2, layerMask))
        {
            bottomUp2Right2 = true;
        }
        bool topDown2Left2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x + 1, jPieceTop.transform.position.y - 2, jPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x + 1, jPieceTop.transform.position.y - 2, jPieceTop.transform.position.z), left, 2, layerMask))
        {
            topDown2Left2 = true;
        }
        bool topUp3Left2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x + 1, jPieceTop.transform.position.y + 3, jPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x + 1, jPieceTop.transform.position.y + 3, jPieceTop.transform.position.z), left, 2, layerMask))
        {
            topUp3Left2 = true;
        }
        bool rightUp2Left2 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 2, jPieceRight.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 1, jPieceRight.transform.position.y + 2, jPieceRight.transform.position.z), left, 2, layerMask))
        {
            rightUp2Left2 = true;
        }
        bool topUp2Left2 = false;
        Debug.DrawRay(new Vector3(jPieceTop.transform.position.x + 1, jPieceTop.transform.position.y + 2, jPieceTop.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceTop.transform.position.x + 1, jPieceTop.transform.position.y + 2, jPieceTop.transform.position.z), left, 2, layerMask))
        {
            topUp2Left2 = true;
        }
        bool leftDown1Right2 = false;
        Debug.DrawRay(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y - 1, jPieceLeft.transform.position.z), right * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceLeft.transform.position.x - 1, jPieceLeft.transform.position.y - 1, jPieceLeft.transform.position.z), right, 2, layerMask))
        {
            topUp2Left2 = true;
        }
        bool rightUp1Right1Left3 = false;
        Debug.DrawRay(new Vector3(jPieceRight.transform.position.x + 2, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left * 3, Color.red);
        if (Physics.Raycast(new Vector3(jPieceRight.transform.position.x + 2, jPieceRight.transform.position.y + 1, jPieceRight.transform.position.z), left, 3, layerMask))
        {
            rightUp1Right1Left3 = true;
        }
        bool bottomLeft2 = false;
        Debug.DrawRay(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), left * 2, Color.red);
        if (Physics.Raycast(new Vector3(jPieceBottom.transform.position.x, jPieceBottom.transform.position.y, jPieceBottom.transform.position.z), left, 2, layerMask))
        {
            bottomLeft2 = true;
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
                if (!leftDown3 && !jPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 2));
                }
                // Kick test 0R 4
                if (!bottomDown3 && !jPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 0R 3
                if (!topUp1Right2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 1));
                }
                // Kick test 0R 2
                if (!jPieceTopBools[3] && !jPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test 0R 1
                if (!jPieceBottomBools[0] && !jPieceBottomBools[1] && !jPieceRightBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test L0 5 MIRRORED
                if (!rightDown3 && !rightDown3Left2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -2));
                }
                // Kick test L0 4 MIRRORED
                if (!bottomDown3 && !bottomDown3Left2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, -2));
                }
                // Kick test L0 3 MIRRORED
                if (!rightUp2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 1));
                }
                // Kick test L0 2 MIRRORED
                if (!jPieceBottomBools[1] && !jPieceRightBools[0] && !jPieceRightBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test L0 1 MIRRORED
                if (!jPieceLeftBools[1] && !jPieceBottomBools[0] && !jPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (jPieceLeftBools[2] || jPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (jPieceRightBools[3] || jPieceTopBools[3])
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
                if (!jPieceLeftBools[0] && !topUp1Right2 && !jPieceTopBools[3])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 2));
                }
                // Kick test R2 4
                if (!leftUp1Left2 && !jPieceTopBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 2));
                }
                // Kick test R2 3
                if (!jPieceRightBools[3] && !rightRight2Down2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -1));
                }
                // Kick test R2 2
                if (!jPieceBottomBools[3] && !bottomRight2Down2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test R2 1
                if (!jPieceBottomBools[2] && !jPieceBottomBools[3] && !jPieceRightBools[3])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 0R 5 MIRRORED
                if (!leftUp2 && !topUp1Right2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 2));
                }
                // Kick test 0R 4 MIRRORED
                if (!leftLeft1Up1Up2 && !jPieceLeftBools[0] && !jPieceTopBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 2));
                }
                // Kick test 0R 3 MIRRORED
                if (!rightRight2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, -1));
                }
                // Kick test 0R 2 MIRRORED
                if (!bottomRight2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(1, 0));
                }
                // Kick test 0R 1 MIRRORED
                if (!jPieceLeftBools[2] && !jPieceBottomBools[2] && !jPieceBottomBools[3])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement test
                if (jPieceRightBools[2] || jPieceLeftBools[2] || jPieceBottomBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (jPieceTopBools[3] || jPieceRightBools[3] || jPieceBottomBools[3])
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
                if (!leftDown3Left2 && !topDown2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, -2));
                }
                // Kick test 2L 4
                if (!bottomDown3Left2 && !bottomDown2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, -2));
                }
                // Kick test 2L 3
                if (!leftUp2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 1));
                }
                // Kick test 2L 2
                if (!jPieceBottomBools[1] && !jPieceLeftBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(1, 0));
                }
                // Kick test 2L 1
                if (!jPieceRightBools[1] && !jPieceBottomBools[1] && !jPieceBottomBools[0])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test R2 5 MIRRORED
                if (!rightDown3 && !jPieceBottomBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -2));
                }
                // Kick test R2 4 MIRRORED
                if (!bottomDown3)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, -2));
                }
                // Kick test R2 3 MIRRORED
                if (!rightUp2Right2 && !jPieceRightBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 1));
                }
                // Kick test R2 2 MIRRORED
                if (!jPieceRightBools[0] && !jPieceRightBools[1] && !jPieceBottomBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test R2 1 MIRRORED
                if (!jPieceBottomBools[1] && !jPieceBottomBools[0] && !jPieceLeftBools[0])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement tests
                if (jPieceRightBools[2] || jPieceTopBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (jPieceLeftBools[3] || jPieceBottomBools[3])
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
                if (!rightUp1Left3 && !rightLeft2Up1Up2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 2));
                }
                // Kick test L0 4
                if (!rightLeft1Up1Up2 && !rightUp1Right2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 2));
                }
                // Kick test L0 3
                if (!topLeft1Up2)
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, -1));
                }
                // Kick test L0 2
                if (!topLeft1Up1Up2 && !jPieceBottomBools[2])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(-1, 0));
                }
                // Kick test L0 1
                if (!jPieceBottomBools[2] && !jPieceBottomBools[3] && !jPieceRightBools[2])
                {
                    pieceMovementScript.setTranslationVectorC(new Vector2(0, 0));
                }
                //// CounterClockwise Rotation tests
                // Kick test 2L 5 MIRRORED
                if (!rightUp1Left3)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 2));
                }
                // Kick test 2L 4 MIRRORED
                if (!rightUp1Right1Left3 && !jPieceRightBools[3])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 2));
                }
                // Kick test 2L 3 MIRRORED
                if (!jPieceTopBools[2] && !jPieceLeftBools[1])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, -1));
                }
                // Kick test 2L 2 MIRRORED
                if (!bottomLeft2)
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(-1, 0));
                }
                // Kick test 2L 1 MIRRORED
                if (!jPieceLeftBools[3] && !jPieceBottomBools[3] && !jPieceBottomBools[2])
                {
                    pieceMovementScript.setTranslationVectorCC(new Vector2(0, 0));
                }
                //// Movement Tests
                if (jPieceBottomBools[2] || jPieceTopBools[2] || jPieceRightBools[2])
                {
                    pieceMovementScript.setCanMoveLeft(false);
                }
                if (jPieceLeftBools[3] || jPieceBottomBools[3] || jPieceRightBools[3])
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