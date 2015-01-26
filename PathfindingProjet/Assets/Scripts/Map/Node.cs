using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	[SerializeField]private Vector2 position;

	public Node(int x, int y)
	{
		this.position = this.transform.position;
	}

	void Start () 
	{
		this.gameObject.name = "(" + this.gameObject.transform.position.x.ToString() + ", " + this.gameObject.transform.position.y.ToString() + ")";
	}
}
