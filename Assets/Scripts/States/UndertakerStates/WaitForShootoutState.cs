using UnityEngine;
using UnityEngine.Events;

public sealed class WaitForShootoutState : State {

	static WaitForShootoutState instance;

	public static WaitForShootoutState Instance { get {return instance;} }

	public State nextState;
	public bool eventOccured = false;
	public Vector3 shootoutCoords;
	public Locations.Location loc;

	public WaitForShootoutState (State nextState) {
		instance = this;
		this.nextState = nextState;
		Wyatt.onShootout += listenForShootout; // subscribe to shootout events
	}

	public void listenForShootout(Locations.Location loc) {
		this.loc = loc;
		this.shootoutCoords = Locations.dictionary [loc];
		eventOccured = true;
	}

	public override void Enter (Agent agent) {
		if (agent.currentLocation != Locations.Location.Undertakers) {
			agent.goalLocation = Locations.Location.Undertakers;
			agent.moveToState (WaitForShootoutState.Instance);
		}
	}

	public override void Execute (Agent agent) {
		if (eventOccured) {
			agent.goalLocation = loc;
			agent.moveToState (nextState);
			eventOccured = false;
		}
	}

	public override void Exit (Agent agent) {
		// do nothing
	}
}
