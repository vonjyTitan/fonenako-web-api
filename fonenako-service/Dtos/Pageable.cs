using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace fonenako_service.Dtos
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Pageable<Model> where Model : class
    {
        [JsonPropertyName("content")]
        public IEnumerable<Model> Content { get; set; } = Array.Empty<Model>();

        [JsonPropertyName("totalFound")]
        public int TotalFound { get; set; }

        [JsonPropertyName("totalPage")]
        public int TotalPage { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        public Pageable()
        {
        }

        public Pageable(int currentPage, int pagseSize, int totalPage, int totalFound, IEnumerable<Model> content)
        {
            CurrentPage = currentPage;
            PageSize = pagseSize;
            TotalPage = totalPage;
            Content = content;
            TotalFound = totalFound;
        }
    }
}
