using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : AStarNode {
	public GameObject obj { get; set; }
	public Locations.Location location { get; set; }
	public float sightCost { get; set; }
	/// <summary>
	/// Initializes a new instance of the <see cref="Node"/> class.
	/// </summary>
	public Node() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Node"/> class.
	/// </summary>
	/// <param name="pos">Position.</param>
	public Node(Vector3 pos) {
		this.position = pos;
	}
				
	/// <summary>
	/// Gets the neighbours, of radius N
	/// </summary>
	/// <returns>The neighbours.</returns>
	/// <param name="N">N.</param>
	public override List<AStarNode> getNeighbours(int N) {
		AStarNode[,] _nodes = BoardManager.Instance._nodes;
		List<AStarNode> neighbors = new List<AStarNode> ();
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

	public Node getFurthestNodeFromThis(int N, Agent.Direction direction) {
		Node[,] _nodes = BoardManager.Instance._nodes;
		List<Node> neighbors = new List<Node> ();
		int x = (int)position.x;
		int y = (int)position.y;
		int count = N;

		switch (direction) {
		case Agent.Direction.UP:
			while (count > 0) { 
				if (isValid (_nodes, x + count, y)) {
					return _nodes [x + count, y];
				} else {
					count--;
				}
			}
			break;
		case Agent.Direction.DOWN:
			while (count > 0) { 
				if (isValid (_nodes, x - count, y)) {
					return _nodes [x - count, y];
				} else {
					count--;
				}
			}
			break;
		case Agent.Direction.RIGHT:
			while (count > 0) { 
				if (isValid (_nodes, x, y + count)) {
					return _nodes [x, y + count];
				} else {
					count--;
				}
			}
			break;
		case Agent.Direction.LEFT:
			while (count > 0) { 
				if (isValid (_nodes, x, y - count)) {
					return _nodes [x, y - count];
				} else {
					count--;
				}
			}
			break;
		}
		return null; 
	}

	/// <summary>
	/// Is this node within the boundary of the game map
	/// </summary>
	/// <returns><c>true</c>, if within bounds, <c>false</c> otherwise.</returns>
	/// <param name="_nodes">Nodes.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public bool isValid(AStarNode[,] _nodes, int x, int y) {
		return (x >= 0 && x < _nodes.GetLength (0) && y >= 0 && y < _nodes.GetLength (1));
	}
		
	/// <summary>
	/// Return a string representation of this node.
	/// </summary>
	/// <returns>The string.</returns>
	public String toString() {
		return "(" + this.position.x + "," + this.position.y + ":" + this.F + ")";
	}
}
