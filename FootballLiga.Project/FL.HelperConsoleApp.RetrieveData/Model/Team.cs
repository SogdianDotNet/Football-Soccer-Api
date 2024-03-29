﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FL.HelperConsoleApp.RetrieveData.Model
{
    public class Team
    {
        public string id { get; set; }
        public string name { get; set; }
        public string stadium { get; set; }
        public Country country { get; set; }
        public List<object> federation { get; set; }
        public string name_ru { get; set; }
    }
}
