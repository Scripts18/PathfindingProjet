using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReversePathFinding : MonoBehaviour 
{

	private static Stack<Node> openSet = new Stack<Node>();
	private static List<Node> closedSet = new List<Node>();

	public static void CalculateHeuristicReversePathFinding(Node start, Node goal)
	{
		goal.heuristicActualCostInitialState = 0;
		goal.heuristicEstimatedTotalNodePathCost = goal.CalculateHeuristic(start.transform.position);

		openSet.Push(goal);
		closedSet.Clear();

		resumeReversePathFinding(start, goal);
	}


	private static bool resumeReversePathFinding(Node origin, Node goal)
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

                if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                {
                    openSet.Push(neighbor);
                }

				if(openSet.Contains(neighbor) && (currentNode.heuristicEstimatedTotalNodePathCost > (neighbor.heuristicActualCostInitialState + tempHeuristicCost)))
				{
                    neighbor.heuristicEstimatedTotalNodePathCost = neighbor.heuristicActualCostInitialState + tempHeuristicCost;
				}

			}
			
		}

		return false;
	}

	public static float abstracDist(Node start, Node goal)
	{
        if (openSet.Count == 0)  
        {
            CalculateHeuristicReversePathFinding(start, goal);
        }
            
		if(closedSet.Contains(start))
		{
            return start.heuristicActualCostInitialState;
		}

		if(resumeReversePathFinding(start, goal))
		{
            return start.heuristicActualCostInitialState;
		}

		return Mathf.Infinity;
	}
}
