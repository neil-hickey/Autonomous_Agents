using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStarNode {
	// reference to this nodes parent node, (for a star)
	public AStarNode parentNode { get; set; }

	// a star costs
	public float G { get; set; } // start to here
	public float H { get; set; } // here to end
	public float F { get { return this.G + this.H; } } // Estimated total cost (F = G + H)

	// costs associated with this node, for sound and movement.
	public float moveCost { get; set; }
	public float soundCost { get; set; }

	// position
	public Vector3 position { get; set; }

	public abstract List<AStarNode> getNeighbours(int N) ;

	/// <summary>
	/// Gets or sets the parent node.
	/// The start node's parent is always null.
	/// </summary>
	/// <value>The parent node.</value>
	public AStarNode ParentNode {
		get { return this.parentNode; }
		set {
			// When setting the parent, also calculate include the previous nodes G value
			this.parentNode = value;
			this.G = this.G + this.parentNode.G;
		}
	}
}
