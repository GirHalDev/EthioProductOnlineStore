using EthioProductShoppingCenter.Controllers;
using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Models;
using EthioProductShoppingCenter.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace EthioProductShoppingCenter
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IRepository<tblProduct>, GenericRepository<tblProduct>>();
            container.RegisterType<IRepository<tblCart>, GenericRepository<tblCart>>();
            container.RegisterType<IRepository<tblOrder>, GenericRepository<tblOrder>>();
            container.RegisterType<IGenericUnitOfWork<EthioProductEntities>, GenericUnitOfWork<EthioProductEntities>>();
            container.RegisterType<IShoppingCart, ShoppingCartRepository>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}