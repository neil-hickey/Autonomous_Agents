using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class AStar {

	public enum ASTAR_CHOICES {MOVEMENT, HEARING, SMELL};
	public static BoardManager boardManager { get; set; }
	private AStarNode[,] graph;
	private ASTAR_CHOICES choice = ASTAR_CHOICES.MOVEMENT;
	private float boundary = 0.0f;
	public bool debug { get ; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="AStar"/> class.
	///  Default Constructor - Default to movement cost A Star
	/// </summary>
	public AStar() {
		copyNodes (boardManager._nodes);
	}
		
	/// <summary>
	/// Initializes a new instance of the <see cref="AStar"/> class.
	/// </summary>
	/// <param name="choice">Choice. choice of A Star graph to use, i.e movement, hearing</param>
	public AStar(ASTAR_CHOICES choice) {
		// make a copy of the map, because we are editing the G / F values of the nodes...
		this.choice = choice;
		copyNodes (boardManager._nodes);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AStar"/> class.
	/// Use bounded A Star, which returns the path up to the boundary.
	/// </summary>
	/// <param name="choice">Choice.</param>
	/// <param name="boundary">Boundary.</param>
	public AStar(ASTAR_CHOICES choice, float boundary) {
		// make a copy of the map, because we are editing the G / F values of the nodes...
		this.choice = choice;
		this.boundary = boundary;
		copyNodes (boardManager._nodes);
	}

	/// <summary>
	/// Copies the nodes.
	/// private function to make a copy of the graph, using the G cost of choice
	/// </summary>
	/// <param name="graph">Graph.</param>
	private void copyNodes(AStarNode[,] graph) {
		this.graph = new AStarNode[graph.GetLength(0), graph.GetLength(1)];

		for (int i = 0; i < graph.GetLength(0); i++) {
			for (int j = 0; j < graph.GetLength(1); j++) {
				this.graph [i, j] = graph [i, j];
				this.graph [i, j].parentNode = null;
				switch (this.choice) {
				case ASTAR_CHOICES.MOVEMENT:
					this.graph [i, j].G = this.graph [i, j].moveCost;
					break;
				case ASTAR_CHOICES.HEARING:
					this.graph [i, j].G = this.graph [i, j].soundCost;
					break;
				}
			}
		}
	}
		
	/// <summary>
	/// Finds the path.
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="startPos">Start position.</param>
	/// <param name="goalPos">Goal position.</param>
	public List<AStarNode> findPath(Vector3 startPos, Vector3 goalPos) {
		AStarNode start = graph[(int)startPos.x, (int)startPos.y];
		AStarNode goal = graph[(int)goalPos.x, (int)goalPos.y];

		if (startPos == goalPos) {
			return new List<AStarNode> ();
		}

		/** 
		 * the openset dict contains the nodes and a reference to its node in the DLL
		 * this allows us to 
		 * 1) delete - O(1) (delete O(1) because we have the ref to the DLLnode itself, just change the pointers)
		 * 2) insert - O(n) (must insert by traversing the DLL)
		 * 3) get lowest F Value - O(1) (head of inorder DLL)
		 * 4) membership test - O(1) (hashmap lookup)
		 */
		Dictionary<AStarNode, DLLNode> _openSet = new Dictionary<AStarNode, DLLNode>();
		DoublyLinkedList fValues = new DoublyLinkedList ();

		performAStar (_openSet, fValues, start, goal);

		List<AStarNode> path =  getPath (goal);

		if (debug)
			debugVisuals (path);

		return path;
	}

	private void debugVisuals(List<AStarNode> path) {
		for (int i = 0; i < path.Count; i++) {
			AStarNode node = path [i];
			GameObject o = BoardManager.Instance._groundTiles [(int)node.position.x, (int)node.position.y];
			o.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.0f, 0.0f, 0.5f);
		}
	}

	/// <summary>
	/// Performs A star.
	/// AStar algorithm as per page 12 of CS 7056 Autonomous Agents coursework
	/// https://www.scss.tcd.ie/Mads.Haahr/CS7056/notes/003.pdf
	/// </summary>
	/// <param name="openSet">Open set.</param>
	/// <param name="fValues">F values.</param>
	/// <param name="start">Start.</param>
	/// <param name="goal">Goal.</param>
	public void performAStar(Dictionary<AStarNode, DLLNode> openSet, DoublyLinkedList fValues, AStarNode start, AStarNode goal) {
		DLLNode _node = fValues.Insert (start);
		openSet.Add (start, _node);

		HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

		DLLNode currentDLLNode = fValues.getHead ();
		AStarNode currentNode = currentDLLNode.Node;

		while (currentNode.position != goal.position) {
			currentDLLNode = fValues.getHead ();
			currentNode = currentDLLNode.Node;

			deleteFromOpen (openSet, fValues, currentNode, currentDLLNode);
			closedSet.Add (currentNode);

			foreach (AStarNode neighbor in currentNode.getNeighbours (1)) {
				int neighborX = (int)neighbor.position.x;
				int neighborY = (int)neighbor.position.y;

				switch (this.choice) {
				case ASTAR_CHOICES.MOVEMENT:
					currentNode.H = getManhattanDistance (currentNode.position, goal.position);
					neighbor.H = getManhattanDistance (neighbor.position, goal.position);
					break;
				case ASTAR_CHOICES.HEARING:
					currentNode.H = getManhattanDistance (currentNode.position, goal.position);
					neighbor.H = getManhattanDistance (neighbor.position, goal.position);
					break;
				}

				var cost = neighbor.G;

				if (openSet.ContainsKey (neighbor) && cost < graph [neighborX, neighborY].G) {
					DLLNode referencedNode;
					openSet.TryGetValue (neighbor, out referencedNode);
					deleteFromOpen (openSet, fValues, neighbor, referencedNode);
				}
				if (closedSet.Contains (neighbor) && cost < graph [neighborX, neighborY].G) {
					closedSet.Remove (neighbor);
				}
				if (!openSet.ContainsKey (neighbor) && !closedSet.Contains (neighbor)) {
					neighbor.ParentNode = currentNode;
					DLLNode val = fValues.Insert (neighbor);
					openSet.Add (neighbor, val);
				}
			} // end foreach
		} // end while
	}
		
	/// <summary>
	/// Gets the path.
	/// </summary>
	/// <returns>The path. </returns>
	/// <param name="endNode">End node.</param>
	public List<AStarNode> getPath(AStarNode endNode) {
		List<AStarNode> path = new List<AStarNode>();
		AStarNode node = endNode;

		if (this.boundary > 0) {
			this.boundary -= node.G;
			while (node.ParentNode != null && this.boundary > 0) {
				path.Add (node);
				node = node.ParentNode;
				this.boundary -= node.G;
			}
		} else {
			while (node.ParentNode != null) {
				path.Add (node);
				node = node.ParentNode;
			}
		}

		// Reverse the list so it's in the correct order when returned
		path.Reverse();
		return path;
	}

	/// <summary>
	/// Deletes from open.
	/// </summary>
	/// <param name="openSet">Open set.</param>
	/// <param name="fValues">F values.</param>
	/// <param name="n">N.</param>
	/// <param name="v">V.</param>
	public void deleteFromOpen(Dictionary<AStarNode, DLLNode> openSet, DoublyLinkedList fValues, AStarNode n, DLLNode v) {
		openSet.Remove (n);
		fValues.Delete (v);
	}
		
	/// <summary>
	/// Heuristic used in A Star
	/// Gets the manhattan distance.
	/// Manhattan Distance - The distance between two points measured along axes at right angles
	/// </summary>
	/// <returns>The manhattan distance.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static float getManhattanDistance(Vector3 a, Vector3 b) {
		float deltaX = Math.Abs(a.x - b.x);
		float deltaY = Math.Abs(a.y - b.y);
		return (float) deltaX + deltaY;
	}
}

