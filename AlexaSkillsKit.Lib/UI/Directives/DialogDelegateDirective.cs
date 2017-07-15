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

using AlexaSkillsKit.Speechlet;

namespace AlexaSkillsKit.UI.Directives
{
    /// <summary>
    /// Delegate Directive
    /// Sends Alexa a command to handle the next turn in the dialog with the user.
    /// You can use this directive if the skill has a dialog model and the current status of the dialog (dialogState) is either STARTED or IN_PROGRESS. 
    /// You cannot return this directive if the dialogState is COMPLETED.
    /// </summary>
    public class DialogDelegateDirective : DialogDirective
    {
        public override DirectiveTypesEnum Type { get => DirectiveTypesEnum.Dialog_Delegate; }
        public virtual ConfirmationStatusEnum ConfirmationStatus { get; set; }
    }
}