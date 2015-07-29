using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	public new string name;

	ChunkManager chunkManager;
	BlockLibrary blockLibrary;
	Transform[,] blocks;
	Transform[,] background;
	int offset = 0;
	int extra = 1;
	int xPos;

	int worldWidth = 200;
	int worldHeight;

	void Awake () {
		blockLibrary = GameObject.Find ("WorldGenerator").GetComponent<BlockLibrary> ();
		chunkManager = GameObject.Find ("WorldGenerator").GetComponent<ChunkManager> ();
		offset = chunkManager.offset;
		worldHeight = chunkManager.chunkHeight;
		xPos = int.Parse (transform.position.x.ToString ());
		GenerateWorld ();
	}

	int Noise (int x, int y, float scale, float mag, float exp){
		x += offset;
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*mag),(exp) )); 
	}

	public void DamageBlock(float x, float y, Transform item) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		if (blocks [xNew, yNew] != null) {
			if(item != null) {
				Item i = item.GetComponent<Item>();
				if(i.type == "shovel")
					blocks [xNew, yNew].transform.GetComponent<Block>().health -= Time.deltaTime * 2f;
				else
					blocks [xNew, yNew].transform.GetComponent<Block>().health -= Time.deltaTime;
			}
			else
				blocks [xNew, yNew].transform.GetComponent<Block>().health -= Time.deltaTime;
			
			if(blocks [xNew, yNew].transform.GetComponent<Block>().health <= 0){
				blocks [xNew, yNew].transform.GetComponent<Block>().Explode ();
				Destroy (blocks [xNew, yNew].gameObject);
				blocks [xNew, yNew] = null;
				if(yNew < chunkManager.chunkHeight){
					Transform above = blocks[xNew, yNew + 1];
					if(above != null) {
						if(above.GetComponent<Block>().needsSupport)
						{
							blocks [xNew, yNew + 1].transform.GetComponent<Block> ().Explode ();
							Destroy (blocks [xNew, yNew + 1].gameObject);
							blocks [xNew, yNew + 1] = null;
						}
					}
				}
			}
		}
	}

	public void DestroyBlock(float x, float y, bool remains) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		if (blocks [xNew, yNew] != null) {
			if (remains)
				blocks [xNew, yNew].transform.GetComponent<Block> ().Explode ();
			Destroy (blocks [xNew, yNew].gameObject);
			blocks [xNew, yNew] = null;
			if(yNew < chunkManager.chunkHeight){
				Transform above = blocks[xNew, yNew + 1];
				if(above != null) {
					Debug.Log (above);
					if(above.GetComponent<Block>().needsSupport)
					{
						if (remains)
							blocks [xNew, yNew + 1].transform.GetComponent<Block> ().Explode ();
						Destroy (blocks [xNew, yNew + 1].gameObject);
						blocks [xNew, yNew + 1] = null;
					}
				}
			}
		}
	}

	public void PlaceBlock(float x, float y, Transform block) {
		int xx = int.Parse (x.ToString ());
		int yy = int.Parse (y.ToString ());
		Transform b = Instantiate (block, new Vector3 (xx + transform.position.x, yy, 0), Quaternion.identity) as Transform;
		b.parent = transform;
		b.localPosition = new Vector3 (xx, yy, 0);
		blocks [xx, yy] = b;
	}

	public Transform GetBlock(float x, float y) {
		int xx = int.Parse (x.ToString ());
		int yy = int.Parse (y.ToString ());
		return blocks [xx, yy];
	}

	void GenerateWorld() {
		blocks = new Transform[chunkManager.chunkWidth, worldHeight];
		background = new Transform[chunkManager.chunkWidth, worldHeight];
		for (int x = xPos; x < chunkManager.chunkWidth + xPos; x++) {
			for (int y = 0; y < worldHeight; y++){
				int stoneHeight = 0;
				int dirtHeight = 0;
				
				stoneHeight = Noise (x, 0, 80f, 2f, 1f);
				stoneHeight += Noise (x, 0, 50f, 30f, 1f);
				stoneHeight += Noise (x, 0, 10f, 10f, 0.75f);
				stoneHeight += 45;
				
				/*dirtHeight = Noise (x, 0, 100f, 35f, 1f);
				dirtHeight += Noise (x, 100, 50f, 30f, 1f);
				dirtHeight += 45;*/

				dirtHeight = Noise (x, 0, 80f, 2f, 1f);
				dirtHeight += Noise (x, 0, 50f, 30f, 1f);
				dirtHeight += 55;
				
				Transform block = null;
				Transform backgroundBlock = null;
				
				if(y <= stoneHeight){
					if(Noise(x, y, 8, 30, 1) > 10f){							// Create some caves
						if(Noise(x, y, 3, 30, 1) <= 4) {			// Create gravel in stone
							block = blockLibrary.gravelStone;	
						}
						else if(Noise(x, y, 6, 30, 1) <= 3)			// Create coal ore
							block = blockLibrary.coal;
						else if(Noise(x + 1000, y, 6, 30, 1) <= 3)			// Create iron ore
							block = blockLibrary.ironOre;
						else
							block = blockLibrary.stone;
					}
					backgroundBlock = blockLibrary.stoneBack;
				}
				else if (y == dirtHeight){
					block = blockLibrary.grass;
					backgroundBlock = blockLibrary.dirtBack;
				}
				else if(y < dirtHeight){
					if(Noise(x, y, 3, 30, 1) <= 4)				// Create gravel in dirt
						block = blockLibrary.gravelDirt;
					else
						block = blockLibrary.dirt;
					
					backgroundBlock = blockLibrary.dirtBack;
				} else if(y == dirtHeight + 1) {
					if(Noise(x, y, 5, 30, 1) <= 8) {
						block = blockLibrary.grassLeaves;
					} else if(Noise(x, y, 2, 10, 1) <= 3) {							// Create saplings
						if(y < worldHeight - 6) 
							block = blockLibrary.sapling;
					}
				}
				
				if(block != null){
					block = Instantiate (block, new Vector2(0, 0), Quaternion.identity) as Transform;
					block.parent = transform;
					block.localPosition = new Vector2(x - xPos, y);
					blocks[x - xPos, y] = block;
				} else blocks[x - xPos, y] = null;

				if(backgroundBlock != null){
					block = Instantiate (backgroundBlock, new Vector2(0, 0), Quaternion.identity) as Transform;
					block.parent = transform;
					block.localPosition = new Vector2(x - xPos, y);
					background[x - xPos, y] = backgroundBlock;
				} else background[x - xPos, y] = null;
			}
		}
	}
}
