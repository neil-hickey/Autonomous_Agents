using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class Jimmy : Agent {
	
	public void Awake () {
		this.currentLocation = Locations.Location.Undertakers; // where to start undertaker
		this.currentPosition = Locations.dictionary [currentLocation];
		this.stateMachine = new StateMachine();
		this.stateMachine.Init(this, new WaitForShootoutState(CollectBodies.Instance), JimmyGlobalState.Instance);
	}

	public override void ChangeState (State state) {
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.stateMachine.Update();
	}

	public override void checkSenses() {
		PlayerManager.Instance.senseAgents (this);
	}

	public override void SenseEventOccured(SenseEvent theEvent) {
		switch (theEvent.senseType) {
		case SenseEvent.SenseType.HEARING:
			Debug.Log("Jimmy can hear something coming from: " + theEvent.sourcePosition);
			break;
		default:
			break;
		}
	}
}