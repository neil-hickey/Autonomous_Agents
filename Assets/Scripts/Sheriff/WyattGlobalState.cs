using UnityEngine;

public sealed class WyattGlobalState : State {

	static readonly WyattGlobalState instance = new WyattGlobalState();

	public static WyattGlobalState Instance {
		get {
			return instance;
		}
	}

	static WyattGlobalState () {}
	private WyattGlobalState () {}

	public override void Enter (Agent agent) {

	}

	public override void Execute (Agent agent) {

	}

	public override void Exit (Agent agent) {

	}
}
