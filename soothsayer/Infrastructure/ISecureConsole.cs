﻿namespace soothsayer.Infrastructure
{
    public interface ISecureConsole
    {
        string ReadLine(char maskCharacter);
    }
}