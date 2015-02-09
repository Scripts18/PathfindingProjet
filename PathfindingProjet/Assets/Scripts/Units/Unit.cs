using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : ControlGroup 
{
	[SerializeField]private Vector3 position;
    [SerializeField]private Vector3 target;

    float step = 5 * Time.deltaTime;

    private Stack<Node> movementOrders = new Stack<Node>();

    //Temporary until UnitPathFinding
    [SerializeField]private GroupPathFinding pathFinding;

    //Testing Purpose 
    [SerializeField] private Map currentMap;

    public bool isMoving = false;
    private bool doNextMovement = true;

    private Node lastMovement;

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
            Debug.Log("New position at : " + newPosition.transform.position.ToString() + " from : " + _position.ToString());
            _position = newPosition.transform.position;
            
        }
		
        this.moveUnitTo(_position);
    }

	private Node getNewPosition(int _depth, Node _origin)
	{
        _origin.depth = _depth;
        if (_depth > 5)
        {
            return _origin;
        }
        

        Node newPosition = _origin;
        Node tempNode = null;

        if (!_origin.IsObstacle() && !_origin.IsOccupied())
        {
            Debug.Log("I'm a chewchew");
            return newPosition;

        }
        else
        {
            Debug.Log("I'm a gummy bear");
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
        this.currentMap.MapTiles[(int)this.transform.position.x][(int)this.transform.position.y].GetComponent<Node>().isUnitWaiting = false;
        this.movementOrders = this.pathFinding.PathFinding(this.currentMap.MapTiles[(int)this.transform.position.x][(int)this.transform.position.y].GetComponent<Node>(), this.currentMap.MapTiles[(int)_position.x][(int)_position.y].GetComponent<Node>());

        this.doNextMovement = true;
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
        
        if (this.movementOrders.Count - 1 > 0)
        {
            this.lastMovement.SetOccupingObject(null);
            this.target = this.movementOrders.Pop().transform.position;
            this.doNextMovement = true; 
        }
        else
        {
            this.doNextMovement = false;
            this.currentMap.MapTiles[(int)this.transform.position.x][(int)this.transform.position.y].GetComponent<Node>().isUnitWaiting = true;
        }
    }

    public void cancelMovements()
    {
        while (this.movementOrders.Count - 1 != 0)
        {
            this.movementOrders.Pop();
        }
    }

    public void changePath(Vector3 newOrder, bool queueMovement)
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

        if (queueMovement)
        {
            this.cancelMovements();
        }

        for(int i = 0; i < newPath.Count - 1; i++)
        {
            concatStack.Push(newPath.Pop());
        }

        for (int i = 0; i < concatStack.Count - 1; i++)
        {
            this.movementOrders.Push(concatStack.Pop());
        }  
    }

    public void ForceSetPosition(Vector3 _position)
    {
        this.target = _position;
        this.transform.position = _position;
    }
}
