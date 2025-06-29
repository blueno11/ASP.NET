using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BanHangDienMay.Models;

public partial class QlbanHangDienMayContext : DbContext
{
    public QlbanHangDienMayContext()
    {
    }

    public QlbanHangDienMayContext(DbContextOptions<QlbanHangDienMayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhapKho> NhapKhos { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<YeuCauBaoTri> YeuCauBaoTris { get; set; }

    public virtual DbSet<YeuCauDoiTra> YeuCauDoiTras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__ChiTietD__CD8DB514AB93A672");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.MaChiTiet).HasColumnName("ma_chi_tiet");
            entity.Property(e => e.GiaBan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("gia_ban");
            entity.Property(e => e.MaHoaDon).HasColumnName("ma_hoa_don");
            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.SoLuong)
                .HasDefaultValue(1)
                .HasColumnName("so_luong");
            entity.Property(e => e.SoThangBaoHanh)
                .HasDefaultValue(0)
                .HasColumnName("so_thang_bao_hanh");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__ChiTietDo__ma_ho__656C112C");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__ChiTietDo__ma_sa__66603565");
        });

        modelBuilder.Entity<DanhMucSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMucS__70624BDD76B5DDD7");

            entity.ToTable("DanhMucSanPham");

            entity.Property(e => e.MaDanhMuc).HasColumnName("ma_danh_muc");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("ten_danh_muc");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__DBE2D9E352157EE9");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHoaDon).HasColumnName("ma_hoa_don");
            entity.Property(e => e.MaKhachHang).HasColumnName("ma_khach_hang");
            entity.Property(e => e.MaKhuyenMai).HasColumnName("ma_khuyen_mai");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_dat");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tong_tien");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__HoaDon__ma_khach__5EBF139D");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__HoaDon__ma_khuye__619B8048");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__HoaDon__ma_nguoi__628FA481");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__C9817AF66CD20B49");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.Email, "UQ__KhachHan__AB6E61647A4AAB0A").IsUnique();

            entity.HasIndex(e => e.SoDienThoai, "UQ__KhachHan__BD03D94C402E5D14").IsUnique();

            entity.Property(e => e.MaKhachHang).HasColumnName("ma_khach_hang");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(200)
                .HasColumnName("dia_chi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.diemThuong).HasColumnName("diem_thuong");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .HasColumnName("so_dien_thoai");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(100)
                .HasColumnName("ten_khach_hang");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_tao");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__01A88CB39160E1C9");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.MaKhuyenMai).HasColumnName("ma_khuyen_mai");
            entity.Property(e => e.MoTa)
                .HasMaxLength(500)
                .HasColumnName("mo_ta");
            entity.Property(e => e.NgayBatDau).HasColumnName("ngay_bat_dau");
            entity.Property(e => e.NgayKetThuc).HasColumnName("ngay_ket_thuc");
            entity.Property(e => e.PhanTramGiam)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("phan_tram_giam");
            entity.Property(e => e.TenKhuyenMai)
                .HasMaxLength(100)
                .HasColumnName("ten_khuyen_mai");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__19C32CF781107D50");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__363698B36B6D6B70").IsUnique();

            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(100)
                .HasColumnName("mat_khau");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .HasColumnName("ten_dang_nhap");
            entity.Property(e => e.VaiTro)
                .HasMaxLength(50)
                .HasColumnName("vai_tro");
        });

        modelBuilder.Entity<NhapKho>(entity =>
        {
            entity.HasKey(e => e.MaNhapKho).HasName("PK__NhapKho__41601CCF9A88CD25");

            entity.ToTable("NhapKho");

            entity.Property(e => e.MaNhapKho).HasColumnName("ma_nhap_kho");
            entity.Property(e => e.LyDo)
                .HasMaxLength(500)
                .HasColumnName("ly_do");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_nhap");
            entity.Property(e => e.SoLuong)
                .HasDefaultValue(1)
                .HasColumnName("so_luong");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.NhapKhos)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__NhapKho__ma_nguo__01142BA1");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.NhapKhos)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__NhapKho__ma_san___7D439ABD");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__9D25990C0340FC4E");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.MaDanhMuc).HasColumnName("ma_danh_muc");
            entity.Property(e => e.SoLuong).HasColumnName("so_luong");
            entity.Property(e => e.SoThangBaoHanh)
                .HasDefaultValue(0)
                .HasColumnName("so_thang_bao_hanh");
            entity.Property(e => e.TenSanPham)
                .HasMaxLength(100)
                .HasColumnName("ten_san_pham");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasColumnName("trang_thai");
            entity.Property(e => e.LinkHinhAnh)
                .HasMaxLength(300)
                .HasColumnName("link_hinh_anh");
            entity.Property(e => e.Gia).HasColumnName("gia");
            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK__SanPham__ma_danh__4BAC3F29");
        });

        modelBuilder.Entity<YeuCauBaoTri>(entity =>
        {
            entity.HasKey(e => e.MaBaoTri).HasName("PK__YeuCauBa__EC20241E0A78B318");

            entity.ToTable("YeuCauBaoTri");

            entity.Property(e => e.MaBaoTri).HasColumnName("ma_bao_tri");
            entity.Property(e => e.MaKhachHang).HasColumnName("ma_khach_hang");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.MoTaSuCo)
                .HasMaxLength(500)
                .HasColumnName("mo_ta_su_co");
            entity.Property(e => e.NgayXuLy).HasColumnName("ngay_xu_ly");
            entity.Property(e => e.NgayYeuCau)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_yeu_cau");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.YeuCauBaoTris)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__YeuCauBao__ma_kh__76969D2E");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.YeuCauBaoTris)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__YeuCauBao__ma_ng__797309D9");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.YeuCauBaoTris)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__YeuCauBao__ma_sa__75A278F5");
        });

        modelBuilder.Entity<YeuCauDoiTra>(entity =>
        {
            entity.HasKey(e => e.MaYeuCau).HasName("PK__YeuCauDo__CAD816D2F9D0CDFC");

            entity.ToTable("YeuCauDoiTra");

            entity.Property(e => e.MaYeuCau).HasColumnName("ma_yeu_cau");
            entity.Property(e => e.LyDo)
                .HasMaxLength(500)
                .HasColumnName("ly_do");
            entity.Property(e => e.MaHoaDon).HasColumnName("ma_hoa_don");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.NgayXuLy).HasColumnName("ngay_xu_ly");
            entity.Property(e => e.NgayYeuCau)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ngay_yeu_cau");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.YeuCauDoiTras)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__YeuCauDoi__ma_ho__6E01572D");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.YeuCauDoiTras)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__YeuCauDoi__ma_ng__71D1E811");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.YeuCauDoiTras)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__YeuCauDoi__ma_sa__6EF57B66");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
