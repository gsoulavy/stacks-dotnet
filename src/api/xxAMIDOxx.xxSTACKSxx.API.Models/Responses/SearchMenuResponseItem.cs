using System;

namespace xxAMIDOxx.xxSTACKSxx.API.Models.Responses
{
    /// <summary>
    /// Response model representing a search result item in the SearchMenu api endpoint
    /// </summary>
    public class SearchMenuResponseItem
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        public Guid Id { get; set; }

        /// <example>Menu name</example>
        public string Name { get; set; }

        /// <example>Menu description</example>
        public string Description { get; set; }

        /// <summary>
        /// Represents the status of the menu. False if disabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
