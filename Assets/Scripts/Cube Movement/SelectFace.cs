using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SelectFace : MonoBehaviour
{
    private CubeState cubeState;
    private ReadCube readCube;
    private int layerMask = 1 << 8;

    //face rotate check variables
    public Text TurnsLeftValue; 
    public int currentRotations = 3;
    private int faceRotated = 1; 
    
    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
        TurnsLeftValue.text = currentRotations.ToString() + "";
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        { 
            //Face selection detected, remove a turn off current value;
            currentRotations -= faceRotated;
            //Update Flips Value to UI
            TurnsLeftValue.text = currentRotations.ToString() + "";
            //Ran out of turns, turn off script to stop face rotations
            if (currentRotations < 1)
            {
                this.enabled = false; 
            }

            // read the current state of the cube            
            readCube.ReadState();

            // raycast from the mouse towards the cube to see if a face is hit  
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
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
