
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Workspace.Managers;

public class TrussEditorUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private WorkspaceManager trussManager;
    [SerializeField] private UIDocument mainUiDocument;
    [SerializeField] private UIDocument selectionUiDocument;
    [SerializeField]private VisualTreeAsset mainUiAsset;
    [SerializeField]private VisualTreeAsset selectionUiAsset;
    private void OnEnable()
    {
        if (trussManager == null)
            trussManager = FindFirstObjectByType<WorkspaceManager>();
        
        if (mainUiDocument == null || selectionUiDocument == null)
        {
            Debug.LogError("One or more UIDocument references are missing. Please assign them in the Inspector.");
            return;
        }
        
        // Set up UI
        SetupMainUI(mainUiDocument.rootVisualElement);
        SetupSelectionUI(selectionUiDocument.rootVisualElement);
    }
    
    private void SetupMainUI(VisualElement root)
    {
        // Apply the visual tree asset if needed
        if (mainUiAsset != null && mainUiDocument.visualTreeAsset == null)
        {
            mainUiDocument.visualTreeAsset = mainUiAsset;
        }
        
        // Set data source
        if (trussManager != null)
        {
            root.dataSource = trussManager;
        }
        
        // Additional setup for main UI...
    }
    
    private void SetupSelectionUI(VisualElement root)
    {
        // Apply the visual tree asset if needed
        if (selectionUiAsset != null && selectionUiDocument.visualTreeAsset == null)
        {
            selectionUiDocument.visualTreeAsset = selectionUiAsset;
        }
        
        // Set data source
        if (trussManager != null)
        {
            root.dataSource = trussManager;
        }
        
        // Additional setup for selection UI...
    }
}
