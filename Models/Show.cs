//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlackStoneMovies.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Show
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Show()
        {
            this.Bookings = new HashSet<Booking>();
        }
    
        public Nullable<int> Price { get; set; }
        public int SId { get; set; }
        public Nullable<int> MShow { get; set; }
        public Nullable<int> AShow { get; set; }
        public Nullable<int> EShow { get; set; }
        public Nullable<int> MId { get; set; }
        public Nullable<System.DateTime> Mday { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Movy Movy { get; set; }
    }
}
