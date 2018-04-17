using System;
using fluxo.DATA.Models;

namespace fluxo.API.Helpers
{
    public static class UserExtensions
    {
        public static bool IsValid(this User user) {
            //TODO: Add payment verification
            return DateTime.Now < user.Created.AddDays(7);
        }
    }
}