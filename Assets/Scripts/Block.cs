using UnityEngine;
using System.Collections;

public enum ToolType { AXE, SHOVEL, PICKAXE }

public class Block : MonoBehaviour {

	public new string name;
	public ToolType specialistTool;
	public float health;
	public Transform particle;
	public bool needsSupport;
	public Transform dropBlock;
	public Item item;

	BlockLibrary blockLibrary;
	ChunkManager chunkManager;

	Chunk chunk;

	void Start () {
		blockLibrary = GameObject.Find ("WorldGenerator").GetComponent<BlockLibrary> ();
		chunkManager = GameObject.Find ("WorldGenerator").GetComponent<ChunkManager> ();
		transform.name = name;
		if (name == "Sapling") {
			chunkManager.DestroyDirectBlock (transform.position.x, transform.position.y, false);
			chunkManager.PlaceDirectBlock(transform.position.x, transform.position.y, blockLibrary.logBottom);
			chunkManager.PlaceDirectBlock(transform.position.x, transform.position.y + 1, blockLibrary.log);
			chunkManager.PlaceDirectBlock(transform.position.x, transform.position.y + 2, blockLibrary.log);
			chunkManager.PlaceDirectBlock(transform.position.x - 1, transform.position.y + 3, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x - 1, transform.position.y + 4, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x - 1, transform.position.y + 5, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x, transform.position.y + 3, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x, transform.position.y + 4, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x, transform.position.y + 5, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x + 1, transform.position.y + 3, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x + 1, transform.position.y + 4, blockLibrary.leaves);
			chunkManager.PlaceDirectBlock(transform.position.x + 1, transform.position.y + 5, blockLibrary.leaves);
		}
	}

	public void Explode() {
		Transform t = Instantiate (particle, transform.position, Quaternion.identity) as Transform;
		t.GetComponent<Rigidbody2D> ().AddForce (new Vector2(Random.Range (-5, 5), 10) * 20f);
		t.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
	}
}
