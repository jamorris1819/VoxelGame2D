using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	public int worldWidth;
	public int worldHeight;

	public int caveScale, caveMag, caveExp;

	BlockLibrary blockLibrary;

	Transform[,] blocks;
	Transform[,] background;

	int offset;

	void Awake () {
		offset = Random.Range (-1000000, 1000000);
		blockLibrary = GetComponent<BlockLibrary> ();
		GenerateWorld ();
	}

	int Noise (int x, int y, float scale, float mag, float exp){
		x += offset;
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*mag),(exp) )); 
	}

	public Transform[,] Blocks {
		get { return blocks; }
	}

	public void DestroyBlock(float x, float y, Transform item) {
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
			}
		}
	}

	public Transform GetBlock(float x, float y) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		return blocks [xNew, yNew];
	}

	public bool IsBlock(float x, float y) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		return (blocks [xNew, yNew] != null);
	}

	public void PlaceBlock(float x, float y, Transform block) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		Transform b = Instantiate (block, new Vector2 (xNew + 0f, yNew + 0f), Quaternion.identity) as Transform;
		blocks [xNew, yNew] = b;
	}

	public bool AdjacentBlock(float x, float y) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		return !(blocks [xNew + 1, yNew] == null && blocks [xNew - 1, yNew] == null &&
		         blocks [xNew, yNew + 1] == null && blocks [xNew, yNew - 1] == null);
	}

	public bool GrowingRoom(float x, float y) {
		int xNew = int.Parse (x.ToString ());
		int yNew = int.Parse (y.ToString ());
		return (blocks [xNew + 1, yNew + 5] == null && blocks [xNew, yNew + 5] == null && blocks [xNew - 1, yNew + 5] == null &&
		        blocks [xNew + 1, yNew + 4] == null && blocks [xNew, yNew + 4] == null && blocks [xNew - 1, yNew + 4] == null &&
		        blocks [xNew + 1, yNew + 3] == null && blocks [xNew, yNew + 3] == null && blocks [xNew - 1, yNew + 3] == null &&
		        blocks [xNew + 1, yNew + 2] == null && blocks [xNew, yNew + 2] == null && blocks [xNew - 1, yNew + 2] == null &&
		        blocks [xNew + 1, yNew + 1] == null && blocks [xNew, yNew + 1] == null && blocks [xNew - 1, yNew + 1] == null &&
		        blocks [xNew + 1, yNew] == null && blocks [xNew - 1, yNew] == null);
	}

	void GenerateWorld() {
		blocks = new Transform[worldWidth, worldHeight];
		background = new Transform[worldWidth, worldHeight];
		for (int x = 0; x < worldWidth; x++) {
			for (int y = 0; y < worldHeight; y++){
				int stoneHeight, dirtHeight = 0;

				stoneHeight = Noise (x, 0, 80f, 2f, 1f);
				stoneHeight += Noise (x, 0, 50f, 30f, 1f);
				stoneHeight += Noise (x, 0, 10f, 10f, 0.75f);
				stoneHeight += 45;

				dirtHeight = Noise (x, 0, 100f, 35f, 1f);
				dirtHeight += Noise (x, 100, 50f, 30f, 1f);
				dirtHeight += 45;

				Transform block = null;

				if(y <= stoneHeight){
					if(Noise(x, y, caveScale, caveMag, caveExp) > 10){
						if(Noise(x + offset, y + offset, 3, 30, 1) <= 4)			// Create gravel in stone
							block = blockLibrary.gravelStone;	
						else if(Noise(x, y, 5, 30, 1) <= 4)			// Create coal ore
							block = blockLibrary.coal;
						else
							block = blockLibrary.stone;
					}
					else {
						block = blockLibrary.stoneBack;
					}
				}
				else if (y == dirtHeight)
					block = blockLibrary.grass;
				else if(y < dirtHeight){
					if(Noise(x + offset, y + offset, 3, 30, 1) <= 4)				// Create gravel in dirt
						block = blockLibrary.gravelDirt;
					else
						block = blockLibrary.dirt;
				} else if(y == dirtHeight + 1) {
					if(Noise(x, y, 5, 30, 1) <= 8) {
						block = blockLibrary.grassLeaves;
					} else if(Noise(x, y, 2, 10, 1) <= 3) {							// Create saplings
						if(y < worldHeight - 6) 
							block = blockLibrary.sapling;
					}
				}

				if(block != null){
					block = Instantiate (block, new Vector2(x + 0f, y + 0f), Quaternion.identity) as Transform;
					blocks[x, y] = block;
				} else blocks[x, y] = null;
			}
		}
	}
}
