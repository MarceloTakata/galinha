using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class celeiro : MonoBehaviour {

	private const float limiteInferior = -4.75f;

	public int idCeleiro;
	public bool jaOcupado;

	public Sprite celeiroVazio;
	public Sprite celeiroCheio;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D colisao)
	{
		// Se o celeiro não estiver ocupado
		if (!jaOcupado) {
			// Verifica se quem colidiu foi o player
			if (colisao.gameObject.tag == "Player") {
				// TEMP: Soma 1 na pontuação
				pontuacao.pontos += 1;
				// Coloca o player na posição inicial
				colisao.gameObject.transform.position = new Vector2 (0, limiteInferior);
				// Marca que está ocupado
				jaOcupado = true;
				// Altera a imagem do celeiro
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = celeiroCheio;
			}
		}
	}

}
