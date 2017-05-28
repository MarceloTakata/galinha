using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class galinha : MonoBehaviour {

	private const float limiteInferior = -4.75f;
	private const float limiteEsquerdo = -8;
	private const float limiteDireito = 8;

	private Vector3 posicao;

	private float posRetaX, posRetaY;
	private float posClickX, posClickY;
	private bool AcimaA, AcimaB;
	private Rigidbody2D player;
	private float posicaoX, posicaoY;
	public float velocidade;
	private SpriteRenderer playerSR;
	private AudioSource[] playerAS;
	private AudioSource audioMorri;
	private AudioSource audioBatida;
	private float tempoParaAndar;
	public float tempoParaAndarDefault;

	// Use this for initialization
	void Start () {
		player = GetComponent<Rigidbody2D> ();
		playerSR = GetComponent<SpriteRenderer> ();
		playerAS = GetComponents<AudioSource> ();
		audioMorri = playerAS [0];
		audioBatida = playerAS[1];
		posicaoX = 0;
		posicaoY = 0;
		tempoParaAndar = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// movimento do player
		// Vai diminuindo a contagem regressiva
		tempoParaAndar -= Time.deltaTime;
		// Se a contagem for zerada
		if (tempoParaAndar <= 0) {
			// Força a zerar o tempo
			tempoParaAndar = 0;
			// Se o botão esquerdo foi pressionado
			if (Input.GetMouseButton (0)) {
				// Pega a posição clicada
				posicao = Input.mousePosition;

				posClickX = posicao.x;
				posClickY = posicao.y;

				// Cálculo da posição referente à reta "a"
				posRetaX = (Screen.width * posClickY) / Screen.height;
				posRetaY = (Screen.height * posClickX) / Screen.width;
				AcimaA = (posClickX <= posRetaX) && (posClickY >= posRetaY);

				// Cálculo da posição referente à reta "b"
				posRetaX = Screen.width - (Screen.width * posClickY) / Screen.height;
				posRetaY = Screen.height - (Screen.height * posClickX) / Screen.width;
				AcimaB = (posClickX >= posRetaX) && (posClickY >= posRetaY);

				// Para cima
				if (AcimaA && AcimaB) {
					posicaoY = velocidade;
					posicaoX = 0;
				} else if (AcimaA && !AcimaB) {
					// Para esquerda
					if (player.position.x > limiteEsquerdo) {
						posicaoX = velocidade * -1;
						posicaoY = 0;
						playerSR.flipX = true;
					} else {
						// Verifica se excedeu limite esquerdo
						posicaoX = 0;
						player.position = new Vector2 (limiteEsquerdo, player.position.y);
					}
				} else if (!AcimaA && AcimaB) {
					// Para direita
					if (player.position.x < limiteDireito) {
						posicaoX = velocidade;
						posicaoY = 0;
						playerSR.flipX = false;
					} else {
						// Verifica se excedeu limite direito
						posicaoX = 0;
						player.position = new Vector2 (limiteDireito, player.position.y);
					}
				} else if (!AcimaA && !AcimaB) {
					// Para baixo
					if (player.position.y > limiteInferior) {
						posicaoY = velocidade * -1;
						posicaoX = 0;
					} else {
						// Verifica se excedeu limite inferior
						posicaoY = 0;
						player.position = new Vector2 (player.position.x, limiteInferior);
					}
				}
			} else {
				// Mantém o player parado, botão do mouse não clicado
				posicaoX = 0;
				posicaoY = 0;
			}
		} else {
			// Mantém o player parado, acabou de ser atropelado
			posicaoX = 0;
			posicaoY = 0;
		}
		// Atualiza a posição do player
		player.velocity = new Vector2(posicaoX, posicaoY);
	}

	void OnCollisionStay2D(Collision2D colisao) {
		// Se o objeto que colidiu com o player for V1, V2, V3,...,Vn (primeira posição da tag = "V")
		if (colisao.gameObject.tag.Substring (0, 1) == "V") {
			// Toca áudio de batida
			PlayAudio (audioBatida);
			// Recua o player para baixo
			posicaoY = player.position.y - velocidade / 10;
			// Verifica se excedeu limite inferior
			if (posicaoY < limiteInferior) {
				posicaoY = limiteInferior;
			}
			// Se o player estiver do lado esquerdo do veículo
			if (player.position.x < colisao.rigidbody.position.x) {
				// Recua o player para esquerda
				posicaoX = player.position.x - velocidade / 10;
				// Verifica se excedeu limite esquerdo
				if (posicaoX < limiteEsquerdo) {
					posicaoX = limiteEsquerdo;
				}
			} else {
				// Se o player estiver do lado direito do veículo
				// obs: colisao.collider.offset.x é o limite mais à direita do veículo em relação ao próprio veículo
				if (player.position.x > colisao.rigidbody.position.x + colisao.collider.offset.x) {
					// Recua o player para direita
					posicaoX = player.position.x + velocidade / 10;
					// Verifica se excedeu limite direito
					if (posicaoX > limiteDireito) {
						posicaoX = limiteDireito;
					}
				}
			}
			// Atualiza posição do player
			player.position = new Vector2 (posicaoX, posicaoY);
			// Inicializa a contagem regressiva para poder andar de novo
			tempoParaAndar = tempoParaAndarDefault;
		}
		// Se o objeto que colidiu com o player for do tipo celeiro
		if (colisao.gameObject.tag == "celeiro") {
			// TEMP: Soma 1 na pontuação
			pontuacao.pontos += 1;
			// Coloca o player na posição inicial
			player.position = new Vector2 (0, limiteInferior);
		}
	}

	void PlayAudio(AudioSource audio) {
		// Se o áudio passado em "audio" não estiver tocando, pode tocar.
		if (!audio.isPlaying) {
			audio.Play ();
		}
	}
}
