using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class celeiro : MonoBehaviour {

	public int idCeleiro;
	public bool jaOcupado;

	public Sprite celeiroVazio;
	public Sprite celeiroCheio;

	public AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		idCeleiro = int.Parse (this.gameObject.name.Substring (7, 1).ToString ());
		jaOcupado = manter.celeiroJaOcupado [idCeleiro];
		if (jaOcupado) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = celeiroCheio;
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = celeiroVazio;
		}
		// Altera a imagem do celeiro
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
				manter.pontos += 1;
				// Coloca o player na posição inicial
				//colisao.gameObject.transform.position = new Vector2 (0, comum.limiteInferior);
				// Marca que está ocupado
				jaOcupado = true;
				idCeleiro = int.Parse (this.gameObject.name.Substring (7, 1).ToString ());
				manter.celeiroJaOcupado [idCeleiro] = true;
				// Toca áudio de chegada
				comum.PlayAudio(audio);
				// Altera a imagem do celeiro
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = celeiroCheio;
				//SceneManager.UnloadSceneAsync ("rio");
				SceneManager.LoadSceneAsync ("estrada");
			}
		}
	}

}
