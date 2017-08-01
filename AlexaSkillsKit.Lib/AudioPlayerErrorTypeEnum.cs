using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ra.AlexaSkillsKit
{
    public enum AudioPlayerErrorTypeEnum
    {
        NOT_SET,

        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        [EnumMember(Value = "MEDIA_ERROR_UNKNOWN")]
        MEDIA_ERROR_UNKNOWN,

        /// <summary>
        /// Alexa recognized the request as being malformed. E.g. bad request, unauthorized, forbidden, not found, etc.
        /// </summary>
        MEDIA_ERROR_INVALID_REQUEST,

        /// <summary>
        /// Alexa was unable to reach the URL for the stream.
        /// </summary>
        MEDIA_ERROR_SERVICE_UNAVAILABLE,

        /// <summary>
        /// Alexa accepted the request, but was unable to process the request as expected.
        /// </summary>
        MEDIA_ERROR_INTERNAL_SERVER_ERROR,

        /// <summary>
        /// There was an internal error on the device.
        /// </summary>
        MEDIA_ERROR_INTERNAL_DEVICE_ERROR
    }
}
 
 
 
 
 