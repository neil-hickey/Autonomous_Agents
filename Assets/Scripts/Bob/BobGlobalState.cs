using UnityEngine;

public sealed class BobGlobalState : State {

	static readonly BobGlobalState instance = new BobGlobalState();

	public static BobGlobalState Instance {
		get {
			return instance;
		}
	}

	static BobGlobalState () {}
	private BobGlobalState () {}

	public override void Enter (Agent agent) {

	}

	public override void Execute (Agent agent) {

	}

	public override void Exit (Agent agent) {

	}
}
