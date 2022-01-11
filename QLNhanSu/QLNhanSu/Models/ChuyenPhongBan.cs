﻿//------------------------------------------------------------------------------
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

    public partial class ChuyenPhongBan
    {

        [Display(Name = "Mã chuyển phòng ban")]
        public int MACPB { get; set; }
        [Display(Name = "Mã chuyển nhân viên")]
        public string MANV { get; set; }
        [Display(Name = "Mã phòng ban")]
        public string MAPB { get; set; }
        [Display(Name = "Mã phòng ban chuyển đến")]
        public string MAPBCHUYENDEN { get; set; }
        [Display(Name = "Lý do")]
        public string LYDO { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Ngày chuyển")]
        public Nullable<System.DateTime> NGAYCHUYEN { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
        public virtual PhongBan PhongBan { get; set; }
        public virtual PhongBan PhongBan1 { get; set; }
    }
}