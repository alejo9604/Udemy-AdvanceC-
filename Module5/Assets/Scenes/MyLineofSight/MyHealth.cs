using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHealth : MonoBehaviour {

	[SerializeField]
	private float healthPoints = 100f;

	public float HealthPoints
	{
		get { return healthPoints; }
		set
		{
			healthPoints = value;

			//If health is < 0 then die
			if (healthPoints <= 0)
				Destroy(gameObject);
		}
	}
}
