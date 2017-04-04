using UnityEngine;
using System.Collections.Generic;

public abstract class Agent : MonoBehaviour {
	public abstract void Update ();
	public abstract void ChangeState (State state);
	public abstract void SenseEventOccured(SenseEvent theEvent);
	public abstract void checkSenses ();

	// each agent can contain the following
	public Locations.Location currentLocation;
	public Locations.Location goalLocation;
	public StateMachine stateMachine;
	public Node currentNode;
	public Vector3 currentPosition;
	public Vector3 goalPosition; 
	public bool spotted = false;
	public bool debugRaycasting = false;
	public bool isAlive = true;

	public Vector3 GoalPosition {
		get {
			this.goalPosition = Locations.dictionary [this.goalLocation];
			return this.goalPosition;
		}
		set { this.goalPosition = value; }
	}
		
	public void moveToState(State nextState) {
		this.ChangeState (new TransitionState (nextState));
	}
		
	public List<RaycastHit2D> RayCast (string name) {
		List<RaycastHit2D> objs = new List<RaycastHit2D> ();
		Node[,] graph = GameManager.instance.boardScript._nodes;
		Node currNode = graph [(int)this.currentPosition.x, (int)this.currentPosition.y];

		List<Node> neighbours = currNode.getNeighbours (graph, 2);
		foreach (Node neighbor in neighbours) {
//			if (debugRaycasting)
//				Debug.DrawLine (this.currentPosition, neighbor.position, Color.blue);

			var layerMask = ~(1 << LayerMask.NameToLayer (name));

			spotted = Physics2D.Linecast (this.currentPosition, neighbor.position, layerMask);
			if (spotted) {
				RaycastHit2D[] hitObject = Physics2D.RaycastAll (this.currentPosition, neighbor.position, layerMask);


				foreach (RaycastHit2D o in hitObject) {
					objs.Add (o);
				}
			}
		}
		return objs;
	}
}