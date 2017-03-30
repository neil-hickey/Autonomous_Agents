using UnityEngine;
using System.Collections;

abstract public class State {

	abstract public void Enter (Agent agent);
	abstract public void Execute (Agent agent);
	abstract public void Exit (Agent agent);
}