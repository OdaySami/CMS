﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Core.Enums
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException() : base("InValid Date") 
        {
        
        }
    }
}
