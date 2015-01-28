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
        this.movementOrders.Push(null);

        GameObject gameObjectMap = GameObject.FindGameObjectWithTag("Map");

        if (gameObjectMap != null)
        {
            this.currentMap = gameObjectMap.GetComponent<Map>();
        }

        //this.moveUnitTo(new Vector3(9, 9, 0));
	}

	// Update is called once per frame
	void Update () 
	{
        // Move our position a step closer to the target.
        this.transform.position = Vector3.MoveTowards(transform.position, this.target, this.step);

       if (this.transform.position == this.target && this.isMoving)
       {
           this.movementDone();
       }
       else if (this.doNextMovement)
       {
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

        if (this.movementOrders.Peek() != null)
        {
            this.target = this.movementOrders.Pop().transform.position;
            this.doNextMovement = true;
        }
    }

    private void cancelMovements()
    {
        while (this.movementOrders.Peek() != null)
        {
            this.movementOrders.Pop();
        }
    }

    private void changePath(Vector3 newOrder, bool queueMovement)
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
