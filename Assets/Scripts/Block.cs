using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public new string name;
	public string specialistTool;
	public float health;
	public Transform particle;
	public bool needsSupport;

	BlockLibrary blockLibrary;
	ChunkManager chunkManager;

	Chunk chunk;

	void Start () {
		blockLibrary = GameObject.Find ("WorldGenerator").GetComponent<BlockLibrary> ();
		chunkManager = GameObject.Find ("WorldGenerator").GetComponent<ChunkManager> ();
		/*if (name == "Sapling") {
			// this tree must grow!
			Vector2 pos = new Vector2(Mathf.Floor (transform.position.x), Mathf.Floor (transform.position.y));
			Debug.Log (chunkManager.BlockToWorldPosX(pos.x));
			//if(generator.GrowingRoom(pos.x, pos.y)){
				chunkManager.DestroyBlock (pos.x, pos.y, false);
				chunkManager.PlaceBlock(chunkManager.BlockToWorldPosX(pos.x), chunkManager.GetChunk (pos.x).InverseTransformPoint (new Vector2(0, pos.y)).y, blockLibrary.logBottom);
				chunkManager.PlaceBlock(pos.x, pos.y + 1, blockLibrary.log);
				chunkManager.PlaceBlock(pos.x, pos.y + 2, blockLibrary.log);
				chunkManager.PlaceBlock(pos.x - 1, pos.y + 3, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x - 1, pos.y + 4, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x - 1, pos.y + 5, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x, pos.y + 3, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x, pos.y + 4, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x, pos.y + 5, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x + 1, pos.y + 3, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x + 1, pos.y + 4, blockLibrary.leaves);
				chunkManager.PlaceBlock(pos.x + 1, pos.y + 5, blockLibrary.leaves);
			//} else {
			//	generator.DestroyBlock (pos.x, pos.y, null);
			//}
		}*/
	}

	public void Explode() {
		Transform t = Instantiate (particle, transform.position, Quaternion.identity) as Transform;
		t.GetComponent<Rigidbody2D> ().AddForce (new Vector2(Random.Range (-5, 5), 10) * 20f);
		t.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
	}
}
