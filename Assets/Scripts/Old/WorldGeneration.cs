using UnityEngine;
using System.Collections;
using SimplexNoise;

public class WorldGeneration : MonoBehaviour {

	public Transform dirtBlock;
	public Transform stoneBlock;
	public Transform grassBlock;
	public Transform coalBlock;
	public float f;
	public int seed;
	public float scale;
	public float mag;
	public float exp;
	public Transform[] blocks;

	void Awake () {
		Debug.Log (SimplexNoise.Noise.Generate(17));
		GenerateWorld ();
	}

	void Update () {
	}

	int Noise (int x, int y, float scale, float mag, float exp){
		
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale,y/scale)*mag),(exp) )); 
		
	}

	void GenerateWorld() {
		for (int x = 0; x < 200; x++) {
			for(int y = 0; y < 100; y++) {
				float value = Noise (x, 2, scale, mag, exp);
				value += Noise(x,0, 50,30,1);
				value+= Noise(x,0, 10,10,1);
				value+=45;

				int dirt = Noise(x,0, 100f,35,1);
				dirt+= Noise(x,100, 50,30,1);
				dirt+=40;
				if(y == dirt) {
					if(Noise(x,y,16,18,1)<=10)
						Instantiate (grassBlock, new Vector2(x, y), Quaternion.identity);
				}
				else if(y <= value){
					if(Noise(x,y,8,30,1)<=16){
						if(Noise(x,y, 8, 18, 1) <= 4)
							Instantiate (dirtBlock, new Vector2(x, y), Quaternion.identity);
						if(Noise(x,y, 5, 30, 1) <= 6)
							Instantiate (coalBlock, new Vector2(x, y), Quaternion.identity);
						else
							Instantiate (stoneBlock, new Vector2(x, y), Quaternion.identity);
					}

				} else if(y <= dirt) {
					if(Noise(x,y,16,14,1)<=10)
						Instantiate (dirtBlock, new Vector2(x, y), Quaternion.identity);
				}
			}
		}
	}
}


