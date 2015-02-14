using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupPathFinding : MonoBehaviour 
{

	[SerializeField]private ReversePathFinding reverseA;
    //Already Evaluated Nodes
	protected List<Node> closedSet = new List<Node>();

    //Nodes to be evaluated, including start node
    protected List<Node> openSet = new List<Node>();

    public Stack<Node> PathFinding(Node start, Node goal)
    {
        closedSet.Clear();
        openSet.Clear();

	    Node currentNode;

	    openSet.Add(start);

	    start.actualCostInitialState = 0;

		reverseA.CalculateHeuristicReversePathFinding(start, goal);
		start.estimatedTotalNodePathCost = reverseA.abstracDist(start, goal);

        Debug.Log(goal.transform.position);
		Debug.Log (start.estimatedTotalNodePathCost);

		//if(start.estimatedTotalNodePathCost != Mathf.Infinity)
		if(true)
		{
		    while (openSet.Count != 0)
		    {
		        currentNode = findLowestTotalCost();

		        if (currentNode == goal)
		        {
		            return reconstructPath(currentNode);
		        }

		        openSet.Remove(currentNode);
		        closedSet.Add(currentNode);

		        foreach (Node neighbor in currentNode.neighbors)
		        {
					if (closedSet.Contains(neighbor) || neighbor.IsObstacle())
					{
						continue;
		            }

					float tempPathCost = currentNode.actualCostInitialState + currentNode.CalculateHeuristic(neighbor.transform.position);

		            if (!openSet.Contains(neighbor) || (tempPathCost < neighbor.actualCostInitialState))
		            {
		                neighbor.parent = currentNode;
		                neighbor.actualCostInitialState = tempPathCost;

                        neighbor.estimatedTotalNodePathCost = tempPathCost + neighbor.CalculateHeuristic(goal.transform.position);

						if (!openSet.Contains(neighbor))
		                {
		                    openSet.Add(neighbor);
		                }
		            }
		        }

		    }
		}


        return null;
    }

    protected Node findLowestTotalCost()
    {
        Node templowestNodeCost = null;

        foreach (Node node in openSet)
        {
            if ((templowestNodeCost == null || templowestNodeCost.estimatedTotalNodePathCost > node.estimatedTotalNodePathCost))
            {
                templowestNodeCost = node;
            }
        }

        return templowestNodeCost;
    }

    protected float distanceBetween(Node startNode, Node goalNode)
    {
		return Vector3.Distance(startNode.transform.position, goalNode.transform.position);
    }

    protected Stack<Node> reconstructPath(Node currentNode)
    {
        Stack<Node> totalPath = new Stack<Node>();
        Node parent;

        totalPath.Push(null);

        while (currentNode.parent != null)
        {
            totalPath.Push(currentNode);
            parent = currentNode;
            currentNode = currentNode.parent;
            parent.ClearParent();
        }

        return totalPath;
    }
}
