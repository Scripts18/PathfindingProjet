using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    //Best Known Path Cost Through This Node
    public float bestKnownPathCost;

    public float heuristicBestKnownPathCost;

    //Estimated Total Cost From Start To Goal Through This Node
    public float totalNodePathCost;

    public float heuristicTotalNodePathCost;

    public List<float> timeReservation = new List<float>();

    public List<Node> neighbors;
    public GameObject occupiedBy;
    public Node parent;


    public Node heuristicParent;

    public bool isUnitWaiting = false;

	void Start () 
	{
		this.gameObject.name = "(" + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ")";
	}

    public void AddNeighbor(Node _node)
    {
        this.neighbors.Add(_node);
    }

    public void AddNeighbor(GameObject _node)
    {
        this.neighbors.Add(_node.GetComponent<Node>());
    }

    public bool IsOccupied()
    {
        return this.occupiedBy != null;
    }

    public GameObject GetOccupingObject()
    {
        return this.occupiedBy;
    }

    public void SetOccupingObject(GameObject _object)
    {
        this.occupiedBy = (_object);
    }

    public void ClearParent()
    {
        this.parent = null;
    }


    public float CalculateHeuristic(Vector3 _goalPosition)
    {
        if (this.timeReservation.Contains((int)_goalPosition.x + (int)_goalPosition.y))
        {
            return -1;
        }

        return System.Math.Abs(_goalPosition.x - this.transform.position.x) + System.Math.Abs(_goalPosition.y - this.transform.position.y);
    }

    public float CalculateHeuristicReversePathFinding(Node start, Node goal)
    {
        Node currentNode;

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        openSet.Add(start);

        start.heuristicBestKnownPathCost = 0;

        start.heuristicTotalNodePathCost = start.heuristicBestKnownPathCost + start.CalculateHeuristic(goal.transform.position);

        while (openSet.Count != 0)
        {
            currentNode = findLowestTotalCost(openSet);
            
            if (currentNode == goal)
            {
                //Debug.Log("Found goal " + currentNode.transform.position);
                return calculateNumberNodes(currentNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Node neighbor in currentNode.neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tempPathCost = currentNode.heuristicBestKnownPathCost + distanceBetween(currentNode, neighbor);

                if ((!openSet.Contains(neighbor) || (tempPathCost < neighbor.heuristicBestKnownPathCost && tempPathCost != -1)) && tempPathCost != currentNode.timeReservation.BinarySearch(tempPathCost) && !currentNode.isUnitWaiting)
                {
                    neighbor.heuristicParent = currentNode;
                    neighbor.heuristicBestKnownPathCost = tempPathCost;
                    neighbor.heuristicTotalNodePathCost = neighbor.CalculateHeuristic(goal.transform.position);
					
					if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }

        }
        
        return 0;
    }

    private int calculateNumberNodes(Node currentNode)
    {
        Node parent;
        int numberNode = 0;

        while (currentNode.heuristicParent != null)
        {
            parent = currentNode;
            currentNode = currentNode.heuristicParent;
            parent.ClearParent();
            numberNode++;
        }

        return numberNode;
    }

    private Node findLowestTotalCost(List<Node> openSet)
    {
        Node templowestNodeCost = null;

        foreach (Node node in openSet)
        {
            if (templowestNodeCost == null || templowestNodeCost.heuristicTotalNodePathCost > node.heuristicTotalNodePathCost)
            {
                templowestNodeCost = node;
            }
        }

        return templowestNodeCost;
    }

    private float distanceBetween(Node startNode, Node goalNode)
    {
        return Vector3.Distance(startNode.transform.position, goalNode.transform.position);
    }
}
