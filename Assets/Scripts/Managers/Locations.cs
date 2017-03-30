using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Locations {

	public enum Location {Bank, OutLawCamp, Cemetery, Shack, SheriffsOffice, GoldMine, Saloon, Undertakers, OnTheMove}

	public static Dictionary<Location, Vector3> dictionary = new Dictionary<Location, Vector3>();

	public Locations() {
		BoardManager.OnBuildingCreate += BuildingCreated; // subscribe to building events
	}

	public void BuildingCreated(Locations.Location loc, Vector3 pos) {
		dictionary.Add (loc, pos);
	}
}

