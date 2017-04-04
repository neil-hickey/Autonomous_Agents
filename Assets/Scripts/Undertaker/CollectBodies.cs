using UnityEngine;
using System.Collections;

public class CollectBodies : State {

	static readonly CollectBodies instance = new CollectBodies();
	public static CollectBodies Instance {get{return instance;}}

	static CollectBodies () {}
	private CollectBodies () {}

	public override void Enter (Agent agent) {
		Debug.Log ("Collecting Bodies");
	}

	public override void Execute (Agent agent) {
		agent.goalLocation = Locations.Location.Cemetery;
		agent.moveToState (WaitForShootoutState.Instance);

	}

	public override void Exit (Agent agent) {
		Debug.Log ("Done collecting bodies");
	}

}

