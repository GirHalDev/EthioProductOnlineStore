using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Infrastructure
{
    public class AutoMapperWebProfile : AutoMapper.Profile
    {
        public AutoMapperWebProfile()
        {
            CreateMap<tblOrder, OrderVM>();
            CreateMap<OrderVM, tblOrder>();
        }
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(x => x.AddProfile<AutoMapperWebProfile>());
        }
        
    }
}