using UnityEngine;
using System.Collections;

public class Unit : ControlGroup 
{

	[SerializeField]private Vector2 position;
    [SerializeField]private Vector2 target;

    public override void ComputePathfinding()
    {

    }

    public override void moveToPosition(Vector2 _position)
    {
        moveUnitTo(_position);
    }
    public override void moveToPosition(int _x, int _y)
    {
        moveUnitTo(new Vector2(_x, _y));
    }
    public override void moveToPosition(GameObject _gameobject)
    {
        moveUnitTo(_gameobject.transform.position);
    }

    private void moveUnitTo(Vector2 _position)
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
	}

    public void ForceSetPosition(Vector2 _position)
    {
        this.target = _position;
        this.transform.position = _position;
    }
}
