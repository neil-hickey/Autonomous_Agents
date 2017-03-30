using UnityEngine;
using System.Collections.Generic;

public sealed class TransitionState : State {

	List<Node> path = new List<Node>();
	State nextState;
	Locations.Location goalLoc;

	public TransitionState(State nextState) {
		this.nextState = nextState; // which state to move to after we have moved!
	}

	public TransitionState (Locations.Location loc, State nextState) {
		this.goalLoc = loc;
		this.nextState = nextState; // which state to move to after we have moved!
	}
		
	public override void Enter (Agent agent) {
		agent.currentLocation = Locations.Location.OnTheMove;
		this.path = new AStar().findPath (agent.currentPosition, agent.GoalPosition);

		if (PlayerManager.Instance.debugMovement) {
			foreach (Node n in this.path) {
				BoardManager.Instance._debugMovement [(int)n.position.x, (int)n.position.y] = 1;
			}
		}
	}
		
	static int x = 5, y = 0;
	public override void Execute (Agent agent) {
		y++;
		if (y % x == 0) {
			if (this.path.Count > 0) {
				Node nextNode = path [0];
				path.RemoveAt (0);
				agent.currentPosition = nextNode.position;

				if (PlayerManager.Instance.debugMovement) {
					BoardManager.Instance._debugMovement [(int)agent.currentPosition.x, (int)agent.currentPosition.y] = 0;
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
