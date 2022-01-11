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

    public partial class QuanLyLuong
    {
        [Display(Name = "Mã quản lý lương")]
        public int MAQLL { get; set; }
        [Display(Name = "Mã nhân viên")]
        public string MANV { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Ngày tính lương")]
        public Nullable<System.DateTime> NGAYTINHLUONG { get; set; }
        [Display(Name = "Thực lãnh")]
        public Nullable<float> THUCLANH { get; set; }
        [Display(Name = "Lương cơ bản")]
        public Nullable<float> LUONGCOBAN { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
    }
}
