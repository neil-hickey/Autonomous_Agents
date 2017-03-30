using UnityEngine;
using System;

[Serializable]
public class Count {
	public int minimum, maximum;

	public Count (int min, int max) {
		minimum = min;
		maximum = max;
	}
}