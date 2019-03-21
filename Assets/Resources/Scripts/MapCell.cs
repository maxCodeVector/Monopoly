using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {
	public List<GameObject> nextCell;
	public MapController controller;
	public GameObject bomb;

	public void triggerEvents(){
		controller.triggerEvents();
	}

	public void setPlayer(GamePlayer player){
		controller.setPlayer(player);
	}
	// Use this for initialization
	void Start () {
		switch (GetComponent<SpriteRenderer>().sprite.name)
		{
			case "busStation": 
				this.controller = BusController.getInstance();
				break;
			case "ATM": 
				this.controller = ATMController.getInstance();
				break;
			case "darkRoom":
				this.controller = DarkController.getInstance(); 
				break;
			case "diningHall": 
				this.controller = DiningController.getInstance();
				break;
			case "entertainment": 
				this.controller = EntertainController.getInstance();
				break;
			case "exercise": 
				this.controller = ExerciseController.getInstance();
				break;
			case "fortune": 
				this.controller = FortuneController.getInstance();
				break;
			case "game": 
				this.controller = GameMapController.getInstance();
				break;
			case "healthCenter": 
				this.controller = HealthController.getInstance();
				break;
			case "house": 
				Vector3 dir = nextCell[0].transform.position - transform.position;
				Vector3 targetPosition =  Quaternion.AngleAxis(270, Vector3.up)* dir + transform.position;
				this.controller = new HouseController(this, targetPosition);
				break;
			case "misfortune": 
				this.controller = MisfortuneController.getInstance();
				break;
			case "shop": 
				this.controller = ShopController.getInstance();
				break;
			case "teachBuilding": 
				this.controller = TeachController.getInstance();
				break;
			case "work": 
				this.controller = WorkController.getInstance();
				break;
			default:
				this.controller = EmptyController.getInstance();
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


