using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
	//Node Prefab
	[SerializeField]private GameObject MapNode;

	//Unit Prefab
	[SerializeField]private GameObject Unit;

	//Size of the map (x , y)
	[SerializeField]private Vector2 mapSize;
    [SerializeField]private int numberUnits;

	//List containning the rows
	public List<List<GameObject>> MapTiles = new List<List<GameObject>>();

	void Start () 
	{
        this.generateMap();
	}

    private void generateMap()
    {
        Camera.main.orthographicSize = this.mapSize[0] <= this.mapSize[1] ? this.mapSize[0] / 2 + 1 : this.mapSize[1] / 2 + 1;

        Camera.main.transform.position = new Vector3(this.mapSize[0] / 2, this.mapSize[1] / 2, -20);

        for (int x = 0; x < this.mapSize[0]; x++)
        {
            this.MapTiles.Add(new List<GameObject>());

            for (int y = 0; y < this.mapSize[1]; y++)
            {
                Object newNode = GameObject.Instantiate(MapNode, new Vector3(x, y), Quaternion.identity);
                ((GameObject)newNode).transform.parent = this.gameObject.transform;
                this.MapTiles[x].Add(((GameObject)newNode));
            }
        }

        float sizeX = this.mapSize[0] - 1;
        float sizeY = this.mapSize[1] - 1;

        for (int x = 0; x < this.mapSize[0]; x++)
        {
            for (int y = 0; y < this.mapSize[1]; y++)
            {
                Node currentNode = this.MapTiles[x][y].GetComponent<Node>();

                if(x > 0)
                {
                    currentNode.AddNeighbor(this.MapTiles[x - 1][y]);
                }
                if(y > 0)
                {
                    currentNode.AddNeighbor(this.MapTiles[x][y - 1]);
                }
                if (x < sizeX)
                {
                    currentNode.AddNeighbor(this.MapTiles[x + 1][y]);
                }
                if (y < sizeY)
                {
                    currentNode.AddNeighbor(this.MapTiles[x][y + 1]);
                }
            }
        }

        for (int i = 0; i < this.numberUnits; ++i)
        {
            GameObject unit = null;
            //Gameobject unit Instantiate
            this.placeUnit().SetOccupingObject(unit);
        }
    }

    private Node placeUnit()
    {
        int x = (int)Random.Range(0, this.mapSize[0]);
        int y = (int)Random.Range(0, this.mapSize[1]);
        Node node = this.MapTiles[x][y].GetComponent<Node>();
        if (node.IsOccupied())
        {
            node = this.placeUnit();
        }

        return node;
    }
}
