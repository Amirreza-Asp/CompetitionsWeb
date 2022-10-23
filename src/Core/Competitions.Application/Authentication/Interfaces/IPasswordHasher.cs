﻿namespace Competitions.Application.Authentication.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword ( string password );

        public bool VerifyPassword ( string hashedPassword , string enteredPassword );
    }
}
