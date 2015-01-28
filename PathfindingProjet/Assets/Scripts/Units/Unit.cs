using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : ControlGroup 
{
	[SerializeField]private Vector3 position;
    [SerializeField]private Vector3 target;

    private Stack<Node> movementOrders = new Stack<Node>();

    //Temporary until UnitPathFinding
    [SerializeField]private GroupPathFinding pathFinding;

    //Testing Purpose 
    [SerializeField] private Map currentMap;

    public bool isMoving;

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
	void Start () 
	{
        this.movementOrders.Push(null);

        GameObject gameObjectMap = GameObject.FindGameObjectWithTag("Map");

        if (gameObjectMap != null)
        {
            this.currentMap = gameObjectMap.GetComponent<Map>();
        }

        this.moveUnitTo(new Vector3(9, 9, 0));
	}

	// Update is called once per frame
	void Update () 
	{
        // The step size is equal to speed times frame time.
        var step = 5 * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, this.target, step);
       
       if (this.movementOrders.Peek() != null && this.transform.position == this.target)
       {
           this.target = this.movementOrders.Pop().transform.position;
       }
    }

    public void ForceSetPosition(Vector3 _position)
    {
        this.target = _position;
        this.transform.position = _position;
    }
}
