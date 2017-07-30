using System;
using System.Collections.Generic;
using System.Text;

namespace Ra.AlexaSkillsKit
{
    public class User_Address
    {
        /// <summary>
        /// Abbreviation for the state, province or region associated with the device specified in the request. 
        /// This value may be an empty string for non-US countries.
        /// </summary>
        public string StateOrRegion { get; set; }

        public string PostalCode { get; set; }
        /// <summary>
        /// The city for the device specified in the request.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The two-letter country code for the device specified in the request.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The first line of the address.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the address.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the address.
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// The district or county associated with the device. 
        /// This value may be an empty string for non-US countries.
        /// </summary>
        public string DistrictOrCounty { get; set; }

        public override string ToString()
        {
            return $@"StateOrRegion:{StateOrRegion ?? "<NULL>"}
City: {City ?? "<NULL>"}
CountryCode: {CountryCode ?? "<NULL>"}
AddressLine1: {AddressLine1 ?? "<NULL>"}
AddressLine2: {AddressLine2 ?? "<NULL>"}
AddressLine3: {AddressLine3 ?? "<NULL>"}
DistrictOrCounty: {DistrictOrCounty ?? "<NULL>"}";

        }
    }
}
