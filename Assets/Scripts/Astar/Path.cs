using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Path : IEnumerable<Node> {
	public Node LastStep { get; private set; }
	public Path PreviousSteps { get; private set; }
	public double TotalCost { get; private set; }

	private Path(Node lastStep, Path previousSteps, double totalCost) {
		LastStep = lastStep;
		PreviousSteps = previousSteps;
		TotalCost = totalCost;
	}

	public Path(Node start) : this(start, null, 0) {}

	public Path AddStep(Node step, double stepCost) {
		return new Path(step, this, TotalCost + stepCost);
	}

	public IEnumerator<Node> GetEnumerator() {
		for (Path p = this; p != null; p = p.PreviousSteps)
			yield return p.LastStep;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}