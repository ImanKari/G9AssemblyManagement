﻿using System;
using System.Net;
using G9AssemblyManagement_NUnitTest.DataType;

namespace G9AssemblyManagement_NUnitTest.Inherit
{
    internal class
        G9CGenericInterfaceInheritInterfaceTest : G9ITestGenericInterfaceInheritInterface<int, string, DateTime,
            IPAddress, bool>
    {
        public int Field1 { get; set; }
        public string Field2 { get; set; }
        public DateTime Field3 { get; set; }
        public IPAddress Field4 { get; set; }
        public bool IsGame { get; set; }

        public void Initialize()
        {
            Field1 = 9;
            Field2 = "G9TM.Com";
            Field3 = DateTime.MaxValue;
            Field4 = IPAddress.Any;
            IsGame = true;
        }
    }
}