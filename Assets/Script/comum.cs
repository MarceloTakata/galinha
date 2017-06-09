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

	public static Vector2 trataMovimento(SpriteRenderer playerSR, Vector3 posicao, float X, float Y, float velocidade){
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

	public static Vector2 trataRecuo(float X, float Y, float esquerda, float direita, float recuo, int velocidadeVeiculo){
		float posicaoX, posicaoY;
		// Objeto em movimento: veículo
		if (velocidadeVeiculo > 0) {
			// Recua o player para baixo
			posicaoX = X;
			posicaoY = Y - recuo;
			//posicaoY = -1 * recuo * velocidade;
			//posicaoX = 0;
			// Verifica se excedeu limite inferior
			if (Y < limiteInferior) {
				posicaoY = limiteInferior;
				posicaoY = 0;
			}
			// Se o player estiver do lado esquerdo do veículo
			if (X < esquerda) {
				// Recua o player para esquerda
				posicaoX = X - recuo;
				//posicaoX = -1 * recuo * velocidade;
				// Verifica se excedeu limite esquerdo
				if (X < limiteEsquerdo) {
					posicaoX = limiteEsquerdo;
					//posicaoX = 0;
				}
			} else {
				// Se o player estiver do lado direito do veículo
				// obs: colisao.collider.offset.x é o limite mais à direita do veículo em relação ao próprio veículo
				if (X > direita) {
					// Recua o player para direita
					posicaoX = X + recuo;
					//posicaoX = recuo * velocidade;
					// Verifica se excedeu limite direito
					if (X > limiteDireito) {
						posicaoX = limiteDireito;
						//posicaoX = 0;
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
}
