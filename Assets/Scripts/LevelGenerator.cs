using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platform30; 
    [SerializeField] private GameObject platform45; 
    [SerializeField] private GameObject redPlatform22; 
    [SerializeField] private GameObject redPlatform45;
    [SerializeField] private GameObject helix;

    [SerializeField] private Player _player;

    private void Awake()
    {
        _player.onDeath.AddListener(() =>
        {
            Time.timeScale = 0;
            StartCoroutine(Timer(2, () => SceneManager.LoadScene("MainScene")));
            

        });

    }
    

    IEnumerator Timer(float seconds, Action fuck)
    {
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        fuck.Invoke();
    }

    private void Update()
    {
        // throw new NotImplementedException();
    }
}
