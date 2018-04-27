using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public int size_x;
	public int size_y;
	public int node_size = 1;
	private Node[,] grid;

	private void Awake()
	{
		GenerateGrid();

	}
	private void Update()
	{

	}

	public void GenerateGrid()
	{
		grid = new Node[size_x, size_y];

		for (int i = 0; i < size_x; i++)
		{
			for (int j = 0; j < size_y; j++)
			{
				Vector3 nodePosition = new Vector3(node_size * i + node_size * 0.5f, node_size * j + node_size * 0.5f, 0);
				Vector3 worldNodePosition = transform.position + nodePosition;

				Collider[] colliders = Physics.OverlapSphere(worldNodePosition, node_size*0.5f);

				bool isTransitable = false;
				bool isConstructable = false;
				for (int k = 0; k < colliders.Length; k++)
				{
					if (colliders[k].tag == "camino" || colliders[k].tag == "exit")

						isTransitable = true;

					if (colliders[k].tag == "zonatorres")

					{
						isTransitable = false;
						isConstructable = true;
					}
				}

				grid[i, j] = new Node(i, j, node_size, worldNodePosition, isTransitable,isConstructable);                
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (grid != null)
		{

			for (int i = 0; i < grid.GetLength(0); i++)
			{
				for (int j = 0; j < grid.GetLength(1); j++)
				{
					if (grid[i, j].isTransitable)
					{
						Gizmos.color = Color.green;
						Vector3 scale = new Vector3(node_size, node_size, node_size);
						Gizmos.DrawWireCube(grid[i, j].worldPosition, scale);
					}
					//Debug.Log(grid[i, j].worldPosition);

					if (grid[i, j].isTransitable == false)
					{
						Gizmos.color = Color.red;
						Vector3 scale = new Vector3(node_size, node_size, node_size);
						Gizmos.DrawCube(grid[i, j].worldPosition, scale/2);
					}

					if (grid[i, j].isConstructable == true)
					{
						Gizmos.color = Color.blue;
						Vector3 scale = new Vector3(node_size, node_size, node_size);
						Gizmos.DrawCube(grid[i, j].worldPosition, scale / 2);
					}
				}
			}
		}


	}

	public Node GetNodeContainingPosition(Vector3 worldPosition)
	{
		Vector3 localPosition = worldPosition - transform.position;
		int x = ((int)localPosition.x / node_size);
		int y = ((int)localPosition.y / node_size);

		if (x < size_x && x >= 0 && y < size_y && y >= 0)
			return grid[x, y];

		return null;
	}

	public Node GetNode(int x, int y)
	{
		//Mirar si x o y son valores validos

		if (x < 0 || y < 0 || x > size_x-1 || y > size_y-1)
		{
			Debug.LogWarning("Se ha pedido un nodo NO VALIDO en la posicion: " + x + ", " + y);
			return null;
		}

		return grid[x, y];
	}

	public List<Node> GetNeighbours(Node nodo, bool extended)
	{
		List<Node> listaDeNodosADevolver = new List<Node>();

		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (!extended)
				{
					if (Mathf.Abs(i) == Mathf.Abs(j))
						continue;
				}
				else
				{
					if (i == 0 && j == 0)
						continue;
				}

				Node vecino = GetNode(nodo.gridPositionX + i, nodo.gridPositionY + j);
				if (vecino != null)
					listaDeNodosADevolver.Add(vecino);
			}
		}

		return listaDeNodosADevolver;
	}

	private int calculateCost (Node actualNode, Node finalNode)
	{
		Vector2 trueDirection = new Vector2 (finalNode.gridPositionX - actualNode.gridPositionX, finalNode.gridPositionY - actualNode.gridPositionY);
		int totalCost = (int)trueDirection.x + (int)trueDirection.y;

		return Mathf.Abs(totalCost);
	}

	public List<Node> FindPath(Vector3 startPos, Vector3 targetPos) 
	{
		// Get the start and ending nodes
		Node startNode = GetNodeContainingPosition(startPos);
		Node finalNode = GetNodeContainingPosition (targetPos);
		Node actualNode;


		startNode.F = calculateCost (startNode, finalNode);
		startNode.H = startNode.F;
		startNode.G = startNode.F - startNode.H;

		// Create OPEN and CLOSE lists
		List<Node> openNodes = new List<Node>();
		List<Node> closedNodes = new List<Node>();

		// Add the start node to OPEN
		openNodes.Add(startNode);
		actualNode = startNode;


		// While we have nodes in the OPEN list
		while(openNodes.Count > 0)
		{
			// Get the node in the OPEN list with the lowest F cost or 
			// with the lowest F but Lower H (distance to end node)
			Node theNode = openNodes[0];

			for (int i = 0; i < openNodes.Count; i++) 
			{

				openNodes [i].H = calculateCost(openNodes[i],finalNode);
				openNodes [i].G = calculateCost (startNode, openNodes [i]);
				openNodes [i].F = openNodes[i].G+openNodes[i].H;

				if (openNodes [i].F == theNode.F)
				{
					if (openNodes [i].H <= theNode.H) 
					{
						theNode = openNodes [i];
					}
				} 
				else if (openNodes [i].F < theNode.F) 
				{
					theNode = openNodes [i];
				}
			}
			// Change that node from OPEN to CLOSE
			openNodes.Remove(theNode);
			closedNodes.Add (theNode);
			// If it's the ending node, we're done: Get the total path found and finish
			if (theNode==finalNode)
			{

				for (int i = 0; i < openNodes.Count; i++)
				{
					openNodes[i].G = 0;
					openNodes[i].H = 0;
					openNodes[i].F = 0;
				}

				for (int i = 0; i < closedNodes.Count; i++)
				{
					closedNodes[i].G = 0;
					closedNodes[i].H = 0;
					closedNodes[i].F = 0;
				}
				return getPathFromNode(startNode, theNode);

			}
			// If not, get all his neighbours
			// Foreach neighbour
			List<Node> Neighbours = GetNeighbours(theNode,false);
			foreach(Node N in Neighbours)
			{		
				// If that neighbour is not walkable or it's closed, 
				if (N.isTransitable == false || closedNodes.Contains (N))
				{
					continue;
					// skip that neighbour and get next
				}

				if (N.F == 0 || N.G < calculateCost(N,theNode)) 
				{
					N.G = calculateCost (N, startNode);
					N.H = calculateCost (N, finalNode);
					N.F = N.G + N.H;

					N.parent = theNode;
					// If we haven't visited that node yet, or this node new G cost is shorter than before

					// Set its G,H & F costs and set the actual node as the parent of this neighbour

				}

				// if the neighbour it's not on the OPEN list, add it.

				if (openNodes.Contains (N) == false)
					openNodes.Add (N);
			}
		}
		return null;
	}

	private List<Node> getPathFromNode(Node startNode, Node finalNode)
	{
		List<Node> resultPath = new List<Node>();
		Node currentNode = finalNode;

		if(resultPath.Count>1000)
		{
			resultPath  = new List<Node>();
			return null;
		}

		while (currentNode!= startNode)
		{
			resultPath.Add(currentNode);
			currentNode = currentNode.parent;

		}

		resultPath.Reverse();

		return resultPath;
	}

}


public class Node {

	public int gridPositionX;
	public int gridPositionY;
	public int size;
	public Vector3 worldPosition;
	public int F=0;
	public int G;
	public int H;
	public Node parent;

	public bool isTransitable = true;
	public bool isConstructable = false;

	public Node() { }

	public Node(int _gridPositionX, int _gridPositionY, int _size, Vector3 _position, bool _isTransitable,bool _isConstructable)
	{
		gridPositionX = _gridPositionX;
		gridPositionY = _gridPositionY;
		size = _size;
		worldPosition = _position;
		isTransitable = _isTransitable;
		isConstructable = _isConstructable;
	}

	public void Grita()
	{
		Debug.Log("AAAAARRRGHHH " + isTransitable);
	}
}
