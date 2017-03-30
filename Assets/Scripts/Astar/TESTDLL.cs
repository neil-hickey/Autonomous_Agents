using UnityEngine;
using System.Collections;

public class TESTDLL : MonoBehaviour
{

	public DoublyLinkedList dll;

	// Use this for initialization
	public void Awake ()
	{
		dll = new DoublyLinkedList ();

		Node n1 = new Node ();
		n1.G = 2;
		Node n2 = new Node ();
		n2.G = 1;

		DLLNode nx = dll.Insert (n2);
		Debug.Log (dll.toString ());
		dll.Delete (nx);
		Debug.Log (dll.toString ());
		DLLNode ny = dll.Insert (n1);
		Debug.Log (dll.toString ());
		DLLNode nz = dll.Insert (n1);
		Debug.Log (dll.toString ());
		dll.Delete (nz);

		Debug.Log (dll.toString ());
	}
	
	// Update is called once per frame
	public void Update ()
	{
		
	}
}

