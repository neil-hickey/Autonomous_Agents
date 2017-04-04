using UnityEngine;

public sealed class LurkState : State {

	static readonly LurkState instance = new LurkState();

	public static LurkState Instance { get {return instance;} }

	static int x = 140, y = 0;

	public override void Enter (Agent agent) {
		// do nothing
	}

	public override void Execute (Agent agent) {
		var _agent = (Jesse)agent;

		// once we had waited long enough
		_agent.IncreaseWaitedTime(1);
		if (_agent.WaitedLongEnough ()) {
			y++;
			if (y % x == 0) {
				// change location with 30% chance of going to the bank
				int r = Random.Range (0, 100);

				if (r < 30) { // #30% chance
					_agent.moveToBank ();
					_agent.moveToState (RobState.Instance);
				} else {
					_agent.swapLocation ();
					_agent.moveToState (LurkState.Instance);
				}
			}
		}
	}

	public override void Exit (Agent agent) {
		// do nothing
	}
}
