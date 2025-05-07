
using System;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

public class TrussNodeViewController: MonoBehaviour
{
    public UIDocument uiDocument; // Assign your UIDocument in the Inspector
    public TrussNodeViewModel viewModel;
    
    private void OnEnable()
    {
        if(uiDocument == null)
        {
            Debug.LogError("No document assigned to TrussNodeViewController.");
            return;
        }

        if (viewModel == null)
        {
            Debug.LogError("No view model assigned to TrussNodeViewController.");
            return;
        }
        
        VisualElement root = uiDocument.rootVisualElement;

        root.dataSource = viewModel;
    }
}