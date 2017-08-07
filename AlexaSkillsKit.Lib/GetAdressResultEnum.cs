/*
 * Copyright 2017 realares
 * Original Copyright 2015 Stefan Negritoiu (FreeBusy). 
 * See LICENSE file for more information.
 */


namespace Ra.AlexaSkillsKit
{
    public enum GetAdressResultEnum
    {
        OK,
        Error_NoDeviceId,
        Error_NoConsentToken,
        Error_RequestNotSupported,
        Error_Unkown,
        /// <summary>
        /// The query did not return any results
        /// </summary>
        Error_NoContent,
        /// <summary>
        /// The authentication token is invalid or doesn’t have access to the resource.
        /// </summary>
        Error_Forbidden,
        /// <summary>
        /// The method is not supported.
        /// </summary>
        Error_Method_Not_Allowed,
        /// <summary>
        /// The skill has been throttled due to an excessive number of requests.
        /// </summary>
        Error_Too_Many_Requests,
        /// <summary>
        /// An unexpected error occurred.
        /// </summary>
        Error_InternalServerError,

    }

}
