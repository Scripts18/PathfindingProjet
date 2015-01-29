using UnityEngine;
using System.Collections.Generic;

public class Group : ControlGroup 
{
    [SerializeField]private List<ControlGroup> listUnits = new List<ControlGroup>();

    public override void ComputePathfinding()
    {

    }

    private void calculateCenter()
    {
        Vector3 sum = new Vector3(0, 0, 0);

        foreach (ControlGroup controlGroup in this.listUnits)
        {
            sum += controlGroup.transform.position;
        }

        if (this.listUnits.Count > 0)
        {
            Vector3 center = sum / this.listUnits.Count;
            this.transform.position = new Vector3((int)center.x, (int)center.y, 0);
        }
    }

    public void SetSquareFormation()
    {
        this.calculateCenter();

        Vector3 positionInFormation = new Vector3(-1, 1, 0);
        Vector3 formationOffset = new Vector3(1, -1, 0);
        Vector3 magicMultiplicator = new Vector3(-1, -1, 0);

        foreach (ControlGroup controlGroup in this.listUnits)
        {
            controlGroup.moveToPosition((this.transform.position + positionInFormation));
            controlGroup.offsetFromCenter = positionInFormation;
            positionInFormation = Vector3.Scale(positionInFormation, formationOffset);
            formationOffset = Vector3.Scale(formationOffset, magicMultiplicator);
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
	
	// Update is called once per frame
	void Update () 
    {
	}
}
