using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selecao : MonoBehaviour {

	private Vector2 posicaoInicial;
	private Vector2 posicaoFinal;

	public SpriteRenderer[] player;
//	private SpriteRenderer playerToSelect;
	public int idPlayer;
	private float scaleX, scaleY;
	private float arrastoX, arrastoY;

	// Use this for initialization
	void Start () {
		idPlayer = 0;
//		playerToSelect = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			posicaoInicial = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(0)) {
			posicaoFinal = Input.mousePosition;
			// Verificar se o arrasto foi na vertical ou na horizontal
			arrastoX = Mathf.Abs((posicaoFinal.x - posicaoInicial.x));
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
			} else {
				DontDestroyOnLoad(transform.gameObject);
				// Arrasto horizontal
				SceneManager.LoadScene ("Jogo");
			}
			this.GetComponentInParent<SpriteRenderer> ().sprite = player [idPlayer].sprite;
//			if (idPlayer == 0) {
//				scaleX = 0.56f;
//				scaleY = 0.56f;
//			} else {
//				scaleX = 0.25f;
//				scaleY = 0.25f;
//			}
//			playerToSelect.transform.localScale = new Vector2 (scaleX, scaleY);
		}
	}
}
