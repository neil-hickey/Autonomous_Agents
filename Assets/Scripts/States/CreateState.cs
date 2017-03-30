using UnityEngine;

public sealed class CreateState : State {
	
	static readonly CreateState instance = new CreateState();
	
	public static CreateState Instance {
		get {
			return instance;
		}
	}
	
	static CreateState () {}
	private CreateState () {}
	
	public override void Enter (Agent agent) {

		Debug.Log("Gathering creative energies...");
	}
	
	public override void Execute (Agent agent) {
		var _agent = (Bob)agent;

		_agent.CreateTime();
		Debug.Log("...creating more time, for a total of " + _agent.createdTime + " unit" 
			+ (_agent.createdTime > 1 ? "s" : "") + "...");
		_agent.ChangeState(WaitState.Instance);
	}
	
	public override void Exit (Agent agent) {

	}
}
