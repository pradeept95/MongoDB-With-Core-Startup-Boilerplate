using System;
using System.Collections.Generic;
using System.Text;

namespace KNN.NULLPrinter.Core.Dto.Order
{ 
    public class OrderDto
    { 
        public string Id { get; set; }

        public string code { get; set; }
        public bool status { get; set; }  //open = 1 or close = 0 
        public List<OrderDetailDto> items { get; set; } = new List<OrderDetailDto>();
         
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class OrderDetailDto
    { 
        public string Id { get; set; } 
        public string code { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public bool status { get; set; }  //open = 1 or close = 0
    }
}
