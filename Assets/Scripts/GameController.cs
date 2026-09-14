using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public TextMesh infoText;
	public GameObject ball;
	public Player player;
	public Cup[] cups;

	public PauseMenuController pauseMenu;

	private int aciertos = 0;
	private int intentos = 0;

	void Start () {
		infoText.text = "\u00a1Elige el vaso correcto!";

		StartCoroutine (ShuffleRoutine());
	}

	void Update () {
		if (player.picked) {
			intentos++;
			if (player.won) {
				aciertos++;
				infoText.text = "\u00a1Ganaste!";
			} else {
				infoText.text = "Perdiste, \u00a1int\u00e9ntalo de nuevo!";
			}

			player.picked = false;
			pauseMenu.Show(aciertos, intentos);
		}
	}

	public void ReiniciarRonda () {
		foreach (Cup cup in cups) {
			cup.ball = null;
		}
		infoText.text = "\u00a1Elige el vaso correcto!";
		StartCoroutine (ShuffleRoutine());
	}

	private IEnumerator ShuffleRoutine () {
		yield return new WaitForSeconds (1f);

		foreach (Cup cup in cups) {
			cup.MoveUp ();
		}

		yield return new WaitForSeconds (0.5f);

		Cup targetCup = cups[Random.Range(0, cups.Length)];
		targetCup.ball = ball;
		ball.transform.position = new Vector3 (
			targetCup.transform.position.x,
			ball.transform.position.y,
			targetCup.transform.position.z
		);

		yield return new WaitForSeconds (1.0f);

		foreach (Cup cup in cups) {
			cup.MoveDown ();
		}

		yield return new WaitForSeconds (1.0f);

		for (int i = 0; i < 5; i++) {
			Cup cup1 = cups[Random.Range(0, cups.Length)];
			Cup cup2 = cup1;

			while (cup2 == cup1) {
				cup2 = cups[Random.Range(0, cups.Length)];
			}

			Vector3 cup1Position = cup1.targetPosition;

			cup1.targetPosition = cup2.targetPosition;
			cup2.targetPosition = cup1Position;

			yield return new WaitForSeconds (0.75f);
		}

		player.canPick = true;
	}
}
