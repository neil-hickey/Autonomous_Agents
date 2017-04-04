using UnityEngine;
using System.Collections;

public class DepositAtBankState : State {

	static readonly DepositAtBankState instance = new DepositAtBankState();
	public static DepositAtBankState Instance {get{return instance;}}

	public override void Enter (Agent agent) {
		// do nothing
	}

	public override void Execute (Agent agent) {
		var _agent = (Wyatt)agent;

		if (_agent.recoveredLoot > 0) {
			Debug.Log ("Got some loot from the outlaw, to the bank!");
			_agent.moveToState (CelebrateState.Instance);
		} else {
			_agent.ChangeState (PatrolState.Instance);
		}
	}

	public override void Exit (Agent agent) {
		// do nothing
	}

}

