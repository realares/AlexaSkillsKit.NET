using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSkillsKit.Speechlet.Resolution
{

    /// <summary>
    /// A code indicating the results of attempting to resolve the user utterance against the defined slot types.
    /// </summary>
    public enum ResolutionStatusCodeEnum
    {
        /// <summary>
        ///  The spoken value matched a value or synonym explicitly defined in your custom slot type.
        /// </summary>
        [EnumMember(Value = "ER_SUCCESS_MATCH")]
        ER_SUCCESS_MATCH,

        /// <summary>
        /// The spoken value did not match any values or synonyms explicitly defined in your custom slot type.
        /// </summary>
        [EnumMember(Value = "ER_SUCCESS_NO_MATCH")]
        ER_SUCCESS_NO_MATCH,

        /// <summary>
        ///  An error occurred due to a timeout.
        /// </summary>
        [EnumMember(Value = "ER_ERROR_TIMEOUT")]
        ER_ERROR_TIMEOUT,

        /// <summary>
        /// An error occurred due to an exception during processing.
        /// </summary>
        [EnumMember(Value = "ER_ERROR_EXCEPTION")]
        ER_ERROR_EXCEPTION

    }
}
