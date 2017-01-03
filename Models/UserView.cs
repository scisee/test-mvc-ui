using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVC.Models
{
    public class UserView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
    }
}