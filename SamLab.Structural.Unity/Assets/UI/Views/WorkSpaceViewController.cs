
using System.Collections.Generic;
using Core.Application;
using Core.Interfaces;
using Structure.Base;
using Structure.Base.Constraints;
using Structure.Managers;
using UI.ViewModels;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using Workspace.Geometry.Interfaces;
using Workspace.Interaction;
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
    private TreeView structureTree;
    
    private Dictionary<VisualElement, IStructuralElement> treeElementsMap = new Dictionary<VisualElement, IStructuralElement>();
    private IStructuralElement currentHoveredElement;
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
        
        
        workSpaceViewModel.propertyChanged += WorkSpaceViewModelOnpropertyChanged;
        
        // Set up UI
        SetupViewControllers();
        CreateDataSourceBinding(workSpaceView.rootVisualElement);
        CreateDataSourceBinding(selectionView.rootVisualElement);

        structureTree = workSpaceView.rootVisualElement.Q<TreeView>();
        structureTree.selectionChanged += StructureTreeOnselectionChanged;
        structureTree.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        SetUpSessionTreeView(structureTree);

    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        Vector2 mousePosition = evt.mousePosition;
        VisualElement elementUnderMouse = structureTree.panel?.Pick(mousePosition);
        
        if (elementUnderMouse == null) return;


        if (!treeElementsMap.TryGetValue(elementUnderMouse, out var element)) return;

        if (element == currentHoveredElement || element.SceneObject == null) return;
        
        var hoverColorChange = element.SceneObject.GetComponent<HoverColorChange>();
        
        if(hoverColorChange != null)
            hoverColorChange.setHovered = true;
        
        if(currentHoveredElement != null)
            currentHoveredElement.SceneObject.GetComponent<HoverColorChange>().setHovered = false;
        
        currentHoveredElement = element;
       
            
    }

    private void StructureTreeOnselectionChanged(IEnumerable<object> obj)
    {
        if(obj == null) return;

        foreach (var element in obj)
        {
            if (element is IStructuralElement node)
            {
                SelectionEvents.PublishIStructuralElementSelected(node);
            } 
        }
    }

    private void WorkSpaceViewModelOnpropertyChanged(object sender, BindablePropertyChangedEventArgs e)
    {
        CreateOrUpdateTreeView();
        
    }

    private void SetUpSessionTreeView(TreeView sessionsObjectTree)
    {
        sessionsObjectTree.SetRootItems(GetTreeViewItemData());

        sessionsObjectTree.makeItem = () => new Label();
        sessionsObjectTree.bindItem = (VisualElement element, int index) =>
        {
            var iStructuralElementData = sessionsObjectTree.GetItemDataForIndex<IStructuralElement>(index);
            (element as Label).text = iStructuralElementData.Name;
            treeElementsMap.TryAdd(element, iStructuralElementData);
        };
            
        sessionsObjectTree.ExpandAll();
    }

    private void CreateOrUpdateTreeView()
    {
        structureTree.SetRootItems(GetTreeViewItemData());
        // SetUpSessionTreeView(structureTree);
        structureTree.Rebuild();
        structureTree.ExpandAll();
    }

    private void SetupViewControllers()
    {
        _topButtonsController = new TopButtonsController(workSpaceView.rootVisualElement, commandManager);
    }

    private void CreateDataSourceBinding(VisualElement root)
    {
        root.dataSource = workSpaceViewModel;
    }

    private IList<TreeViewItemData<IStructuralElement>> GetTreeViewItemData()
    {
        int id = 0;

        //Update rather than recreate.
        //Needs more event types to specify which element is added to the structure
        
        var treeView = new List<TreeViewItemData<IStructuralElement>>(workSpaceViewModel.Structures.Count);
        
        foreach (var structure in workSpaceViewModel.Structures)
        {
            var nodes = new List<TreeViewItemData<IStructuralElement>>(structure.Nodes.Count);
            var members = new List<TreeViewItemData<IStructuralElement>>(structure.Members.Count);
            var supports = new List<TreeViewItemData<IStructuralElement>>(structure.Supports.Count);
            

            foreach (var trussMember in structure.Members)
            {
                members.Add(new TreeViewItemData<IStructuralElement>(id++, trussMember));
            }

            foreach (var trussNode in structure.Nodes)
            {
                nodes.Add(new TreeViewItemData<IStructuralElement>(id++, trussNode));
            }

            foreach (var support in structure.Supports)
            {
                supports.Add(new TreeViewItemData<IStructuralElement>(id++, support));
            }

            
            treeView.Add(new TreeViewItemData<IStructuralElement>(
                id++, 
                structure, 
                new List<TreeViewItemData<IStructuralElement>>()
                {
                    new TreeViewItemData<IStructuralElement>(id++, new StructuralTreeGroupHeader(){Name = "Nodes"}, nodes),
                    new TreeViewItemData<IStructuralElement>(id++, new StructuralTreeGroupHeader(){Name = "Members"}, members),
                    new TreeViewItemData<IStructuralElement>(id++, new StructuralTreeGroupHeader(){Name = "Supports"}, supports),
                }
            ));
            
        }

        
        return treeView; 
    }

}
    


