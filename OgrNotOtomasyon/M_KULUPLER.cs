//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OgrNotOtomasyon
{
    using System;
    using System.Collections.Generic;
    
    public partial class M_KULUPLER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_KULUPLER()
        {
            this.M_OGRENCI = new HashSet<M_OGRENCI>();
        }
    
        public short KULUPID { get; set; }
        public string KULUPAD { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_OGRENCI> M_OGRENCI { get; set; }
    }
}