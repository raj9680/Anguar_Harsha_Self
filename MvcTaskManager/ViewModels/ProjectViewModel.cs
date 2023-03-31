﻿using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
    public class ProjectViewModel
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string DateOfStart { get; set; }
        public int? TeamSize { get; set; }
        public bool Active { get; set; }
        public string Status { get; set; }
        public int ClientLocationID { get; set; }
        public ClientLocation ClientLocation { get; set; }
    }
}