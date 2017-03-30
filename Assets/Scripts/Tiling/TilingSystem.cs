using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Lean; //from Unity asset "LeanPool" - freely available in the Asset Store; used here for object pooling

public class TilingSystem : MonoBehaviour {

	public List<TileSprite> TileSprites;
	public Vector2 MapSize;
	public Sprite DefaultImage;
	public GameObject TileContainerPrefab;
	public GameObject TilePrefab;
	public Vector2 CurrentPosition;
	public Vector2 ViewPortSize;
	private TileSprite[,] _map;
	private GameObject _tileContainer;
	private List<GameObject> _tiles = new List<GameObject>();

	//create a map of size MapSize of unset tiles
	private void DefaultTiles() {
		for (var y = 0; y < MapSize.y - 1; y++) {
			for (var x = 0; x < MapSize.x - 1; x++) {
				_map[x, y] = new TileSprite("Plain", DefaultImage, Tiles.Plain);
			}
		}
	}
				
	//set the tiles of the map to what is specified in TileSprites
	private void SetTiles() {
		var index = 0;
		for (var y = 0; y < MapSize.y - 1; y++) {
			for (var x = 0; x < MapSize.x - 1; x++) {
				_map[x, y] = new TileSprite(TileSprites[index].Name, TileSprites[index].TileImage, TileSprites[index].TileType);
				index++;
				if (index > TileSprites.Count - 1) 
					index = 0;
			}
		}
	}

	private void AddTilesToMap() {

		foreach (GameObject o in _tiles) {
			LeanPool.Despawn(o);
		}
		_tiles.Clear();
		LeanPool.Despawn(_tileContainer);
		_tileContainer = LeanPool.Spawn(TileContainerPrefab);

		var tileSize = 0.5f;
		var viewOffsetX = ViewPortSize.x;
		var viewOffsetY = ViewPortSize.y;

		for (var y = -viewOffsetY; y < viewOffsetY; y++) {
			for (var x = -viewOffsetX; x < viewOffsetX; x++) {
				var tX = x * tileSize;
				var tY = y * tileSize;

				var iX = x + CurrentPosition.x;
				var iY = y + CurrentPosition.y;

				if (iX < 0)
					continue;
				if (iY < 0)
					continue;
				if (iX > MapSize.x - 2)
					continue;
				if (iY > MapSize.y - 2)
					continue;

				var tile = LeanPool.Spawn (TilePrefab);
				tile.transform.position = new Vector3 (tX, tY, 0);
				tile.transform.SetParent (_tileContainer.transform);
				
				//set an image for the tile - do this when you create your tile prefabs (for shack, mountains, ...)
				var renderer = tile.GetComponent<SpriteRenderer> ();
				renderer.sprite = _map [(int)x + (int)CurrentPosition.x, (int)y + (int)CurrentPosition.y].TileImage;
				
				_tiles.Add (tile);
			}
		}
	}

	public void Start() {
		_map = new TileSprite[(int)MapSize.x, (int)MapSize.y];
		DefaultTiles ();
		SetTiles ();
		AddTilesToMap ();
	}

	public void Update() {
		AddTilesToMap ();
	}

	private TileSprite FindTile(Tiles tile) {
		foreach (TileSprite tileSprite in TileSprites) {
			if (tileSprite.TileType == tile) return tileSprite;
		}
		return null;
	}
}
