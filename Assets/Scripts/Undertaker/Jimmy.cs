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
		RayCast ("Jimmy");
		this.stateMachine.Update();
	}

	public override void SenseEventOccured(SenseEvent theEvent) {

	}

//	public override void RayCast() {
//		Node[,] graph = GameManager.instance.boardScript._nodes;
//		Node currNode = graph[(int)this.currentPosition.x, (int)this.currentPosition.y];
//
//		List<Node> neighbours = currNode.getNeighbours (graph, 2);
//		foreach (Node neighbor in neighbours) {
//			Debug.DrawLine (this.currentPosition, neighbor.position, Color.blue);
//			var layerMask = ~( (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Jimmy")) );
//
//			spotted = Physics2D.Linecast (this.currentPosition, neighbor.position, layerMask);
//		}
//	}

}