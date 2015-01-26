using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
	//Node Prefab
	[SerializeField]private GameObject MapNode;

	//Size of the map (x , y)
	[SerializeField]private Vector2 mapSize;

	//List containning the rows
	private List<List<GameObject>> MapNodes = new List<List<GameObject>>();

	void Start () 
	{
		Camera.main.orthographicSize = this.mapSize[0] <= this.mapSize[1] ? this.mapSize[0]/2 + 1 : this.mapSize[1]/2 + 1;

		Camera.main.transform.position = new Vector3(this.mapSize[0]/2, this.mapSize[1]/2, -20);

		for(int x = 0; x < this.mapSize[0]; x++)
		{
			this.MapNodes.Add(new List<GameObject>());

			for(int y = 0; y < this.mapSize[1]; y++)
			{
				Object newNode = GameObject.Instantiate(MapNode, new Vector3(x,y), Quaternion.identity);
				((GameObject) newNode).transform.parent = this.gameObject.transform;
				this.MapNodes[x].Add(((GameObject)newNode));
			}
		}
	}
}
