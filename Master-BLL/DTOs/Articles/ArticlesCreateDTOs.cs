﻿using Master_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_BLL.DTOs.Articles
{
    public class ArticlesCreateDTOs
    {
        public string ArticlesTitle { get; set; }
        public string ArticlesContent { get; set;}
        public List<Comments> Comments { get; set;}
        
    }
}
