using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Application.StructuralAnalysis;
using Assets.Application.Workspace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Application
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
