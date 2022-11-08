using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SlidingTile_LevelEditor.Commands
{
    class IncNormalCommand : Command
    {
        private List<Command> _commands;
        private Point _point;
        private int _commandIndex;
        public bool _isCurrentCommand;
        public IncNormalCommand(List<Command> commands, Point point, int commandIndex)
        {
            _commands = commands;
            _point = point;
            _commandIndex = commandIndex;
            _isCurrentCommand = true;
            Execute();
        }

        public override void Execute()
        {
            _commands.Add(this);
        }

        public override void Redo()
        {
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return _isCurrentCommand.ToString() + ">" + _commandIndex.ToString() + ":"  + _point.X.ToString() + "," + _point.Y.ToString();
        }
    }
}
