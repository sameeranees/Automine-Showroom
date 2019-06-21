//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Software_Engineering.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public partial class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public Nullable<int> Mileage { get; set; }
        public Nullable<int> Year { get; set; }
        public string CC { get; set; }
        [DisplayName("Buying Price")]
        public long buyingPrice { get; set; }
        [DisplayName("Selling Price")]
        public Nullable<long> sellingPrice { get; set; }
        [DisplayName("Maintainance Cost")]
        public Nullable<long> maintainanceCost { get; set; }
        public string Condition { get; set; }
        public bool Imported { get; set; }
        [DisplayName("Previous Owner")]
        public string ownerName { get; set; }
        [DisplayName("Date Purchased")]
        public System.DateTime purchasedDate { get; set; }
        [DisplayName("Date Sold")]
        public Nullable<System.DateTime> soldDate { get; set; }
        [DisplayName("Registeration No")]
        public string registerationNo { get; set; }
        public Nullable<int> customerId { get; set; }
        public Nullable<int> trackerId { get; set; }
        public Nullable<int> insuranceId { get; set; }
        public Nullable<System.DateTime> insExpiry { get; set; }
        public Nullable<System.DateTime> traExpiry { get; set; }
        public byte[] Image { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }
        public byte[] Image4 { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Insurance Insurance { get; set; }
        public virtual Tracker Tracker { get; set; }
        public string Month { get; set; }
        public IEnumerable<SelectListItem> months { get; set; }
    }
    public class ViewModel
    {
        public List<Car> Sold { get; set; }
        public List<Car> UnSold { get; set; }
    }

}
