using AutoMapper;
using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.DomainLayer;
using EthioProductShoppingCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Infrastructure
{
    public class MappingConfig : AutoMapper.Profile
    {
        public static IMapper MappingObjects()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<tblOrder, OrderVM>().ReverseMap();
                cfg.CreateMap<tblProduct, Product>().ReverseMap();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}