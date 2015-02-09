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

    public bool isObstacle = false;


    public Node heuristicParent;

    public bool isUnitWaiting = false;

    public int depth = 0;

	void Start () 
	{
		this.gameObject.name = "(" + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ")";
	}

    public void AddNeighbor(Node _node)
    {
        this.neighbors.Add(_node);
    }

    public bool IsObstacle()
    {
        return this.isObstacle;
    }

    public void setAsObstacle(bool _isObstacle)
    {
        this.isObstacle = _isObstacle;
        if (this.isObstacle)
        {
			this.transform.GetComponent<SpriteRenderer>().color = new Color(90, 55, 5);
        }
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

	public static float distanceBetween(Node startNode, Node goalNode)
    {
        return Vector3.Distance(startNode.transform.position, goalNode.transform.position);
    }
}
