using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
	
	public TextMeshProUGUI scoreCounter;
	private int score = 0;

	// Method for adding or subtracting from total score.
	public void ScoreCalculator (bool correctSwipe) {
		
		Debug.Log("Value passed in: " + correctSwipe);

		// If swipe was correct, add. Otherwise, deduct.
		if (correctSwipe) {
			score += 1;
		} else {
			score -= 1;
		}

		// Update UI accordingly.
		UpdateScore();
	}

	// Method sets UI to new score.
	private void UpdateScore () {
		scoreCounter.text = score.ToString();
	}
}
