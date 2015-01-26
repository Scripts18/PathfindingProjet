using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

	[SerializeField]private Vector2 position;

    //Best Known Path Cost Through This Node
    public float bestKnownPathCost;

    //Estimated Total Cost From Start To Goal Through This Node
    public float totalNodePathCost;

    public List<Node> neighbors;

	public Node(int x, int y)
	{
		this.position = this.transform.position;
	}

	void Start () 
	{
        this.linkNeighbor();
		this.gameObject.name = "(" + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ")";
	}

    private void linkNeighbor()
    {

    }
}
