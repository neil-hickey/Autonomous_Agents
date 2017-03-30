using UnityEngine;

public class Bob : Agent {
	public static int WAIT_TIME = 5;
	public int waitedTime = 0;
	public int createdTime = 0;

//	public Locations.Location currentLocation = Locations.Location.GoldMine; // where to start bob

	public void Awake () {
		this.stateMachine = new StateMachine();
		this.stateMachine.Init(this, WaitState.Instance, BobGlobalState.Instance);
		Jesse.OnBankRobbery += RobberyOccured; // subscribe to robbery events
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
		
	public override void ChangeState (State state) {
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.stateMachine.Update();
	}

	public override void SenseEventOccured(SenseEvent theEvent) {

	}

	public void swapLocation() {
		this.currentPosition = Locations.dictionary [this.currentLocation];

		switch (this.currentLocation) {
		case Locations.Location.Bank:
			// if we are in the bank, lets move randomly (with equal prob) to one of the places we can
			if (Random.Range (0, 2) == 1) {
				this.currentLocation = Locations.Location.GoldMine;
				this.goalPosition = Locations.dictionary [Locations.Location.GoldMine];
			} else {
				this.currentLocation = Locations.Location.Shack;
				this.goalPosition = Locations.dictionary [Locations.Location.Shack];
			}
			break;
		case Locations.Location.Shack:
			this.currentLocation = Locations.Location.GoldMine;
			this.goalPosition = Locations.dictionary [Locations.Location.GoldMine];
			break;
		case Locations.Location.GoldMine:
			this.currentLocation = Locations.Location.Bank;
			this.goalPosition = Locations.dictionary [Locations.Location.Bank];
			break;
		default:
			// we managed to move to a place we shouldnt? move back to GoldMine!
			this.currentLocation = Locations.Location.GoldMine;
			this.goalPosition = Locations.dictionary [Locations.Location.GoldMine];
			break;
		} // end switch

	}

	public void RobberyOccured () {
		Debug.Log ("Bob is aware of the robbery by jessie!!!!");
	}
}
