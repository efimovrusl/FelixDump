using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Managers
{
[RequireComponent(typeof(UIDocument))]
public class UIMenuManager : MonoBehaviour
{
    [Inject] private GameManager gameManager;

    private UIDocument uiDocument;
    private VisualElement rootVisualElement;

    private VisualElement mainMenu;
    private VisualElement startButton;
    private VisualElement settingsButton;
    private VisualElement quitButton;
    
    private VisualElement mainSettings;
    private VisualElement backToMainMenuButton;
    
    
    private void Start()
    {
        if (!TryGetComponent(out uiDocument))
        {
            throw new Exception("No UIDocument found.");
        }

        // gameManager = FindObjectOfType<GameManager>();
        rootVisualElement = uiDocument.rootVisualElement;
        
        mainMenu = rootVisualElement.Q("MainMenu");
        startButton = rootVisualElement.Q("startButton");
        settingsButton = rootVisualElement.Q("settingsButton");
        quitButton = rootVisualElement.Q("quitButton");

        mainSettings = rootVisualElement.Q("MainSettings");
        backToMainMenuButton = rootVisualElement.Q("backToMainMenuButton");

        startButton.RegisterCallback<ClickEvent>(_ => gameManager.LoadLevelScene());
        settingsButton.RegisterCallback<ClickEvent>(_ => EnableMainSettings());
        quitButton.RegisterCallback<ClickEvent>(_ => gameManager.QuitApplication());

        backToMainMenuButton.RegisterCallback<ClickEvent>(_ => EnableMainMenu());
        
        
        rootVisualElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt) { }
    
    private void EnableMainMenu()
    {
        DisableScreens();
        mainMenu.style.display = DisplayStyle.Flex;
    }
    
    private void EnableMainSettings()
    {
        DisableScreens();
        mainSettings.style.display = DisplayStyle.Flex;
    }
    
    private void DisableScreens()
    {
        if (mainMenu == null) throw new Exception("No main menu uxml tree");
        mainMenu.style.display = DisplayStyle.None;

        if (mainSettings == null) throw new Exception("No main settings uxml tree");
        mainSettings.style.display = DisplayStyle.None;
    }
}
}
