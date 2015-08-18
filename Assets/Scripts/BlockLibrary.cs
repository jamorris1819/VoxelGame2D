using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockLibrary : MonoBehaviour {
	public Transform dirt;
	public Transform dirtBack;
	public Transform stone;
	public Transform stoneBack;
	public Transform grass;
	public Transform coal;
	public Transform ironOre;
	public Transform gravelDirt;
	public Transform gravelStone;
	public Transform grassLeaves;
	public Transform sapling;
	public Transform log;
	public Transform logBottom;
	public Transform logBlock;
	public Transform leaves;
	public Dictionary<Sprite, Transform> lookup = new Dictionary<Sprite, Transform>();

	void Start() {
		//lookup.Add (dirt.GetComponent<SpriteRenderer> ().sprite, dirt);
		//lookup.Add (logBlock.GetComponent<SpriteRenderer> ().sprite, logBlock);
	}
}
