using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
	public Transform target;//目标物体
	public float smoothing = 3;//平滑系数
    public bool rotate = true;
	Vector3 dis;
	void LateUpdate (){
		//目标物体要到达的目标位置 = 当前物体的位置 + 当前摄像机的位置
		Vector3 targetPos = target.position + dis;
		//使用线性插值计算让摄像机用smoothing * Time.deltaTime时间从当前位置到移动到目标位置
		this.transform.position = Vector3.Lerp (this.transform.position, targetPos, smoothing * Time.deltaTime);
	}
	void Start () {
		target = GameController.clientPlayer.gameObject.GetComponent<Transform>();
		dis = this.transform.position - target.position;
	}
	void Update () {
        // if(rotate){
        //     // father.rotation.
        // }
	}
}
