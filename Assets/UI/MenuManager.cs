using System;
using System.Windows.Forms;
using UnityEngine.UIElements;

public class MenuManager : VisualElement
{

    private VisualElement mainMenu;
    private VisualElement startButton;
    private VisualElement settingsButton;
    private VisualElement quitButton;
    
    private VisualElement mainSettings;
    private VisualElement backToMainMenuButton;
    
    // To appear in the list of custom controls
    public new class UxmlFactory : UxmlFactory<MenuManager, UxmlTraits> { }
    
    // Can be used for adding custom parameters for 
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public MenuManager()
    {
        
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        
        EnableMainMenu();

    }

    private void OnGeometryChange(GeometryChangedEvent evt)
    {
        mainMenu = this.Q("MainMenu");
        startButton = this.Q("startButton");
        settingsButton = this.Q("settingsButton");
        quitButton = this.Q("quitButton");

        mainSettings = this.Q("MainSettings");
        backToMainMenuButton = this.Q("backToMainMenuButton");
        
        
        settingsButton.RegisterCallback<ClickEvent>(_ =>
        {
            EnableMainSettings();
        });
        
        backToMainMenuButton.RegisterCallback<ClickEvent>(_ =>
        {
            EnableMainMenu();
        });
        
        
        // this.RegisterCallback<ClickEvent>(_ =>
        // {
        //     mainMenu.style.display = mainMenu.style.display == 
        //                              DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        // });
        
        
        
    }


    public void EnableMainMenu()
    {
        DisableScreens();
        mainMenu.style.display = DisplayStyle.Flex;
    }
    
    public void EnableMainSettings()
    {
        DisableScreens();
        mainSettings.style.display = DisplayStyle.Flex;
    }
    
    private void DisableScreens()
    {
        if (mainMenu == null) throw new Exception("No main menu uxml tree");
        if (mainSettings == null) throw new Exception("No main settings uxml tree");
        mainMenu.style.display = DisplayStyle.None;
        mainSettings.style.display = DisplayStyle.None;
    }

    
}


