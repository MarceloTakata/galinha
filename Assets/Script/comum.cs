using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comum : MonoBehaviour {

	private const float limiteInferior = -4.75f;
	private const float limiteEsquerdo = -8;
	private const float limiteDireito = 8;

	public static void Log(string msg){
		Debug.LogError (msg);
	}

	public static void PlayAudio(AudioSource audio) {
		// Se o áudio passado em "audio" não estiver tocando, pode tocar.
		if (!audio.isPlaying) {
			audio.Play ();
		}
	}

	public static Vector2 trataMovimentoOld(SpriteRenderer playerSR, Vector3 posicao, float X, float Y, float velocidade){
		float posRetaX, posRetaY;
		float posClickX, posClickY;
		float posicaoX, posicaoY;
		bool AcimaA, AcimaB;

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

		posicaoX = 0;
		posicaoY = 0;

		// Para cima
		if (AcimaA && AcimaB) {
			posicaoY = velocidade;
			posicaoX = 0;
		} else if (AcimaA && !AcimaB) {
			// Para esquerda
			if (X > limiteEsquerdo) {
				posicaoX = velocidade * -1;
				posicaoY = 0;
				playerSR.flipX = true;
			} else {
				// Verifica se excedeu limite esquerdo
				posicaoX = 0;
			}
		} else if (!AcimaA && AcimaB) {
			// Para direita
			if (X < limiteDireito) {
				posicaoX = velocidade;
				posicaoY = 0;
				playerSR.flipX = false;
			} else {
				// Verifica se excedeu limite direito
				posicaoX = 0;
			}
		} else if (!AcimaA && !AcimaB) {
			// Para baixo
			if (Y > limiteInferior) {
				posicaoY = velocidade * -1;
				posicaoX = 0;
			} else {
				// Verifica se excedeu limite inferior
				posicaoY = 0;
			}
		}
		return new Vector2(posicaoX, posicaoY);
	}

	public static Vector2 trataMovimento(SpriteRenderer playerSR, float X, float Y, Vector2 posicaoInicial, Vector2 posicaoFinal, float velocidade){
		float posicaoX = 0, posicaoY = 0;
		// Verificar se o arrasto foi na vertical ou na horizontal
		float arrastoX = Mathf.Abs ((posicaoFinal.x - posicaoInicial.x));
		float arrastoY = Mathf.Abs ((posicaoFinal.y - posicaoInicial.y));
		// Arrasto horizontal
		if (arrastoX > arrastoY) {
			// Arrastou para direita
			if (posicaoFinal.x > posicaoInicial.x) {
				if (X < limiteDireito) {
					posicaoX = velocidade;
					posicaoY = 0;
					playerSR.flipX = false;
				} else {
					// Verifica se excedeu limite direito
					posicaoX = 0;
				}
			} else {
				// Arrastou para esquerda
				if (X > limiteEsquerdo) {
					posicaoX = velocidade * -1;
					posicaoY = 0;
					playerSR.flipX = true;
				} else {
					// Verifica se excedeu limite esquerdo
					posicaoX = 0;
				}
			}
		} else {
			// Arrasto vertical
			// Arrastou para cima
			if (posicaoFinal.y > posicaoInicial.y) {
				posicaoY = velocidade;
				posicaoX = 0;
			} else {
				// Arrastou para baixo
				if (Y > limiteInferior) {
					posicaoY = velocidade * -1;
					posicaoX = 0;
				} else {
					// Verifica se excedeu limite inferior
					posicaoY = 0;
				}
			}
		}
		return new Vector2 (posicaoX, posicaoY);
	}

	public static Vector2 trataRecuo(float X, float Y, float esquerda, float direita, float recuo, int velocidadeVeiculo){
		float posicaoX, posicaoY;
		// Objeto em movimento: veículo
		if (velocidadeVeiculo > 0) {
			// Recua o player para baixo
			posicaoX = X;
			posicaoY = Y - recuo;
			// Verifica se excedeu limite inferior
			if (Y < limiteInferior) {
				posicaoY = limiteInferior;
			}
			// Se o player estiver do lado esquerdo do veículo
			if (X < esquerda) {
				// Recua o player para esquerda
				posicaoX = X - recuo;
				// Verifica se excedeu limite esquerdo
				if (X < limiteEsquerdo) {
					posicaoX = limiteEsquerdo;
				}
			} else {
				// Se o player estiver do lado direito do veículo
				// obs: colisao.collider.offset.x é o limite mais à direita do veículo em relação ao próprio veículo
				if (X > direita) {
					// Recua o player para direita
					posicaoX = X + recuo;
					// Verifica se excedeu limite direito
					if (X > limiteDireito) {
						posicaoX = limiteDireito;
					}
				}
			}
		} else {
			// Objeto parado: muro
			posicaoX = X;
			posicaoY = Y - recuo;
			//posicaoX = 0;
		}
		return new Vector2 (posicaoX, posicaoY);
	}

	public static Vector2 trataRio(float X, float Y, Collision2D colisao){
		float posicaoX, posicaoY;
		posicaoX = X;
		posicaoY = colisao.transform.position.y;
		return new Vector2 (posicaoX, posicaoY);
	}
}
