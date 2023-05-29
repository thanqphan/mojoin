$(document).ready(function () {
    // Lấy giá trị RoomTypeId của thẻ li đầu tiên trong danh sách
    var selectedRoomTypeId = $('.type-item:first').data('room-type-id');
    // Đưa giá trị RoomTypeId vào input hidden
    $('#room-type-id').val(selectedRoomTypeId);
    // Sáng màu thẻ li đầu tiên
    $('.type-item:first').addClass('selected');

    // Bắt sự kiện click vào các thẻ li
    $('.type-item').click(function () {
        // Loại bỏ lớp 'selected' của các thẻ li khác
        $('.type-item').removeClass('selected');
        // Thêm lớp 'selected' vào thẻ li được chọn
        $(this).addClass('selected');
        // Lấy giá trị RoomTypeIdcủa thẻ li được chọn
        var selectedRoomTypeId = $(this).data('room-type-id');
        // Đưa giá trị RoomTypeId vào input hidden
        $('#room-type-id').val(selectedRoomTypeId);
    });

    // Bắt sự kiện submit form
    $('form').submit(function () {
        // Kiểm tra xem RoomTypeId có được chọn không
        if ($('#room-type-id').val() == '') {
            // Hiển thị thông báo lỗi
            alert('Vui lòng chọn loại phòng!');
            return false;
        }
    });
});