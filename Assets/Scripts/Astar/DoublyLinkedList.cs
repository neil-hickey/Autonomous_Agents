using UnityEngine;
using System.Collections.Generic;
using System;

public class DoublyLinkedList {
	private DLLNode head;
	public int count;

	// constructor
	public DoublyLinkedList() {
		head = null;
		count = 0;
	}

	public int Count {
		get { return count; }
	}
		
	public DLLNode Insert(Node node) {
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

	public DLLNode getHead() {
		return GetValue (0);
	}
		
	public String toString() {
		String s = "";

		DLLNode prev;
		DLLNode curr = head;
		while (curr != null) {
			prev = curr;
			s += " VAL: " + curr.Node.F;
			curr = curr.Next;
		}
		return s;
	}
}