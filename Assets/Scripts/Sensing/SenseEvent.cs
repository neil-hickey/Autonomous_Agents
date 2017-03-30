using UnityEngine;
using System.Collections;

public class SenseEvent
{
	public enum SenseType {SIGHT, HEARING, SMELL};
	public SenseType senseType;
	public int intensity;
	public Vector3 sourcePosition;

	public SenseEvent(SenseType senseType, int intensity, Vector3 sourcePosition) {
		this.senseType = senseType;
		this.intensity = intensity;
		this.sourcePosition = sourcePosition;
	}
}

