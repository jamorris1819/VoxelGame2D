using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public float jumpSpeed;
	public Camera cam;
	public Inventory inventory;
	public Transform currentItem;
	public Transform currentBlock;
	public Item item;

	BlockLibrary blockLibrary;
	ChunkManager chunkManager;
	Transform savedCurrentItem;

	Transform[,] blocks;

	void Start () {
		blockLibrary = GameObject.Find ("WorldGenerator").GetComponent<BlockLibrary> ();
		chunkManager = GameObject.Find ("WorldGenerator").GetComponent<ChunkManager> ();
		/*if (currentItem != null) {
			savedCurrentItem = Instantiate(currentItem);
			savedCurrentItem.SetParent(transform.Find ("arm left"));
			savedCurrentItem.localPosition = new Vector3(0f, -0.5f, 0);
			savedCurrentItem = currentItem;
		}*/
		UpdateBlocks ();
	}

	void UpdateBlocks() {
		//blocks = GameObject.Find ("WorldGenerator").GetComponent<Generator> ().Blocks;
	}

	void FixedUpdate () {
		if (currentItem != savedCurrentItem) {
			Destroy(savedCurrentItem as Transform);
			savedCurrentItem = currentItem;
			savedCurrentItem = Instantiate(currentItem);
			savedCurrentItem.SetParent(transform.Find ("arm left"));
			savedCurrentItem.localPosition = new Vector3(0f, -0.5f, 0);
			savedCurrentItem = currentItem;
		}

		GetComponent<Rigidbody2D>().AddForce (Vector2.right * Input.GetAxisRaw ("Horizontal") * speed);
		if (Input.GetKey (KeyCode.W) && GetComponent<Rigidbody2D>().velocity.y == 0)
			GetComponent<Rigidbody2D> ().AddForce (Vector2.up * jumpSpeed);
		GetComponent<Animator>().SetBool("Walking", (Mathf.Abs (Input.GetAxisRaw ("Horizontal")) == 1f));
		if(Input.GetAxisRaw ("Horizontal") == 1)
			transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));
		else if(Input.GetAxisRaw ("Horizontal") == -1)
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			Vector2 clickPosOriginal = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector2 transformPos = new Vector2 (transform.position.x, transform.position.y);
			Vector3 clickPos = new Vector3 (Mathf.Round (clickPosOriginal.x), Mathf.Round (clickPosOriginal.y), 0f);
			Chunk chunk = chunkManager.GetChunk(clickPosOriginal.x).GetComponent<Chunk>();
			float distance = Vector2.Distance (clickPosOriginal, transformPos);
			if(distance <= 4f)
				chunkManager.DamageBlock (clickPos.x, clickPos.y);
		} else if (Input.GetMouseButtonDown (1)) {
			Vector2 clickPosOriginal = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector2 transformPos = new Vector2 (transform.position.x, transform.position.y);
			Vector3 clickPos = new Vector3 (Mathf.Round (clickPosOriginal.x), Mathf.Round (clickPosOriginal.y), 0f);
			Chunk chunk = chunkManager.GetChunk(clickPosOriginal.x).GetComponent<Chunk>();
			float distance = Vector2.Distance (clickPosOriginal, transformPos);
			if(distance <= 4f && item != null) {
				if(inventory.NumOfItem(item) > 0) {
					chunkManager.PlaceBlock (clickPos.x, clickPos.y, item.placeableBlock);
					inventory.ExpendItem (item);
				}
			}
		}
	}
}
