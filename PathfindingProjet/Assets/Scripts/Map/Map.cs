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

        this.percentageObstacles = Random.Range(0, (int)30);

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
                newNode.setAsObstacle(isObstacle);
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

        float mapPercentageLimit =(float)  (this.mapSize.x * this.mapSize.y * 0.2);

        List<Node> listNodeChecked = new List<Node>();
        foreach (List<Node> row in this.MapTiles)
        {
            foreach (Node node in row)
            {
                if (!node.IsObstacle())
                {
                    listNodeChecked = this.getLinkedTiles(node, listNodeChecked);
                    if (listNodeChecked.Count <= mapPercentageLimit)
                    {
                        foreach (Node nodeToChange in listNodeChecked)
                        {
                            nodeToChange.setAsObstacle(true);
                        }
                    }
                }
                listNodeChecked.Clear();
            }
        }

        List<Group> listGroups = new List<Group>();
        int numberGroup = Random.Range(2, (int)4);
        int numberUnitsRand = Random.Range(4, (int)10);

        for (int j = 0; j < numberGroup; ++j)
        {
            Group groupOne = ((GameObject)GameObject.Instantiate(this.group, Vector3.zero, Quaternion.identity)).GetComponent<Group>();
            groupOne.SetMap(this);

            for (int i = 0; i < numberUnitsRand; ++i)
            {
			    GameObject newUnit = (GameObject)GameObject.Instantiate(this.unit, Vector3.zero, Quaternion.identity);
                Unit newUnitComponent = newUnit.GetComponent<Unit>();

                this.placeUnit(newUnitComponent).SetOccupingObject(newUnit);
                newUnitComponent.SetMap(this);
                groupOne.AddUnit(newUnitComponent);
                //newUnitComponent.moveToPosition(Random.Range(0, (int)this.mapSize.x), Random.Range(0, (int)this.mapSize.y));
            }
		
            groupOne.SetCircleFormation();
            //groupOne.SetLineFormation(false);
            //groupOne.SetSquareFormation(3);
            //groupOne.moveToPosition(2 + Random.Range(1, (int)10), 2 + Random.Range(1, (int)10));
            listGroups.Add(groupOne);
        }
    }

    private List<Node> getLinkedTiles(Node _origin, List<Node> _listNodes)
    {
        _listNodes.Add(_origin);

        foreach (Node neighbor in _origin.neighbors)
        {
            if (!_listNodes.Contains(neighbor) && !neighbor.IsObstacle())
            {
                _listNodes = getLinkedTiles(neighbor, _listNodes);
            }
        }

        return _listNodes;
    }

    private Node placeUnit(Unit newUnit)
    {
        int x = Random.Range(0, (int)this.mapSize[0]);
        int y = Random.Range(0, (int)this.mapSize[1]);

        Node node = this.MapTiles[x][y].GetComponent<Node>();

        if (node.IsOccupied() || node.IsObstacle())
        {
           node = this.placeUnit(newUnit);
        }
        else
        {
			node.isReserved = true;
            newUnit.ForceSetPosition(node.transform.position);
        }

        return node;
    }
}
