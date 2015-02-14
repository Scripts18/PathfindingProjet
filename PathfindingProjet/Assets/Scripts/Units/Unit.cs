using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : ControlGroup 
{
	[SerializeField]private Vector3 position;
    [SerializeField]private Vector3 target;

    float step = 1 * Time.deltaTime;

    private Stack<Node> movementOrders = new Stack<Node>();

    //Temporary until UnitPathFinding
    [SerializeField]private GroupPathFinding pathFinding;

    //Testing Purpose 
    [SerializeField] private Map currentMap;

    public bool isMoving = false;
    private bool doNextMovement = true;

    private Node lastMovement;

    private Color unitColor;
    [SerializeField]private GameObject dot;


    public override void ComputePathfinding()
    {

    }

    public override void moveToPosition(Vector3 _position)
    {
        validateOrder(_position);
    }
    public override void moveToPosition(int _x, int _y)
    {
        validateOrder(new Vector3(_x, _y));
    }
    public override void moveToPosition(GameObject _gameobject)
    {
        validateOrder(_gameobject.transform.position);
    }

    private void validateOrder(Vector3 _position)
    {
        if (_position.x < 0)
        {
            _position.Set(0, _position.y, 0);
        }
        else if (_position.x > (this.currentMap.getMapSize().x - 1))
        {
            _position.Set((this.currentMap.getMapSize().x - 1), _position.y, 0);
        }
        if (_position.y < 0)
        {
            _position.Set(_position.x, 0, 0);
        }
        else if (_position.y > (this.currentMap.getMapSize().y - 1))
        {
            _position.Set(_position.x, (this.currentMap.getMapSize().y - 1), 0);
        }

		Node checkedNode = this.currentMap.MapTiles [(int)_position.x] [(int)_position.y].GetComponent<Node>();
        Node newPosition = this.getNewPosition(0, checkedNode);

        if (newPosition != null)
        {
            _position = newPosition.transform.position;
            
        }
		
        this.moveUnitTo(_position);
    }

	private Node getNewPosition(int _depth, Node _origin)
	{
        _origin.depth = _depth;
        if (_depth > 3)
        {
            return _origin;
        }
        

        Node newPosition = _origin;
        Node tempNode = null;

        if (!_origin.IsObstacle())
        {
            return newPosition;
        }
        else
        {
            newPosition.depth += 10000;
        }

        foreach (Node neighbor in _origin.neighbors)
        {
            tempNode = this.getNewPosition((_depth + 1), neighbor);
            if (tempNode.depth < newPosition.depth)
            {
                newPosition = tempNode;
            }
        }

        return newPosition;
	}

    private void moveUnitTo(Vector3 _position)
    {
        this.movementOrders = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)this.transform.position.x][(int)this.transform.position.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)_position.x][(int)_position.y].GetComponent<Node>());

        if (this.movementOrders != null && this.movementOrders.Count - 1 > 0)
		{
			this.doNextMovement = true;
		} 
    }

	// Use this for initialization
    public void SetMap(Map _map)
    {
        if (_map != null)
        {
            this.currentMap = _map;
        }
    }

	void Start () 
	{
        if (this.currentMap == null)
        {
            GameObject gameObjectMap = GameObject.FindGameObjectWithTag("Map");
            this.currentMap = gameObjectMap.GetComponent<Map>();
        }
        this.unitColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
	}

	// Update is called once per frame
	void Update () 
	{
        // Move our position a step closer to the target.
        if (this.isMoving)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, this.target, this.step);
        }

        if (this.transform.position == this.target && this.isMoving)
        {
           this.currentMap.MapTiles[(int)this.target.x][(int)this.target.y].SetOccupingObject(this.gameObject);
           GameObject newDot = (GameObject) GameObject.Instantiate(this.dot, new Vector3(this.transform.position.x, this.transform.position.y, -5), Quaternion.identity);
           newDot.GetComponent<SpriteRenderer>().color = this.unitColor;
            
           this.movementDone(); 
        }
        else if (this.doNextMovement)
        {
             this.lastMovement = this.currentMap.MapTiles[(int)this.target.x][(int)this.target.y];
             this.startMoving();
        }
    }

    private void startMoving()
    {
        this.isMoving = true;
    }

    private void movementDone()
    {
        this.isMoving = false;

        if (this.movementOrders != null && this.movementOrders.Count > 0)
        {
            this.lastMovement.SetOccupingObject(null);
            Node targetNode = this.movementOrders.Pop();

            if (targetNode != null)
            {
                this.target = targetNode.transform.position;
            }
            else
            {
                Debug.Log("NPE with " + this.movementOrders.Count + " moves left.");
            }
            this.doNextMovement = true; 
        }
        else
        {
            this.doNextMovement = false;
        }
    }

    public void cancelMovements()
    {
        while (this.movementOrders.Count - 1 != 0)
        {
            this.movementOrders.Pop();
        }
    }

    public override void changePath(Vector3 newOrder)
    {
        Stack<Node> newPath;

        if (this.isMoving)
        {
            newPath = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)this.target.x][(int)this.target.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)newOrder.x][(int)newOrder.y].GetComponent<Node>());
        }
        else
        {
            newPath = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)this.transform.position.x][(int)this.transform.position.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)newOrder.x][(int)newOrder.y].GetComponent<Node>());
        }

        Stack<Node> concatStack = new Stack<Node>();

        this.cancelMovements();

        for(int i = 0; i < newPath.Count - 1; i++)
        {
            concatStack.Push(newPath.Pop());
        }

        for (int i = 0; i < concatStack.Count - 1; i++)
        {
            this.movementOrders.Push(concatStack.Pop());
        }  
    }

    public override void queuePath(Vector3 _newOrder)
    {
        //var elem = result.ElementAt(1);
        Stack<Node> newPath;
        if (this.movementOrders != null)
        {

            int count = this.movementOrders.Count;
            List<Node> node = new List<Node>();
            node.AddRange(this.movementOrders.ToArray());

            newPath = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)node[node.Count - 2].transform.position.x][(int)node[node.Count - 2].transform.position.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)_newOrder.x][(int)_newOrder.y].GetComponent<Node>());

            Debug.Log("Start point : " + newPath.Peek().transform.position);

            if (newPath == null)
            {
                return;
            }

            Stack<Node> concatStack = new Stack<Node>();

            for (int i = 0; i < count - 1; i++)
            {
                Node tempNode = this.movementOrders.Pop();
                if (tempNode != null)
                {

                    concatStack.Push(tempNode);
                }
            }
            Debug.Log("End point : " + concatStack.Peek().transform.position);

            int concatCount = concatStack.Count;

            for (int i = 0; i < concatCount; i++)
            {
                Node tempNode = concatStack.Pop();
                newPath.Push(tempNode);
            }
        }
        else
        {
            newPath = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)this.target.x][(int)this.target.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)_newOrder.x][(int)_newOrder.y].GetComponent<Node>());
        }
        
        this.movementOrders = newPath;
    }

    public void ForceSetPosition(Vector3 _position)
    {
        this.target = _position;
        this.transform.position = _position;
    }
}
