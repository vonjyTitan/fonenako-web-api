using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace fonenako_service.Dtos
{
    public class Pageable<Model> where Model : class
    {
        [JsonPropertyName("content")]
        public IEnumerable<Model> Content { get; set; } = Array.Empty<Model>();

        [JsonPropertyName("totalPage")]
        public int TotalPage { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        public Pageable()
        {
        }

        public Pageable(int currentPage, int pagseSize, int totalPage, IEnumerable<Model> content)
        {
            CurrentPage = currentPage;
            PageSize = pagseSize;
            TotalPage = totalPage;
            Content = content;
        }
    }
}
