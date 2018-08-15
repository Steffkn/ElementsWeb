$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        if ($('#sidebar').hasClass('active')) {
            localStorage.setItem("dashboard", false);
            $('#sidebar').removeClass('active');
        }
        else {
            localStorage.setItem("dashboard", true);
            $('#sidebar').addClass('active');
        }
    });

    var isActive = localStorage.getItem("dashboard") === 'true';
    if (isActive == true) {
        $('#sidebar').addClass('active');
    }
    else {
        $('#sidebar').removeClass('active');
    }
});
