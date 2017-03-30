using UnityEngine;

public sealed class JesseGlobalState : State {

	static readonly JesseGlobalState instance = new JesseGlobalState();

	public static JesseGlobalState Instance {
		get {
			return instance;
		}
	}

	static JesseGlobalState () {}
	private JesseGlobalState () {}

	public override void Enter (Agent agent) {
		
	}

	public override void Execute (Agent agent) {
		
	}

	public override void Exit (Agent agent) {
		
	}
}
