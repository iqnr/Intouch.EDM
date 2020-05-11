﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Intouch.Edm.Models.Dtos.TaskOptionDto
{
    public class TaskOptionDto
    {
    }

    public class CheckedOption
    {
        public string text { get; set; }
        public bool completed { get; set; }
        public Nullable<DateTime> completedTime { get; set; }
        public string userTaskId { get; set; }
        public int id { get; set; }
    }

    public class Item
    {
        public CheckedOption checkedOption { get; set; }
        public string userTaskText { get; set; }
    }

    public class Result
    {
        public int totalCount { get; set; }
        public List<Item> items { get; set; }
    }

    public class RootObject
    {
        public Result result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }
}