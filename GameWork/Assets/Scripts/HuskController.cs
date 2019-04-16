using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskController : MonoBehaviour {

	public GameObject Husk;
	public Animator anim;
	public string clip;
	public int layer;
	
	private bool isTouching = false;
	private int currentFrame = 0;
	
	void Update () {
		if (this.IsTopLayer()) {
			// Shift color from light to dark to indicate that the Husk is on top
			Husk.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f), new Color(0.85f, 0.85f, 0.85f), Mathf.PingPong(Time.time, 1));
			if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
				if (!isTouching) {
					Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
					RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

					if (hit.collider == Husk.GetComponent<BoxCollider2D>()) {
						isTouching = true;
					}
				} else if (isTouching) {
					this.Transition();
				}
			} else if (Input.touchCount == 0) {
				isTouching = false;
				// Destroy husk
				if (currentFrame == 5) {
					StartCoroutine(this.DestroyHusk());
				}
				// Reset husk position
				if (currentFrame != 0 && currentFrame != 5) {
					StartCoroutine(this.ReverseFrames());
				}
			}
		}
	}

	void Transition () {
		Vector3 huskPos = Camera.main.WorldToScreenPoint(Husk.transform.position);
		Vector3 fingerPos = Input.GetTouch(0).position;

		// huskPos.y = half of the sprite height
		if (fingerPos.y <= huskPos.y * 0.5f) {
			currentFrame = 5;
		} else if (fingerPos.y <= huskPos.y * 0.75f) {
			currentFrame = 4;
		} else if (fingerPos.y <= huskPos.y) {
			currentFrame = 3;
		} else if (fingerPos.y <= huskPos.y * 1.25f) {
			currentFrame = 2;
		} else if (fingerPos.y <= huskPos.y * 1.5f) {
			currentFrame = 1;
		}

		this.PlayFrame(currentFrame);
	}

	void PlayFrame (int frame) {
		anim.speed = 0;
		anim.Play(clip, 0, (1f / 6) * frame);
	}

	bool IsTopLayer () {
		// Get Husk siblings minus the Cob
		bool isTopLayer = Husk.transform.parent && layer == Husk.transform.parent.childCount - 1;
		Husk.GetComponent<BoxCollider2D>().enabled = isTopLayer;
		return isTopLayer;
	}

	IEnumerator ReverseFrames () {
		for (int i = currentFrame; i >= 0; i--) {
			currentFrame = i;
			this.PlayFrame(i);
        	yield return new WaitForSeconds(0.05f);
		}
    }

	IEnumerator DestroyHusk () {
		// Sets the Husk's gravity so it can fall
		Husk.GetComponent<Rigidbody2D>().gravityScale = 3f;
		// Remove the Husk's transform parent
		Husk.transform.parent = null;
		
		// While visible on screen
		while (Husk.transform.position.y >= -10) {
			// Rotate 25 degrees every 0.05 seconds
			Husk.transform.Rotate(Vector3.down, 25);
			yield return new WaitForSeconds (0.05f);
		}

		Destroy(Husk);
	}
}
