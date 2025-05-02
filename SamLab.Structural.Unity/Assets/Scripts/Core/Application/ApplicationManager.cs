using Analysis.Managers;
using UnityEngine;
using Workspace.Managers;

namespace Core.Application
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