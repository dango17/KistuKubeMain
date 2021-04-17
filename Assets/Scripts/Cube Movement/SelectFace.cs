//Daniel Oldham 
//S1903729
//13/11/20
//Manages what face has been selected by the player, also tracks the 
//players current turns remaining until this script disables itself

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFace : MonoBehaviour
{
    public static SelectFace instance;

    private CubeState cubeState;
    private ReadCube readCube;
    private int layerMask = 1 << 8;

    //face rotate check variables
    public Text TurnsLeftValue;
    public int currentRotations = 3;
    private int faceRotated = 1;

    [Header("Challenge 1 icons")]
    //Challenge Icon ints 
    public GameObject Challenge1True;
    public GameObject Challenge1False;

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
        TurnsLeftValue.text = currentRotations.ToString() + "";

        //Challenge notifications
        //This challenge will start true, will go false once player runs out of turns
        Challenge1True.GetComponent<RawImage>().enabled = true;
        Challenge1False.GetComponent<RawImage>().enabled = false;
        GameObject.Find("LevelController").GetComponent<ChallengeManager>().Challenge1Complete(true);

    }

    //Update is called once per frame
    void Update()
    {
        instance = this; 

        if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Began && !CubeState.autoRotating)
        {            
            // read the current state of the cube            
            readCube.ReadState();
            // raycast from the mouse click/finger pos towards the cube to see if a face is hit  
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                //Face selection detected, remove a turn off the current turn value;
                currentRotations -= faceRotated;
                //Update Flips Value to UI
                TurnsLeftValue.text = currentRotations.ToString() + "";
                //Ran out of turns, turn off script to stop face rotations &
                //disable the additional challenge in pause menu, player didnt do it
                if (currentRotations < 1)
                {
                    this.enabled = false;
                    Challenge1True.GetComponent<RawImage>().enabled = false;
                    Challenge1False.GetComponent<RawImage>().enabled = true;
                    GameObject.Find("LevelController").GetComponent<ChallengeManager>().Challenge1Complete(true);

                }

                GameObject face = hit.collider.gameObject;
                // Make a list of all the sides (lists of face GameObjects)
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };
                // If the face hit exists within a side
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        //Pick it up
                        cubeState.PickUp(cubeSide);
                        //start the side rotation logic
                        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);                                            
                    }
                }
            }
        }
    }
}
