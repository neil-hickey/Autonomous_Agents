using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using Lean;

[System.Serializable]
public class PlayerManager : MonoBehaviour {
	public static PlayerManager instance = null;

	public static PlayerManager Instance {get{return instance;}}

	public const int SIGHT_BOUNDARY = 5, HEARING_BOUNDARY = 15;

	/// <summary>
	/// list of players (agents) in the game
	/// </summary>
	public List<GameObject> players;

	/// <summary>
	/// pairing of gameobject to its script, makes things a bit easier later on 
	/// </summary>
	public Dictionary<GameObject, Agent> playerScriptPairings = new Dictionary<GameObject, Agent>();

	public GameObject debugPrefab;

	// Use this for initialization
	void Awake () {
		instance = this;
	}

	/// <summary>
	/// Spawns the players in the game from the players List
	/// creates a new gameobject using lean
	/// </summary>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void spawnPlayers<T>() where T : Agent {
		for (int i = 0; i < players.Count; i++) {
			players[i] = LeanPool.Spawn (players[i]);

			foreach (T entry in players[i].GetComponents<T> ()) {
				playerScriptPairings[players[i]] = players[i].GetComponent<T> ();
			}
		}
	}

	/// <summary>
	/// Update this instance. Update is called once per frame
	/// </summary>
	public void Update () {
		updatePlayers ();
	}

	/// <summary>
	/// Updates the players.
	/// </summary>
	public void updatePlayers() {
		updateMovement ();
		updateSenses ();
	}

	/// <summary>
	/// Updates the senses.
	/// </summary>
	public void updateSenses() {
		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			entry.Value.checkSenses ();
		}
	}
		
	/// <summary>
	/// Perform sensing for each agent
	/// Each agent finds the A star path to another agent, and if its cost is within the boundary of hearing it can hear it!  
	/// Publishes a sense event to the agent with the information
	/// </summary>
	/// <param name="agent">Agent.</param>
	public void senseAgents(Agent agent) {
		Vector3 agentPosition = agent.currentPosition;

		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			if (agent.GetType() != entry.Value.GetType()) {
				// sight
				Dictionary<Agent.Direction, RaycastHit2D[]> seenObjects = agent.RayCast (agent.GetType().ToString(), true);
				checkForPlayers (seenObjects, entry.Value);

				// hearing
				List<AStarNode> soundSensing = new AStar(AStar.ASTAR_CHOICES.HEARING, HEARING_BOUNDARY)
					.findPath (agentPosition, entry.Value.currentPosition);
				
				if (soundSensing.Count > 0 && 
					soundSensing [soundSensing.Count - 1].position == entry.Value.currentPosition) {
					// publish event to both parties... and remove other party from next iterations
					agent.SenseEventOccured(new SenseEvent(SenseEvent.SenseType.HEARING, entry.Value.currentPosition));
				}
			}
		}
	}

	/// <summary>
	/// Checks the four directions for any agents hit by raycasting, within a bound (factoring in sight cost)
	/// </summary>
	/// <param name="objs">Objects.</param>
	/// <param name="currAgent">Curr agent.</param>
	public void checkForPlayers(Dictionary<Agent.Direction, RaycastHit2D[]> objs, Agent currAgent) {
		if (objs.Count > 0) {
			foreach(KeyValuePair<Agent.Direction, RaycastHit2D[]> entry in objs) {
				float totalSightCost = 0.0f;

				foreach (RaycastHit2D hitObj in entry.Value) {
					Vector3 pos = hitObj.transform.position;
					Node node = BoardManager.Instance._nodes [(int)pos.x, (int)pos.y];
					totalSightCost += node.sightCost;

					if (totalSightCost < SIGHT_BOUNDARY) {
						// if we are still within the boundary, we can see an agent

						GameObject go = hitObj.collider.gameObject;
						if (PlayerManager.Instance.playerScriptPairings.ContainsKey (go)) {
							// if the hit object is an agent, publish an event

							Agent agent;
							PlayerManager.Instance.playerScriptPairings.TryGetValue (go, out agent);

							// publish the event of seeing another agent within the bound
							currAgent.SenseEventOccured (new SenseEvent (SenseEvent.SenseType.SIGHT, hitObj.transform.position));
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Updates the movement of all players in the gameworld 
	/// </summary>
	private void updateMovement() {
		foreach (KeyValuePair<GameObject, Agent> entry in playerScriptPairings) {
			if (!isPositionSame(entry.Key.transform.position, entry.Value.currentPosition)) {
				entry.Key.transform.position = entry.Value.currentPosition;
			}
		}
	}

	private bool isCoLocated(Locations.Location loc1, Locations.Location loc2) {
		return loc1 == loc2;
	}

	private bool isPositionSame(Vector3 pos1, Vector3 pos2) {
		return pos1 == pos2;
	}

	/// <summary>
	/// Function to check for other agents at the players location
	/// </summary>
	/// <returns>The agents at my location.</returns>
	/// <param name="agent">Agent.</param>
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

