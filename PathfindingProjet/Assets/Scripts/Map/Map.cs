using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
	//Node Prefab
	[SerializeField]private GameObject MapNode;

	//Unit Prefab
	[SerializeField]private GameObject unit;
    [SerializeField]private GameObject group;

	//Size of the map (x , y)
	[SerializeField]private Vector3 mapSize;
    [SerializeField]private int percentageObstacles;
    [SerializeField]private int numberUnits;

	//List containning the rows
	public List<List<Node>> MapTiles = new List<List<Node>>();

	void Start () 
	{
        Application.runInBackground = true;
        this.generateMap();
	}

    public Vector3 getMapSize()
    {
        return this.mapSize;
    }

    private void generateMap()
    {
        Camera.main.orthographicSize = this.mapSize[0] <= this.mapSize[1] ? this.mapSize[0] / 2 + 1 : this.mapSize[1] / 2 + 1;
        Camera.main.transform.position = new Vector3(this.mapSize[0] / 2, this.mapSize[1] / 2, -20);

        bool isObstacle = false;

        for (int x = 0; x < this.mapSize[0]; x++)
        {
            this.MapTiles.Add(new List<Node>());

            for (int y = 0; y < this.mapSize[1]; y++)
            {
                int nodeValue = Random.Range(0, 100);
                if (nodeValue <= this.percentageObstacles)
                {
                    isObstacle = true;
                }
                else
                {
                    isObstacle = false;
                }

                Node newNode = ((GameObject)GameObject.Instantiate(MapNode, new Vector3(x, y), Quaternion.identity)).GetComponent<Node>();
                newNode.gameObject.transform.parent = this.gameObject.transform;
                //newNode.setObstacle(isObstacle);
                this.MapTiles[x].Add((newNode));
            }
        }

        float sizeX = this.mapSize[0] - 1;
        float sizeY = this.mapSize[1] - 1;

        for (int x = 0; x < this.mapSize[0]; x++)
        {
            for (int y = 0; y < this.mapSize[1]; y++)
            {
                if(x > 0)
                {
                    this.MapTiles[x][y].AddNeighbor(this.MapTiles[x - 1][y]);
                }
                if(y > 0)
                {
                    this.MapTiles[x][y].AddNeighbor(this.MapTiles[x][y - 1]);
                }
                if (x < sizeX)
                {
                    this.MapTiles[x][y].AddNeighbor(this.MapTiles[x + 1][y]);
                }
                if (y < sizeY)
                {
                    this.MapTiles[x][y].AddNeighbor(this.MapTiles[x][y + 1]);
                }
            }
        }

        Group groupOne = ((GameObject)GameObject.Instantiate(this.group, Vector3.zero, Quaternion.identity)).GetComponent<Group>();

        for (int i = 0; i < this.numberUnits; ++i)
        {
			GameObject newUnit = (GameObject)GameObject.Instantiate(this.unit, Vector3.zero, Quaternion.identity);
            Unit newUnitComponent = newUnit.GetComponent<Unit>();

            this.placeUnit(newUnitComponent).SetOccupingObject(newUnit);
            newUnitComponent.SetMap(this);
            groupOne.AddUnit(newUnitComponent);
            newUnitComponent.moveToPosition(Random.Range(0, (int)this.mapSize.x), Random.Range(0, (int)this.mapSize.y));
        }

<<<<<<< HEAD
        //groupOne.SetCircleFormation(15);
=======
        groupOne.SetCircleFormation();
>>>>>>> origin/master
        //groupOne.SetLineFormation(false);
        //groupOne.SetSquareFormation(3);
        //groupOne.moveToPosition(10, 8);
    }

    private Node placeUnit(Unit newUnit)
    {
        int x = Random.Range(0, (int)this.mapSize[0]);
        int y = Random.Range(0, (int)this.mapSize[1]);

        Node node = this.MapTiles[x][y].GetComponent<Node>();

        if (node.IsOccupied())
        {
           node = this.placeUnit(newUnit);
        }
        else
        {
            newUnit.ForceSetPosition(node.transform.position);
        }

        return node;
    }
}
