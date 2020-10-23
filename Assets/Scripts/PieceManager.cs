using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityScript.Scripting.Pipeline;

// Numbers for the bag arrays
// 1 = tpiece
// 2 = spiece
// 3 = zpiece
// 4 = jpiece
// 5 = lpiece
// 6 = ipiece
// 7 = opiece

public class PieceManager : MonoBehaviour
{
    #region Private Serialized Field or Get/Set

    [SerializeField] private PieceVisual holdPreview;
    [SerializeField] private PieceVisual nextPreview1;
    [SerializeField] private PieceVisual nextPreview2;
    [SerializeField] private PieceVisual nextPreview3;
    [SerializeField] private PieceVisual nextPreview4;
    [SerializeField] private PieceVisual nextPreview5;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject idlePoint;
    [SerializeField] private GameObject tPiece;
    [SerializeField] private GameObject sPiece;
    [SerializeField] private GameObject zPiece;
    [SerializeField] private GameObject jPiece;
    [SerializeField] private GameObject lPiece;
    [SerializeField] private GameObject iPiece;
    [SerializeField] private GameObject oPiece;
    [SerializeField] private GameObject tPieceDead;
    [SerializeField] private GameObject sPieceDead;
    [SerializeField] private GameObject zPieceDead;
    [SerializeField] private GameObject jPieceDead;
    [SerializeField] private GameObject lPieceDead;
    [SerializeField] private GameObject iPieceDead;
    [SerializeField] private GameObject oPieceDead;
    [SerializeField] private GameObject tPieceGhost;
    [SerializeField] private GameObject sPieceGhost;
    [SerializeField] private GameObject zPieceGhost;
    [SerializeField] private GameObject jPieceGhost;
    [SerializeField] private GameObject lPieceGhost;
    [SerializeField] private GameObject iPieceGhost;
    [SerializeField] private GameObject oPieceGhost;

    #endregion

    #region Private Fields

    private TPieceCollision tPieceCollisionScript;
    private SPieceCollision sPieceCollisionScript;
    private ZPieceCollision zPieceCollisionScript;
    private JPieceCollision jPieceCollisionScript;
    private LPieceCollision lPieceCollisionScript;
    private IPieceCollision iPieceCollisionScript;
    private OPieceCollision oPieceCollisionScript;
    private PieceMovement pieceMovementScript;

    private int[] firstBag;
    private int[] secondBag;
    private int currentPiece;
    private int heldPiece;
    private bool canHold;
    private bool onSecondBag;
    private int counter;

    #endregion

    #region Public Fields



    #endregion

    #region Monobehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        tPieceCollisionScript = GetComponent<TPieceCollision>();
        sPieceCollisionScript = GetComponent<SPieceCollision>();
        zPieceCollisionScript = GetComponent<ZPieceCollision>();
        jPieceCollisionScript = GetComponent<JPieceCollision>();
        lPieceCollisionScript = GetComponent<LPieceCollision>();
        iPieceCollisionScript = GetComponent<IPieceCollision>();
        oPieceCollisionScript = GetComponent<OPieceCollision>();
        pieceMovementScript = GetComponent<PieceMovement>();
        firstBag = generatePieces();
        secondBag = generatePieces();
        for (int i = 0; i < 7; i++)
        {
            Debug.Log(firstBag[i]);
        }
        Debug.Log("t");
        for (int i = 0; i < 7; i++)
        {
            Debug.Log(secondBag[i]);
        }
        heldPiece = 8;
        counter = 0;
        canHold = true;
        onSecondBag = false;
        drawNextPiece();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            holdPiece();
        }
    }

    #endregion

    #region Public Methods

    public void drawNextPiece()
    {
        if (!onSecondBag)
        {
            if (counter >= 7)
            {
                onSecondBag = true;
                counter = 0;
                currentPiece = secondBag[counter];
                firstBag = generatePieces();
                Debug.Log("Shuffled First Bag");
                counter++;
            }
            else
            {
                currentPiece = firstBag[counter];
                counter++;
            }
        }
        else if (onSecondBag)
        {
            if (counter >= 7)
            {
                onSecondBag = false;
                counter = 0;
                currentPiece = firstBag[counter];
                secondBag = generatePieces();
                Debug.Log("Shuffled second Bag");
                counter++;
            }
            else
            {
                currentPiece = secondBag[counter];
                counter++;
            }
        }
        setupCurrentPiece();
        canHold = true;
        updatePieceVisuals();
    }

    public void instantiateDeadPiece()
    {
        switch (currentPiece)
        {
            case 0:
                Instantiate(tPieceDead, tPiece.transform.position, tPiece.transform.rotation);
                break;
            case 1:
                Instantiate(sPieceDead, sPiece.transform.position, sPiece.transform.rotation);
                break;
            case 2:
                Instantiate(zPieceDead, zPiece.transform.position, zPiece.transform.rotation);
                break;
            case 3:
                Instantiate(jPieceDead, jPiece.transform.position, jPiece.transform.rotation);
                break;
            case 4:
                Instantiate(lPieceDead, lPiece.transform.position, lPiece.transform.rotation);
                break;
            case 5:
                Instantiate(iPieceDead, iPiece.transform.position, iPiece.transform.rotation);
                break;
            case 6:
                Instantiate(oPieceDead, oPiece.transform.position, oPiece.transform.rotation);
                break;
            default:
                Debug.Log("instantiateDeadPiece defaulted its switch statement...?");
                break;
        }
        drawNextPiece();
    }

    public void showGhostPiece()
    {
        switch (currentPiece)
        {
            case 0:
                tPieceGhost.transform.position = new Vector3(tPiece.transform.position.x, tPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, tPiece.transform.position.z);
                tPieceGhost.transform.rotation = tPiece.transform.rotation;
                break;
            case 1:
                sPieceGhost.transform.position = new Vector3(sPiece.transform.position.x, sPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, sPiece.transform.position.z);
                sPieceGhost.transform.rotation = sPiece.transform.rotation;
                break;
            case 2:
                zPieceGhost.transform.position = new Vector3(zPiece.transform.position.x, zPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, zPiece.transform.position.z);
                zPieceGhost.transform.rotation = zPiece.transform.rotation;
                break;
            case 3:
                jPieceGhost.transform.position = new Vector3(jPiece.transform.position.x, jPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, jPiece.transform.position.z);
                jPieceGhost.transform.rotation = jPiece.transform.rotation;
                break;
            case 4:
                lPieceGhost.transform.position = new Vector3(lPiece.transform.position.x, lPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, lPiece.transform.position.z);
                lPieceGhost.transform.rotation = lPiece.transform.rotation;
                break;
            case 5:
                iPieceGhost.transform.position = new Vector3(iPiece.transform.position.x, iPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, iPiece.transform.position.z);
                iPieceGhost.transform.rotation = iPiece.transform.rotation;
                break;
            case 6:
                oPieceGhost.transform.position = new Vector3(oPiece.transform.position.x, oPiece.transform.position.y - PieceMovement.distanceDown + 0.5f, oPiece.transform.position.z);
                oPieceGhost.transform.rotation = oPiece.transform.rotation;
                break;
            default:
                Debug.Log("showGhostPiece defaulted its switch statement...?");
                break;
        }
    }

    public void hideGhostPieces()
    {
        tPieceGhost.transform.position = idlePoint.transform.position;
        sPieceGhost.transform.position = idlePoint.transform.position;
        zPieceGhost.transform.position = idlePoint.transform.position;
        jPieceGhost.transform.position = idlePoint.transform.position;
        lPieceGhost.transform.position = idlePoint.transform.position;
        iPieceGhost.transform.position = idlePoint.transform.position;
        oPieceGhost.transform.position = idlePoint.transform.position;
    }

    #endregion

    #region Private Methods

    private void holdPiece()
    {
        if(canHold)
        {
            if (heldPiece != 8)
            {
                int temp = currentPiece;
                currentPiece = heldPiece;
                heldPiece = temp;
                canHold = false;
                setupCurrentPiece();
            }
            else
            {
                heldPiece = currentPiece;
                drawNextPiece();
                canHold = false;
            }
        }
        else
        {
            Debug.Log("Cannot hold.");
        }
        updatePieceVisuals();
    }

    // This function is pretty rad, js
    private void updatePieceVisuals()
    {
        holdPreview.pieceToShow = heldPiece;

        int[] piecesToShow = new int[5];
        int j = counter;
        for(int i = 0; i < 5; i++)
        {
            if (!onSecondBag)
            {
                if (j >= 7)
                {
                    piecesToShow[i] = secondBag[j - 7];
                }
                else
                {
                    piecesToShow[i] = firstBag[j];
                }
            }
            else if (onSecondBag)
            {
                if (j >= 7)
                {
                    piecesToShow[i] = firstBag[j - 7];
                }
                else
                {
                    piecesToShow[i] = secondBag[j];
                }
            }
            j++;
        }
        nextPreview1.pieceToShow = piecesToShow[0];
        nextPreview2.pieceToShow = piecesToShow[1];
        nextPreview3.pieceToShow = piecesToShow[2];
        nextPreview4.pieceToShow = piecesToShow[3];
        nextPreview5.pieceToShow = piecesToShow[4];
    }

    private int[] generatePieces()
    {
        // Generate a sorted bag
        PieceNodes pieces = new PieceNodes();
        for (int i = 0; i < 7; i++)
        {
            pieces.Push(i);
        }
        // Randomize the sorted bag
        int[] bag = new int[7];
        int j = 0;
        for (int i = 0; i < 7; i++)
        {
            int randomN = 0;
            randomN = (int)Random.Range(0, 7-i);
            bag[j++] = pieces.pop(randomN);
        }
        return bag;
    }

    private void setupCurrentPiece()
    {
        hideGhostPieces();
        resetPieces();
        switch (currentPiece)
        {
            case 0:
                activateTPiece();
                break;
            case 1:
                activateSPiece();
                break;
            case 2:
                activateZPiece();
                break;
            case 3:
                activateJPiece();
                break;
            case 4:
                activateLPiece();
                break;
            case 5:
                activateIPiece();
                break;
            case 6:
                activateOPiece();
                break;
            default:
                Debug.Log("setupCurrentPiece defaulted its switch statement...?");
                break;
        }
    }

    private void resetPieces()
    {
        tPiece.transform.position = idlePoint.transform.position;
        sPiece.transform.position = idlePoint.transform.position;
        zPiece.transform.position = idlePoint.transform.position;
        jPiece.transform.position = idlePoint.transform.position;
        lPiece.transform.position = idlePoint.transform.position;
        iPiece.transform.position = idlePoint.transform.position;
        oPiece.transform.position = idlePoint.transform.position;
        tPiece.transform.rotation = Quaternion.identity;
        sPiece.transform.rotation = Quaternion.identity;
        zPiece.transform.rotation = Quaternion.identity;
        jPiece.transform.rotation = Quaternion.identity;
        lPiece.transform.rotation = Quaternion.identity;
        iPiece.transform.rotation = Quaternion.identity;
        oPiece.transform.rotation = Quaternion.identity;
        tPieceCollisionScript.enabled = false;
        sPieceCollisionScript.enabled = false;
        zPieceCollisionScript.enabled = false;
        jPieceCollisionScript.enabled = false;
        lPieceCollisionScript.enabled = false;
        iPieceCollisionScript.enabled = false;
        oPieceCollisionScript.enabled = false;
    }

    private void activateTPiece()
    {
        tPiece.transform.position = spawnPoint.transform.position;
        tPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = tPiece;
    }

    private void activateSPiece()
    {
        sPiece.transform.position = spawnPoint.transform.position;
        sPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = sPiece;
    }

    private void activateZPiece()
    {
        zPiece.transform.position = spawnPoint.transform.position;
        zPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = zPiece;
    }

    private void activateJPiece()
    {
        jPiece.transform.position = spawnPoint.transform.position;
        jPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = jPiece;
    }

    private void activateLPiece()
    {
        lPiece.transform.position = spawnPoint.transform.position;
        lPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = lPiece;
    }

    private void activateIPiece()
    {
        iPiece.transform.position = new Vector3((float)(spawnPoint.transform.position.x + 0.5), (float)(spawnPoint.transform.position.y + 0.5), spawnPoint.transform.position.z);
        iPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = iPiece;
    }

    private void activateOPiece()
    {
        oPiece.transform.position = new Vector3((float)(spawnPoint.transform.position.x + 0.5), (float)(spawnPoint.transform.position.y + 0.5), spawnPoint.transform.position.z);
        oPieceCollisionScript.enabled = true;
        pieceMovementScript.currentPiece = oPiece;
    }

    #endregion
}

// I made a sort-of custom Linked List data structure to use in the
// random bag generation.
public class PieceNodes
{

    Node head;

    public class Node
    {
        public int data;
        public Node next;

        public Node(int d)
        {
            data = d;
            next = null;
        }
    }

    public void Push(int new_data)
    {
        Node new_node = new Node(new_data);
        new_node.next = head;
        head = new_node;
    }

    public int pop(int position)
    {
        Node returnNode = head;
        Node temp = head;

        if (position == 0)
        {
            head = temp.next;
            return returnNode.data;
        }

        for (int i = 0; temp != null && i < position - 1; i++)
            temp = temp.next;

        Node next = temp.next.next;
        returnNode = temp.next;
        temp.next = next;
        return returnNode.data;
    }
}