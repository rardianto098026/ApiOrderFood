﻿namespace ApiOrderFood.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Status { get; set; }
    }
}
