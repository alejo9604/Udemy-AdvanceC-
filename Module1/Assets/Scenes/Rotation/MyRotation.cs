using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRotation : MonoBehaviour {

	private Transform ThisTransform = null;
	public float RotSpeed = 90f;
	public Transform Target;

	void Awake () {
		ThisTransform = GetComponent<Transform>();
	}
	

	void Update () {

		Quaternion DestRot = Quaternion.LookRotation(Target.position - ThisTransform.position, Vector3.up);

		ThisTransform.rotation = Quaternion.RotateTowards(ThisTransform.rotation, DestRot, RotSpeed * Time.deltaTime);

		//ThisTransform.rotation *= Quaternion.AngleAxis(RotSpeed * Time.deltaTime, Vector3.up);
		//ThisTransform.rotation = Quaternion.Euler(0f, 90f, 0f);
	}
}
