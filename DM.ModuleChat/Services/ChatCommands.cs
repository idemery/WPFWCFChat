using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace DM.ModuleChat.Services
{
    public class ChatCommands : IChatCommands
    {
        private CompositeCommand _connectCommand = new CompositeCommand();
        public CompositeCommand Connect
        {
            get
            {
                return _connectCommand;
            }
        }

        private CompositeCommand _disconnectCommand = new CompositeCommand();
        public CompositeCommand Disconnect
        {
            get
            {
                return _disconnectCommand;
            }
        }

        private CompositeCommand _sendMessageCommand = new CompositeCommand();
        public CompositeCommand SendMessage
        {
            get
            {
                return _sendMessageCommand;
            }
        }

        private CompositeCommand _sendScreenShotCommand = new CompositeCommand();
        public CompositeCommand SendScreenShot
        {
            get
            {
                return _sendScreenShotCommand;
            }
        }

        private CompositeCommand _startScreenCastingCommand = new CompositeCommand();
        public CompositeCommand StartScreenCasting
        {
            get
            {
                return _startScreenCastingCommand;
            }
        }

        private CompositeCommand _stopScreenCastingCommand = new CompositeCommand();
        public CompositeCommand StopScreenCasting
        {
            get
            {
                return _stopScreenCastingCommand;
            }
        }
    }
}
