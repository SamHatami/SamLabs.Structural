using Assets.Scripts.Analysis.Managers;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;

namespace Assets.Scripts.Core.Application
{
    public class ApplicationManager : MonoBehaviour
    {
        private CommandManager _commandManager;
        private WorkspaceManager _workspaceManager;
        private AnalysisManager _analysisManager;

        private void Start()
        {
            _commandManager = new CommandManager();
            _workspaceManager = new WorkspaceManager();
            _analysisManager = new AnalysisManager();
        }

        

    }
}
