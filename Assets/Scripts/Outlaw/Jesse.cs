using UnityEngine;
using System.Collections.Generic;

public class Jesse : Agent {

	// Delegates / events
	public delegate void bankRobbery();
	public static event bankRobbery OnBankRobbery;

	public static int WAIT_TIME = 5;
	public int waitedTime = 0, createdTime = 0, totalLoot = 0;

	public void Awake () {
		init ();
	}
		
	/**
	 * Initialization function for Jesse
	 * 	Resets the instance variables and resets the statemachine
	 */ 
	public void init() {
		this.isAlive = true;
		this.totalLoot = 0;
		this.createdTime = 0;
		this.waitedTime = 0;
		this.currentLocation = Locations.Location.OutLawCamp; // where to start jesse
		this.currentPosition = Locations.dictionary [Locations.Location.OutLawCamp];
		this.stateMachine = new StateMachine();
		this.stateMachine.Init(this, LurkState.Instance, JesseGlobalState.Instance);
	}

	public void IncreaseWaitedTime (int amount) {
		this.waitedTime += amount;
	}

	public bool WaitedLongEnough () {
		return this.waitedTime >= WAIT_TIME;
	}

	public void CreateTime () {
		this.createdTime++;
		this.waitedTime = 0;
	}

	/**
	 * Rob the bank of a random amount
	 *  Publishes the event to any subscribers, notifying them of the robbery
	 *  @return int amount - random int in range (1,10) which Jesse has stolen
	 */ 
	public int robBank() {
		if (OnBankRobbery != null)
			// broadcast the robbery to subscribers
			OnBankRobbery ();

		int amount = Random.Range (1, 10);
		this.totalLoot += amount;
		Debug.Log("Robbed the bank, now have gold: " + this.totalLoot);
		return amount;
	}

	public override void ChangeState (State state) {
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		if (!this.isAlive) {
			// if Jesse dies, reset the agent
			init ();
		} else {
			this.stateMachine.Update ();
		}
	}

	public override void checkSenses() {
		// sight 
//		List<RaycastHit2D> thingsInView = RayCast ("Jesse");
//		checkSight (thingsInView);

		PlayerManager.Instance.senseAgents (this);
	}

	public void checkSight(List<RaycastHit2D> thingsInView) {
		if (thingsInView.Count > 0) {
			foreach (RaycastHit2D hitRayCast in thingsInView) {
				GameObject hitGameObject = hitRayCast.collider.gameObject;
				if (PlayerManager.Instance.playerScriptPairings.ContainsKey (hitGameObject)) {
					Agent agent;
					PlayerManager.Instance.playerScriptPairings.TryGetValue (hitGameObject, out agent);

					if (agent is Wyatt) {
//						SenseEvent = new SenseEvent(SenseEvent.SenseType.SIGHT
						
					}
				}
			}
		}
	}

	public override void SenseEventOccured(SenseEvent theEvent) {
		switch (theEvent.senseType) {
		case SenseEvent.SenseType.HEARING:
			Debug.Log("Jesse can hear something coming from: " + theEvent.sourcePosition);
			break;
		default:
			break;
		}
	}
		
	/**
	 * Helper function to move Jesse to the bank.
	 */
	public void moveToBank() {
		this.goalLocation = Locations.Location.Bank;
	}

	/**
	 * Swap Jesse's location to:
	 *  1) OutlawCamp / Cemetery from the bank
	 *  2) Swap between cemetery and outlawcamp
	 */ 
	public void swapLocation() {
		switch (this.currentLocation) {
		case Locations.Location.Bank:
			// if we are in the bank, lets move randomly (with equal prob) to one of the places we can
			if (Random.Range (0, 2) == 1) {
				this.goalLocation = Locations.Location.OutLawCamp;
			} else {
				this.goalLocation = Locations.Location.Cemetery;
			}
			break;
		case Locations.Location.Cemetery:
			this.goalLocation = Locations.Location.OutLawCamp;
			break;
		case Locations.Location.OutLawCamp:
			this.goalLocation = Locations.Location.Cemetery;
			break;
		default:
			// we managed to move to a place we shouldnt? move back to cemetery!
			this.goalLocation = Locations.Location.Cemetery;
			break;
		} // end switch
	} // end swapLocation()
}
