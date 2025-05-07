
using System.Collections.Generic;
using Core.Application;
using Core.Interfaces;
using Structure.Base;
using Structure.Base.Constraints;
using Structure.Managers;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;
using Workspace.Geometry.Interfaces;
using TreeView = UnityEngine.UIElements.TreeView;

public class WorkSpaceViewController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("WorkSpace UI")]
    [SerializeField] private UIDocument workSpaceView;
    [SerializeField] private WorkSpaceViewModel workSpaceViewModel;
    
    [Header("Selection UI ")]
    [SerializeField] private UIDocument selectionView;
    [SerializeField] private SelectionViewModel selectionViewModel;

    [Header("Managers")]
    [SerializeField] private  CommandManager commandManager;
    
    TopButtonsController _topButtonsController;
    
    
    private void OnEnable()
    {
        if (workSpaceViewModel == null)
        {
            Debug.LogWarning("WorkSpace view model is null");
            return;
        }
        
        if (workSpaceView == null || selectionView == null)
        {
            Debug.LogError("One or more UIDocument references are missing. Please assign them in the Inspector.");
            return;
        }
        
        // Set up UI
        SetupViewControllers();
        CreateDataSourceBinding(workSpaceView.rootVisualElement);
        CreateDataSourceBinding(selectionView.rootVisualElement);

        var sessionsObjectTree = workSpaceView.rootVisualElement.Q<TreeView>();

        SetUpSessionTreeView(sessionsObjectTree);

    }

    private void SetUpSessionTreeView(TreeView sessionsObjectTree)
    {
        sessionsObjectTree.SetRootItems(GetTreeViewItemData());

        sessionsObjectTree.makeItem = () => new Label();
        sessionsObjectTree.bindItem = (VisualElement element, int index) =>
            (element as Label).text = sessionsObjectTree.GetItemDataForIndex<IStructuralElement>(index).Name;
    }

    private void CreateOrUpdateTreeView(TreeView treeView)
    {
        
    }

    private void SetupViewControllers()
    {
        _topButtonsController = new TopButtonsController(workSpaceView.rootVisualElement, commandManager);
    }

    private void CreateDataSourceBinding(VisualElement root)
    {
        root.dataSource = workSpaceViewModel;
    }

    private IList<TreeViewItemData<TrussStructure>> GetTreeViewItemData()
    {
        int id = 0;
        var sessionStructures = new List<TreeViewItemData<TrussStructure>>(workSpaceViewModel.Structures.Count);
        foreach (var structure in workSpaceViewModel.Structures)
        {
            var nodes = new List<TreeViewItemData<TrussNode>>(structure.Nodes.Count);
            var members = new List<TreeViewItemData<TrussMember>>(structure.Members.Count);
            var supports = new List<TreeViewItemData<IConstraint>>(structure.Supports.Count);

            foreach (var trussMember in structure.Members)
            {
                members.Add(new TreeViewItemData<TrussMember>(id++, trussMember));
            }

            foreach (var trussNode in structure.Nodes)
            {
                nodes.Add(new TreeViewItemData<TrussNode>(id++, trussNode));
            }

            foreach (var support in structure.Supports)
            {
                supports.Add(new TreeViewItemData<IConstraint>(id++, support));
            }
            
            sessionStructures.Add(new TreeViewItemData<TrussStructure>(id++, structure));
        }
        
        return sessionStructures; 
    }

}
    


