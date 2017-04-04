using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node  {
	public GameObject obj { get; set; }
	public Locations.Location location { get; set; }
	public Vector3 position { get; set; }

	// reference to this nodes parent node, (for a star)
	public Node parentNode { get; set; }

	// costs associated with this node, for sound and movement.
	public float moveCost { get; set; }
	public float soundCost { get; set; }

	// a star costs
	public float G { get; set; } // start to here
	public float H { get; set; } // here to end
	public float F { get { return this.G + this.H; } } // Estimated total cost (F = G + H)

	public Node() { }

	public Node(Vector3 pos) {
		this.position = pos;
	}
				
	public List<Node> getNeighbours(Node[,] _nodes, int N) {
		List<Node> neighbors = new List<Node> ();
		int x = (int)position.x;
		int y = (int)position.y;

		int count = 1;
		while (count <= N) {
			// up
			if (isValid (_nodes, x + count, y)) {
				neighbors.Add (_nodes [x + count, y]);
			}
			//down
			if (isValid (_nodes, x - count, y)) {
				neighbors.Add (_nodes [x - count, y]);
			}
			//right
			if (isValid (_nodes, x, y + count)) {
				neighbors.Add (_nodes [x, y + count]);
			}
			//left
			if (isValid (_nodes, x, y - count)) {
				neighbors.Add (_nodes [x, y - count]);
			}
			count++;
		}
		return neighbors;
	}

	/**
	 * Is this node within the boundary of the game map
	 * @param Node[,] _nodes - Game Map Nodes
	 * @param int x, y - position to check
	 * @return bool - is within the bounds
	 */
	public bool isValid(Node[,] _nodes, int x, int y) {
		return (x >= 0 && x < _nodes.GetLength (0) && y >= 0 && y < _nodes.GetLength (1));
	}

	/**
	 * Gets or sets the parent node. The start node's parent is always null.
	 * @return Node - parent node of this node
	 */ 
	public Node ParentNode {
		get { return this.parentNode; }
		set {
			// When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
			this.parentNode = value;
			this.G = this.G + this.parentNode.G;
		}
	}

	/**
	 * Heuristic used in A Star
	 * Manhattan Distance - The distance between two points measured along axes at right angles
	 * 
	 * @return float - manahattan distance between two vectors
	 */  
	public static float getManhattanDistance(Vector3 a, Vector3 b) {
		float deltaX = Math.Abs(a.x - b.x);
		float deltaY = Math.Abs(a.y - b.y);
		return (float) deltaX + deltaY;
	}
		
	/**
	 * Return a string representation of this node.
	 * @return String
	 */ 
	public String toString() {
		return "(" + this.position.x + "," + this.position.y + ":" + this.F + ")";
	}
}
