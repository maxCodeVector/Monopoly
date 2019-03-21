using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour {
	public Transform center;
	public Transform camera;
	public List<GameObject> UI;
    private Vector3 nowPos;//鼠标的位置
    private Vector3 latePos;//延迟鼠标的位置
	private bool down = false;
	private bool change;
	private Touch oldTouch1;  //上次触摸点1(手指1)
    private Touch oldTouch2;  //上次触摸点2(手指2)

	public Transform target;//目标物体
	public float smoothing = 3;//平滑系数
	public float delayTime = 1.5f;
	public static float timePast = 100f;

	// Use this for initialization
	void Start () {
		target = GameController.clientPlayer.transform;
		camera.LookAt(center.transform);
		// dis = transform.position - target.position;
	}

	void LateUpdate (){
		//目标物体要到达的目标位置 = 当前物体的位置 + 当前摄像机的位置
		Vector3 targetPos = target.position;
		//使用线性插值计算让摄像机用smoothing * Time.deltaTime时间从当前位置到移动到目标位置
		center.position = Vector3.Lerp (center.position, targetPos, smoothing * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
		int angleX = (int)center.localEulerAngles.x;
		// mouse control
		if(Input.GetMouseButtonDown(0)){
			down = true;
		}
		if(Input.GetMouseButtonUp(0)){
			down = false;
			change = false;
		}
		if (down){
			nowPos = Input.mousePosition;//获得鼠标位置
			//按下期间鼠标是否移动
			if (nowPos != latePos && change){
				float deltaX = (nowPos.x - latePos.x) *Time .deltaTime ;
				float deltaY = (nowPos.y - latePos.y) *Time .deltaTime ;
				center.Rotate(0, deltaX * 4, 0, Space.World);
				if(angleX >= 30 && angleX < 200){ // only down
					if(deltaY > 0){
						center.Rotate(-deltaY * 4, 0, 0, Space.Self);
					}
				} else if(angleX <= 330 && angleX > 201){ // only up
					if(deltaY < 0){
						center.Rotate(-deltaY * 4, 0, 0, Space.Self);
					}
				} else{
					// if(angleX - deltaY * 4)
					center.Rotate(-deltaY * 4, 0, 0, Space.Self);
				}
			}
			change = true;
			latePos = Input.mousePosition;
		}
		// Zoom In
		if (Input.GetAxis("Mouse ScrollWheel") > 0){
			Vector3 temp = center.position - camera.position;
			if(temp.magnitude >= 10){
				Vector3 vec = temp.normalized;
				camera.position += vec * 2;
			}
		}
		// Zoom Out
		if (Input.GetAxis("Mouse ScrollWheel") < 0){
			Vector3 temp = center.position - camera.position;
			if(temp.magnitude <= 50){
				Vector3 vec = -temp.normalized;
				camera.position += vec * 2;
			}
		}


		// device touch control
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved){
            Vector2 touchDelPos = Input.GetTouch(0).deltaPosition;
			center.Rotate(0, touchDelPos.x / 10, 0, Space.World);
			if(angleX >= 30 && angleX < 200){ // only down
					if(touchDelPos.y > 0){
						center.Rotate(-touchDelPos.y / 10, 0, 0, Space.Self);
					}
				} else if(angleX <= 330 && angleX > 201){ // only up
					if(touchDelPos.y < 0){
						center.Rotate(-touchDelPos.y / 10, 0, 0, Space.Self);
					}
				} else{
					center.Rotate(-touchDelPos.y / 10, 0, 0, Space.Self);
				}
        }else if(Input.touchCount >= 2){
        //多点触摸, 放大缩小
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        //第2点刚开始接触屏幕, 只记录，不做处理
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }
        //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        //两个距离之差，为正表示放大手势， 为负表示缩小手势
        float distance = (newDistance - oldDistance) / 20; 
		Vector3 temp = center.position - camera.position;
		Vector3 vec = temp.normalized;

		// Zoom In
		if (distance > 0){
			if(temp.magnitude >= 10){
				camera.position += vec * distance;
			}
		}
		// Zoom Out
		if (distance < 0){
			if(temp.magnitude <= 50){
				camera.position += vec * distance;
			}
		}

        //记住最新的触摸点，下次使用
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
        }

		timePast += Time.deltaTime;
		if(timePast > delayTime && !GameController.isGameOver){
			if(GameController.startPlayer != null){
				target = GameController.startPlayer.transform;
			}
		}
		if (GameController.startPlayer == GameController.clientPlayer){
			foreach(GameObject x in UI){
				x.SetActive(true);
			}
		}else{
			foreach(GameObject x in UI){
				x.SetActive(false);
			}
		}
	}
}
