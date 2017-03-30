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
		
	public void init() {
		Debug.Log ("RESETING NEW OUTLAW!");
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
			init ();
		} else {
			this.stateMachine.Update ();
			checkSenses ();
		}
	}

	public void checkSenses() {
		// sight 
		List<RaycastHit2D> thingsInView = RayCast ("Jesse");
		checkSight (thingsInView);

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
		case SenseEvent.SenseType.SIGHT:
			break;
		default:
			break;
		}
	}


//	public void RayCast() {
////		LookAround (this.currentPosition, 2);
//
//		Node[,] graph = GameManager.instance.boardScript._nodes;
//		Node currNode = graph[(int)this.currentPosition.x, (int)this.currentPosition.y];
//
//		List<Node> neighbours = currNode.getNeighbours (graph, 2);
//		foreach (Node neighbor in neighbours) {
//			if (debugRaycasting)
//				Debug.DrawLine (this.currentPosition, neighbor.position, Color.blue);
//
//			var layerMask = ~( (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Jesse")) );
//
//			spotted = Physics2D.Linecast (this.currentPosition, neighbor.position, layerMask);
//			if (spotted) {
//				RaycastHit2D hitObject = Physics2D.Linecast (this.currentPosition, neighbor.position, layerMask);
//				GameObject obj = hitObject.collider.gameObject;
//
//				if (PlayerManager.instance.playerScriptPairings.ContainsKey (obj)) {
//					Agent agent;
//					PlayerManager.instance.playerScriptPairings.TryGetValue (obj, out agent);
//
//					if (agent is Wyatt) {
////						Debug.LogError ("IM LOOKING @ THE SHERIFF");
//					}
//				}
//
//			}
//		}
//	}

	public void moveToBank() {
		this.goalLocation = Locations.Location.Bank;
	}

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
