using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    [SerializeField] private GameObject Exit;
    void Start()
    {
        Main.SetActive(true);
        Exit.SetActive(false);
    }


    public void StartGame()
    {
        SceneManager.LoadScene("MAINGAME", LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Exit.SetActive(true);
    }

    public void EndCancel()
    {
        Exit.SetActive(false);
    }

    public void EndContinue()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		pplication.Quit();
#endif
    }
}
