﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EthioProductShoppingCenter.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EthioProductEntities : DbContext
    {
        public EthioProductEntities()
            : base("name=EthioProductEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblCartStatu> tblCartStatus { get; set; }
        public virtual DbSet<tblCatagory> tblCatagories { get; set; }
        public virtual DbSet<tblMemberRole> tblMemberRoles { get; set; }
        public virtual DbSet<tblMember> tblMembers { get; set; }
        public virtual DbSet<tblOrder> tblOrders { get; set; }
        public virtual DbSet<tblOrderDetail> tblOrderDetails { get; set; }
        public virtual DbSet<tblProduct> tblProducts { get; set; }
        public virtual DbSet<tblRole> tblRoles { get; set; }
        public virtual DbSet<tblShippingDetail> tblShippingDetails { get; set; }
        public virtual DbSet<tblSlideImage> tblSlideImages { get; set; }
        public virtual DbSet<tblCart> tblCarts { get; set; }
    }
}
