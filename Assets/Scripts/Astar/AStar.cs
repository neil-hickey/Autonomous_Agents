using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class AStar {

	public static BoardManager boardManager { get; set; }
	private Node[,] graph;

	public AStar() {
		// map a copy of the map, because we are editing the G / F values of the nodes...
		copyNodes (boardManager._nodes);
	}

	private void copyNodes(Node[,] graph) {
		this.graph = new Node[graph.GetLength(0), graph.GetLength(1)];

		for (int i = 0; i < graph.GetLength(0); i++) {
			for (int j = 0; j < graph.GetLength(1); j++) {
				this.graph [i, j] = graph [i, j];
				this.graph [i, j].parentNode = new Node();
				this.graph [i, j].G = this.graph [i, j].moveCost;
			}
		}
	}
		
	public List<Node> findPath(Vector3 startPos, Vector3 goalPos) {
		Node start = graph[(int)startPos.x, (int)startPos.y];
		Node goal = graph[(int)goalPos.x, (int)goalPos.y];

		if (startPos == goalPos) {
			return new List<Node> ();
		}

		/** 
		 * the openset dict contains the nodes and a reference to its node in the DLL
		 * this allows us to 
		 * 1) delete - O(1) (delete O(1) because we have the ref to the DLLnode itself, just change the pointers)
		 * 2) insert - O(n) (must insert by traversing the DLL)
		 * 3) get lowest F Value - O(1) (head of inorder DLL)
		 * 4) membership test - O(1) (hashmap lookup)
		 */
		Dictionary<Node, DLLNode> _openSet = new Dictionary<Node, DLLNode>();
		DoublyLinkedList fValues = new DoublyLinkedList ();

		performAStar (_openSet, fValues, start, goal);

		return getPath (goal);
	}

	/**
	 * AStar algorithm as per page 12 of CS 7056 Autonomous Agents coursework
	 * https://www.scss.tcd.ie/Mads.Haahr/CS7056/notes/003.pdf
	 * 
	 */
	public void performAStar(Dictionary<Node, DLLNode> openSet, DoublyLinkedList fValues, Node start, Node goal) {
		DLLNode _node = fValues.Insert (start);
		openSet.Add (start, _node);

		HashSet<Node> closedSet = new HashSet<Node>();

		DLLNode currentDLLNode = fValues.getHead ();
		Node currentNode = currentDLLNode.Node;

		while (currentNode.position != goal.position) {
			currentDLLNode = fValues.getHead ();
			currentNode = currentDLLNode.Node;

			deleteFromOpen (openSet, fValues, currentNode, currentDLLNode);
			closedSet.Add (currentNode);

			foreach (Node neighbor in currentNode.getNeighbours (graph, 1)) {
				int neighborX = (int)neighbor.position.x;
				int neighborY = (int)neighbor.position.y;

				currentNode.H = Node.GetTraversalCost (currentNode.position, goal.position);
				neighbor.H = Node.GetTraversalCost (neighbor.position, goal.position);
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
		
	public List<Node> getPath(Node endNode) {
		List<Node> path = new List<Node>();
		Node node = endNode;
		while (node.ParentNode != null) {
			path.Add(node);
			node = node.ParentNode;
		}

		// Reverse the list so it's in the correct order when returned
		path.Reverse();
		return path;
	}

	public void deleteFromOpen(Dictionary<Node, DLLNode> openSet, DoublyLinkedList fValues, Node n, DLLNode v) {
		openSet.Remove (n);
		fValues.Delete (v);
	}
		
}

