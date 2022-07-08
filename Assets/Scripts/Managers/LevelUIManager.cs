using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Managers
{
[RequireComponent(typeof(UIDocument))]
public class LevelUIManager : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    private UIDocument uiDocument;
    private VisualElement rootVisualElement;

    private void Start()
    {
        if (!TryGetComponent(out uiDocument))
        {
            throw new Exception("No UIDocument found.");
        }

        rootVisualElement = uiDocument.rootVisualElement;
    }

    public static void LoadResultsMenu()
    {
        
    }
}
}
