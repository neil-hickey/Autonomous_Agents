using UnityEngine;
using System.Collections;

public class PatrolState : State {

	static readonly PatrolState instance = new PatrolState();
	public static PatrolState Instance {get{return instance;}}
		
	static PatrolState () {}
	private PatrolState () {}

	public override void Enter (Agent agent) {
//		Debug.Log ("Entering Patrol State");

		var _agent = (Wyatt)agent;

		Jesse outlaw = _agent.checkLocationForOutlaw ();

		if (outlaw != null) {
			_agent.performShootout (outlaw);
			_agent.moveToState (DepositAtBankState.Instance);
		} 

//		Debug.Log("Starting sheriff to patrol at " + _agent.currentLocation + " ...");
	}

	public override void Execute (Agent agent) {
		var _agent = (Wyatt)agent;

		_agent.swapLocationRandomly ();

		_agent.moveToState(PatrolState.Instance);
	}

	public override void Exit (Agent agent) {

	}

}

