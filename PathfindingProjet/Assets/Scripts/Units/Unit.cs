using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : ControlGroup 
{
	[SerializeField]private Vector3 position;
    [SerializeField]private Vector3 target;

    [SerializeField]private float step = 5 * Time.deltaTime;

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
        moveUnitTo(_position);
    }
    public override void moveToPosition(int _x, int _y)
    {
        moveUnitTo(new Vector3(_x, _y));
    }
    public override void moveToPosition(GameObject _gameobject)
    {
        moveUnitTo(_gameobject.transform.position);
    }

    private void moveUnitTo(Vector3 _position)
    {
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
           this.movementDone(); 
       }
       else if (this.doNextMovement)
       {
           this.lastMovement = this.currentMap.MapTiles[(int)this.target.x][(int)this.target.y];
           this.startMoving();
       }

       if (this.isMoving == false)
       {
           this.currentMap.MapTiles[(int)this.target.x][(int)this.target.y].SetOccupingObject(this.gameObject);
       }
    }

    private void startMoving()
    {
        this.isMoving = true;
    }

    private void movementDone()
    {
        this.isMoving = false;
        
        if (this.movementOrders.Count - 1 != 0)
        {
            this.lastMovement.SetOccupingObject(null);
            this.target = this.movementOrders.Pop().transform.position;
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
