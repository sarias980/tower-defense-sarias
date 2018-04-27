using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class scr_enemys : MonoBehaviour {


	int vida;
	int oro;
	public int type;

	private Grid grid;
	public int speed;

	public List<Node> resultPath;
	public Vector3 startPos;
	private GameObject exit;
	public Node currentNode;
	public AudioClip audioDie;
	AudioSource audioS;
	private float currentHealth = 150.0f;
	private float maxHealth = 150.0f;
	private Quaternion rotation;
	public Text moneytxt;
	public Text vidatxt;
    public GameObject enemy;

    // Use this for initialization
    void Start () {
		audioS = GameObject.Find("Main Camera").GetComponent<AudioSource> ();
		moneytxt = GameObject.Find ("Money").GetComponent<Text> ();
		vidatxt = GameObject.Find ("Vida").GetComponent<Text> ();
		exit = GameObject.FindGameObjectWithTag("exit");
		grid = GameObject.FindGameObjectWithTag("mapa").GetComponent<Grid>();

		currentNode = grid.GetNodeContainingPosition(transform.position);
		transform.position = currentNode.worldPosition;

		resultPath = grid.FindPath(transform.position, exit.transform.position);
	
		switch (type) {
		case 1:
			vida = 25;
			speed = 5;
			oro = 5;
			break;
		case 2:
			oro = 20;
			vida = 100;
			speed = 1;
			break;
		case 0:
			oro = 10;
			vida = 40;
			speed = 3;
			break;
		}
    }
	
	// Update is called once per frame
	void Update () {
		if (mouse.vidas > 0) {
			if (vida <= 0) {

				audioS.PlayOneShot (audioDie);
				mouse.money += oro;
				moneytxt.text = "" + mouse.money;
				Destroy (this.gameObject);
			}
			currentNode = grid.GetNodeContainingPosition (transform.position);
			resultPath = grid.FindPath (transform.position, exit.transform.position);
			if (Mathf.Round (transform.position.x * 10) / 10 == currentNode.worldPosition.x && Mathf.Round (transform.position.y * 10) / 10 == currentNode.worldPosition.y) {
				resultPath = grid.FindPath (transform.position, exit.transform.position);
				Debug.Log ("ese");
			}
	
			Rotation (resultPath [0].worldPosition);

			if (resultPath != null) {    
				transform.position = Vector3.MoveTowards (transform.position, resultPath [0].worldPosition, speed * Time.deltaTime);
			}
			if (Vector3.Distance (transform.position, exit.transform.position) < 1) {
				mouse.vidas -= 1;
				vidatxt.text = "Vidas: " + mouse.vidas;
				Destroy (this.gameObject);
			}
		}
    }

	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "bala") {
			int damage = other.GetComponent<bala> ().damage;
			vida -= damage;
			Debug.Log ("hit");


			Destroy (other.gameObject);
		}

	} 
	public void Rotation(Vector3 target)
	{
		Vector3 dir = target - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle-90, transform.forward);
	}
}
