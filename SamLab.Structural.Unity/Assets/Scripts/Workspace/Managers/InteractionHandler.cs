using System;
using UnityEngine;
using UnityEngine.Events;

namespace Workspace.Managers
{
    public class InteractionHandler : MonoBehaviour
    {
        public UnityEvent AwaitNodeSelection;
        public bool IsOccupied { get; private set; } = false;

        public void Awake()
        {
            AwaitNodeSelection = new UnityEvent();
            AwaitNodeSelection.AddListener(OnAwaitNodeSelection);
        }

        private void OnAwaitNodeSelection()
        {
            throw new NotImplementedException();
        }

        private void OnAwaitElementSelection()
        {
            throw new NotImplementedException();
        }

        private void OnNewPositionSelection()
        {
        }
    }
}