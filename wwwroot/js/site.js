// Toast notifications
window.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.erp-toast').forEach(function (el) {
        var toast = new bootstrap.Toast(el, { delay: 4000 });
        toast.show();
    });

    // Active sidebar link
    var path = window.location.pathname.toLowerCase();
    document.querySelectorAll('#sidebar .sidebar-nav a').forEach(function (a) {
        var href = a.getAttribute('href') || '';
        if (href !== '/' && path.startsWith(href.toLowerCase())) {
            a.classList.add('active');
        } else if (href === '/' && path === '/') {
            a.classList.add('active');
        }
    });

    // Mobile sidebar toggle
    var toggle = document.getElementById('sidebar-toggle');
    var sidebar = document.getElementById('sidebar');
    if (toggle && sidebar) {
        toggle.addEventListener('click', function () {
            sidebar.classList.toggle('open');
        });
    }
});
