using System.Collections;
using System.Collections.Generic;

public class Node{
	public Cell value;
	public List<Node> children;
	public bool discovered = false;
	public Node predecessor;

	public Node(Cell value, Node predecessor){
		this.value = value;
		this.predecessor = predecessor;
		children = new List<Node> ();
	}

	public void addChild(Node child){
		children.Add (child);
	}
}
