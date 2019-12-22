using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public User FromUser { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public User[] ToUsers { get; set; }
    }
}
