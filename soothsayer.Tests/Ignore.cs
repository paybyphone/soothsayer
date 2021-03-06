﻿using System;
using soothsayer.Infrastructure;

namespace soothsayer.Tests
{
    public static class Ignore
    {
        public static void Exception(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown and ignored: {0}".FormatWith(ex.Message));
            }
        }
    }
}

