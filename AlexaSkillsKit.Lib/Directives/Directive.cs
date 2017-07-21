/* Copyright(c) 2017 Frank Kuchta
The MIT License (MIT)
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Directives
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
}