using UnityEngine;

public sealed class JimmyGlobalState : State {

	static readonly JimmyGlobalState instance = new JimmyGlobalState();

	public static JimmyGlobalState Instance {
		get {
			return instance;
		}
	}

	static JimmyGlobalState () {}
	private JimmyGlobalState () {}

	public override void Enter (Agent agent) {

	}

	public override void Execute (Agent agent) {

	}

	public override void Exit (Agent agent) {

	}
}
