using System;
using Microsoft.AspNetCore.Http;

using fluxo.DATA.Helpers;
using fluxo.DATA.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace fluxo.API.Helpers
{
    public static class Extensions
    {
        public static bool IsValid(this User user) {
            //TODO: Add payment verification
            return DateTime.Now < user.Created.AddDays(7);
        }

        public static bool IsOwner(this User user) {
            return user.OrganizationOwned != null;
        }

        public static bool IsAdmin(this User user) {
            return user.TeamsAssigned.Any(t => !t.Team.IsCustom);
        }

        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}