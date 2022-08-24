using System;
using System.Collections;
using Platforms;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Managers
{
public class LevelManager : MonoBehaviour
{
    // Injected before mono-behaviours' awake phase
    [Inject] private Player player;
    [Inject] private SceneLoader sceneLoader;
    [Inject] private LevelController levelController;
    [Inject] private InputManager inputManager;
    [Inject] private LevelUIManager levelUIManager;
    [Inject] private PerformanceManager performanceManager;


    private Vector3 initialPlayerPosition;

    private void Start()
    {
        #if UNITY_EDITOR
        PlayerStatistics.ResetLevel();
        #endif

        levelUIManager.StartDisplayingFPS( performanceManager );

        initialPlayerPosition = player.transform.position;

        void GenerateLevel()
        {
            levelController.GenerateLevel( PlayerStatistics.Level(), PlayerStatistics.Level() / 2 + 16 );
        }


        void RestartLevel()
        {
            levelController.DestroyLevel();
            inputManager.StopAllCoroutines();
            levelUIManager.SetInGameScore( 0 );
            GenerateLevel();
            player.TeleportTo( initialPlayerPosition );
        }

        GenerateLevel();

        player.OnDeath += RestartLevel;

        levelController.OnPointsIncrease += points => { levelUIManager.SetInGameScore( points ); };
        levelController.OnLevelPass += () => { levelUIManager.LoadResultsMenu(); };
        levelUIManager.OnNextLevelButtonClick += () =>
        {
            PlayerStatistics.IncreaseLevel();
            RestartLevel();
        };
    }


    private static class PlayerStatistics
    {
        public static void IncreaseLevel() => SetLevel( Level() + 1 );

        public static void ResetLevel() => SetLevel( 0 );

        private static void SetLevel( int level ) => PlayerPrefs.SetInt( "level", Math.Clamp( level, 0, 255 ) );

        public static int Level() => PlayerPrefs.GetInt( "level" );
    }
}
}