using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReversePathFinding : MonoBehaviour {

	private static List<Node> openSet = new List<Node>();
	private static List<Node> closedSet = new List<Node>();

	public static void CalculateHeuristicReversePathFinding(Node start, Node goal)
	{
		goal.heuristicBestKnownPathCost = 0;
		goal.heuristicTotalNodePathCost = goal.CalculateHeuristic(start.transform.position);

		closedSet.Clear();

		resumeReversePathFinding(goal);
	}


	private static bool resumeReversePathFinding(Node goal)
	{
		Node currentNode = goal;

		while (openSet.Count != 0)
		{
			currentNode = findLowestTotalCost();

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			
			if (currentNode == goal)
			{
				return true;
			}

			foreach (Node neighbor in currentNode.neighbors)
			{
				neighbor.heuristicBestKnownPathCost = currentNode.bestKnownPathCost + Node.distanceBetween(currentNode, neighbor);

				float tempHeuristicCost = Node.distanceBetween(neighbor, goal);

				if(!openSet.Contains(neighbor) && closedSet.Contains(neighbor))
				{
					openSet.Add(neighbor);
				}

				if(openSet.Contains(neighbor) && tempHeuristicCost < neighbor.heuristicBestKnownPathCost)
				{
					neighbor.heuristicBestKnownPathCost = tempHeuristicCost;
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
			if (templowestNodeCost == null || templowestNodeCost.heuristicTotalNodePathCost > node.heuristicTotalNodePathCost)
			{
				templowestNodeCost = node;
			}
		}
		
		return templowestNodeCost;
	}

	
	public static float abstracDist(Node start, Node goal)
	{
		if(goal.IsObstacle())
		{
			return -1;
		}

		if(closedSet.Contains(start))
		{
			return start.heuristicBestKnownPathCost;
		}

		if(resumeReversePathFinding(start))
		{
			return start.heuristicBestKnownPathCost;
		}

		return -1;
	}
}
