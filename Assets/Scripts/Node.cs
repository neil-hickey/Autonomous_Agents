using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node  {
	public GameObject obj { get; set; }
	public Locations.Location location { get; set; }
	public Vector3 position { get; set; }
	public bool isWall { get; set; }
	public Node parentNode { get; set; }
	public float moveCost { get; set; }

	// a star costs
	public float G { get; set; } // start to here
	public float H { get; set; } // here to end
	public float F { get { return this.G + this.H; } } // Estimated total cost (F = G + H)

	public Node () {
		isWall = false;
	}

	public bool IsWalkable() {
		return !isWall;
	}

	public Node(Vector3 pos) : this() {
		this.position = pos;
	}

	public Node (GameObject obj, Vector3 pos, float moveCost) : this() {
		this.obj = obj;
		this.position = pos;
		this.G = moveCost;
		this.moveCost = moveCost;
	}

	public Node (GameObject obj, Locations.Location loc, Vector3 pos, float moveCost) : this() {
		this.obj = obj;
		this.location = loc;
		this.position = pos;
		this.G = moveCost;
		this.moveCost = moveCost;
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

	public bool isValid(Node[,] _nodes, int x, int y) {
		return (x >= 0 && x < _nodes.GetLength (0) && y >= 0 && y < _nodes.GetLength (1));
	}

	/// Gets or sets the parent node. The start node's parent is always null.
	public Node ParentNode {
		get { return this.parentNode; }
		set {
			// When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
			this.parentNode = value;
			this.G = this.G + this.parentNode.G;
		}
	}

	public static float GetTraversalCost(Vector3 a, Vector3 b) {
		float deltaX = a.x - b.x;
		float deltaY = a.y - b.y;
		return (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
	}
		
	public String toString() {
		return "(" + this.position.x + "," + this.position.y + ":" + this.F + ")";
	}
}
