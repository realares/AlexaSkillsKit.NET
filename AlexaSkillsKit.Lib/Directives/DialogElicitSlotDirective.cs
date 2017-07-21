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


namespace AlexaSkillsKit.Directives
{
    /// <summary>
    /// ElicitSlot Directive
    /// Sends Alexa a command to ask the user for the value of a specific slot.
    /// Specify the name of the slot to elicit in the slotToElicit property.Provide a prompt to ask the user for the slot value in an OutputSpeech object in the response.
    /// If your skill does not meet the requirements to use the Dialog directives, returning Dialog.ElicitSlot causes an error.
    /// </summary>
    public class DialogElicitSlotDirective : DialogDirective
    {
        public override DirectiveTypesEnum Type { get => DirectiveTypesEnum.Dialog_ElicitSlot; }

        public string SlotToElicit { get; set; }
    }
}