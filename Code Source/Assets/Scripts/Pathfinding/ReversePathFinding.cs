using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <author>Marc-André Larochelle</author>
public class ReversePathFinding : MonoBehaviour 
{

	private Stack<Node> openSet = new Stack<Node>();
	private List<Node> closedSet = new List<Node>();

	public void CalculateHeuristicReversePathFinding(Node start, Node goal)
	{
		goal.heuristicActualCostInitialState = 0;
		goal.heuristicEstimatedTotalNodePathCost = goal.CalculateHeuristic(start.transform.position);

		openSet.Clear();
		openSet.Push(goal);
		closedSet.Clear();

		resumeReversePathFinding(start, goal);
	}


	private bool resumeReversePathFinding(Node origin, Node goal)
	{
		Node currentNode = null;

		while (openSet.Count != 0)
		{
			currentNode = openSet.Pop();

			closedSet.Add(currentNode);
			
			if (currentNode == origin)
			{
				return true;
			}

			foreach (Node neighbor in currentNode.neighbors)
			{
				neighbor.heuristicActualCostInitialState = currentNode.heuristicActualCostInitialState + neighbor.CalculateHeuristic(currentNode.transform.position);

				float tempHeuristicCost = neighbor.CalculateHeuristic(origin.transform.position);

                if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor) && !neighbor.IsObstacle())
                {
                    openSet.Push(neighbor);
                }

				if(!neighbor.IsObstacle())
				{
					if(openSet.Contains(neighbor) && (currentNode.heuristicEstimatedTotalNodePathCost > (neighbor.heuristicActualCostInitialState + tempHeuristicCost)))
					{
                	    neighbor.heuristicEstimatedTotalNodePathCost = neighbor.heuristicActualCostInitialState + tempHeuristicCost;
					}
				}

			}
			
		}

		return false;
	}

	public float abstracDist(Node start, Node goal)
	{
        if (openSet.Count == 0)  
        {
            CalculateHeuristicReversePathFinding(start, goal);
        }
            
		if(closedSet.Contains(start))
		{
            //return start.heuristicActualCostInitialState;
			return goal.heuristicEstimatedTotalNodePathCost;
		}

		if(resumeReversePathFinding(start, goal))
		{
            //return start.heuristicActualCostInitialState;
			return goal.heuristicEstimatedTotalNodePathCost;
		}

		return Mathf.Infinity;
	}
}
