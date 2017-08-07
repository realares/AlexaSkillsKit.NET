/*
 * Copyright 2017 realares
 * Original Copyright 2015 Stefan Negritoiu (FreeBusy). 
 * See LICENSE file for more information.
 */


namespace Ra.AlexaSkillsKit
{
    public class AddressRequestResult
    {
        public AddressRequestResult(GetAdressResultEnum resultCode, User_Address address = null)
        {
            this.ResultCode = resultCode;
            this.Address = address;
        }

        public GetAdressResultEnum ResultCode { get; private set; }

        public User_Address Address { get; private set; }
    }

}
