using Core.Application;
using Structure.Commands;
using UnityEngine;
using UnityEngine.UIElements;

public class TopButtonsController
{
    private readonly VisualElement _topButtonUiContainer;
    private readonly CommandManager _commandManager;
    [SerializeField] private UIDocument _view;
    public VisualElement TopButtonUiContainer; 
    public CommandManager CommandManager;
    public Button AddElementButton { get; set; }
    public Button AddNodeButton { get; set; }

    public TopButtonsController(VisualElement topButtonUiContainer, CommandManager commandManager)
    {
        _topButtonUiContainer = topButtonUiContainer;
        _commandManager = commandManager;

        SetupButtons();
    }

    private void SetupButtons()
    {
        AddElementButton = _topButtonUiContainer.Q<Button>("AddElement");
        AddNodeButton = _topButtonUiContainer.Q<Button>("AddNode");
        
        AddNodeButton.clicked += AddNodeButtonOnclicked;
        AddElementButton.clicked += AddElementButtonOnclicked;
    }

    private void AddElementButtonOnclicked()
    {
        _commandManager.ExecuteCommand(new ElementCommands.CreateElement());
    }


    private void AddNodeButtonOnclicked()
    {
        Debug.Log("AddNodeButtonOnclicked");
    }
}
