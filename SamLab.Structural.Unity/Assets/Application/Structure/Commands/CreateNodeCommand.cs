using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Application
{
    public class CreateNodeCommand : ICommand
    {
        public string Name { get; set; } = "Create Node";

        public void Execute()
        {

        }

        public void Undo()
        {
        }
    }
}
