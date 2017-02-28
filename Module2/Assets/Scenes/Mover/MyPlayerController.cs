using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MyPlayerController : MonoBehaviour {

	public float MaxSpeed = 10f;
	public float RotSpeed = 10f;

	private Transform ThisTransform = null;
	private CharacterController ThisController = null;


	// Use this for initialization
	void Awake() {
		ThisTransform = GetComponent<Transform>();
		ThisController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float Horz = CrossPlatformInputManager.GetAxis("Horizontal");
		float Vert = CrossPlatformInputManager.GetAxis("Vertical");

		//Update Rotation
		ThisTransform.rotation *= Quaternion.Euler(0f, RotSpeed * Horz * Time.deltaTime, 0f);

		//Update position
		//ThisTransform.position += ThisTransform.forward * MaxSpeed * Vert * Time.deltaTime;
		ThisController.SimpleMove(ThisTransform.forward * MaxSpeed * Vert);


	}
}
