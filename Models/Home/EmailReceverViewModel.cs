using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Models.Home
{
    public class EmailReceverViewModel
    {
        
        public string From { get; set; }
        //public string Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}