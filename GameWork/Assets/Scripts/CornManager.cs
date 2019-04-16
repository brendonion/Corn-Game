using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that manages the creation and manipulation of corn.
public class CornManager : MonoBehaviour {

    // Set the values used to determine if a corn is swiped left or right.
    const float swipedLeft = -2.0f;
    const float swipedRight = 2.0f;

	public GameObject Corn;
    public GameObject Cob;
    public CornUtil cornUtil;
    public Sprite goodCorn;
    public Sprite badCorn; 

    public ScoreManager scoreManager;

    private bool isTouching = false;
    private bool isNaked = false;
    private bool gameStarted = true;
    private bool isGoodCorn;

    private void Update () {

        if (gameStarted) {
            // Initialize corn with 4 - 6 husks to start.
            this.InitializeCorn(Random.Range(4, 7));
            gameStarted = false;
        }

        // If a corn has been husked enter the if.        
        if (isNaked) {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
                if (!isTouching) {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                    if (hit.collider == Corn.GetComponentInChildren<BoxCollider2D>()) {
                        isTouching = true;
                    }
                } else if (isTouching && Input.GetTouch(0).phase == TouchPhase.Moved) {
                    Vector3 fingerPos = Input.GetTouch(0).position;
                    fingerPos.z = 8f;
                    Vector3 realWorldPos = Camera.main.ScreenToWorldPoint(fingerPos);

                    Corn.transform.position = realWorldPos;
                }
                
            } else if (isTouching && Input.GetTouch(0).phase == TouchPhase.Ended) {
                cornUtil.InitializeStartingPosition(Corn.transform.position, cornUtil.centerPosition, newLaunchTime: 0.2f);
                isTouching = false;
            } else if (Input.touchCount == 0) {
                cornUtil.ExtrapolateCurrentPosition(cornUtil.endingPosition);
            }

            if (Corn.transform.position.x > swipedRight) {
                
                cornUtil.InitializeStartingPosition(Corn.transform.position, new Vector3(5f, 0), 0.1f);
                if (this.DoneTravelling()) {
                    // If corn is good add points. Otherwise deduct.
                    scoreManager.ScoreCalculator(isGoodCorn);
                    // TODO: Determine when to increase husk count.
                    this.InitializeCorn(Random.Range(4, 7));
                }
            } else if (Corn.transform.position.x < swipedLeft) {

                cornUtil.InitializeStartingPosition(Corn.transform.position, new Vector3(-5f, 0), 0.1f);
                if (this.DoneTravelling()) {
                    // If corn is good add points. Otherwise deduct.
                    scoreManager.ScoreCalculator(!isGoodCorn);
                    this.InitializeCorn(Random.Range(4, 7));
                }
            }
        }

        // If no touches are registered enter.
        if (Input.touchCount == 0 && !gameStarted) {
            cornUtil.ExtrapolateCurrentPosition(cornUtil.endingPosition);
        }

        // If the cob is the only thing that is left set isNaked to true;
        if (Corn.transform.childCount <= 1) {
            isNaked = true;
        }
    }

    // Method used to reset a husked and swiped corn to it's prior glory.
    public void InitializeCorn (int huskCount) {

        // Set position to the specified initial position.
        Corn.transform.position = cornUtil.initialPosition;
        
        // Create new husks and attach.
        // FUTURE ISSUE: the position values are hard coded. When you pass in Vector3 coordinates this way they are world coordinates, not coordinates related to the parent.
        for (int i = 1; i <= huskCount; i++) {
            int rand = Random.Range(1, 4);
            if (i != huskCount) {
                // Spawn each husk at least once
                if (i == 1 || (i != 2 && i != 3 && rand == 1)) {
                    GameObject MidHusk = (GameObject) Instantiate(Resources.Load("Husks/MiddleHusk"), new Vector3 (4.323f, -5.832f), Quaternion.Euler(0, 0, 0), Corn.transform);
                    MidHusk.GetComponent<HuskController>().layer = i;
                    MidHusk.GetComponent<SpriteRenderer>().sortingOrder = i;
                }
                if (i == 2 ||  (i != 1 && i != 3 && rand == 2)) {
                    GameObject LeftHusk = (GameObject) Instantiate(Resources.Load("Husks/LeftHusk"), new Vector3 (3.227f, -5.832f), Quaternion.Euler(0, 0, 0), Corn.transform);
                    LeftHusk.GetComponent<HuskController>().layer = i;
                    LeftHusk.GetComponent<SpriteRenderer>().sortingOrder = i;
                }
                if (i == 3 ||  (i != 1 && i != 2 && rand == 3)) {
                    GameObject RightHusk = (GameObject) Instantiate(Resources.Load("Husks/RightHusk"), new Vector3 (5.373f, -5.832f), Quaternion.Euler(0, 0, 0), Corn.transform);
                    RightHusk.GetComponent<HuskController>().layer = i;
                    RightHusk.GetComponent<SpriteRenderer>().sortingOrder = i;
                }
            } else {
                // Guarantee the middle husk is the last one to spawn
                GameObject MidHusk = (GameObject) Instantiate(Resources.Load("Husks/MiddleHusk"), new Vector3 (4.323f, -5.832f), Quaternion.Euler(0, 0, 0), Corn.transform);
                MidHusk.GetComponent<HuskController>().layer = i;
                MidHusk.GetComponent<SpriteRenderer>().sortingOrder = i;
            }
        }

        // Set bool denoting that the corn is in need of husking. 
        isNaked = false;

        // Randomly choose the cob sprite, set bool value for scoring.
        if (Random.Range(1, 3) == 1) {
            isGoodCorn = true;
            Cob.GetComponent<SpriteRenderer>().sprite = goodCorn;
        } else {
            isGoodCorn = false;
            Cob.GetComponent<SpriteRenderer>().sprite = badCorn;
        }

        // Set initial position for launch.
        cornUtil.InitializeStartingPosition(cornUtil.initialPosition, cornUtil.centerPosition);
    }

    public bool DoneTravelling () {
        return Corn.transform.position == cornUtil.endingPosition;
    }
}
