using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chunk : MonoBehaviour {

	// References
	ChunkManager chunkManager;
	BlockLibrary blockLibrary;
	// Arrays
	Transform[,] foregroundBlocks;
	Transform[,] backgroundBlocks;
	// Miscellaneous variables
	int xPos;
	Transform worldGenerator;
	public GameObject[] invSlots;
	public Sprite itemBack;
	public GameObject[] invNumbers;
	public int[] invNumberValues;

	void Awake() {
		worldGenerator = GameObject.Find ("WorldGenerator").transform;
		chunkManager = worldGenerator.GetComponent<ChunkManager> ();
		blockLibrary = worldGenerator.GetComponent<BlockLibrary> ();
		invSlots = new GameObject[9];
		invSlots[0] = GameObject.Find ("slot0");
		invSlots[1] = GameObject.Find ("slot1");
		invSlots[2] = GameObject.Find ("slot2");
		invSlots[3] = GameObject.Find ("slot3");
		invSlots[4] = GameObject.Find ("slot4");
		invSlots[5] = GameObject.Find ("slot5");
		invSlots[6] = GameObject.Find ("slot6");
		invSlots[7] = GameObject.Find ("slot7");
		invSlots[8] = GameObject.Find ("slot8");

		invNumbers = new GameObject[9];
		invNumbers[0] = GameObject.Find ("slot0number");
		invNumbers[1] = GameObject.Find ("slot1number");
		invNumbers[2] = GameObject.Find ("slot2number");
		invNumbers[3] = GameObject.Find ("slot3number");
		invNumbers[4] = GameObject.Find ("slot4number");
		invNumbers[5] = GameObject.Find ("slot5number");
		invNumbers[6] = GameObject.Find ("slot6number");
		invNumbers[7] = GameObject.Find ("slot7number");
		invNumbers[8] = GameObject.Find ("slot8number");

		invNumberValues = new int[9];
		for (int i = 0; i < 9; i++) {
			invNumberValues[i] = 0;
		}
		xPos = int.Parse (transform.position.x.ToString ());
		GenerateChunk ();
	}

	int Noise(int x, int y, float scale, float magnitude, float exponent) {
		return (int) Mathf.Pow (Mathf.PerlinNoise ((x + chunkManager.offset) / scale, y / scale) * magnitude, exponent);
	}

	void GenerateChunk() {
		foregroundBlocks = new Transform[chunkManager.chunkWidth, chunkManager.chunkHeight];
		backgroundBlocks = new Transform[chunkManager.chunkWidth, chunkManager.chunkHeight];

		for(int x = xPos; x < chunkManager.chunkWidth + xPos; x++) {
			for (int y = 0; y < chunkManager.chunkHeight; y++) {
				// Define height variables
				int stoneHeight = 0;
				int dirtHeight = 0;
				// Generate height for stone layer
				stoneHeight += Noise (x, 0, 80f, 2f, 1f);
				stoneHeight += Noise (x, 0, 50f, 30f, 1f);
				stoneHeight += Noise (x, 0, 10f, 10f, 0.75f);
				stoneHeight += 45;
				// Generate height for dirt layer
				dirtHeight += Noise (x, 0, 80f, 2f, 1f);
				dirtHeight += Noise (x, 0, 50f, 30f, 1f);
				dirtHeight += 55;
				// Blocks to be placed into the array
				Transform foregroundBlock = null;
				Transform backgroundBlock = null;
				// Decide which block should be used
				if(y == dirtHeight + 1) {							// If the block is just above the dirt, perhaps make some grass leaves.
					if(Noise(x, y, 5, 30, 1) <= 8)
						foregroundBlock = blockLibrary.grassLeaves;
				} else if(y == dirtHeight) {							// If the block is on the surface of the dirt
					foregroundBlock = blockLibrary.grass;
					backgroundBlock = blockLibrary.dirtBack;
				} else if (y < dirtHeight && y > stoneHeight) {	// If the block is between the surface of the dirt and the surface of the stone
					if(Noise(x, y, 3, 30, 1) <= 4)				
						foregroundBlock = blockLibrary.gravelDirt;
					else
						foregroundBlock = blockLibrary.dirt;

					backgroundBlock = blockLibrary.dirtBack;
				} else if (y <= stoneHeight) {
					if(Noise(x, y, 8, 30, 1) > 10f){			// If the block isn't in a cave
						if(Noise(x, y, 3, 30, 1) <= 4)				
							foregroundBlock = blockLibrary.gravelStone;
						else if(Noise(x, y, 6, 30, 1) <= 3)			
							foregroundBlock = blockLibrary.coal;
						else if(Noise(x + 1000, y, 6, 30, 1) <= 3)			
							foregroundBlock = blockLibrary.ironOre;
						else
							foregroundBlock = blockLibrary.stone;
					}

					backgroundBlock = blockLibrary.stoneBack;
				}

				if(foregroundBlock != null){
					foregroundBlock = Instantiate (foregroundBlock, new Vector2(0, 0), Quaternion.identity) as Transform;
					foregroundBlock.parent = transform;
					foregroundBlock.localPosition = new Vector2(x - xPos, y);
					foregroundBlocks[x - xPos, y] = foregroundBlock;
				} else foregroundBlocks[x - xPos, y] = null;
				
				if(backgroundBlock != null){
					backgroundBlock = Instantiate (backgroundBlock, new Vector2(0, 0), Quaternion.identity) as Transform;
					backgroundBlock.parent = transform;
					backgroundBlock.localPosition = new Vector2(x - xPos, y);
					backgroundBlocks[x - xPos, y] = backgroundBlock;
				} else backgroundBlocks[x - xPos, y] = null;
			}
		}
	}

	public void PlaceBlock(float x, float y, Transform block) {
		int xx = int.Parse (x.ToString ());
		int yy = int.Parse (y.ToString ());
		Transform b = Instantiate (block, new Vector3 (xx + transform.position.x, yy, 0), Quaternion.identity) as Transform;
		b.parent = transform;
		b.localPosition = new Vector3 (xx, yy, 0);
		foregroundBlocks [xx, yy] = b;
	}
	
	public Transform GetBlock(float x, float y) {
		int xx = int.Parse (x.ToString ());
		int yy = int.Parse (y.ToString ());
		return foregroundBlocks [xx, yy];
	}

	public void DamageBlock(float x, float y) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		Transform block = foregroundBlocks [xNew, yNew];
		if (block != null) {
			Block blockComponent = block.GetComponent<Block> ();
			blockComponent.health -= Time.deltaTime;
			if(blockComponent.health <= 0) {
				for(int i = 0; i <= 9; i++) {
					if(invSlots[i].GetComponent<Image>().sprite == itemBack || invSlots[i].GetComponent<Image>().sprite == block.GetComponent<SpriteRenderer>().sprite){
						invSlots[i].GetComponent<Image>().sprite = block.GetComponent<SpriteRenderer>().sprite;
						invNumberValues[i] += 1;
						invNumbers[i].GetComponent<Text>().text = (invNumberValues[i] > 1) ? invNumberValues[i].ToString () : "";
						break;
					}
				}
				DestroyBlock (x, y, true);
			}
		}
		
	}

	public void DestroyBlock(float x, float y, bool explode) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		Transform block = foregroundBlocks [xNew, yNew];
		Block blockComponent = block.GetComponent<Block> ();
		if (block != null) {
			if(explode)
				blockComponent.Explode ();
			Destroy (block.gameObject);
			foregroundBlocks[xNew, yNew] = null;
			// Now we need to check if the block above needs support. If it does, and this is gone, it should be destroyed.
			if(yNew < chunkManager.chunkHeight) {
				Transform blockAbove = foregroundBlocks[xNew, yNew + 1];
				if(blockAbove != null) {
					Block blockAboveComponent = blockAbove.GetComponent<Block>();
					if(blockAboveComponent.needsSupport) {
						if(explode)
							blockAboveComponent.Explode ();
						Destroy (blockAbove.gameObject);
						foregroundBlocks[xNew, yNew + 1] = null;
					}
				}
			}
		}
	}
}
