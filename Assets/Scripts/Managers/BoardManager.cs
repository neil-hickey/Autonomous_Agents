using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Lean; //from Unity asset "LeanPool" - freely available in the Asset Store; used here for object pooling

public class BoardManager : MonoBehaviour {
	public static BoardManager instance = null;
	public static BoardManager Instance { get { return instance; } set { instance = value; } }

	// constants 
	public const float BUILDING_MOVEMENT_COST = 500.0f;
	public const float TERRAIN_MOVEMENT_COST = 5.0f, TERRAIN_SOUND_COST = 1.0f;
	public const float MOUNTAIN_MOVEMENT_COST = 15.0f, MOUNTAIN_SOUND_COST = 20.0f;
	public const float tileSize = 1f;

	// Delegates / events
	public delegate void createBuilding(Locations.Location loc, Vector3 pos);
	public static event createBuilding OnBuildingCreate;
			
	// ivars
	private Transform boardHolder;
	private MapGenerator mapGenerator;

	public Vector2 MapSize;
	public GameObject[] floorTiles, mountainTiles, outerWallTiles;
	public GameObject shack, mine, bank, sherrifsOffice, outlawCamp, cemetery, saloon, undertakers;
	public GameObject ground;
	public GameObject debugTile;

	private List <Vector3> gridPositions = new List<Vector3>(); 

	// the map
	public Node[,] _nodes { get; set; }

	// debugging 
	public GameObject[,] _groundTiles;
	public int[,] _debugMovement {get;set;}


	/*
	 * Clears our list gridPositions and prepares it to generate a new board.
	 */
	void InitialiseList () {
		_nodes = new Node[(int) MapSize.x, (int)MapSize.y];
		_groundTiles = new GameObject[(int) MapSize.x, (int)MapSize.y];
		_debugMovement = new int[(int) MapSize.x, (int)MapSize.y];
	
		gridPositions.Clear ();

		for (int x = 0; x < MapSize.x; x++) {
			for (int y = 0; y < MapSize.y; y++) {
				gridPositions.Add (new Vector3(x, y, 0f));
				_nodes [x, y] = new Node (new Vector3(x, y, 0f));
				_debugMovement [x, y] = 0;
			}
		}
	}

	/* 
	 * Sets up the outer walls and floor (background) of the game board.
	 */
	void BoardSetup () {

		// ensure we despawn the threads if they exist... 
		for (int i = 0; i < MapSize.x; i++) {
			for (int j = 0; j < MapSize.y; j++) {
				if (_nodes [i, j].obj != null) {
					LeanPool.Despawn (_nodes [i, j].obj);
				}
			}
		}
		LeanPool.Despawn(boardHolder);

		// Instantiate Board and set boardHolder to its transform.
		Transform _boardHolder = new GameObject ("Board").transform;

		boardHolder = LeanPool.Spawn(_boardHolder);

		// Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for (int x = -1; x < MapSize.x + 1; x++) {
			// Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for (int y = -1; y < MapSize.y + 1; y++) {

				var tX = x * tileSize;
				var tY = y * tileSize;

				// Choose a random floor tile
				GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

				// Check if we current position is at board edge, 
				// if so choose a random outer wall prefab from our array of outer wall tiles.
				if (x < 0 || x >= MapSize.x || y < 0 || y >= MapSize.y) {
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				
					// lets add ground under our cactus' shall we 
					var groundTile = LeanPool.Spawn (ground);
					groundTile.transform.position = new Vector3 (tX, tY, 0);
					groundTile.transform.SetParent (boardHolder);
				}
					
				var tile = LeanPool.Spawn (toInstantiate);
				tile.transform.position = new Vector3 (tX, tY, 0);

				// Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				tile.transform.SetParent (boardHolder);

				if (x >= 0 && x < MapSize.x && y >= 0 && y < MapSize.y) {
					_nodes [x, y].obj = tile;
					var _debugTile = LeanPool.Spawn (debugTile);
					_debugTile.transform.position = new Vector3 (tX, tY, 0);

					_groundTiles [x, y] = _debugTile;
					_nodes [x, y].position = tile.transform.position;
					setupCosts (_nodes [x, y], TERRAIN_MOVEMENT_COST, TERRAIN_SOUND_COST);
//					_nodes [x, y].moveCost = TERRAIN_MOVEMENT_COST;
				}
			}
		}
	}


	/**
	 * RandomPosition returns a random position from our list gridPositions.
	 */
	Vector3 RandomPosition () {
		// Declare an integer randomIndex, set it's value to a random number 
		// between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);
	
		Vector3 randomPosition = gridPositions[randomIndex];
	
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}


	void LayoutMountains() {
		int removed = 0;
		for (int x = 0; x < MapSize.x; x++) {
			for (int y = 0; y < MapSize.y; y++) {
				if (mapGenerator.map [x, y] == 0) {
					Node mountainNode = LayoutTile (new Vector3 (x, y, 0), mountainTiles);
					setupCosts (mountainNode, MOUNTAIN_MOVEMENT_COST, MOUNTAIN_SOUND_COST);

					// bit of fancy 2D to 1D array conversions..
					// basically -> if x = 1, y = 1, its actually position 11 if mapysize is 10
					// but hey its a 1d and we are removing.. so its shrinking, account for that!
					gridPositions.RemoveAt (((x * ((int)MapSize.y)) + y) - removed);
					removed++;
				}
			}
		}
	}
		
	Node LayoutTile (Vector3 pos, GameObject[] tiles) {
		var tX = pos.x * tileSize;
		var tY = pos.y * tileSize;

		// Choose a random tile from tileArray and assign it to tileChoice
		GameObject tileChoice = tiles[Random.Range (0, tiles.Length)];

		GameObject tile = LeanPool.Spawn (tileChoice);
		tile.transform.position = new Vector3 (tX, tY, 0);

		_nodes [(int) pos.x, (int) pos.y].obj = tile;
		_nodes [(int)pos.x, (int)pos.y].position = pos;

		// Set the parent of our newly instantiated object instance to boardHolder, 
		// this is just organizational to avoid cluttering hierarchy.
		tile.transform.SetParent (boardHolder);

		return _nodes [(int) pos.x, (int) pos.y];
	}

	/*
	 * LayoutBuildingAtRandom accepts a game object and places it randomly
	 */
	Node LayoutBuildingAtRandom (GameObject tileChoice, Locations.Location loc) { 
		Vector3 randomPosition = RandomPosition();

		GameObject[] _tileChoice = new GameObject[1];
		_tileChoice [0] = tileChoice;
		Node _node = LayoutTile(randomPosition, _tileChoice);

		if (OnBuildingCreate != null) {
			OnBuildingCreate (loc, randomPosition);
		}

		return _node;
	}

	private void setupCosts(Node node, float moveCost, float soundCost) {
		node.moveCost = moveCost;
		node.soundCost = soundCost;
	}

	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene () {
		// Reset our list of gridpositions.
		InitialiseList ();

		// Creates the outer wall (cactus') and grounds.
		BoardSetup ();

		// generate our mountain map shall we? :D
		this.mapGenerator = new MapGenerator ((int)MapSize.x, (int)MapSize.y, "57.08", false, 38);

		// layout our assets at random 
		LayoutMountains();
		Node shackNode = LayoutBuildingAtRandom (shack, Locations.Location.Shack);
		setupCosts (shackNode, BUILDING_MOVEMENT_COST, 10.0f);
		Node mineNode = LayoutBuildingAtRandom (mine, Locations.Location.GoldMine);
		setupCosts (mineNode, BUILDING_MOVEMENT_COST, 10.0f);
		Node bankNode = LayoutBuildingAtRandom (bank, Locations.Location.Bank);
		setupCosts (bankNode, BUILDING_MOVEMENT_COST, 10.0f);
		Node sheriffOfficeNode = LayoutBuildingAtRandom (sherrifsOffice, Locations.Location.SheriffsOffice);
		setupCosts (sheriffOfficeNode, BUILDING_MOVEMENT_COST, 10.0f);
		Node outlawCampNode = LayoutBuildingAtRandom (outlawCamp, Locations.Location.OutLawCamp);
		setupCosts (outlawCampNode, BUILDING_MOVEMENT_COST, 10.0f);
		Node cemeteryNode = LayoutBuildingAtRandom (cemetery, Locations.Location.Cemetery);
		setupCosts (cemeteryNode, BUILDING_MOVEMENT_COST, 20.0f);
		Node saloonNode = LayoutBuildingAtRandom (saloon, Locations.Location.Saloon);
		setupCosts (saloonNode, BUILDING_MOVEMENT_COST, 4.0f);
		Node undertakerNode = LayoutBuildingAtRandom (undertakers, Locations.Location.Undertakers);
		setupCosts (undertakerNode, BUILDING_MOVEMENT_COST, 10.0f);
	}
}

