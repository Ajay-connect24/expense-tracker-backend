﻿
namespace ExpensesAPI.Entities.DataTransferObjects
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}