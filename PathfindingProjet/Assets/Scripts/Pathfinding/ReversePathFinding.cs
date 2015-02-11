using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReversePathFinding : MonoBehaviour 
{

	private static List<Node> openSet = new List<Node>();
	private static List<Node> closedSet = new List<Node>();

	public static void CalculateHeuristicReversePathFinding(Node start, Node goal)
	{
		goal.heuristicActualCostInitialState = 0;
		goal.heuristicEstimatedTotalNodePathCost = goal.CalculateHeuristic(start.transform.position);

		openSet.Add(goal);
		closedSet.Clear();

		resumeReversePathFinding(start, goal);
	}


	private static bool resumeReversePathFinding(Node origin, Node goal)
	{
		Node currentNode = null;

		while (openSet.Count != 0)
		{
			//currentNode = findLowestTotalCost();
			currentNode = openSet[openSet.Count -1];

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			
			if (currentNode == origin)
			{
				return true;
			}

			foreach (Node neighbor in currentNode.neighbors)
			{
				neighbor.heuristicActualCostInitialState = currentNode.actualCostInitialState + neighbor.CalculateHeuristic(currentNode.transform.position);

				float tempHeuristicCost = neighbor.CalculateHeuristic(origin.transform.position);

				if(!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
				{
					openSet.Add(neighbor);
				}

				if(openSet.Contains(neighbor) && currentNode.heuristicEstimatedTotalNodePathCost > (neighbor.heuristicActualCostInitialState + tempHeuristicCost))
				{
                    neighbor.heuristicEstimatedTotalNodePathCost = neighbor.heuristicActualCostInitialState + tempHeuristicCost;
                    openSet.Add(neighbor);
				}

			}
			
		}

		return false;
	}

	
	private static Node findLowestTotalCost()
	{
		Node templowestNodeCost = null;
		
		foreach (Node node in openSet)
		{
			if (templowestNodeCost == null || templowestNodeCost.heuristicEstimatedTotalNodePathCost > node.heuristicEstimatedTotalNodePathCost)
			{
				templowestNodeCost = node;
			}
		}
		
		return templowestNodeCost;
	}

	
	public static float abstracDist(Node start, Node goal)
	{
        if (openSet.Count == 0)
        {
            CalculateHeuristicReversePathFinding(start, goal);
        }

		if(closedSet.Contains(start))
		{
			return start.estimatedTotalNodePathCost;
		}

		if(resumeReversePathFinding(start, goal))
		{
            return start.estimatedTotalNodePathCost;
		}

		return -1;
	}
}
