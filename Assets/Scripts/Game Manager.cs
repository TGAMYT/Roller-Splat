using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Button button;
    public static GameManager singleton;
    private  GroundModulator[] groundPieces;
    void Start()
    {
        button = GetComponent<Button>();
        SetNewLevel();
    }
    private void SetNewLevel()
    {
        groundPieces=FindObjectsOfType<GroundModulator>();
    }

    void Awake()
    {
        if (singleton == null)
        {
            singleton=this;
        }else if (singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onLevelFinishedLoading;
    }

    private void onLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished=true;
        for (int i = 0; i < groundPieces.Length; i++)
        {
            if (groundPieces[i].isColoured==false)
            {
                isFinished=false;
                break;
            }
        }
        if (isFinished)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex==7)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
