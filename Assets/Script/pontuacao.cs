using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pontuacao : MonoBehaviour {

	public static int pontos;
	public static int fase;
	public Text pontosTXT;

	public Sprite celeiroVazio;
	public Sprite celeiroCheio;
	private SpriteRenderer[] celeiros;
	private celeiro[] cel;

	// Use this for initialization
	void Start () {
		pontos = 0;
		fase = 1;
	}
	
	// Update is called once per frame
	void Update () {
		pontosTXT.text = "Pontos: " + pontos.ToString () + " Fase: " + fase.ToString () + " Player selectonado: " + selecao.idPlayer;
		// Se os 5 celeiros forem ocupados
		if (pontuacao.pontos == 5) {
			// Zera
			pontuacao.pontos = 0;
			// Sobe de fase
			pontuacao.fase += 1;
			// Obtém todos os objetos do tipo SpriteRenderer
			celeiros = FindObjectsOfType<SpriteRenderer> ();
			// Loop para pegar os objetos do tipo SpriteRenderer
			foreach (SpriteRenderer c in celeiros){
				// Se for celeiro
				if (c.tag == "celeiro") {
					// Troca imagem para celeiro vazio
					c.sprite = celeiroVazio;
				}
			}
			// Obtém todos os objetos do tipo celeiro (classe)
			cel = FindObjectsOfType<celeiro> ();
			// Loop para pegar todos os objetos do tipo celeiro
			foreach (celeiro c in cel) {
				// Esvazia o celeiro
				c.jaOcupado = false;
			}
		}
	}
}
