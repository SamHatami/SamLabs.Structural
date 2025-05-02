using Core.Application;
using Structure.Commands;
using UnityEngine;
using UnityEngine.UIElements;
public class TopButtonsController: MonoBehaviour
{
    public VisualElement topButtonUiContainer; 
    public CommandManager commandManager;
    public Button AddElementButton { get; set; }
    public Button AddNodeButton { get; set; }

    private void Awake()
    {
        
        topButtonUiContainer = FindFirstObjectByType<UIDocument>().rootVisualElement;
        commandManager = FindFirstObjectByType<CommandManager>();
    }

    private void OnEnable()
    {
        AddElementButton = topButtonUiContainer.Q<Button>("AddElement");
        AddNodeButton = topButtonUiContainer.Q<Button>("AddNode");
        
        AddNodeButton.clicked += AddNodeButtonOnclicked;
        AddElementButton.clicked += AddElementButtonOnclicked;
    }

    private void AddElementButtonOnclicked()
    {
        commandManager.ExecuteCommand(new ElementCommands.CreateElement());
    }


    private void AddNodeButtonOnclicked()
    {
        Debug.Log("AddNodeButtonOnclicked");
    }
}
