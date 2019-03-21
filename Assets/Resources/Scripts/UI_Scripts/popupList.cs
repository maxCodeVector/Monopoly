using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupList : MonoBehaviour {
	public UIPopupList list;
	public GameObject arrow;
	// Use this for initialization
	void Start () {
		EventDelegate ed = new EventDelegate(this, "changePointPosition");
		EventDelegate.Add(list.onChange, ed);
		// list.onChange.Add(ed);
	}
	
	private void changePointPosition(){
		switch (list.items.IndexOf(UIPopupList.current.value))
		{
			case 0:
				arrow.transform.localPosition = new Vector3(-203, 50, 0);
				break;
			case 1:
				arrow.transform.localPosition = new Vector3(-93, 102, 0);
				break;
			case 2:
				arrow.transform.localPosition = new Vector3(-192, -33, 0);
				break;
			case 3:
				arrow.transform.localPosition = new Vector3(-192, -111, 0);
				break;
			case 4:
				arrow.transform.localPosition = new Vector3(-93, -89, 0);
				break;
		}
	}
	// Update is called once per frame
	void Update () {
		}
}

