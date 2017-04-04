using UnityEngine;
using System.Collections;

public class PatrolState : State {

	static readonly PatrolState instance = new PatrolState();
	public static PatrolState Instance {get{return instance;}}
		
	public override void Enter (Agent agent) {
		var _agent = (Wyatt)agent;

		Jesse outlaw = _agent.checkLocationForOutlaw ();

		if (outlaw != null) {
			// whilst patrolling, we have found the outlaw!
			// Shootout Time!
			_agent.performShootout (outlaw);

			// after the shootout we deposit the loot recovered in the bank
			_agent.moveToState (DepositAtBankState.Instance); 
		} 
	}

	public override void Execute (Agent agent) {
		var _agent = (Wyatt)agent;

		// Patrolling involves moving randomly around the map
		_agent.swapLocationRandomly ();
		_agent.moveToState(PatrolState.Instance);
	}

	public override void Exit (Agent agent) {
		// do nothing
	}
}

