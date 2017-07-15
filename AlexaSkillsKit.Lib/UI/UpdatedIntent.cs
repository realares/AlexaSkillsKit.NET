using AlexaSkillsKit.Speechlet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI
{
    public partial class UpdatedIntent
    {
        public string Name { get; set; }

        public ConfirmationStatusEnum ConfirmationStatus { get; set; }

        public Dictionary<string, Slot> Slots { get; set; }
    }
}
