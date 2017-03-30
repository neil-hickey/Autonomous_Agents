using UnityEngine;
using System.Collections;

public class DLLNode {
	private DLLNode next;
	private DLLNode prev;
	private Node node;

	public DLLNode() {
		next = null;
	}

	public DLLNode Next {
		get { return next; }
		set { next = value; }
	}

	public DLLNode Prev {
		get { return prev; }
		set { prev = value; }
	}

	public Node Node {
		get { return node; }
		set { node = value; }
	}
}

