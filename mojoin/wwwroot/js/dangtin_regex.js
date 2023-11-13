// Lấy đối tượng của các trường nhập liệu
const sonhaField = document.querySelector('input[name="SoNha"]');
const areaField = document.querySelector('input[name="Area"]');
const priceField = document.querySelector('input[name="Price"]');
const titleField = document.querySelector('input[name="Title"]');
const descriptionField = document.querySelector('textarea[name="Description"]');
const videoField = document.querySelector('input[name="Video"]');
// Thêm sự kiện khi người dùng thoát khỏi trường nhập liệu
/*sonhaField.addEventListener('blur', validateSoNha);*/
areaField.addEventListener('blur', validateArea);
priceField.addEventListener('blur', validatePrice);
titleField.addEventListener('blur', validateTitle);
descriptionField.addEventListener('blur', validateDescription);
videoField.addEventListener('blur', validateVideo);

function validateArea() {
    // Lấy giá trị của trường diện tích
    const areaValue = areaField.value.trim();

    // Kiểm tra xem trường có rỗng hay không
    if (areaValue === '') {
        const errorMessage = 'Vui lòng nhập diện tích sử dụng';
        document.querySelector('#fieldArea-error').innerHTML = errorMessage;
        document.querySelector('#fieldArea-error').style.display = 'block';
    } else {
        // Kiểm tra xem giá trị nhập vào có phải là số hay không
        if (isNaN(areaValue)) {
            const errorMessage = 'Diện tích phải là số';
            document.querySelector('#fieldArea-error').innerHTML = errorMessage;
            document.querySelector('#fieldArea-error').style.display = 'block';
        } else {
            // Kiểm tra xem giá trị nhập vào có thỏa mãn điều kiện hay không
            if (areaValue < 10 || areaValue > 8000) {
                const errorMessage = 'Diện tích phải > 10 và < 8.000m2';
                document.querySelector('#fieldArea-error').innerHTML = errorMessage;
                document.querySelector('#fieldArea-error').style.display = 'block';
            } else {
                document.querySelector('#fieldArea-error').innerHTML = '';
                document.querySelector('#fieldArea-error').style.display = 'none';
            }
        }
    }
}

function validatePrice() {
    // Lấy giá trị của trường giá
    const priceValue = priceField.value.trim();

    // Kiểm tra xem trường có rỗng hay không
    if (priceValue === '') {
        document.querySelector('#fieldPrice-error').innerHTML = 'Vui lòng nhập giá';
    } else {
        // Kiểm tra xem giá trị nhập vào có phải là số hay không
        if (isNaN(priceValue)) {
            document.querySelector('#fieldPrice-error').innerHTML = 'Giá phải là số';
        } else {
            if (priceValue < 1000000 || priceValue > 30000000) {
                const errorMessage = 'Giá phòng phải > 1.000.000 và < 30.000.000VNĐ';
                document.querySelector('#fieldPrice-error').innerHTML = errorMessage;
                document.querySelector('#fieldPrice-error').style.display = 'block';
            } else {
                document.querySelector('#fieldPrice-error').innerHTML = '';
                document.querySelector('#fieldPrice-error').style.display = 'none';
            }
        }
    }
}
function validateTitle() {
    // Lấy giá trị của trường tiêu đề
    const titleValue = titleField.value.trim();

    // Kiểm tra xem trường có rỗng hay không
    if (titleValue === '') {
        document.querySelector('#fieldTitle-error').innerHTML = 'Vui lòng nhập Tiêu đề';
    } else if (titleValue.length < 40 || titleValue.length > 200) {
        // Kiểm tra độ dài của giá trị tiêu đề
        const errorMessage = 'Tiêu đề phải có ít nhất 50 kí tự và không quá 200 kí tự';
        document.querySelector('#fieldTitle-error').innerHTML = errorMessage;
        document.querySelector('#fieldTitle-error').style.display = 'block';
    } else {
        document.querySelector('#fieldTitle-error').innerHTML = '';
        document.querySelector('#fieldTitle-error').style.display = 'none';
    }
}

function validateDescription() {
    // Lấy giá trị của trường mô tả
    const descriptionValue = descriptionField.value.trim();

    // Kiểm tra xem trường có rỗng hay không
    if (descriptionValue === '') {
        document.querySelector('#fieldDescription-error').innerHTML = 'Vui lòng nhập Mô tả';
    } else if (descriptionValue.length < 1000 || descriptionValue.length > 10000) {
        // Kiểm tra độ dài của giá trị mô tả
        const errorMessage = 'Mô tả phải có ít nhất 1000 kí tự và không quá 10000 kí tự';
        document.querySelector('#fieldDescription-error').innerHTML = errorMessage;
        document.querySelector('#fieldDescription-error').style.display = 'block';
    } else {
        document.querySelector('#fieldDescription-error').innerHTML = '';
        document.querySelector('#fieldDescription-error').style.display = 'none';
    }
}
function validateVideo() {
    // Lấy giá trị của trường mô tả
    const videoValue = videoField.value.trim();

    // Biểu thức chính quy để kiểm tra đường link từ YouTube
    const youtubeRegex = /^(https?:\/\/)?(www\.)?(youtube\.com\/(embed\/|v\/|watch\?v=)|youtu\.be\/)/;

    // Kiểm tra xem trường có rỗng hay không
    if (videoValue === '') {
        document.querySelector('#fieldVideo-error').innerHTML = 'Bạn có chắc không dùng video?';
    } else if (youtubeRegex.test(videoValue)) {
        document.querySelector('#fieldVideo-error').innerHTML = '';
        document.querySelector('#fieldVideo-error').style.display = 'none';
    } else {
        document.querySelector('#fieldVideo-error').innerHTML = 'Đây không phải là đường link từ YouTube.';
    }
}
