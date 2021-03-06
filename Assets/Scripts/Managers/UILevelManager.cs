using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Managers
{
[RequireComponent(typeof(UIDocument))]
public class UILevelManager : MonoBehaviour
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

        // gameManager = FindObjectOfType<GameManager>();
        rootVisualElement = uiDocument.rootVisualElement;
    }
}
}
