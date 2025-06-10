// Dashboard JavaScript functionality
document.addEventListener('DOMContentLoaded', function () {

    // Sidebar submenu toggle
    const submenuToggles = document.querySelectorAll('.submenu-toggle');
    submenuToggles.forEach(toggle => {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();
            const menuItem = this.closest('.menu-item');
            const submenu = menuItem.querySelector('.submenu');

            // Close other open submenus
            document.querySelectorAll('.menu-item.has-submenu').forEach(item => {
                if (item !== menuItem) {
                    item.classList.remove('open');
                }
            });

            // Toggle current submenu
            menuItem.classList.toggle('open');
        });
    });

    // Mobile sidebar toggle
    const sidebarToggle = document.getElementById('sidebar-toggle');
    const sidebar = document.querySelector('.sidebar');

    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function () {
            sidebar.classList.toggle('open');
        });
    }

    // Search functionality
    const searchInput = document.querySelector('.search-box input');
    if (searchInput) {
        searchInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                const searchTerm = this.value.trim();
                if (searchTerm) {
                    console.log('Searching for:', searchTerm);
                    // Add search functionality here
                }
            }
        });
    }

    // Refresh button
    const refreshBtn = document.querySelector('.refresh-btn');
    if (refreshBtn) {
        refreshBtn.addEventListener('click', function () {
            this.style.transform = 'rotate(360deg)';
            this.style.transition = 'transform 0.5s ease';

            setTimeout(() => {
                this.style.transform = '';
                this.style.transition = '';
            }, 500);

            // Add refresh functionality here
            console.log('Refreshing dashboard...');
        });
    }

    // Notification bell
    const notificationBtn = document.querySelector('.notification-btn');
    if (notificationBtn) {
        notificationBtn.addEventListener('click', function () {
            console.log('Opening notifications...');
            // Add notification panel toggle here
        });
    }

    // Quick action cards
    const quickActionCards = document.querySelectorAll('.quick-action-card');
    quickActionCards.forEach(card => {
        card.addEventListener('click', function () {
            const action = this.querySelector('span').textContent.trim();
            console.log('Quick action clicked:', action);

            // Add quick action functionality based on the action type
            switch (action) {
                case 'Thêm sản phẩm':
                    // Navigate to add product page
                    break;
                case 'Tạo đơn hàng':
                    // Navigate to create order page
                    break;
                case 'Thêm khách hàng':
                    // Navigate to add customer page
                    break;
                case 'Xem báo cáo':
                    // Navigate to reports page
                    break;
            }
        });
    });

    // Activity item links
    const activityLinks = document.querySelectorAll('.activity-link');
    activityLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            console.log('Activity link clicked');
            // Add navigation to activity detail here
        });
    });

    // Animate stats on page load
    animateStats();

    // Auto-refresh dashboard data (optional)
    // setInterval(refreshDashboardData, 30000); // Refresh every 30 seconds
});

// Animate stat numbers on page load
function animateStats() {
    const statNumbers = document.querySelectorAll('.stat-content h2');

    statNumbers.forEach(stat => {
        const finalValue = stat.textContent.trim();
        const numericValue = parseFloat(finalValue.replace(/[^\d.]/g, ''));

        if (!isNaN(numericValue)) {
            let currentValue = 0;
            const increment = numericValue / 50; // 50 steps for animation
            const timer = setInterval(() => {
                currentValue += increment;
                if (currentValue >= numericValue) {
                    currentValue = numericValue;
                    clearInterval(timer);
                }

                // Format the number based on original format
                let formattedValue;
                if (finalValue.includes('T ₫')) {
                    formattedValue = currentValue.toFixed(1) + ' T ₫';
                } else if (finalValue.includes(',')) {
                    formattedValue = Math.floor(currentValue).toLocaleString();
                } else {
                    formattedValue = Math.floor(currentValue).toString();
                }

                stat.textContent = formattedValue;
            }, 30);
        }
    });
}

// Refresh dashboard data function
function refreshDashboardData() {
    // This would typically make an AJAX call to get updated data
    console.log('Refreshing dashboard data...');

    // Example: Update timestamp
    const activities = document.querySelectorAll('.activity-time');
    activities.forEach(time => {
        // Update relative time stamps
    });
}

// Utility function to format numbers
function formatNumber(num, decimals = 0) {
    if (num >= 1000000000) {
        return (num / 1000000000).toFixed(decimals) + 'B';
    } else if (num >= 1000000) {
        return (num / 1000000).toFixed(decimals) + 'M';
    } else if (num >= 1000) {
        return (num / 1000).toFixed(decimals) + 'K';
    }
    return num.toString();
}

// Handle window resize for responsive behavior
window.addEventListener('resize', function () {
    // Handle responsive adjustments if needed
    if (window.innerWidth <= 640) {
        // Mobile adjustments
        const sidebar = document.querySelector('.sidebar');
        if (sidebar && !sidebar.classList.contains('open')) {
            // Ensure sidebar is hidden on mobile
        }
    }
});

// Quyên
/*function editProduct(id) {
    $.get(`/api/products/${id}`, function (data) {
        $('#productId').val(data.maSanPham);
        $('#productName').val(data.tenSanPham);
        $('#categorySelect').val(data.maDanhMuc);
        $('#statusSelect').val(data.trangThai);
        $('#quantity').val(data.soLuong);
        $('#warranty').val(data.soThangBaoHanh);
        $('#productModal').modal('show');
    });
}

function saveProduct() {
    const id = $('#productId').val();
    const product = {
        maSanPham: id,
        tenSanPham: $('#productName').val(),
        maDanhMuc: $('#categorySelect').val(),
        trangThai: $('#statusSelect').val(),
        soLuong: $('#quantity').val(),
        soThangBaoHanh: $('#warranty').val()
    };

    const method = id ? 'PUT' : 'POST';
    const url = id ? `/api/products/Update/${id}` : '/api/products/Insert';

    $.ajax({
        url: url,
        method: method,
        contentType: 'application/json',
        data: JSON.stringify(product),
        success: function () {
            $('#productModal').modal('hide');
            $('#productTable').DataTable().ajax.reload();
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function deleteProduct(id) {
    if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này không?")) {
        $.ajax({
            url: `/api/products/Delete/${id}`,
            method: 'DELETE',
            success: function () {
                $('#productTable').DataTable().ajax.reload();
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    }
}
*/