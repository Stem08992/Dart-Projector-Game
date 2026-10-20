using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MainController : MonoBehaviour {
    public GameObject GameOverScreen;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI playerOneScoreText;
    public TextMeshProUGUI playerTwoScoreText;
    public GameObject Targets;
    public DeathText deathText;

    public List<string> players;
    public List<int> scores;
    public List<AudioClip> audios;
    public AudioSource audioSource;

    public GameObject TurnPlaque;
    public TextMeshProUGUI playerPlaqueTurnTextMeshPro;
    public TextMeshProUGUI roundPlaqueTurnTextMeshPro;
    
    public int currentPlayerIndex = 0;
    public int currentPlayerTurn = 0;
    public int roundCounter = 0;

    public int maxTurns = 3;
    public int maxRounds = 3;

    public TextMeshProUGUI playerNameAndScoreTextMeshPro;
    public TextMeshProUGUI playerTurnTextMeshPro;
    public TextMeshProUGUI playerRoundTextMeshPro;

    public float xBounds = 800;
    public float yBounds = -300;
    public float baseScale = 5;
    
    public enum GameState {
        SetUp,
        InGame,
        GameOver
    }
    
    public static MainController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public GameState gameState = GameState.SetUp;
    
    // Start is called before the first frame update
    private void Start() {
        GameOverScreen.SetActive(false);
        HideZombies();
        StartCoroutine(EnumeratorSetPlaque(players[currentPlayerIndex], roundCounter, true));
        SetTargets();
        UpdateInGameUI();
    }

    private void HideZombies() {
        for (int i = 0; i < Targets.transform.childCount; i++) {
            Targets.transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    public void CheckIfNextTurn(PossibleTargets possibleTargets, Vector3 position, int damage) {
        scores[currentPlayerIndex] += damage;
        deathText.SpawnSelf(position, damage);
        
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        switch (possibleTargets) {
            case PossibleTargets.Human: {
                audioSource.PlayOneShot(audios[0]);
                break;
            }
            case PossibleTargets.Zombie: {
                audioSource.PlayOneShot(audios[1]);
                break;
            } 
            case PossibleTargets.Background: {
                audioSource.PlayOneShot(audios[2]);
                break;
            }
            case PossibleTargets.Nothing: {
                break;
            }
        }
        
        
        currentPlayerTurn++;
        bool playerChange = false;
        if ((currentPlayerTurn + 1) > maxTurns) {
            currentPlayerTurn = 0;
            currentPlayerIndex++;
            playerChange = true;
        }
        
        bool nextRound = false;
        if (currentPlayerIndex >= players.Count) {
            currentPlayerIndex = 0;
            roundCounter++;
            nextRound = true;
        }

        if (playerChange) {
            StartCoroutine(EnumeratorSetPlaque(players[currentPlayerIndex], roundCounter, nextRound));   
        }
            
        if ((roundCounter + 1) >= (maxRounds + 1)) {
            gameState = GameState.GameOver;
            HideZombies();
            GameOverScreen.SetActive(true);
            if (scores[0] == scores[1]) {
                winnerText.text = "It's a Tie";
            }
            else{
                winnerText.text = scores[0] > scores[1] ? "Player 1 wins!" : "Player 2 wins!";
            }
            playerOneScoreText.text = "Player 1 Score: " + scores[0];
            playerTwoScoreText.text = "Player 2 Score: " + scores[1];
        }
        else {
            StartCoroutine(EnumeratorSetTargets());
            UpdateInGameUI();   
        }
    }

    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateInGameUI() {
        playerNameAndScoreTextMeshPro.text = "Player: " + players[currentPlayerIndex] + " | Points: " + scores[currentPlayerIndex];
        playerTurnTextMeshPro.text = "Turn: " + (currentPlayerTurn + 1) + " / " + maxTurns;
        playerRoundTextMeshPro.text = "Round: " + (roundCounter + 1) + " / " + maxRounds;
    }

    private IEnumerator EnumeratorSetTargets() {
        yield return new WaitForSeconds(0.5f);
        SetTargets();
    }
    
    private void SetTargets() {
        HideZombies();

        List<int> spots = new List<int>();
        for (int i = 0; i < Targets.transform.childCount; i++) {
            spots.Add(i);
        }

        for (int i = 0; i < 3; i++) {
            int listIndex = Random.Range(0, spots.Count);
            int targetIndex = spots[listIndex];
            spots.RemoveAt(listIndex);

            Transform target = Targets.transform.GetChild(targetIndex);
            target.gameObject.SetActive(true);

            float x = i switch {
                1 => Random.Range(-xBounds, -xBounds / 2f),
                2 => Random.Range(-xBounds / 5f, xBounds / 5f),
                _ => Random.Range(xBounds / 2f, xBounds)
            };
            float y = Random.Range(yBounds, -100f);
            float scale = (1.2f * y) / yBounds;

            target.localPosition = new Vector3(x, y, 0);
            target.localScale = new Vector3(baseScale, baseScale, 0) * scale;
        }
    }

    private IEnumerator EnumeratorSetPlaque(string playerName, int round, bool isRoundChange) {
        TurnPlaque.SetActive(true);
        audioSource.PlayOneShot(audios[3]);
        if (isRoundChange) {
            roundPlaqueTurnTextMeshPro.text = "Round " + (round + 1) + " Start!";
            playerPlaqueTurnTextMeshPro.gameObject.transform.localPosition = new Vector3(0, -26, 0);
        }
        else {
            roundPlaqueTurnTextMeshPro.text = "";
            playerPlaqueTurnTextMeshPro.gameObject.transform.localPosition = new Vector3(0, 54, 0);
            
        }
        playerPlaqueTurnTextMeshPro.text = playerName +"'s Turn";
        yield return new WaitForSeconds(2f);
        TurnPlaque.SetActive(false);
    }
}

public enum PossibleTargets {
    Nothing,
    Zombie,
    Human,
    Background
}