using UnityEngine;
using System.Collections;

public sealed class RobState : State {

	static readonly RobState instance = new RobState();

	public static RobState Instance {
		get {
			return instance;
		}
	}

	static RobState () {}
	private RobState () {}

	public override void Enter (Agent agent) {

	}

	public override void Execute (Agent agent) {
		var _agent = (Jesse)agent;

		int robbedAmount = _agent.robBank ();
		Debug.Log("...robbing the bank of " + robbedAmount + "...");
		agent.ChangeState (LurkState.Instance);
	}
		
	public override void Exit (Agent agent) {

//		Debug.Log("...robbed the bank!");
//		_agent.swapLocation ();
//		Debug.Log ("Moving Jesse back to " + _agent.currentLocation + "...");
	}
}
