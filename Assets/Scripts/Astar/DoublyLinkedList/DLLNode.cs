using UnityEngine;
using System.Collections;

public class DLLNode {
	private DLLNode next;
	private DLLNode prev;
	private AStarNode node;

	/// <summary>
	/// Initializes a new instance of the <see cref="DLLNode"/> class.
	/// </summary>
	public DLLNode() {
		next = null;
	}

	/// <summary>
	/// Gets or sets the next.
	/// </summary>
	/// <value>The next.</value>
	public DLLNode Next {
		get { return next; }
		set { next = value; }
	}

	/// <summary>
	/// Gets or sets the previous.
	/// </summary>
	/// <value>The previous.</value>
	public DLLNode Prev {
		get { return prev; }
		set { prev = value; }
	}

	/// <summary>
	/// Gets or sets the node.
	/// </summary>
	/// <value>The node.</value>
	public AStarNode Node {
		get { return node; }
		set { node = value; }
	}
}

