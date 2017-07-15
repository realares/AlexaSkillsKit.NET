using AlexaSkillsKit.Speechlet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.UI.Directives
{
    public class Directive
    {
        public virtual DirectiveTypesEnum Type { get; set; }

 
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DirectiveTypesEnum
        {
            
            [EnumMember(Value = "Display.RenderTemplate")]
            Display_RenderTemplate,


            [EnumMember(Value = "VideoApp.Launch")]
            VideoApp_Launch,

            
            [EnumMember(Value = "AudioPlayer.Play")]
            AudioPlayer_Play,

            /// <summary>
            /// Sends Alexa a command to handle the next turn in the dialog with the user.
            /// You can use this directive if the skill has a dialog model and the current status of the 
            /// dialog (dialogState) is either STARTED or IN_PROGRESS. You cannot return this directive if the dialogState is COMPLETED.
            /// </summary>
            [EnumMember(Value = "Dialog.Delegate")]
            Dialog_Delegate,

            /// <summary>
            /// Sends Alexa a command to ask the user for the value of a specific slot. 
            /// Specify the name of the slot to elicit in the slotToElicit property. 
            /// Provide a prompt to ask the user for the slot value in an OutputSpeech object in the response.
            /// </summary>
            [EnumMember(Value = "Dialog.ElicitSlot")]
            Dialog_ElicitSlot,

            /// <summary>
            /// Sends Alexa a command to ask the user for the value of a specific slot. 
            /// Specify the name of the slot to elicit in the slotToElicit property. 
            /// Provide a prompt to ask the user for the slot value in an OutputSpeech object in the response.
            /// </summary>
            [EnumMember(Value = "Dialog.ConfirmSlot")]
            Dialog_ConfirmSlot,

            /// <summary>
            /// Sends Alexa a command to confirm the all the information the user has provided for the intent before the skill takes action. 
            /// Provide a prompt to ask the user for confirmation in an OutputSpeech object in the response. 
            /// Be sure to repeat back all the values the user needs to confirm in the prompt.
            /// </summary>
            [EnumMember(Value = "Dialog.ConfirmIntent")]
            Dialog_ConfirmIntent,
        }


    }

    public class AudioPlayerDirective : Directive
    {
        public virtual string PlayBehavior { get; set; } //TODO: Enum?
        public virtual AudioItem AudioItem { get; set; }

        public enum PlayBehaviorEnum
        {
            REPLACE_ALL,
            ENQUEUE
        }
    }

    public class DialogDirective : Directive
    {
       public virtual List<UpdatedIntent> UpdatedIntent { get; set; }

        //public string SlotToElicit { get; set; }
        //public string SlotToConfirm { get; set; }

    }

    public class DialogDelegateDirective : DialogDirective
    {
        public override DirectiveTypesEnum Type { get => DirectiveTypesEnum.Dialog_Delegate;  }
        public virtual ConfirmationStatusEnum ConfirmationStatus { get; set; }
    }
}