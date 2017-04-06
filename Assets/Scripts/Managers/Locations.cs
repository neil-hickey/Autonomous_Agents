using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Locations {

	public enum Location {Bank, OutLawCamp, Cemetery, Shack, SheriffsOffice, GoldMine, Saloon, Undertakers, OnTheMove}

	public static Dictionary<Location, Vector3> dictionary = new Dictionary<Location, Vector3>();

	/// <summary>
	/// Initializes a new instance of the <see cref="Locations"/> class.
	/// </summary>
	public Locations() {
		BoardManager.OnBuildingCreate += BuildingCreated; // subscribe to building events
	}

	/// <summary>
	/// On Building created, add to the locations dictionary
	/// </summary>
	/// <param name="loc">Location.</param>
	/// <param name="pos">Position.</param>
	public void BuildingCreated(Locations.Location loc, Vector3 pos) {
		dictionary.Add (loc, pos);
	}
}

