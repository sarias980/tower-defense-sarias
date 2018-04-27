using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musica : MonoBehaviour {

    public AudioSource sonido;
	public AudioClip fondo;


    // Use this for initialization
    void Start () {
        sonido = GetComponent<AudioSource>();
		sonido.clip = fondo;
        sonido.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
