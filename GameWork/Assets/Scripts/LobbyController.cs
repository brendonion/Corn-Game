using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour {

    private Transform lobbyCorn;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Change scene!
    public void NextScene () {
        
        SceneManager.LoadScene("Main");
    }

    private void Start () {
        
        lobbyCorn = GameObject.Find("SpinningCorn_0").GetComponent<Transform>();
        posOffset = lobbyCorn.position;
    }

    private void Update () {
        
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        lobbyCorn.position = tempPos;
    }
}
