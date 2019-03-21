using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class MapController{
	public GameObject uiRoot;
	public abstract void setPlayer(GamePlayer player);
	public abstract void triggerEvents();
}