//ASTAR SCRIPT SCRIPT
//Use: AStar method
//Created By: Iain Farlow
//Created On: 10/04/2021
//Last Edited: 16/04/2021
//Edited By: Iain Farlow
//Due to time restriction this was an attempt to quickly convert a c++ AStar method I had previously written into c#
//Was abandoned due to time restictions and is NOT in the current build of the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Node info
public class Node
{
    public bool obstacle = false;
    public bool visited = false;
    public float globalGoal = 0.0f;
    public float localGoal = 0.0f;
    public int x = 0;
    public int y = 0;
    public List<Node> vecNeighbours = new List<Node>();
    public Node parent = null;
};

public class ASTAR
{
    Node[] m_nodes = null;
    Node m_startNode = null;
    Node m_endNode = null;

    public bool Initilise(char[] a_mapArray, int a_scannedDist)
    {
        //create array based on dimentios of cube face
        int dimentions = (a_scannedDist * 2) + 1;
        m_nodes = new Node[dimentions * dimentions];

        //Fill the array
        for (int i = 0; i < (dimentions * dimentions); i++)
        {
            m_nodes[i] = new Node();
        }

        for (int x = 0; x < dimentions; x++)
        {
            for (int y = 0; y < dimentions; y++)
            {
                //set node postions
                m_nodes[y * dimentions + x].x = x;
                m_nodes[y * dimentions + x].y = y;
                //set parents and visted to base state
                m_nodes[y * dimentions + x].parent = null;
                m_nodes[y * dimentions + x].visited = false;

                //get point from map
                char pointType = a_mapArray[y * dimentions + x];
                //Assign data based on points char value
                switch (pointType)
                {
                    case 'x':
                        {
                            //node is obsticle
                            m_nodes[y * dimentions + x].obstacle = true;
                            break;
                        }
                    case '.':
                        {
                            //node not obsticle
                            m_nodes[y * dimentions + x].obstacle = false;
                            break;
                        }
                    case 'A':
                        {
                            //start node
                            m_nodes[y * dimentions + x].obstacle = false;
                            m_startNode = m_nodes[y * dimentions + x];
                            break;
                        }
                    case 'B':
                        {
                            //end node
                            m_nodes[y * dimentions + x].obstacle = false;
                            m_endNode = m_nodes[y * dimentions + x];
                            break;
                        }
                    default:
                        {
                            //Unkown Type Error
                            return false;
                        }
                }
            }
        }
        //Check for start and end node
        if (m_startNode == null || m_endNode == null)
        {
            //error
            return false;
        }
        //iterate though map dimensions 
        for (int x = 0; x < dimentions; x++)
        {
            for (int y = 0; y < dimentions; y++)
            {
                //Location of point (for edges)
                //Dependant on the edge it is on get relevant neighbours
                //If on no edge get all nesw neightbours
                //Example. If on north edge, only get esw neighbours
                if (y > 0)
                {
                    //get relevant node from coordinates, push to the neightbours vector neighbour node
                    m_nodes[y * dimentions + x].vecNeighbours.Add(m_nodes[(y - 1) * dimentions + x]);
                }
                if (y < dimentions)
                {
                    m_nodes[y * dimentions + x].vecNeighbours.Add(m_nodes[(y + 1) * dimentions + x]);
                }
                if (x > 0)
                {
                    m_nodes[y * dimentions + x].vecNeighbours.Add(m_nodes[y * dimentions + (x - 1)]);
                }
                if (x < dimentions)
                {
                    m_nodes[y * dimentions + x].vecNeighbours.Add(m_nodes[y * dimentions + (x + 1)]);
                }
            }
        }
        //Initlisation success
        return true;
    }

    public void Solve(int a_scannedDist)
    {
        int dimentions = (a_scannedDist * 2) + 1;
        //iterate though nodes
        for (int x = 0; x < dimentions; x++)
        {
            for (int y = 0; y < dimentions; y++)
            {
                //set nodes to base workable value
                m_nodes[y * dimentions + x].visited = false;
                m_nodes[y * dimentions + x].globalGoal = float.PositiveInfinity;
                m_nodes[y * dimentions + x].localGoal = float.PositiveInfinity;
                m_nodes[y * dimentions + x].parent = null;
            }
        }

        //set start conditions 
        Node currentNode = m_startNode;
        //get distance between start and end
        m_startNode.globalGoal = distance(m_startNode, m_endNode);
        m_startNode.localGoal = 0.0f;

        //create list of nodes to test
        List<Node> nonTestedNodes = new List<Node>();
        nonTestedNodes.Add(m_startNode);

        //while there are nodes to test
        while (nonTestedNodes != null)
        {
            //sort the nodes based on their global goal (distance to end)
            nonTestedNodes.Sort((lhs, rhs) => lhs.globalGoal.CompareTo(rhs.globalGoal));

            //get rid of tested nodes
            while (nonTestedNodes != null && nonTestedNodes[0].visited)
            {
                nonTestedNodes[0] = null;
            }

            //if list of nodes is empty break
            if (nonTestedNodes == null)
            {
                break;
            }

            //get the current node based on first node in the nodes to test
            currentNode = nonTestedNodes[0];
            //set current to visted
            currentNode.visited = true;

            //for each neightbour the current node has
            foreach (Node neighbouringNode in currentNode.vecNeighbours)
            {
                if (!neighbouringNode.visited && !neighbouringNode.obstacle)
                {
                    //add any non tested, relevant neighbours to the nodes to test vector
                    nonTestedNodes.Add(neighbouringNode);
                }

                //get possiblyLowerGoal from the current nodes local and distance between itself and the neighbour 
                float possiblyLowerGoal = currentNode.localGoal + distance(currentNode, neighbouringNode);

                //if the possiblyLowerGoal is less than neightbours local
                if (possiblyLowerGoal < neighbouringNode.localGoal)
                {
                    //set new current and localgoal based on neightbour
                    neighbouringNode.parent = currentNode;
                    neighbouringNode.localGoal = possiblyLowerGoal;

                    //get neightbours global gloat using the distance from it to end node
                    neighbouringNode.globalGoal = neighbouringNode.localGoal + distance(neighbouringNode, m_endNode);
                }
            }
        }
    }

    float distance(Node a, Node b)
    {
        //use pythag to get direct distnace to end
        return (Mathf.Sqrt((float)((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y))));
    }

    public Node GetEndNode() { return m_endNode; }
}
