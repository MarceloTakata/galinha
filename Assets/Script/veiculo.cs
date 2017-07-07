using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class veiculo : MonoBehaviour {

	public int mover;
	private Rigidbody2D carro;
	private float velocidade;
	private float posInicialY;
	public GameObject veiculoPreFab;
	public float posInicial;
	public int direcao;

	float minSpeed = 2f;
	float maxSpeed = 3f;

	float tipoV = 0f;

	// Use this for initialization
	void Start () {
		carro = GetComponent<Rigidbody2D> ();
		if (this.gameObject.scene.name == "estrada" && carro.tag.Substring (0, 1).ToString () == "V" ||
		    this.gameObject.scene.name == "rio" && carro.tag.Substring (0, 1).ToString () == "O") {
			posInicialY = carro.position.y;
			tipoV = 1f;
			if (carro.tag.Substring (0, 1).ToString () == "O") {
				tipoV = Random.Range (1f, 1.5f);
			}
			velocidade = mover * (Random.Range (minSpeed, maxSpeed)) * direcao * tipoV;
		} else {
			carro.position = new Vector2 (50, 50);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (this.gameObject.scene.name == "estrada" && carro.tag.Substring(0,1).ToString() == "V" ||
			this.gameObject.scene.name == "rio" && carro.tag.Substring(0,1).ToString() == "O") {
			carro.velocity = new Vector2 (velocidade, posInicialY);
		} else {
			carro.velocity = new Vector2(50, 50);
		}
	}

	void OnBecameInvisible() {
		if (this.gameObject.scene.name == "estrada" && carro.tag.Substring (0, 1).ToString () == "V" ||
		    this.gameObject.scene.name == "rio" && carro.tag.Substring (0, 1).ToString () == "O") {
			carro.position = new Vector2 (posInicial + (-1 * direcao * Random.Range (minSpeed, maxSpeed)), posInicialY);
		} else {
			carro.velocity = new Vector2 (50, 50);
		}
	}
}
