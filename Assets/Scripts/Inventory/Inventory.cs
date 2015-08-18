using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;
	private List<GameObject> allSlots;
	private static int emptySlots;
	private static Slot from, to;
	private static GameObject hoverObject;
	private float hoverYOffset;
	private static GameObject tooltip;
	private static Text sizeText;
	private static Text visualText;

	public int slots;
	public int rows;
	public float paddingLeft;
	public float paddingTop;
	public float slotSize;
	public GameObject slotPrefab;
	public GameObject iconPrefab;
	public Canvas canvas;
	public EventSystem eventSystem;
	public Text sizeTextObject;
	public Text visualTextObject;
	public GameObject tooltipObject;
	public Image showBlock;

	void Start () {
		tooltip = tooltipObject;
		sizeText = sizeTextObject;
		visualText = visualTextObject;
		CreateLayout ();
	}
	 
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			if(!eventSystem.IsPointerOverGameObject (-1) && from != null) {
				from.GetComponent<Image>().color = Color.white;
				from.ClearSlot ();
				Destroy (GameObject.Find ("Hover"));
				to = null;
				from = null;
				hoverObject = null;
			}
		}

		if (hoverObject != null) {
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
			position.Set (position.x, position.y - hoverYOffset);
			hoverObject.transform.position = canvas.transform.TransformPoint (position);
		}
	}

	public static int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	public void ShowTooltip(GameObject slot) {
		Slot tmpSlot = slot.GetComponent<Slot> ();
		if (!tmpSlot.IsEmpty && hoverObject == null) {
			visualText.text = tmpSlot.CurrentItem.GetTooltip ();
			sizeText.text = visualText.text;

			tooltip.SetActive (true);
			float xPos = slot.transform.position.x;
			float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y - paddingTop;
			tooltip.transform.position = new Vector2(xPos, yPos);
		}
	}

	public void HideTooltip() {
		tooltip.SetActive (false);
	}

	private void CreateLayout() {
		allSlots = new List<GameObject> ();
		hoverYOffset = slotSize * 0.01f;
		emptySlots = slots;
		inventoryWidth = (slots / rows) * (slotSize + paddingLeft) + paddingLeft;
		inventoryHeight = rows * (slotSize + paddingTop) + paddingTop;
		inventoryRect = GetComponent<RectTransform> ();
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);
		int columns = slots / rows;
		for (int y = 0; y < rows; y++) {
			for(int x = 0; x < columns; x++) {
				GameObject newSlot = (GameObject)Instantiate (slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				newSlot.name = "Slot";
				newSlot.transform.SetParent (this.transform.parent);
				slotRect.localPosition = inventoryRect.localPosition + new Vector3(paddingLeft * (x + 1) + (slotSize * x), -paddingTop * (y + 1) - (slotSize * y));
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotSize);
				newSlot.transform.SetParent (this.transform);
				allSlots.Add (newSlot);
			}
		}
	}

	public bool AddItem(Item item) {
		if (item.maxSize == 1) {
			PlaceEmpty (item);
			return true;
		} else {
			foreach(GameObject slot in allSlots) {
				Slot tmp = slot.GetComponent<Slot>();
				if(!tmp.IsEmpty) {
					if(tmp.CurrentItem.itemName == item.itemName && tmp.IsAvailable) {
						tmp.AddItem (item);
						return true;
					}
				}
			}
			if(emptySlots > 0)
				PlaceEmpty (item);
		}
		return false;
	}

	public void MoveItem(GameObject clicked) {
		if (from == null) {
			if (!clicked.GetComponent<Slot> ().IsEmpty) {
				from = clicked.GetComponent<Slot> ();
				from.GetComponent<Image> ().color = Color.gray;
				hoverObject = Instantiate (iconPrefab) as GameObject;
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = "Hover";
				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedtransform = clicked.GetComponent<RectTransform>();

				hoverTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, clickedtransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, clickedtransform.sizeDelta.y);

				hoverObject.transform.SetParent (GameObject.Find ("Canvas").transform, true);
				hoverObject.transform.localScale = from.gameObject.transform.localScale;
			}
		} else if (to == null) {
			to = clicked.GetComponent<Slot>();
			Destroy (GameObject.Find ("Hover"));
		}

		if (from != null && to != null) {
			Stack<Item> tempTo = new Stack<Item>(to.Items);
			to.AddItems (from.Items);
			if (tempTo.Count == 0) {
				from.ClearSlot ();
			} else {
				from.AddItems (tempTo);
			}
			from.GetComponent<Image>().color = Color.white;
			to = null;
			from = null;
		}
	}

	private bool PlaceEmpty(Item item) {
		if (emptySlots > 0) {
			foreach(GameObject slot in allSlots) {
				Slot tmp = slot.GetComponent<Slot>();
				if(tmp.IsEmpty) {
					tmp.AddItem (item);
					emptySlots--;
					return true;
				}
			}
		}
		return false;
	}

	public void ExpendItem(Item item) {
		Slot toPurge = null;
		foreach (GameObject slot in allSlots) {
			Slot tmpSlot = slot.GetComponent<Slot>();
			if(tmpSlot.Items.Count > 0) {
				if(tmpSlot.CurrentItem == item) {
					tmpSlot.ExpendItem ();
					break;
				}
			}
		}
		Debug.Log (NumOfItem (item) + " remain");
	}

	public int NumOfItem(Item item) {
		int number = 0;
		foreach (GameObject slot in allSlots) {
			Slot tmpSlot = slot.GetComponent<Slot>();
			if(tmpSlot.Items.Count > 0) {
				if(tmpSlot.CurrentItem == item)
					number += tmpSlot.Items.Count;
			}
		}
		return number;
	}
}
