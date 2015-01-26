using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    //Best Known Path Cost Through This Node
    public float bestKnownPathCost;

    //Estimated Total Cost From Start To Goal Through This Node
    public float totalNodePathCost;

    public float heuristicValue;

    public List<Node> neighbors;
    public GameObject occupiedBy;

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
        this.occupiedBy = _object;
    }


    public float CalculateHeuristic(Vector2 _goalPosition)
    {
        if (this.heuristicValue < 0)
        {
            this.heuristicValue = System.Math.Abs(_goalPosition[0] - this.transform.position[0]) + System.Math.Abs(_goalPosition[1] - this.transform.position[1]);
        }

        return this.heuristicValue;
    }

    public void ResetValuePathfinding()
    {
        this.heuristicValue = -1;
    }
}
