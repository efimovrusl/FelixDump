using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Managers
{
[RequireComponent( typeof( UIDocument ) )]
public class LevelUIManager : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    private UIDocument uiDocument;
    private VisualElement rootVisualElement;

    private Label inGameScoreLabel;

    private void Start()
    {
        if ( !TryGetComponent( out uiDocument ) )
        {
            throw new Exception( "No UIDocument found." );
        }

        rootVisualElement = uiDocument.rootVisualElement;

        inGameScoreLabel = rootVisualElement.Q<Label>( "InGameScoreLabel" );
    }

    public void LoadResultsMenu()
    {
    }
    
    public void SetInGameScore( int score )
    {
        inGameScoreLabel.text = $"{score}";
    }
}
}