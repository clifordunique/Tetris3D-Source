using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

/*
 * PieceMovement handles all of the movements
 * related to a piece/tetromino and its respective movement-handle.
 * Gravity, moving left/right, rotating, and speeds 
 * at which each of those happen are controlled here.
 * 
 * This class is freaking epic.
*/

public class PieceMovement : MonoBehaviour
{

    #region Private Serialized (Or Get/Set) Field

    [Header("Seralized Fields")]
    [SerializeField] private float gravityTime = 1f;
    [SerializeField] private float movementTime = 0.1f;
    [SerializeField] private float softDropTime = 0.1f;
    //[SerializeField] private GameObject movementChecks;

    // Gets and Sets
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private bool canRotateClockwise = true;
    private bool canRotateCounterClockwise = true;
    private Vector2 translationVectorC = new Vector2(0,0);
    private Vector2 translationVectorCC = new Vector2(0, 0);
    // WallKicks and Twists are advanced Tetris moves. I coded them in because I'm a badass.

    #endregion

    #region Private Fields

    // Fields for gravity movement
    private float gravityTimer = 0f;

    // Fields for input bools
    private bool inputLeft = false;
    private bool inputRight = false;
    private bool inputRC = false;
    private bool inputRCC = false;
    //private bool inputHold = false;

    // Fields for moving left/right
    private bool isGrounded = false;
    private bool movingLeft = false;
    private bool movingRight = false;
    private bool movedLeft = false;
    private bool leftDelay = false;
    private bool movedRight = false;
    private bool rightDelay = false;
    private float moveLeftTimer = 0f;
    private float moveRightTimer = 0f;

    #endregion

    #region Public Get/Set Fields

    [Header("Public Fields")]
    public GameObject currentPiece;

    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {

    }

    // Due to errors where input would cause duplicate maneuvers (double jumping a T-Piece),
    // Input will be grabbed in Update() but processed in FixedUpdate()
    void Update()
    {
        // Gravity Handler
        if (!isGrounded)
        gravityTimer += Time.deltaTime;
        if (gravityTimer >= gravityTime && !isGrounded)
        {
            currentPiece.transform.Translate(new Vector3 (0, -1, 0), Space.World);
            gravityTimer = 0;
        }

        if (Input.GetKey("left"))
        {
            inputLeft = true;
            inputRight = false;
            inputRC = false;
            inputRCC = false;
        } else if (Input.GetKey("right"))
        {
            inputLeft = false;
            inputRight = true;
            inputRC = false;
            inputRCC = false;
        }
        if (Input.GetKeyDown("up"))
        {
            inputLeft = false;
            inputRight = false;
            inputRC = true;
            inputRCC = false;
        } else if (Input.GetKeyDown("z"))
        {
            inputLeft = false;
            inputRight = false;
            inputRC = false;
            inputRCC = true;
        }
    }

    private void FixedUpdate()
    {
        if (inputLeft)
        {
            leftMovementHandler();
        }
        else
        {
            movingLeft = false;
            movedLeft = false;
            leftDelay = false;
            moveLeftTimer = 0f;
        }

        if (inputRight)
        {
            rightMovementHandler();
        }
        else
        {
            movingRight = false;
            movedRight = false;
            rightDelay = false;
            moveRightTimer = 0f;
        }

        if (inputRC)
        {
            if (canRotateClockwise)
            {
                rotateClockwise(translationVectorC);
            }
        } 

        if (inputRCC)
        {
            if (canRotateCounterClockwise)
            {
                rotateCounterClockwise(translationVectorCC);
            }
        }
        inputLeft = false;
        inputRight = false;
        inputRC = false;
        inputRCC = false;
    }

    #endregion

    #region Private Methods

    #region Code for left and right movement was thrown down here to tidy things up

    private void leftMovementHandler()
    {
        movingLeft = true;
        if (!movingRight && canMoveLeft)
        {
            moveLeftTimer += Time.deltaTime;
            // Add some delay
            if (moveLeftTimer >= 0.2f)
            {
                leftDelay = true;
                moveLeftTimer = 0;
            }
            if (!movedLeft && canMoveLeft)
            {
                moveLeft();
                movedLeft = true;
            }
            else if (leftDelay && canMoveLeft)
            {
                moveLeftTimer += Time.deltaTime;
                if (moveLeftTimer >= movementTime)
                {
                    moveLeftTimer = 0f;
                    moveLeft();
                }
            }
        }
    }

    private void rightMovementHandler()
    {
        movingRight = true;
        if (!movingLeft && canMoveRight)
        {
            moveRightTimer += Time.deltaTime;
            // Add some delay
            if (moveRightTimer >= 0.2f)
            {
                rightDelay = true;
                moveRightTimer = 0;
            }
            if (!movedRight && canMoveRight)
            {
                moveRight();
                movedRight = true;
            }
            else if (rightDelay && canMoveRight)
            {
                moveRightTimer += Time.deltaTime;
                if (moveRightTimer >= movementTime)
                {
                    moveRightTimer = 0f;
                    moveRight();
                }
            }
        }
    }

    #endregion

    private void moveLeft()
    {
        currentPiece.transform.Translate(new Vector3(-1, 0, 0), Space.World);
    }

    private void moveRight()
    {
        currentPiece.transform.Translate(new Vector3(1, 0, 0), Space.World);
    }

    private void rotateClockwise(Vector2 translation)
    {
        currentPiece.transform.Translate(new Vector3(translation.x, translation.y, 0), Space.World);
        currentPiece.transform.Rotate(new Vector3(0, 0, -90));
    }

    private void rotateCounterClockwise(Vector2 translation)
    {
        currentPiece.transform.Translate(new Vector3(translation.x, translation.y, 0), Space.World);
        currentPiece.transform.Rotate(new Vector3(0, 0, 90));
    }

    #endregion

    #region Public Methods

    public void setIsGrounded(bool setValue)
    {
        isGrounded = setValue;
    }
    public void setCanMoveLeft(bool setValue)
    {
        canMoveLeft = setValue;
    }
    public void setCanMoveRight(bool setValue)
    {
        canMoveRight = setValue;
    }
    public void setCanRotateClockwise(bool setValue)
    {
        canRotateClockwise = setValue;
    }
    public void setCanRotateCounterClockwise(bool setValue)
    {
        canRotateCounterClockwise = setValue;
    }
    public void setTranslationVectorC(Vector2 setValue)
    {
        canRotateClockwise = true;
        translationVectorC = setValue;
    }
    public void setTranslationVectorCC(Vector2 setValue)
    {
        canRotateCounterClockwise = true;
        translationVectorCC = setValue;
    }

    #endregion
}
