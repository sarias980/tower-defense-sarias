using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class mouse : MonoBehaviour {
	public GameObject turret;
	public GameObject turret2;
	public Grid thegrid;
	public static int vidas;
	public static int money;
	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject salida;
	bool turreta1=false;
	bool turreta2= false;
	float cont;
	Text moneytxt;
	GameObject panel1;
	GameObject panel2;
	// Use this for initialization
	void Start () {
		vidas = 5;
		money = 200;
		moneytxt = GameObject.Find ("Money").GetComponent<Text> ();
		panel1 = GameObject.FindGameObjectWithTag ("turret1pan");
		panel2 = GameObject.FindGameObjectWithTag ("turret2pan");
	}
	
	// Update is called once per frame
	void Update () {
		if (vidas <= 0) {
			GameObject.Find ("GameOver").GetComponent<Text> ().text = "GAME OVER";

            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);
            }

        } else {
			cont += Time.deltaTime;

			if (cont > 1) {
				int tipo = Random.Range (0, 3);
				Debug.Log (tipo);
				if (tipo == 1) {
					Instantiate (enemy1, salida.transform.position, Quaternion.identity);
				} else if (tipo == 2) {
					Instantiate (enemy2, salida.transform.position, Quaternion.identity);
				} else if (tipo == 0) {
					Instantiate (enemy3, salida.transform.position, Quaternion.identity);
				}
				cont = 0;

			}

			if (money >= 100) {
				panel1.SetActive (false);
			} else {
				panel1.SetActive (true);
			}
			if (money >= 50) {
				panel2.SetActive (false);
			} else {
				panel2.SetActive (true);
			}
			if (Input.GetMouseButtonDown (0)) { 
				RaycastHit hit; 
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
				if (Physics.Raycast (ray, out hit, 100.0f)) {
					if (hit.transform.tag == "zonatorres") {
						Node nodo = thegrid.GetNodeContainingPosition (hit.point);

						if (turreta1 && money >= 50) {
							money -= 50;
							moneytxt.text = "" + money;
							GameObject obj = Instantiate (turret, nodo.worldPosition, Quaternion.identity) as GameObject;

						} else if (turreta2 && money >= 100) {
							money -= 100;
							moneytxt.text = "" + money;
							GameObject obj = Instantiate (turret2, nodo.worldPosition, Quaternion.identity) as GameObject;
						}
						Debug.Log ("You selected the " + hit.transform.name); // ensure you picked right object
					}
				}
			}
            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene("2", LoadSceneMode.Single);
            }
        }
	}
	public void turrete1(){
		turreta1 = true;
		turreta2 = false;
	}
	public void turrete2(){
		turreta1 = false;
		turreta2 = true;
	}

    public void exit() {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("2", LoadSceneMode.Single);
        }
    }
}
