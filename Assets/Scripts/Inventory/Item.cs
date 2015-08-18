using UnityEngine;
using System.Collections;

public enum ItemType { BLOCK, EDIBLE, TOOL, ITEM }

public class Item : MonoBehaviour {

	public string itemName;
	public string itemDescription;
	public ItemType type;
	public Sprite sprite;
	public int maxSize;
	public bool block;
	public Transform placeableBlock;

	public void Use() {
		// if edible replenish health
	}

	public string GetTooltip() {
		string stats = string.Empty;
		string newline = string.Empty;

		if(itemDescription != string.Empty) {
			newline = "\n";
		}

		return string.Format ("<color=\"black\"><size=16>{0}</size></color><size=14><i>" + newline + "{1}</i></size>", itemName, itemDescription);
	}
}
