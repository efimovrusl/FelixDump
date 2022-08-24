using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

namespace Managers
{
[RequireComponent( typeof( UIDocument ) )]
public class LevelUIManager : MonoBehaviour
{
    private const string ParentScene = "LevelScene";

    private UIDocument uiDocument;
    private VisualElement rootVisualElement;

    private VisualElement inGameUI;
    private VisualElement resultsMenu;

    private VisualElement nextLevelButton;

    private Label inGameScoreLabel;
    private Label inGameFPSLabel;

    private event Action OnDestroyAction;
    public event Action OnNextLevelButtonClick;


    private void Start()
    {
        if ( !TryGetComponent( out uiDocument ) )
        {
            throw new Exception( "No UIDocument found." );
        }

        rootVisualElement = uiDocument.rootVisualElement;

        inGameUI = rootVisualElement.Q( "InGameUI" );
        resultsMenu = rootVisualElement.Q( "LevelResultsMenu" );
        
        inGameScoreLabel = rootVisualElement.Q<Label>( "InGameScoreLabel" );
        inGameFPSLabel = rootVisualElement.Q<Label>( "InGameFPSLabel" );
        nextLevelButton = rootVisualElement.Q<Button>( "nextLevelButton" );

        nextLevelButton.RegisterCallback<ClickEvent>( _ =>
        {
            OnNextLevelButtonClick?.Invoke();
            UnloadAllMenus();
        } );

        LoadInGameUI();
    }

    public void LoadInGameUI()
    {
        UnloadAllMenus();
        inGameUI.style.display = DisplayStyle.Flex;
    }
    
    public void LoadResultsMenu()
    {
        UnloadAllMenus();
        resultsMenu.style.display = DisplayStyle.Flex;
    }

    private void UnloadAllMenus()
    {
        if ( inGameUI == null ) throw new Exception( "No InGameUI uxml tree" );
        inGameUI.style.display = DisplayStyle.None;
        
        if ( resultsMenu == null ) throw new Exception( "No results menu uxml tree" );
        resultsMenu.style.display = DisplayStyle.None;
    }


    public void SetInGameScore( int score )
    {
        inGameScoreLabel.text = $"{score}";
    }

    public void StartDisplayingFPS( PerformanceManager performanceManager )
    {
        void Set20FramesAvgFPS( int fps ) => inGameFPSLabel.text = $"{fps}";
        performanceManager.OnFpsChange += Set20FramesAvgFPS;
        OnDestroyAction += () => performanceManager.OnFpsChange -= Set20FramesAvgFPS;
    }

    private void OnDestroy()
    {
        OnDestroyAction?.Invoke();
    }
}
}