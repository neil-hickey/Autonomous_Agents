using UnityEngine;

public sealed class LurkState : State {

	static readonly LurkState instance = new LurkState();

	public static LurkState Instance {
		get {
			return instance;
		}
	}

	static int x = 140, y = 0;

	static LurkState () {}
	private LurkState () {}

	public override void Enter (Agent agent) {

//		Debug.Log("Starting outlaw to lurk at " + _agent.currentLocation + " ...");
	}

	public override void Execute (Agent agent) {
		var _agent = (Jesse)agent;

		_agent.IncreaseWaitedTime(1);
//		Debug.Log("...waiting for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");
		if (_agent.WaitedLongEnough ()) {
			y++;
			if (y % x == 0) {
				int r = Random.Range (0, 100);

				if (r < 30) { // #30% chance
					_agent.moveToBank ();
					_agent.moveToState (RobState.Instance);
				} else {
					_agent.swapLocation ();
					_agent.moveToState (LurkState.Instance);
				}
//				Debug.Log ("Current Location: " + _agent.currentLocation.ToString ("f"));
			}
		}
	}

	public override void Exit (Agent agent) {
//		Debug.Log("...waited long enough!");
	}
}
