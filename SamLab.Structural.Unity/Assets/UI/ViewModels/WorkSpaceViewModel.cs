using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Structure.Base;
using Structure.Managers;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Workspace.Managers;

namespace UI.ViewModels
{
    public class WorkSpaceViewModel: MonoBehaviour, INotifyBindablePropertyChanged
    {
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;
        private void Notify([CallerMemberName] string property = "")
        {
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(property));
        }
        
        [SerializeField] private WorkspaceManager workspaceManager;
        [SerializeField] private TrussManager trussManager;

        [CreateProperty] public List<TrussStructure> Structures => trussManager.TrussStructures ;

        private void OnEnable()
        {
            trussManager.OnStructureCollectionChanged += OnStructureCollectionChanged;
            trussManager.OnActiveStructureChanged += OnActiveStructureChanged;
        }

        private void OnActiveStructureChanged(TrussStructure obj)
        {
            Notify();
        }

        private void OnStructureCollectionChanged(List<TrussStructure> obj)
        {
            Notify();   
        }
    }
}