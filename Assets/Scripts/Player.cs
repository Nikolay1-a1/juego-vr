using UnityEngine;
using System.Collections;
using Google.XR.Cardboard;

public class Player : MonoBehaviour {

	public bool canPick = false;

	public bool picked = false;
	public bool won = false;

	[SerializeField] private GameObject reticle;
	[SerializeField] private Color normalColor = Color.white;
	[SerializeField] private Color hoverColor = Color.red;
	[SerializeField] private float maxDistance = 10f;

	private Renderer reticleRenderer;

	void Start () {
		if (reticle != null) reticleRenderer = reticle.GetComponentInChildren<Renderer>();
	}

	void Update () {
		UpdateReticle();

		if (canPick == true) {
			bool triggered = Input.GetKeyDown("space");
#if !UNITY_EDITOR
			triggered = triggered || Api.IsTriggerPressed;
#endif
			if (triggered) {
				RaycastHit hit;

				if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance)) {

					Cup cup = hit.transform.GetComponent<Cup> ();
					if (cup != null) {
						canPick = false;

						picked = true;
						won = (cup.ball != null);

						cup.MoveUp ();
					}

				}
			}
		}
	}

	private void UpdateReticle() {
		if (reticle == null) return;

		RaycastHit hit;
		bool didHit = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance);

		float dist = didHit ? hit.distance : maxDistance;
		reticle.transform.position = transform.position + transform.forward * dist;

		bool overCup = didHit && hit.transform.GetComponent<Cup>() != null;
		if (reticleRenderer != null) {
			reticleRenderer.material.color = overCup ? hoverColor : normalColor;
		}
	}
}
