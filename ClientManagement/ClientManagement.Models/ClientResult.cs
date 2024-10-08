﻿namespace ClientManagement.Models
{
    public class ClientResult
    {
        public string? FilterBy { get; set; }
        public string? SortBy { get; set; }
        public bool Descending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 5;
        public List<ClientModel> Clients { get; set; } = new List<ClientModel>();
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / Limit);
    }
}
