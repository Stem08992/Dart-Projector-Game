using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathText : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float distance = 0.2f;
    public Vector3 startPosition;
    [SerializeField] private float duration = 1f;
    private float timeRemaining;
    private bool isRunning = false;
    public TextMeshProUGUI deathTextMeshPro; 
    
    // Start is called before the first frame update
    void Start() {
        gameObject.SetActive(false);
    }

    private void Update() {
        float xOffset = Mathf.Sin(Time.time * speed) * distance;
        transform.localPosition += new Vector3(xOffset, 01f, 0);

        if (isRunning) {
            timeRemaining -= Time.deltaTime;
        }

        if (!(timeRemaining <= 0f)) return;
        timeRemaining = 0f;
        timeRemaining = duration;
        isRunning = false;
        gameObject.SetActive(false);
    }

    public void SpawnSelf(Vector3 position, int damage) {
        gameObject.SetActive(true);
        if (damage == 0) {
            deathTextMeshPro.text = "Miss Fire";   
        }
        else {
            deathTextMeshPro.text = damage.ToString();   
        }
        transform.localPosition = position;
        timeRemaining = duration;
        isRunning = true;
    }
}
