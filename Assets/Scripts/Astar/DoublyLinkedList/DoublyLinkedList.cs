using UnityEngine;
using System.Collections.Generic;
using System;

public class DoublyLinkedList {
	private DLLNode head;
	public int count;

	/// <summary>
	/// Initializes a new instance of the <see cref="DoublyLinkedList"/> class.
	/// </summary>
	public DoublyLinkedList() {
		head = null;
		count = 0;
	}

	/// <summary>
	/// Gets the count.
	/// </summary>
	/// <value>The count.</value>
	public int Count {
		get { return count; }
	}
		
	/// <summary>
	/// Insert the specified node.
	/// </summary>
	/// <param name="node">Node.</param>
	public DLLNode Insert(AStarNode node) {
		int pos = 0;
		DLLNode current = head;
		DLLNode previous = null;
		while (current != null && current.Node.F < node.F) {
			previous = current;
			current = current.Next;
			pos++;
		}

		DLLNode n = new DLLNode();
		n.Node = node;

		if (previous == null) {
			head = n;
		} else {
			// re-link previous node
			previous.Next = n;
			n.Prev = previous;
		}

		if (current != null) {
			// re-link next node
			current.Prev = n;
			n.Next = current;
		}
		count++;

		return n;
	}

	/// <summary>
	/// Delete the specified p.
	/// </summary>
	/// <param name="p">P.</param>
	public bool Delete(DLLNode p)  {
		// fetch value at position i
		if (p == null) {
			return false;
		}

		DLLNode prev = p.Prev;
		DLLNode next = p.Next;

		if (next == null && prev == null) {
			head = null;
		}
		else if (next == null) {
			// end
			prev.Next = null;
		} else if (prev == null) {
			// start
			head = p.Next;
			p.Next.Prev = null;
		} else {
			prev.Next = next;
			next.Prev = prev;
		}
		count--;
		return true;
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="i">The index.</param>
	public DLLNode GetValue(int i) // fetch value at position i
	{
		DLLNode p = head;
		int ct = 0;
		while (p != null && ct < i) {
			p = p.Next;
			++ct;
		}
		return p;
	}

	/// <summary>
	/// Gets the head.
	/// </summary>
	/// <returns>The head.</returns>
	public DLLNode getHead() {
		return GetValue (0);
	}

	/// <summary>
	/// Tos the string.
	/// </summary>
	/// <returns>The string.</returns>
	public String toString() {
		String s = "";

		DLLNode curr = head;
		while (curr != null) {
			s += " VAL: " + curr.Node.F;
			curr = curr.Next;
		}
		return s;
	}
}