using UnityEngine;
using System.Collections.Generic;

public class Group : ControlGroup 
{
    [SerializeField]
    private List<ControlGroup> listUnits;

    public override void ComputePathfinding()
    {

    }

    protected void calculateCenter()
    {
        Vector3 sum = new Vector3(0, 0);
        foreach (ControlGroup controlGroup in this.listUnits)
        {
            sum += controlGroup.transform.position;
        }
        this.transform.position = sum / this.listUnits.Count;
    }

    public void SetSquareFormation()
    {
        Vector3 positionInFormation = new Vector3(-1, 1, 0);
        Vector3 formationOffset = new Vector3(1, -1, 0);
        Vector3 magicMultiplicator = new Vector3(-1, -1, 0);
        foreach (ControlGroup controlGroup in this.listUnits)
        {
            controlGroup.moveToPosition(this.transform.position + positionInFormation);
            controlGroup.offsetFromCenter = positionInFormation;
            Vector3.Scale(positionInFormation, formationOffset);
            Vector3.Scale(formationOffset, magicMultiplicator);
        }
    }

    public void CancelAnyFormation()
    {
        foreach (ControlGroup controlGroup in this.listUnits)
        {       
            controlGroup.offsetFromCenter = Vector3.zero;
        }
    }

    public void AddUnit(ControlGroup _unit)
    {
        this.listUnits.Add(_unit);
    }

    public void Remove(ControlGroup _unit)
    {
        this.listUnits.Remove(_unit);
    }

    public override void moveToPosition(Vector3 _position)
    {
        moveAllUnitsTo(_position);
    }
    public override void moveToPosition(int _x, int _y)
    {
        moveAllUnitsTo(new Vector3(_x, _y));
    }
    public override void moveToPosition(GameObject _gameobject)
    {
        moveAllUnitsTo(_gameobject.transform.position);
    }

    private void moveAllUnitsTo(Vector3 _position)
    {
        foreach (ControlGroup controlGroup in this.listUnits)
        {
            controlGroup.moveToPosition(_position + controlGroup.offsetFromCenter);
        }
    }

	void Start () 
    {
        this.listUnits = new List<ControlGroup>();
	}
	
	// Update is called once per frame
	void Update () 
    {
       this.calculateCenter();
	}
}
