﻿namespace WebApplication.Models
{
    public class Director : BaseModel
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}