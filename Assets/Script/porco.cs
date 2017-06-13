using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class porco : MonoBehaviour {

	private Vector3 posicao;

	private Rigidbody2D player;
	public float velocidade;
	private SpriteRenderer playerSR;
	private AudioSource[] playerAS;
	private AudioSource audioMorri;
	private AudioSource audioBatida;
	private float tempoParaAndar;
	public float tempoParaAndarDefault;
	private int local;	// 0 = estrada , 1 = rio
	public int myId;
	public float recuo;

	private Vector2 posicaoInicial;
	private Vector2 posicaoFinal;

	private Vector2 ultimaPosicao;
	private int tempoParadoAgua;

	// Use this for initialization
	void Start () {
		player = GetComponent<Rigidbody2D> ();
		playerSR = GetComponent<SpriteRenderer> ();
		playerAS = GetComponents<AudioSource> ();
		audioMorri = playerAS [0];
		audioBatida = playerAS[1];
		tempoParaAndar = 0;
		local = 0;
		ultimaPosicao = new Vector2 (0, 0);
		tempoParadoAgua = 0;
	}

	// Update is called once per frame
	void Update () {
		Vector2 move;
		int debugMove;
		if (manter.idPlayer != myId) {
			this.transform.position = new Vector2(0,-10);
			return;
		}

		// tratamento dentro do rio
		move = player.position;
		comum.trataRio (ref move, ref ultimaPosicao, recuo, audioMorri, ref tempoParadoAgua);
		player.position = move;

		// movimento do player
		// Vai diminuindo a contagem regressiva
		tempoParaAndar -= Time.deltaTime;
		// Se a contagem for zerada
		if (tempoParaAndar <= 0) {
			// Força a zerar o tempo
			tempoParaAndar = 0;
			move = new Vector2 (0, 0);
			debugMove = 1;

			if (debugMove == 0) {
				if (Input.GetMouseButtonDown (0)) {
					posicaoInicial = Input.mousePosition;
					move = new Vector2 (0, 0);
				}
				if (Input.GetMouseButtonUp (0)) {
					posicaoFinal = Input.mousePosition;
					move = comum.trataMovimento (playerSR, this.transform.position.x, this.transform.position.y, posicaoInicial, posicaoFinal, velocidade);
				}
			} else {
				// Se o botão esquerdo foi pressionado
				if (Input.GetMouseButton (0)) {
					// Pega a posição clicada
					posicao = Input.mousePosition;
					move = comum.trataMovimentoOld (playerSR, posicao, this.transform.position.x, this.transform.position.y, velocidade, audioBatida);
				} else {
					// Mantém o player parado, botão do mouse não clicado
					move = new Vector2(0, 0);
				}
			}
		} else {
			// Mantém o player parado, acabou de ser atropelado
			move = new Vector2(0, 0);
		}

		// Atualiza a posição do player
		player.velocity = move;
	}

	void OnCollisionEnter2D(Collision2D colisao) {
		string tipoVeiculo;
		int velocidadeVeiculo;
		tipoVeiculo = colisao.gameObject.tag.Substring (0, 1);
		// Se o objeto que colidiu com o player for V1, V2, V3,...,Vn (primeira posição da tag = "V")
		if (tipoVeiculo == "V") {
			velocidadeVeiculo = int.Parse (colisao.gameObject.tag.Substring (1, 1));
			// Toca áudio de batida
			comum.PlayAudio (audioBatida);
			// Atualiza a posição do player
			player.position = comum.trataRecuo (this.transform.position.x, this.transform.position.y, colisao.transform.position.x, colisao.transform.position.x + colisao.collider.offset.x, recuo, velocidadeVeiculo);
			// Inicializa a contagem regressiva para poder andar de novo
			tempoParaAndar = tempoParaAndarDefault;
		}
	}
}