using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : ControlGroup 
{

	[SerializeField]private Vector3 position;
    [SerializeField]private Vector3 target;

    [SerializeField]private Stack<Node> movementOrders = new Stack<Node>();

    public override void ComputePathfinding()
    {

    }

    protected override void calculateCenter()
    {
        this.center = this.transform.position;
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
        this.target = _position;
    }

	// Use this for initialization
	void Start () 
	{
        
	}

	// Update is called once per frame
	void Update () 
	{
        // The step size is equal to speed times frame time.
        var step = 5 * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, this.target, step);
       
        if (this.transform.position == this.target)
        {
            if (this.movementOrders.Peek() != null)
            {
                this.target = this.movementOrders.Pop().transform.position;
            }
        }
    }

    public void ForceSetPosition(Vector3 _position)
    {
        this.target = _position;
        this.transform.position = _position;
    }
}
