//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TrinhDoHocVan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TrinhDoHocVan()
        {
            this.NhanViens = new HashSet<NhanVien>();
        }

        [Required(ErrorMessage = "Phải nhập mã trình độ học vấn")]
        [Display(Name = "Mã trình độ học vấn")]
        public string MATDHV { get; set; }
        [Required(ErrorMessage = "Phải nhập tên trình độ học vấn")]
        [Display(Name = "Tên trình độ học vấn")]
        public string TENTRINHDO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
