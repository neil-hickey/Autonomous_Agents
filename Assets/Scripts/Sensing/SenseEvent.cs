using UnityEngine;
using System.Collections;

/// <summary>
/// Defines a SenseEvent
///  - Sense events occur when two agents sense one another, for example if one can hear another etc...
///  - Sense events are handled by the agents themselves
/// </summary>
public class SenseEvent {
	public enum SenseType {SIGHT, HEARING, SMELL};
	public SenseType senseType { get; }
	public Vector3 sourcePosition { get; }

	public SenseEvent(SenseType senseType, Vector3 sourcePosition) {
		this.senseType = senseType;
		this.sourcePosition = sourcePosition;
	}
}

