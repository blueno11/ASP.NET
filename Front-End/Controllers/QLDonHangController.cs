using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Front_End.Models;
using System.Text;
namespace Front_End.Controllers
{
    public class QLDonHangController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public QLDonHangController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Index(

           DateTime? fromDate,
           DateTime? toDate,
           decimal? minTotal,
           decimal? maxTotal,
           string maKH,
           string maND,
           string sortBy = "ngay_dat", // Thêm tham số sortBy: mặc định sắp xếp theo ngày đặt
           string sortOrder = "desc",  // Thêm tham số sortOrder: mặc định giảm dần
           int page = 1,
           int pageSize = 10)
        {
            if(HttpContext.Session.GetInt32("MaNguoiDung") == null)
            {
                return RedirectToAction("Index","Login");
            }
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");
            var response = await client.GetAsync("https://localhost:7156/api/QLDonHang");

            if (!response.IsSuccessStatusCode)
                return View(new List<HoaDonDto>());

            var data = await response.Content.ReadAsStringAsync();
            var hoaDons = JsonSerializer.Deserialize<List<HoaDonDto>>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<HoaDonDto>();

            //Phần lọc
            if (fromDate.HasValue)
                hoaDons = hoaDons.Where(h => h.ngay_dat.Date >= fromDate.Value.Date).ToList();

            if (toDate.HasValue)
                hoaDons = hoaDons.Where(h => h.ngay_dat.Date <= toDate.Value.Date).ToList();

            if (minTotal.HasValue)
                hoaDons = hoaDons.Where(h => h.tong_tien >= minTotal.Value).ToList();

            if (maxTotal.HasValue)
                hoaDons = hoaDons.Where(h => h.tong_tien <= maxTotal.Value).ToList();

            //Phần sắp xếp
            switch (sortBy?.ToLower())
            {
                case "ngay_dat":
                    hoaDons = (sortOrder == "asc")
                              ? hoaDons.OrderBy(h => h.ngay_dat).ToList()
                              : hoaDons.OrderByDescending(h => h.ngay_dat).ToList();
                    break;
                case "tong_tien":
                    hoaDons = (sortOrder == "asc")
                              ? hoaDons.OrderBy(h => h.tong_tien).ToList()
                              : hoaDons.OrderByDescending(h => h.tong_tien).ToList();
                    break;
                default:
                    // Mặc định sắp xếp theo ngay_dat giảm dần nếu không có sortBy hợp lệ
                    hoaDons = hoaDons.OrderByDescending(h => h.ngay_dat).ToList();
                    break;
            }

            // Phân trang
            int totalRecords = hoaDons.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var pagedData = hoaDons
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền các tham số hiện tại sang View để duy trì trạng thái
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = totalRecords;

            // Truyền lại các tham số lọc
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.MinTotal = minTotal;
            ViewBag.MaxTotal = maxTotal;
            ViewBag.MaKH = maKH;
            ViewBag.MaND = maND;

            // Truyền các tham số sắp xếp hiện tại
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(pagedData);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? loai, string? tuKhoa)
        {
            if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");
            var viewModel = new GioHangViewModel
            {
                DanhSachLoai = new(),
                DanhSachSanPham = new(),
                LoaiDuocChon = loai,
                DanhSachKhuyenMai = new(),
                TuKhoa = tuKhoa,
                GioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>()
            };

            //Lấy danh sách Loại hàng
            var response = await client.GetAsync("https://localhost:7156/api/Categories/List");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                viewModel.DanhSachLoai = JsonSerializer.Deserialize<List<CategoryDto>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            //Lấy danh sách sản phẩm
            response = await client.GetAsync("https://localhost:7156/api/Products/List");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var danhSachSanPham = JsonSerializer.Deserialize<List<SanPham>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                viewModel.DanhSachSanPham = danhSachSanPham.Where(p => p.TrangThai == "ConHang").ToList();
            }
            //Lấy danh sách khuyến mãi
            response = await client.GetAsync("https://localhost:7156/api/Promotions/List");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                viewModel.DanhSachKhuyenMai = JsonSerializer.Deserialize<List<KhuyenMai>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            //Lấy danh sách khách hàng
            response = await client.GetAsync("https://localhost:7156/api/Customer/List");
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                viewModel.DanhSachKhachHang = JsonSerializer.Deserialize<List<KhachHang>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }


            //lọc
            if (loai.HasValue)
                viewModel.DanhSachSanPham = viewModel.DanhSachSanPham.Where(p => p.MaDanhMuc == loai).ToList();
            if (!string.IsNullOrWhiteSpace(tuKhoa))
                viewModel.DanhSachSanPham = viewModel.DanhSachSanPham.Where(p => p.TenSanPham.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ThemVaoGio(int maSanPham, int? loai, string? tuKhoa)
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>();

            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");
            var response = await client.GetAsync($"https://localhost:7156/api/Products/" + maSanPham);
            if (!response.IsSuccessStatusCode) return RedirectToAction("Create");

            var data = await response.Content.ReadAsStringAsync();
            var sanPham = JsonSerializer.Deserialize<SanPham>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (sanPham != null)
            {
                var spTrongGio = gioHang.FirstOrDefault(sp => sp.MaSanPham == maSanPham);
                if (spTrongGio != null)
                {
                    spTrongGio.SoLuong += 1;
                }
                else
                {
                    gioHang.Add(new GioHangItem
                    {
                        MaSanPham = sanPham.MaSanPham,
                        TenSanPham = sanPham.TenSanPham,
                        Gia = sanPham.Gia,
                        soThangBaoHanh = sanPham.SoThangBaoHanh,
                        SoLuong = 1
                    });
                }
                HttpContext.Session.SetObject("GioHang", gioHang);
            }

            return RedirectToAction("Create", "QlDonHang", new { loai = loai, tuKhoa = tuKhoa });
        }

        public IActionResult XoaKhoiGio(int maSanPham)
        {
            // Lấy giỏ hàng từ Session (nếu chưa có thì tạo mới)
            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang") ?? new List<GioHangItem>();

            // Tìm sản phẩm cần xóa
            var sanPham = gioHang.FirstOrDefault(sp => sp.MaSanPham == maSanPham);
            if (sanPham != null)
            {
                gioHang.Remove(sanPham); // Xóa khỏi danh sách
                HttpContext.Session.SetObject("GioHang", gioHang); // Ghi lại Session
            }

            return RedirectToAction("Create");
        }

        [HttpGet]
        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (HttpContext.Session.GetInt32("MaNguoiDung") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Console.WriteLine("ChiTietHoaDon dang chạy");
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");


            //lấy chi tiết hóa đơn
            var response = await client.GetAsync($"https://localhost:7156/api/QLDonHang/ChiTietDonHang/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var chiTietList = JsonSerializer.Deserialize<List<ChiTietDonHangDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var model = new ChiTietDonHangViewModel
            {
                MaHoaDon = id,
                ChiTietDonHangs = chiTietList ?? new List<ChiTietDonHangDto>()
            };

            //lấy hóa đơn
            response = await client.GetAsync($"https://localhost:7156/api/QLDonHang/" + model.MaHoaDon);
            if (!response.IsSuccessStatusCode)
                return NotFound();
            json = await response.Content.ReadAsStringAsync();
            var hoaDon = JsonSerializer.Deserialize<HoaDonDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (hoaDon != null) model.TongTien = hoaDon.tong_tien;
            
            model.khachHang = hoaDon.MaKhachHangNavigation;
            //lấy khuyến mãi
            if (hoaDon.ma_khuyen_mai != null)
            {
                response = await client.GetAsync($"https://localhost:7156/api/promotions/{hoaDon.ma_khuyen_mai}");
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    var khuyenMai = JsonSerializer.Deserialize<KhuyenMai>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (khuyenMai != null)
                    {
                        model.TenKhuyenMai = khuyenMai.TenKhuyenMai;
                        model.PhanTramGiam = khuyenMai.PhanTramGiam;
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ThanhToan(int tongTienSauGiam, int maKhuyenMai, int maKhachHang)
        {
            var gioHang = HttpContext.Session.GetObject<List<GioHangItem>>("GioHang");
            var client = _httpClientFactory.CreateClient("BanHangDienMayAPI");

            // Lấy mã người dùng từ session
            int? maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            var hoadon = new HoaDonDto
            {
                ma_khach_hang = maKhachHang, // demo
                ma_nguoi_dung = maNguoiDung, // Lấy từ session
                ngay_dat = DateTime.Now,
                ma_khuyen_mai = maKhuyenMai,
                tong_tien = tongTienSauGiam
            };
            Console.WriteLine(tongTienSauGiam);
            var response = await client.PostAsJsonAsync("https://localhost:7156/api/QLDonHang/Add", hoadon);
            if (!response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = response.Content.ReadAsStringAsync();
                return RedirectToAction("Create");
            }
            var json = await response.Content.ReadAsStringAsync();

            var createdHoaDon = JsonSerializer.Deserialize<HoaDonDto>(json);
            Console.WriteLine($"🔢 ma_hoa_don = {createdHoaDon?.ma_hoa_don}");
            int maHoaDonMoi = createdHoaDon.ma_hoa_don;
            // ✅ Chuyển giỏ hàng sang ChiTietDonHang
            var chiTietDonHangList = gioHang.Select(p => new ChiTietDonHangDto
            {
                MaHoaDon = maHoaDonMoi,
                MaSanPham = p.MaSanPham,
                GiaBan = p.Gia,
                SoLuong = p.SoLuong,
                SoThangBaoHanh = 12 // mặc định hoặc lấy từ sản phẩm nếu có
            }).ToList();
            Console.WriteLine(chiTietDonHangList);
            // ✅ Gửi đúng danh sách chi tiết
            var responseChiTiet = await client.PostAsJsonAsync("https://localhost:7156/api/QLDonHang/AddChiTiet", chiTietDonHangList);
            if (responseChiTiet.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm hóa đơn thành công!";
                int diemTang = (int)(tongTienSauGiam * 0.0003);
                var request = new
                {
                    DiemTang = diemTang,
                    MaKhachHang = maKhachHang
                };

                await client.PostAsJsonAsync("https://localhost:7156/api/QLDonHang/tangDiem", request);
                HttpContext.Session.Remove("GioHang"); // Xoá giỏ hàng
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }

    }
}

