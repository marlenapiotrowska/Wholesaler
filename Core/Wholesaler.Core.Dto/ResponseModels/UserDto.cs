﻿namespace Wholesaler.Core.Dto.ResponseModels
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }        
    }
}
