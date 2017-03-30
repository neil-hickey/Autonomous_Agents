using UnityEngine;

public class StateMachine  {
	
	private Agent agent;
	public State state { get; set; }
	public State previousState { get; set; }
	public State globalState { get; set; }

	public void Awake () {
		this.state = null;
	}

	public void Init (Agent agent, State startState, State globalState) {
		this.agent = agent;
		this.state = startState;
		this.previousState = startState;
		this.globalState = globalState;
	}

	public void Update () {
		if (this.globalState != null)
			this.globalState.Execute (this.agent);
		if (this.state != null) 
			this.state.Execute(this.agent);
	}
	
	public void ChangeState (State nextState) {
		this.previousState = this.state;
		if (this.state != null) this.state.Exit(this.agent);
		this.state = nextState;
		if (this.state != null) this.state.Enter(this.agent);
	}

	public void RevertStateToPrevious () {
		ChangeState (this.previousState);	
	}
}