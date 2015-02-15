using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    //Best Known Path Cost Through This Node
    public float actualCostInitialState;

    public float heuristicActualCostInitialState;

    //Estimated Total Cost From Start To Goal Through This Node
    public float estimatedTotalNodePathCost;

    public float heuristicEstimatedTotalNodePathCost;

    public List<Vector2> timeReservation = new List<Vector2>();

    public List<Node> neighbors;
    public GameObject occupiedBy;
    public Node parent;

    public bool isObstacle = false;
	
    public Node heuristicParent;

    public int depth = 0;

	/// <author>Marc-André Larochelle</author>
	void Start () 
	{
		this.gameObject.name = "(" + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ")";
	}

	/// <author>Guillaume Morin</author>
    public void AddNeighbor(Node _node)
    {
        this.neighbors.Add(_node);
    }

	/// <author>Guillaume Morin</author>
    public bool IsObstacle()
    {
        return this.isObstacle;
    }

	/// <author>Guillaume Morin</author>
    public void setAsObstacle(bool _isObstacle)
    {
        this.isObstacle = _isObstacle;
        if (this.isObstacle)
        {
			this.transform.GetComponent<SpriteRenderer>().color = new Color(90, 55, 5);
        }
    }

	/// <author>Guillaume Morin</author>
    public void AddNeighbor(GameObject _node)
    {
        this.neighbors.Add(_node.GetComponent<Node>());
    }

	/// <author>Guillaume Morin</author>
    public bool IsOccupied()
    {
        return this.occupiedBy != null;
    }

	/// <author>Guillaume Morin</author>
    public GameObject GetOccupingObject()
    {
        return this.occupiedBy;
    }

	/// <author>Guillaume Morin</author>
    public void SetOccupingObject(GameObject _object)
    {
        this.occupiedBy = (_object);
    }

	/// <author>Guillaume Morin</author>
    public void ClearParent()
    {
        this.parent = null;
    }

	/// <author>Guillaume Morin</author>
    public float CalculateHeuristic(Vector3 _goalPosition)
    {
        return System.Math.Abs(_goalPosition.x - this.transform.position.x) + System.Math.Abs(_goalPosition.y - this.transform.position.y);
    }

	/// <author>Guillaume Morin</author>
	public static float distanceBetween(Node startNode, Node goalNode)
    {
        return Vector3.Distance(startNode.transform.position, goalNode.transform.position);
    }
}
