using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
	[SerializeField]InputField numberGroupsField;
	[SerializeField]InputField numberUnitsField;

	//Node Prefab
	[SerializeField]private GameObject MapNode;

	//Unit Prefab
	[SerializeField]private GameObject unit;
    [SerializeField]private GameObject group;

	//Size of the map (x , y)
	[SerializeField]private Vector3 mapSize;
    [SerializeField]private int percentageObstacles;


    [SerializeField]private int numberUnits;
	[SerializeField]private int numberGroups;

	private List<Group> groups = new List<Group> ();
	private List<Unit> units = new List<Unit> ();

	//List containning the rows
	public List<List<Node>> MapTiles = new List<List<Node>>();

	public static List<GameObject> gameObjects = new List<GameObject>();

	void Start () 
	{
        Application.runInBackground = true;

		this.numberUnits = Random.Range (7, 12);
		this.numberGroups = Random.Range (1, 3);
		this.percentageObstacles = Random.Range (5, 20);

        this.generateMap();
	}

	public void Reset()
	{
		if (this.numberGroupsField.text.Length > 0 && int.Parse(this.numberGroupsField.text) > 0) 
		{
			this.numberGroups = int.Parse(this.numberGroupsField.text);
		}
		else
		{
			this.numberGroups = Random.Range (1, 3);
		}
		
		if (this.numberUnitsField.text.Length > 0 && int.Parse(this.numberUnitsField.text) > 0)
		{
			this.numberUnits = int.Parse(this.numberUnitsField.text);
		}
		else
		{
			this.numberUnits = Random.Range (7, 12);
		}


		MapTiles.Clear ();
		groups.Clear ();
		units.Clear ();

		foreach (GameObject destroyIt in gameObjects) 
		{
			Destroy(destroyIt);
		}

		this.generateMap ();
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
                newNode.setAsObstacle(isObstacle);
                newNode.gameObject.transform.parent = this.gameObject.transform;
                //newNode.setObstacle(isObstacle);
                this.MapTiles[x].Add((newNode));
				gameObjects.Add(newNode.gameObject);
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



		for(int i = 0; i < this.numberGroups; i++)
		{
			this.groups.Add(((GameObject)GameObject.Instantiate(this.group, Vector3.zero, Quaternion.identity)).GetComponent<Group>());
		}

		foreach (Group tempGroup in this.groups) 
		{
			tempGroup.SetMap(this);

			for (int i = 0; i < this.numberUnits; ++i)
			{
				GameObject newUnit = (GameObject)GameObject.Instantiate(this.unit, Vector3.zero, Quaternion.identity);
				Unit newUnitComponent = newUnit.GetComponent<Unit>();
				
				this.placeUnit(newUnitComponent).SetOccupingObject(newUnit);
				newUnitComponent.SetMap(this);
				tempGroup.AddUnit(newUnitComponent);
				this.units.Add(newUnitComponent);
				gameObjects.Add(newUnit);
			}

			gameObjects.Add(tempGroup.gameObject);

			tempGroup.SetCircleFormation();
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
			node.timeReservation.Add(new Vector2(0, Mathf.Infinity));
            newUnit.ForceSetPosition(node.transform.position);
        }

        return node;
    }
}
