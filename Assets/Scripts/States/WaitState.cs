using UnityEngine;

public sealed class WaitState : State {

	static readonly WaitState instance = new WaitState();

	public static WaitState Instance { get {return instance;} }

	public override void Enter (Agent agent) {
		// do nothing
	}

	public override void Execute (Agent agent) {
		var _agent = (Bob)agent;

		_agent.IncreaseWaitedTime(1);
//		Debug.Log("...waiting for " + _agent.waitedTime + " cycle" + (_agent.waitedTime > 1 ? "s" : "") + " so far...");
		if (_agent.WaitedLongEnough()) 
			_agent.ChangeState(CreateState.Instance);
	}

	public override void Exit (Agent agent) {
		// do nothing
	}
}
