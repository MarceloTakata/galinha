using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selecao : MonoBehaviour {

	private Vector2 posicaoInicial;
	private Vector2 posicaoFinal;

	public SpriteRenderer[] player;
	private static int idPlayer;
	private float scaleX, scaleY;
	private float arrastoX, arrastoY;
	private AudioSource[] audios;
	private int status;	// 0 = selecionando , 1 = baixando som
	private float tempo;

	// Use this for initialization
	void Start () {
		manter.idPlayer = 0;
		audios = GetComponents<AudioSource> ();
		status = 0;
		tempo = 1f;

		//player = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (status == 0) {
			if (Input.GetMouseButtonDown (0)) {
				posicaoInicial = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp (0)) {
				posicaoFinal = Input.mousePosition;
				if (posicaoFinal.x >= (Screen.width * 0.9f) && posicaoFinal.x <= (Screen.width * 0.98f) && posicaoFinal.y >= (Screen.height * 0.039f) && posicaoFinal.y <= (Screen.height * 0.165f)) {
					status = 1;
				} else {
					// Verificar se o arrasto foi na vertical ou na horizontal
					arrastoX = Mathf.Abs ((posicaoFinal.x - posicaoInicial.x));
					arrastoY = Mathf.Abs ((posicaoFinal.y - posicaoInicial.y));
					// Arrasto vertical
					if (arrastoX > arrastoY) {
						// Arrastou para direita
						if (posicaoFinal.x > posicaoInicial.x) {
							idPlayer += 1;
							if (idPlayer > (player.Length - 1)) {
								idPlayer = 0;
							}
						} else {
							// Arrastou para esquerda
							idPlayer -= 1;
							if (idPlayer < 0) {
								idPlayer = player.Length - 1;
							}
						}
						audios[idPlayer].Play ();
					}
					this.GetComponentInParent<SpriteRenderer> ().sprite = player [idPlayer].sprite;
				}
			}
		} else {
			if (status == 1) {
				tempo -= Time.deltaTime;
				if (tempo <= 0) {
					manter.idPlayer = idPlayer;
					SceneManager.UnloadScene ("inicio");
					SceneManager.LoadScene ("estrada");
				}
			}
		}
	}
}
