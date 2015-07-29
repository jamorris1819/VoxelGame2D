using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public float jumpSpeed;
	public Camera cam;

	public Transform currentItem;

	BlockLibrary blockLibrary;
	ChunkManager chunkManager;
	Transform savedCurrentItem;

	Transform[,] blocks;
	Generator generator;

	void Start () {
		generator = GameObject.Find ("WorldGenerator").GetComponent<Generator> ();
		blockLibrary = GameObject.Find ("WorldGenerator").GetComponent<BlockLibrary> ();
		chunkManager = GameObject.Find ("WorldGenerator").GetComponent<ChunkManager> ();
		if (currentItem != null) {
			savedCurrentItem = Instantiate(currentItem);
			savedCurrentItem.SetParent(transform.Find ("arm left"));
			savedCurrentItem.localPosition = new Vector3(0f, -0.5f, 0);
			savedCurrentItem = currentItem;
		}
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

		if (Input.GetMouseButton (0)) {
			/*Vector2 clickPosOriginal = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector2 transformPos = new Vector2 (transform.position.x, transform.position.y);
			Vector3 clickPos = new Vector3 (Mathf.Round (clickPosOriginal.x), Mathf.Round (clickPosOriginal.y), 0f);
			Transform chunk = chunkManager.GetChunk(clickPos.x);
			float distance = Vector2.Distance (clickPos, transformPos);
			clickPos = chunk.TransformPoint (new Vector3 (Mathf.Round (clickPosOriginal.x) - chunk.position.x * 2, Mathf.Round (clickPosOriginal.y), 0f));
			Vector2 direction = clickPosOriginal - transformPos;
			RaycastHit2D rayhit = Physics2D.Raycast (transformPos, direction, distance);
			Collider2D hitObject = rayhit.collider;
			Debug.Log (chunk.position.x);
			Debug.Log (clickPos);*/



			Vector2 clickPosOriginal = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector2 transformPos = new Vector2 (transform.position.x, transform.position.y);
			Vector3 clickPos = new Vector3 (Mathf.Round (clickPosOriginal.x), Mathf.Round (clickPosOriginal.y), 0f);
			Chunk chunk = chunkManager.GetChunk(clickPosOriginal.x).GetComponent<Chunk>();
			float distance = Vector2.Distance (clickPosOriginal, transformPos);
			if(distance <= 4f)
				chunkManager.DamageBlock (clickPos.x, clickPos.y, currentItem);
			else Debug.Log ("Distance: " + distance);




			/*if (hitObject != null) {
				if(distance <= 4f) {
					Vector2 hitCoords = new Vector2 (hitObject.transform.position.x, hitObject.transform.position.y);
					Debug.Log (hitCoords);
					chunk.GetComponent<Chunk>().DestroyBlock (Mathf.Floor (hitCoords.x), Mathf.Floor (hitCoords.y), currentItem);
				}
			} else {
				chunk.GetComponent<Chunk>().DestroyBlock (Mathf.Floor (clickPos.x), Mathf.Floor (clickPos.y), currentItem);
			}*/

		} else if (Input.GetMouseButtonDown (1)) {

			Vector2 clickPosOriginal = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector2 transformPos = new Vector2 (transform.position.x, transform.position.y);
			Vector3 clickPos = new Vector3 (Mathf.Round (clickPosOriginal.x), Mathf.Round (clickPosOriginal.y), 0f);
			Chunk chunk = chunkManager.GetChunk(clickPosOriginal.x).GetComponent<Chunk>();
			float distance = Vector2.Distance (clickPosOriginal, transformPos);
			//Debug.Log (chunkManager.GetBlock (clickPos.x, clickPos.y));
			if(distance <= 4f)
				chunkManager.PlaceBlock (clickPos.x, clickPos.y, blockLibrary.dirt);
			else Debug.Log ("Distance: " + distance);


			/*Vector2 clickPosOriginal = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector3 clickPos = new Vector3 (Mathf.Round (clickPosOriginal.x), Mathf.Round (clickPosOriginal.y), 0f);
			Vector2 transformPos = new Vector2 (transform.position.x, transform.position.y);
			Vector2 direction = clickPosOriginal - transformPos;
			float distance = Vector2.Distance (clickPos, transformPos);
			Transform block = generator.GetBlock (clickPos.x, clickPos.y);
			if (block == null && distance <= 4f) {
				RaycastHit2D rayhit = Physics2D.Raycast (transformPos, direction, distance);
				Debug.DrawRay (transformPos, direction, Color.red);
				Collider2D hit = rayhit.collider;
				//if (hit == null) {
					if (generator.AdjacentBlock (clickPos.x, clickPos.y))
						generator.PlaceBlock (clickPos.x, clickPos.y, blockLibrary.dirt);
					else
						Debug.Log ("No adjacent block");
				//} else {
				//	Debug.Log ("Something in the way: " + hit.transform.name);
				//	Destroy(hit.gameObject);
				//}
			} else {
				Debug.Log ("There's a block there!");
			}*/
		
			/*//RaycastHit2D rayhit = Physics2D.Raycast (transform.position, (clickPosOriginal - new Vector2(transform.position.x, transform.position.y)), Vector2.Distance (transform.position, clickPos));
		Debug.DrawRay (transform.position, (clickPosOriginal - new Vector2(transform.position.x, transform.position.y)), Color.red);
		if(rayhit.collider.transform == GameObject.Find ("WorldGenerator").GetComponent<Generator> ().GetBlock (clickPos.x, clickPos.y)) {
			GameObject.Find ("WorldGenerator").GetComponent<Generator> ().DestroyBlock (clickPos.x, clickPos.y);
		}else {
			Destroy (rayhit.collider.transform.gameObject);
		}
		Debug.Log ("yay");*/
			//UpdateBlocks ();
		}
	}
}
