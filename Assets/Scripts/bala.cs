using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour {

	public int damage;

	private Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = 10 * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		//transform.LookAt(target);
	}

	public void Seek (Transform _target)
	{
		target = _target;
	}
}
