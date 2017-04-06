using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class Wyatt : Agent {

	// declare publish event
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
	}
		
	public override void checkSenses() {
		PlayerManager.Instance.senseAgents (this);
	}

	public override void SenseEventOccured(SenseEvent theEvent) {
		switch (theEvent.senseType) {
		case SenseEvent.SenseType.SIGHT:
			Debug.Log("Wyatt can see something at: " + theEvent.sourcePosition);
			break;
		case SenseEvent.SenseType.HEARING:
			Debug.Log("Wyatt can hear something coming from: " + theEvent.sourcePosition);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Function to check if the outlaw is in the sheriffs current location
	///  Issues a greeting if another agent exists here, but isnt the outlaw
	/// </summary>
	/// <returns>Jesse / null - returns the outlaw agent or null if no outlaw </returns>
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
		
	/// <summary>
	/// Performs the shootout.
	/// </summary>
	/// <param name="jesse">Jesse.</param>
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

	/// <summary>
	/// Checks the loot and if we have loot, sets the goal location the bank
	/// </summary>
	public void checkLootAndDeliver() {
		if (this.recoveredLoot > 0) { 
			this.goalLocation = Locations.Location.Bank;
		}
	}

	public void issueGreeting() {
		Debug.Log("HOWDY!");
	}

	/// <summary>
	/// Function to swap Wyatts Location randomly, exluding the outlaw camp and the special on the move location
	/// </summary>
	public void swapLocationRandomly() {
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
