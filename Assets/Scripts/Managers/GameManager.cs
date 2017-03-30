using UnityEngine;
using System.Collections;
using Lean;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public BoardManager boardScript; //Store a reference to our BoardManager which will set up the level
	public PlayerManager playersScript;

	// Awake is always called before any Start functions
	void Awake() {
		// Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		// If instance already exists and it's not this:
		else if (instance != this)

			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		// Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		// Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();
		BoardManager.instance = boardScript;

		playersScript = GetComponent<PlayerManager>();

//		PlayerManager.Instance.debugMovement = true;
	
		new Locations ();

		// Call the InitGame function to initialize the first level 
		InitGame();
	}

	// Initializes the game for each level.
	void InitGame() {
		// setup the map and game objects
		boardScript.SetupScene();

		// give a star a ref to the map
		AStar.boardManager = boardScript;

		// spawn our players
		playersScript.spawnPlayers<Agent> ();
	}

	// this is called every frame
	void Update() {
		// update players
		playersScript.updatePlayers();

	}

}