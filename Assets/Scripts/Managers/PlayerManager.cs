using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Lean;

[System.Serializable]
public class PlayerManager : MonoBehaviour {
	public static PlayerManager instance = null;
	public static PlayerManager Instance {get{return instance;}}

	// list of players (agents) in the game
	public List<GameObject> players;

	// pairing of gameobject to its script, makes things a bit easier later on 
	public Dictionary<GameObject, Agent> playerScriptPairings = new Dictionary<GameObject, Agent>();

	public bool debugMovement { get; set;}
	public GameObject debugPrefab;

	// Use this for initialization
	void Awake () {
		instance = this;
		this.debugMovement = true;
	}

	/**
	 * Spawns the players in the game from the players List
	 *  creates a new gameobject using lean
	 */
	public void spawnPlayers<T>() where T : Agent {
		for (int i = 0; i < players.Count; i++) {
			players[i] = LeanPool.Spawn (players[i]);

			foreach (T entry in players[i].GetComponents<T> ()) {
				playerScriptPairings[players[i]] = players[i].GetComponent<T> ();
			}
		}
	}

	// Update is called once per frame
	public void Update () {
		if (debugMovement) {
			for (int i = 0; i < BoardManager.Instance.MapSize.x; i++) {
				for (int j = 0; j < BoardManager.Instance.MapSize.y; j++) {
					GameObject o = BoardManager.Instance._groundTiles [i, j];
					if (BoardManager.Instance._debugMovement [i, j] > 0) {
						o.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 0.0f, 0.0f, 0.5f);
					} else {
						o.GetComponent<SpriteRenderer> ().color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
					}
				}
			}

//			for (int i = 0; i < players.Count; i++) {
//				int debugMove = BoardManager.Instance._debugMovement [(int)playerScriptPairings [players[i]].currentPosition.x, (int)playerScriptPairings [players[i]].currentPosition.y];
//				if (debugMove > 0) {
//					GameObject o = BoardManager.Instance._groundTiles[(int)playerScriptPairings [players[i]].currentPosition.x, (int)playerScriptPairings [players[i]].currentPosition.y];
//					o.GetComponent<SpriteRenderer> ().color = new Color (debugMove, 0.0f, 0.0f, 0.5f);
//				}
//			}
		}
	}

	public void updatePlayers() {
		updateMovement ();
		updateSenses ();
	}

	/**
	 * function to ask each agent to update it senses
	 */
	public void updateSenses() {
		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			entry.Value.checkSenses ();
		}
	}
		
	/**
	 * Perform sensing for each agent
	 *  Currently implements Hearing...
	 *  Each agent finds the A star path to another agent, and if its cost is within the boundary of hearing 
	 *   it can hear it! 
	 *   Publishes a sense event to the agent with the information
	 */ 
	public void senseAgents(Agent agent) {
		Vector3 agentPosition = agent.currentPosition;

		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			if (agent.GetType() != entry.Value.GetType()) {
				List<Node> soundSensing = new AStar(AStar.ASTAR_CHOICES.HEARING, 15).findPath (agentPosition, entry.Value.currentPosition);
				if (soundSensing.Count > 0 && soundSensing [soundSensing.Count - 1].position == entry.Value.currentPosition) {
					// publish event to both parties... and remove other party from next iterations
					agent.SenseEventOccured(new SenseEvent(SenseEvent.SenseType.HEARING, 1, entry.Value.currentPosition));
				}
			}
		}
	}

	/**
	 * Updates the movement of all players in the gameworld 
	 */
	private void updateMovement() {
		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			if (!isPositionSame(entry.Key.transform.position, entry.Value.currentPosition)) {
				entry.Key.transform.position = entry.Value.currentPosition;
//				this.boardManager._debugMovement [(int)entry.Value.currentPosition.x, (int)entry.Value.currentPosition.y] += 1;
			}
		}
	}

	private bool isCoLocated(Locations.Location loc1, Locations.Location loc2) {
		return loc1 == loc2;
	}

	private bool isPositionSame(Vector3 pos1, Vector3 pos2) {
		return pos1 == pos2;
	}

	/**
	 * Function to check for other agents at the players location
	 * 
	 * @param Agent agent - the agent asking for other agents at its location
	 * @return List<Agent> agents - returns a list of other agents at its location
	 */
	public List<Agent> getAgentsAtMyLocation(Agent agent) {
		List<Agent> agents = new List<Agent> ();
		Locations.Location agentLocation = agent.currentLocation;

		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			if (isCoLocated(entry.Value.currentLocation, agentLocation) && 
					agent.GetType() != entry.Value.GetType()) 
			{
				agents.Add(entry.Value);
			}
		}
		return agents;
	}
}

