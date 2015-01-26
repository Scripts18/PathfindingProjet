using UnityEngine;
using System.Collections.Generic;

public class Group : ControlGroup 
{
    [SerializeField]
    private List<ControlGroup> listUnits;

    public override void ComputePathfinding()
    {

    }

    public void AddUnit(ControlGroup _unit)
    {
        this.listUnits.Add(_unit);
    }

    public void Remove(ControlGroup _unit)
    {
        this.listUnits.Remove(_unit);
    }

    public override void moveToPosition(Vector2 _position)
    {
        moveAllUnitsTo(_position);
    }
    public override void moveToPosition(int _x, int _y)
    {
        moveAllUnitsTo(new Vector2(_x, _y));
    }
    public override void moveToPosition(GameObject _gameobject)
    {
        moveAllUnitsTo(_gameobject.transform.position);
    }

    private void moveAllUnitsTo(Vector2 _position)
    {
        foreach (ControlGroup controlGroup in this.listUnits)
        {
            controlGroup.moveToPosition(_position);
        }
    }

	void Start () 
    {
        this.listUnits = new List<ControlGroup>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
