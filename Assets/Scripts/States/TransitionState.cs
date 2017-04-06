using UnityEngine;
using System.Collections.Generic;

/**
 * The transition state moves an agent from one location to another, using A Star to determine the path
 * 
 */ 
public sealed class TransitionState : State {

	List<AStarNode> path = new List<AStarNode>();
	State nextState;
	private bool debug {get;set;}

	/// <summary>
	/// Initializes a new instance of the <see cref="TransitionState"/> class.
	/// </summary>
	/// <param name="nextState">Next state.</param>
	public TransitionState(State nextState) {
		this.nextState = nextState; // which state to move to after we have moved!
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TransitionState"/> class.
	/// </summary>
	/// <param name="loc">Location.</param>
	/// <param name="nextState">Next state.</param>
	public TransitionState (Locations.Location loc, State nextState) {
		this.nextState = nextState; // which state to move to after we have moved!
	}
		
	public override void Enter (Agent agent) {
		agent.currentLocation = Locations.Location.OnTheMove;
		AStar aStar = new AStar ();
		aStar.debug = true;
		this.debug = aStar.debug;
		this.path = aStar.findPath (agent.currentPosition, agent.GoalPosition);

		if (this.debug) {
			foreach (Node n in this.path) {
				BoardManager.Instance._debugMovement [(int)n.position.x, (int)n.position.y] += 1;
			}
		}
	}
		
	// used to slow down the movement of the agent...
	static int x = 5, y = 0;
	public override void Execute (Agent agent) {
		y++;
		if (y % x == 0) {
			if (this.path.Count > 0) {
				AStarNode nextNode = path [0];
				path.RemoveAt (0);
				agent.currentPosition = nextNode.position;

				if (this.debug) {
					BoardManager.Instance._debugMovement [(int)nextNode.position.x, (int)nextNode.position.y] -= 1;
					if (BoardManager.Instance._debugMovement [(int)nextNode.position.x, (int)nextNode.position.y] <= 0) {
						GameObject o = BoardManager.Instance._groundTiles [(int)nextNode.position.x, (int)nextNode.position.y];
						o.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.0f, 0.0f, 0.0f);
					}
				}
			}
		}

		// if followed the path, we can move out of transition state
		if (this.path.Count <= 0) {
			agent.ChangeState (this.nextState);
		}
	}

	public override void Exit (Agent agent) {
		// we have reached our destination, so the current location is now the goal 
		agent.currentLocation = agent.goalLocation;
	}
}
