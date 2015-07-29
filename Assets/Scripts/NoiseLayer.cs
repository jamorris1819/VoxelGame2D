using UnityEngine;
using System.Collections;

[System.Serializable]
public class NoiseLayer {
	
	public int scale;
	public int magnitude;
	public float exp;
	public int xOffset;
	int x;

	public NoiseLayer(int x, int scale, int magnitude, int exp, int offset) {
		this.scale = scale;
		this.magnitude = magnitude;
		this.exp = exp;
		this.x = x;
		this.xOffset = offset;
	}

	public int Noise() {
		int xtemp = x + xOffset;
		return (int) (Mathf.Pow ((Mathf.PerlinNoise(xtemp/scale, 0) * magnitude),(exp) )); 
	}
}
