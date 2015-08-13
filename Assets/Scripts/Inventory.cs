using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	Transform[,] items;
	public Sprite inventoryBar;
	Vector2 aspectRatio;

	// Use this for initialization
	void Start () {
		items = new Transform[9, 5];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		//GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(0, new Vector3(0, 1, 0)), new Vector3(Screen.width / 1024f, Screen.height / 768f, 1f)); 
		//GUI.Label (new Rect ((Screen.width - inventoryBar.texture.width) / 2, Screen.height, inventoryBar.texture.width, inventoryBar.texture.height), inventoryBar.texture);
	}
}
