using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<Item> items;
	public Text stackText;
	public Sprite slotEmpty;
	public List<Item> i;
	public Inventory inventory;

	public bool IsEmpty {
		get { return items.Count == 0; }
	}

	public bool IsAvailable {
		get { return CurrentItem.maxSize > items.Count; }
	}

	public Item CurrentItem {
		get { return items.Peek (); }
	}

	public Stack<Item> Items {
		get { return items; }
		set { items = value; }
	}

	void Start () {
		items = new Stack<Item> ();
		RectTransform slotRect = GetComponent<RectTransform> ();
		RectTransform textRect = stackText.GetComponent<RectTransform> ();

		int txtscf = (int)(slotRect.sizeDelta.x * 0.6f);

		stackText.resizeTextMaxSize = txtscf;
		stackText.resizeTextMinSize = txtscf;

		textRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
		textRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
	}

	void Update () {
		if (i.Count != items.Count) {
			i.Clear ();
			foreach (Item it in items) {
				i.Add (it);
			}
		}
	}

	public void AddItem(Item item) {
		items.Push (item);
		if (items.Count > 1) {
			stackText.text = items.Count.ToString ();
		}
		ChangeSprite (item.sprite);
	}

	private void ChangeSprite(Sprite itemSprite) {
		GetComponent<Image> ().sprite = itemSprite;
	}

	private void UseItem() {
		if (!IsEmpty && items != null) {
			Item item = items.Peek ();
			//inventory.showBlock.sprite = item.sprite;
			if(item != null) {
				GameObject.Find ("MalePlayer").GetComponent<Player>().item = item;
				item.Use ();
				stackText.text = items.Count > 1 ? items.Count.ToString () : string.Empty;
				if(IsEmpty) {
					ChangeSprite (slotEmpty);
					Inventory.EmptySlots++;
				}
			}
		}
	}

	public void ExpendItem() {
		if (!IsEmpty && items != null) {
			items.Pop ();
			stackText.text = items.Count > 1 ? items.Count.ToString () : string.Empty;
			if(IsEmpty) {
				ChangeSprite (slotEmpty);
				Inventory.EmptySlots++;
			}
		}
	}

	public void AddItems(Stack<Item> items) {
		this.items = new Stack<Item> (items);
		stackText.text = items.Count > 1 ? items.Count.ToString () : string.Empty;
		ChangeSprite (CurrentItem.sprite);
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right) {
			UseItem ();
		}
	}

	public void ClearSlot() {
		items.Clear ();
		ChangeSprite (slotEmpty);
		stackText.text = string.Empty;
	}
}
