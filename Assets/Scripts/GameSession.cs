using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playersLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    int score = 0;
    // Start is called before the first frame update
    void Awake()
    {


        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions> 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playersLives.ToString();
        scoreText.text = score.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        livesText.text = playersLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playersLives == 1)
        {
            StartCoroutine(ResetGameSession());
            return;
        }
        StartCoroutine(TakeLife());


    }
    public void GainScore(int value)
    {
        score+=value;
    }
    IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene("Level 1");
        Destroy(gameObject);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
    }


    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        playersLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
