using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class Wyatt : Agent {

	public delegate void Shootout(Locations.Location loc);
	public static event Shootout onShootout;

	public int recoveredLoot = 0;

	public void Awake () {
		this.currentLocation = Locations.Location.SheriffsOffice; // where to start sheriff
		this.currentPosition = Locations.dictionary [currentLocation];
		this.stateMachine = new StateMachine();
		this.stateMachine.Init(this, PatrolState.Instance, WyattGlobalState.Instance);
	}
		
	public override void ChangeState (State state) {
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.stateMachine.Update();
//		checkSenses ();
//
//		List<RaycastHit2D> objs = RayCast ("Wyatt");
//		if (objs.Count > 0) {
//			foreach (RaycastHit2D o in objs) {
//				GameObject go = o.collider.gameObject;
//				if (PlayerManager.Instance.playerScriptPairings.ContainsKey (go)) {
//					Agent agent;
//					PlayerManager.Instance.playerScriptPairings.TryGetValue (go, out agent);
//
//					if (agent is Jesse) {
//						Debug.LogError ("IM LOOKING @ THE OUTLAW");
//					}
//				}
//			}
//		}

	}

	public void checkSenses() {
		PlayerManager.Instance.senseAgents (this);
	}

	public override void SenseEventOccured(SenseEvent theEvent) {
		Debug.Log ("Wyatt has received a sense event!");
		switch (theEvent.senseType) {
		case SenseEvent.SenseType.HEARING:
			Debug.Log("Wyatt can hear something coming from: " + theEvent.sourcePosition);
			break;
		default:
			break;
		}
	}

public Jesse checkLocationForOutlaw() {
		List<Agent> agents = PlayerManager.Instance.getAgentsAtMyLocation (this);

		foreach (Agent agent in agents) {
			if (agent is Jesse) {
				return (Jesse) agent;
			}
		}
		// someone else is here
		if (agents.Count > 0) {
			issueGreeting ();
		}
		return null;
	}
		
	public void performShootout (Jesse jesse) {
		Debug.Log ("Shot the outlaw!");
		int loot = jesse.totalLoot;
		jesse.isAlive = false;
		this.recoveredLoot = loot;

		if (onShootout != null) {
			// broadcast the shootout to subscribers
			onShootout (this.currentLocation);
		}

		Debug.Log ("Sheriff has loot: " + this.recoveredLoot);
	}


	public void checkLootAndDeliver() {
		if (this.recoveredLoot > 0) { 
			this.goalLocation = Locations.Location.Bank;
		}
	}

	public void issueGreeting() {
		Debug.Log("HOWDY!");
	}

	public void swapLocationRandomly() {
//		this.currentPosition = Locations.dictionary [this.currentLocation];

		// remove the outlaw camp, because the sheriff should never know about it!
		var data = Enum
			.GetValues(typeof(Locations.Location))
			.Cast<Locations.Location>()
			.Where(item => item != Locations.Location.OutLawCamp && item != Locations.Location.OnTheMove)
			.ToArray();

		Locations.Location randomLocation = (Locations.Location)data.GetValue(Random.Range (0, data.Length));

		this.goalLocation = randomLocation;
	}

}
