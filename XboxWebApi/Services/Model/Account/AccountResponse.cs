using System;
using XboxWebApi.Extensions;

namespace XboxWebApi.Services.Model.Account
{
    public class AccountHomeAddress : IStringable
    {
        public string Street1;
        public string Street2;
        public string City;
        public string State;
        public string PostalCode;
        public string Country;
    }

    public class AccountResponse : IStringable
    {
        public DateTime DateOfBirth;
        public string FirstName;
        public string LastName;
        public AccountHomeAddress HomeAddressInfo;
        public string Email;
        public string GamerTag;
        public ulong Xuid;
        public bool IsAdult;
        public string Locale;
        public string GamerTagChangeReason;
        public ulong OwnerPuid;
        public ulong Puid;
        public bool MsftOptin;
        public bool PartnerOptin;
        public DateTime TouAcceptanceDate;
    }
}