using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAEnemy : MonoBehaviour {

	//------------------------------------------
	public enum ENEMY_STATE { PATROL, CHASE, ATTACK };

	public ENEMY_STATE CurrentState
	{
		get { return currentstate; }

		set
		{
			//Update current state
			currentstate = value;

			//Stop all running coroutines
			StopAllCoroutines();

			switch (currentstate)
			{
				case ENEMY_STATE.PATROL:
					StartCoroutine(IAPatrol());
					break;

				case ENEMY_STATE.CHASE:
					StartCoroutine(IAChase());
					break;

				case ENEMY_STATE.ATTACK:
					StartCoroutine(IAAttack());
					break;
			}
		}
	}

	//------------------------------------------
	[SerializeField]
	private ENEMY_STATE currentstate = ENEMY_STATE.PATROL;
	//------------------------------------------


	private NavMeshAgent ThisAgent = null;
	public Transform PatrolDestination = null;
	private MyLineSight ThisLineSight = null;
	private Transform ThisTransform = null;
	public MyHealth PlayerHealth = null;
	private Transform PlayerTransform = null; 
	public float MaxDamege = 10f;


	void Awake() {
		ThisAgent = GetComponent<NavMeshAgent>();
		ThisLineSight = GetComponent<MyLineSight>();
		ThisTransform = transform;
		PlayerTransform = PlayerHealth.transform;
	}

	void Start()
	{
		CurrentState = ENEMY_STATE.PATROL;
	}
	/*
	void Update () {
		ThisAgent.SetDestination(PatrolDestination.position);
	}
	*/


	public IEnumerator IAPatrol()
	{
		while (currentstate == ENEMY_STATE.PATROL) 
		{
			ThisLineSight.Sensitity = MyLineSight.SightSensitivity.STRICT;

			ThisAgent.Resume();
			ThisAgent.SetDestination(PatrolDestination.position);

			while(ThisAgent.pathPending)
				yield return null;

			if (ThisLineSight.CanSeeTarget)
			{
				ThisAgent.Stop();
				CurrentState = ENEMY_STATE.CHASE;
				yield break;
			}

			yield return null;
		}
	}

	public IEnumerator IAChase()
	{
		while (currentstate == ENEMY_STATE.CHASE)
		{

			ThisLineSight.Sensitity = MyLineSight.SightSensitivity.LOOSE;

			ThisAgent.Resume();
			ThisAgent.SetDestination(ThisLineSight.LastKnowSighting);

			while (ThisAgent.pathPending)
				yield return null;

			if (ThisAgent.remainingDistance <= ThisAgent.stoppingDistance)
			{
				ThisAgent.Stop();

				if (!ThisLineSight.CanSeeTarget)
					CurrentState = ENEMY_STATE.PATROL;
				else
					CurrentState = ENEMY_STATE.ATTACK;

				yield break;
			}

			yield return null;
		}
	}

	public IEnumerator IAAttack()
	{
		while (currentstate == ENEMY_STATE.ATTACK)
		{
			ThisAgent.Resume();
			ThisAgent.SetDestination(PlayerTransform.position);

			while (ThisAgent.pathPending)
				yield return null;

			if (ThisAgent.remainingDistance > ThisAgent.stoppingDistance)
			{
				CurrentState = ENEMY_STATE.CHASE;
				yield break;
			}
			else {
				PlayerHealth.HealthPoints -= MaxDamege * Time.deltaTime;
			}

			yield return null;
		}
	}

}
