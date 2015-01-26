using UnityEngine;
using System.Collections.Generic;

public class Group : ControlGroup 
{
    [SerializeField]
    private List<ControlGroup> listUnits;

    public void ComputePathfinding()
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

	void Start () 
    {
        this.listUnits = new List<ControlGroup>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
