﻿using movies_api.Contracts.DTOs;
using Npgsql;

namespace movies_api.Models
{
    // Single responsibility: encapsulate the properties and behaviour of Title entity.
    public class Title 
    {
        public required string Id { get; set; }
        public int? ContentTypeId { get; set; }
        public string? PrimaryTitle { get; set; }
        public string? OriginalTitle { get; set; }
        public int? IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }
    }
}
