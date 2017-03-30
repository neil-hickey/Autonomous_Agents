using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PrintDebug : MonoBehaviour {
	// some helper functions for debugging this stuff...
	public static void printArray<T>(T nodes, string name) where T : IEnumerable {
		var s = name + ": ";
		foreach (Node element in nodes) {
			s += element.toString() + " ";
		}
		Debug.logger.Log (s);
	}

	public static void printDict<K,V>(Dictionary<K,V> dict) where K : Node {
		foreach (KeyValuePair<K, V> entry in dict) {
			Debug.logger.Log ("K: " + entry.Key.toString() + " V:" + entry.Value);
		}
	}
}

