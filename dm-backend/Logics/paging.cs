﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Models{
    public class Paging
    {
        public int PageValue(int page , int size)
        {
            int value = page * size;
            value -= size;
            return page == 1 ? value : value;
        }
    }
}
