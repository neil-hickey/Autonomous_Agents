using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public abstract class Agent : MonoBehaviour {
	public enum Direction {UP, DOWN, LEFT, RIGHT}

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
		
	public Dictionary<Direction, RaycastHit2D[]> RayCast (string name, bool debugRaycasting) {
		Dictionary<Direction, RaycastHit2D[]> objs = new Dictionary<Direction, RaycastHit2D[]> ();
		Node[,] graph = GameManager.instance.boardScript._nodes;
		Node currNode = graph [(int)this.currentPosition.x, (int)this.currentPosition.y];

		foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
			Node furthestNode = currNode.getFurthestNodeFromThis (2, direction);
			if (furthestNode != null) {
				if (debugRaycasting)
					Debug.DrawLine (this.currentPosition, furthestNode.position, Color.blue);

				var layerMask = ~(1 << LayerMask.NameToLayer (name));

				bool spotted = Physics2D.Linecast (this.currentPosition, furthestNode.position, layerMask);
				if (spotted) {
					RaycastHit2D[] hitObjects = Physics2D.RaycastAll (this.currentPosition, furthestNode.position, layerMask);

					objs.Add (direction, hitObjects);
				}
			}
		}
		return objs;
	}
}