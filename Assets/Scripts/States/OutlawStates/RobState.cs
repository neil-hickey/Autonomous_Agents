using UnityEngine;
using System.Collections;

public sealed class RobState : State {

	static readonly RobState instance = new RobState();

	public static RobState Instance { get {return instance;} }

	public override void Enter (Agent agent) {
		// do nothing
	}

	public override void Execute (Agent agent) {
		var _agent = (Jesse)agent;

		// rob the bank
		int robbedAmount = _agent.robBank ();
		Debug.Log("...robbing the bank of " + robbedAmount + "...");

		// move back to lurking
		agent.ChangeState (LurkState.Instance);
	}
		
	public override void Exit (Agent agent) {
		// do nothing
	}
}
