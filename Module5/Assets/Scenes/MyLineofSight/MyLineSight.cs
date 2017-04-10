using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLineSight : MonoBehaviour {

	//How sensitive should we be to sight
	public enum SightSensitivity { STRICT, LOOSE };

	//Sight sensitivity
	public SightSensitivity Sensitity = SightSensitivity.STRICT;

	//-----------------------------------------

	public float FieldOfView = 45f;
	public Transform Target = null;
	public Transform EyePoint = null;

	public bool CanSeeTarget = false;

	private Transform ThisTransform = null;
	private SphereCollider ThisCollider = null;

	//-----------------------------------------
	//Reference to last know object sighting, if any
	public Vector3 LastKnowSighting = Vector3.zero;

	//-----------------------------------------
	void Awake()
	{
		ThisTransform = GetComponent<Transform>();
		ThisCollider = GetComponent<SphereCollider>();
		LastKnowSighting = ThisTransform.position;
	}

	//-----------------------------------------
	void OnTriggerStay(Collider Other)
	{
		UpdateSight();

		//Update last known sighting
		if (CanSeeTarget)
			LastKnowSighting = Target.position;
	}

	//-----------------------------------------
	bool InFOV()
	{
		Vector3 DirToTarget = Target.position - EyePoint.position;

		float angle = Vector3.Angle(EyePoint.forward, DirToTarget);

		if (angle <= FieldOfView)
			return true;
		return false;
	}
	//------------------------------------------
	bool ClearLineofSight()
	{
		RaycastHit Info;

		if (Physics.Raycast(EyePoint.position, (Target.position - EyePoint.position).normalized, out Info, ThisCollider.radius))
		{
			//If player, then can see player
			if (Info.transform.CompareTag("Player"))
				return true;
		}

		return false;
	}
	//------------------------------------------
	void UpdateSight()
	{
		switch (Sensitity)
		{
			case SightSensitivity.STRICT:
				CanSeeTarget = InFOV() && ClearLineofSight();
				break;

			case SightSensitivity.LOOSE:
				CanSeeTarget = InFOV() || ClearLineofSight();
				break;
		}
	}
}
