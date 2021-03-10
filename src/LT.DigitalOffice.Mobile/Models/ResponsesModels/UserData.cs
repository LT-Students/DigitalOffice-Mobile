using System;

namespace LT.DigitalOffice.Mobile.Models.ResponsesModels
{
    public class UserData
    {
        private Guid _userId;
        public Guid UserId
        {
            get => _userId;

            set
            {
                _userId = new Guid(value.ToString());
            }
        }

        public string Token { get; set; }
    }
}
