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

	// Use this for initialization
	void Start () {
		carro = GetComponent<Rigidbody2D> ();
		if (this.gameObject.scene.name == "estrada" && carro.tag.Substring (0, 1).ToString () == "V" ||
		    this.gameObject.scene.name == "rio" && carro.tag.Substring (0, 1).ToString () == "O") {
			posInicialY = carro.position.y;
			velocidade = mover * (Random.Range (2, 4)) * direcao;
		} else {
			carro.position = new Vector2 (50, 50);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.scene.name == "estrada" && carro.tag.Substring(0,1).ToString() == "V" ||
			this.gameObject.scene.name == "rio" && carro.tag.Substring(0,1).ToString() == "O") {
			carro.velocity = new Vector2 (velocidade, posInicialY);
		} else {
			carro.position = new Vector2 (50, 50);
		}
	}

	void OnBecameInvisible() {
		if (this.gameObject.scene.name == "estrada" && carro.tag.Substring (0, 1).ToString () == "V" ||
		    this.gameObject.scene.name == "rio" && carro.tag.Substring (0, 1).ToString () == "O") {
			Instantiate (veiculoPreFab, new Vector2 (posInicial + (-1 * direcao * Random.Range (2, 5)), posInicialY), transform.localRotation);
			Destroy (this.gameObject);
		} else {
			carro.velocity = new Vector2 (50, 50);
		}
	}
}
