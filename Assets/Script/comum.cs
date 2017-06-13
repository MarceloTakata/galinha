using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class comum : MonoBehaviour {

	public const float limiteInferior = -4.35f;
	public const float limiteEsquerdo = -8;
	public const float limiteDireito = 8;

	public static void Log(string msg){
		Debug.LogError (msg);
	}

	public static void PlayAudio(AudioSource audio) {
		// Se o áudio passado em "audio" não estiver tocando, pode tocar.
		if (!audio.isPlaying) {
			audio.Play ();
		}
	}

	public static Vector2 trataMovimentoOld(SpriteRenderer playerSR, Vector3 posicao, float X, float Y, float velocidade, AudioSource audio){
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
		PlayAudio (audio);
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
					posicaoX = velocidade * 10;
					posicaoY = 0;
					playerSR.flipX = false;
				} else {
					// Verifica se excedeu limite direito
					posicaoX = 0;
				}
			} else {
				// Arrastou para esquerda
				if (X > limiteEsquerdo) {
					posicaoX = velocidade * -10;
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
				posicaoY = velocidade * 10;
				posicaoX = 0;
			} else {
				// Arrastou para baixo
				if (Y > limiteInferior) {
					posicaoY = velocidade * -10;
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

	public static void trataRio(ref Vector2 posicaoPlayer, ref Vector2 ultimaPosicao, float recuo, AudioSource audio, ref int tempo){
		// verifica se o jogador está parado no rio. Se está, então vai afogar.
		if (posicaoPlayer.y > -3f && posicaoPlayer.y < 2.5f) {
			if (posicaoPlayer == ultimaPosicao) {
				tempo += 1;
				if (tempo > 20) {
					manter.morri = true;
					SceneManager.UnloadScene ("rio");
					SceneManager.LoadScene ("estrada");
				}
			} else {
				tempo = 0;
			}
		} else {
			tempo = 0;
		}
		ultimaPosicao = posicaoPlayer;

		// verifica constantemente se chegou nos limites direito e esquerdo (tora / tartaruga)
		if (posicaoPlayer.x > comum.limiteDireito) {
			posicaoPlayer = new Vector2 (comum.limiteDireito - recuo, posicaoPlayer.y);
		} else if (posicaoPlayer.x < comum.limiteEsquerdo) {
			posicaoPlayer = new Vector2 (comum.limiteEsquerdo + recuo, posicaoPlayer.y);
		}
	}
}
