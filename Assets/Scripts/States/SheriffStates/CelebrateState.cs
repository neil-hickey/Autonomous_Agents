using UnityEngine;
using System.Collections;

public class CelebrateState : State {

	static readonly CelebrateState instance = new CelebrateState();
	public static CelebrateState Instance { get{return instance;} }

	public override void Enter (Agent agent) {
		Debug.Log ("Celebration Time!");
	}

	public override void Execute (Agent agent) {
		var _agent = (Wyatt)agent;

		_agent.moveToState (PatrolState.Instance);
	}

	public override void Exit (Agent agent) {
		// do nothing
	}
}

