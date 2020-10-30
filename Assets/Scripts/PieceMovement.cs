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

// Also, WallKicks and Twists are advanced Tetris moves using
// a Super Rotation System. I coded them in, because I'm a badass.

public class PieceMovement : MonoBehaviour
{
    #region Static Field

    // I don't usually like to use static variables for things, but there's a time and place for everything.
    public static float distanceDown = 90f;
    public static bool isSpinning = false;
    public static bool isKicking = false;

    #endregion

    #region Private Serialized (Or Get/Set) Field

    [Header("Seralized Fields")]
    [SerializeField] private float gravityTime = 1f;
    [SerializeField] private float landingTime = 1f;
    [SerializeField] private float movementTime = 0.1f;
    [SerializeField] private float leftRightSlightDelay = 0.05f;
    [SerializeField] private float softDropTime = 0.1f;

    // Gets and Sets
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    private bool canRotateClockwise = true;
    private bool canRotateCounterClockwise = true;
    private Vector2 translationVectorC = new Vector2(0,0);
    private Vector2 translationVectorCC = new Vector2(0, 0);

    #endregion

    #region Private Fields

    private PieceManager pieceManager;

    // Fields for gravity movement
    private float gravityTimer = 0f;
    private float softDropTimer = 0f;

    // Fields for landing
    private float landingTimer = 0f;

    // Fields for input bools
    private bool inputLeft = false;
    private bool inputRight = false;
    private bool inputRC = false;
    private bool inputRCC = false;

    // Fields for moving left/right
    private bool isGrounded = false;
    private bool movingLeft = false;
    private bool movingRight = false;
    private bool movedLeft = false;
    private bool leftDelay = false;
    private bool movedRight = false;
    private bool rightDelay = false;
    private bool isSoftDropping = false;
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
        pieceManager = GetComponent<PieceManager>();
    }

    void Update()
    {
        // Show the ghost piece
        pieceManager.showGhostPiece();


        // Grabbing Inputs
        if (Input.GetKeyDown("space"))
        {
            currentPiece.transform.Translate(new Vector3(0, 0.5f - Mathf.Abs(distanceDown), 0), Space.World);
            landPiece();
        }
        if (Input.GetKey("down"))
        {
            isSoftDropping = true;
        }
        else
        {
            isSoftDropping = false;
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
        if (Input.GetKeyDown("up") || Input.GetKeyDown("x"))
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

    private void LateUpdate()
    {
        // Gravity Handler
        if (!isGrounded)
            gravityTimer += Time.deltaTime;
        if (gravityTimer >= gravityTime && !isGrounded)
        {
            currentPiece.transform.Translate(new Vector3(0, -1, 0), Space.World);
            gravityTimer = 0;
        }

        if (isSoftDropping)
        {
            softDropTimer += Time.deltaTime;
            if (softDropTimer >= softDropTime && !isGrounded)
            {
                currentPiece.transform.Translate(new Vector3(0, -1, 0), Space.World);
                softDropTimer = 0;
            }
        }

        // Piece Landing Handler
        if (isGrounded)
        {
            landingTimer += Time.deltaTime;
        }
        if (landingTimer >= landingTime && isGrounded)
        {
            landPiece();
        }

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
                if (isSpinning)
                    Score.hasSpun = true;
                else
                    Score.hasSpun = false;
                if (isKicking)
                    Score.hasKicked = true;
                else
                    Score.hasKicked = false;
                rotateClockwise(translationVectorC);
            }
        } else if (inputRCC)
        {
            if (canRotateCounterClockwise)
            {
                if (isSpinning)
                    Score.hasSpun = true;
                else
                    Score.hasSpun = false;
                if (isKicking)
                    Score.hasKicked = true;
                else
                    Score.hasKicked = false;
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

    private void landPiece()
    {
        pieceManager.instantiateDeadPiece();
        distanceDown = 90f;
    }

    #region Code for left and right movement was thrown down here to tidy things up

    private void leftMovementHandler()
    {
        movingLeft = true;
        if (!movingRight && canMoveLeft)
        {
            moveLeftTimer += Time.deltaTime;
            // Add some delay
            if (moveLeftTimer >= leftRightSlightDelay)
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
            if (moveRightTimer >= leftRightSlightDelay)
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
        landingTimer = 0;
        currentPiece.transform.Translate(new Vector3(-1, 0, 0), Space.World);
    }

    private void moveRight()
    {
        landingTimer = 0;
        currentPiece.transform.Translate(new Vector3(1, 0, 0), Space.World);
    }

    private void rotateClockwise(Vector2 translation)
    {
        landingTimer = 0;
        currentPiece.transform.Translate(new Vector3(translation.x, translation.y, 0), Space.World);
        currentPiece.transform.Rotate(new Vector3(0, 0, -90));
    }

    private void rotateCounterClockwise(Vector2 translation)
    {
        landingTimer = 0;
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
        isSpinning = false;
        isKicking = false;
        canRotateClockwise = true;
        translationVectorC = setValue;
    }
    public void setTranslationVectorCC(Vector2 setValue)
    {
        isSpinning = false;
        isKicking = false;
        canRotateCounterClockwise = true;
        translationVectorCC = setValue;
    }

    #endregion
}
