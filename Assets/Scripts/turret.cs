using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour {

	float vel;
	public int dano=15;
	public float range = 15f;
	private Transform target;
	private scr_enemys targetEnemy;
	public string enemyTag = "Enemy";
	public AudioClip audioShoot;
	AudioSource audioS;
	public float turnSpeed = 10f;

	public float fireRate = 1f;
	private float fireCountdown = 0f;
	Transform canon;
	public Transform firePoint;
	Quaternion q;
	public GameObject bala;
	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
		canon=this.transform.Find ("canon");
		audioS = GameObject.Find("Main Camera").GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
		{
			

			return;
		}
		if (fireCountdown <= 0f)
		{
			Debug.Log ("shot");
			Shoot ();
			fireCountdown = 1f / fireRate;
		}

		fireCountdown -= Time.deltaTime;
		LockOnTarget();

	}
	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<scr_enemys>();
		} else
		{
			target = null;
		}

	}
	void LockOnTarget ()
	{
		Vector3 vectorToTarget = target.position - transform.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		 q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 12);

	
	}
	void Shoot(){
		
		audioS.PlayOneShot (audioShoot);

		GameObject bullet=Instantiate (bala, canon.position,q);
		bala bulletscr = bullet.GetComponent<bala>();
		bullet.GetComponent<bala> ().damage = dano;
		if (bulletscr != null)
			bulletscr.Seek(target);
	}


	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}


}
