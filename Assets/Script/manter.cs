using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manter : MonoBehaviour {

	public static int idPlayer;
	public static float xPosPlayer;
	public static bool morri;
	public static bool flip;
	public static bool[] celeiroJaOcupado;
	public static int pontos;
	public static int fase;

	// Use this for initialization
	void Start () {
		manter.pontos = 0;
		manter.fase = 1;
		celeiroJaOcupado = new bool[10];
		for (int i = 0; i < 10; i++) {
			celeiroJaOcupado [i] = false;
		}
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
