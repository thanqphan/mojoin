var currentDiv = 0;
var divs = document.querySelectorAll('.form-group');

function nextDiv() {
    var isValid = true;

    if (currentDiv >= 1) {
        isValid = checkRequiredFields();
    }

    if (isValid) {
        if (currentDiv < divs.length - 1) {
            currentDiv++;
            updateDivVisibility();
        }
    }
}

function prevDiv() {
    if (currentDiv >= 0) {
        currentDiv--;
        updateDivVisibility();
    }
}

function updateDivVisibility() {
    for (var i = 0; i < divs.length; i++) {
        if (i === currentDiv) {
            divs[i].style.display = 'block';
        } else {
            divs[i].style.display = 'none';
        }
    }

    // show/hide buttons
    if (currentDiv === 0) {
        document.getElementById('btn-prev').style.display = 'none';
    } else {
        document.getElementById('btn-prev').style.display = 'inline-block';
    }

    if (currentDiv === divs.length - 1) {
        document.getElementById('btn-next').style.display = 'none';
        document.getElementById('btn-save').style.display = 'inline-block';
    } else {
        document.getElementById('btn-next').style.display = 'inline-block';
        document.getElementById('btn-save').style.display = 'none';
    }
}
// Lấy đối tượng của các trường nhập liệu
const areaField = document.querySelector('input[name="fieldArea"]');
const priceField = document.querySelector('input[name="fieldPrice"]');
const titleField = document.querySelector('input[name="fieldTitle"]');
const descriptionField = document.querySelector('input[name="fieldDescription"]');

// Thêm sự kiện khi người dùng thoát khỏi trường nhập liệu
areaField.addEventListener('blur', validateArea);
priceField.addEventListener('blur', validatePrice);
titleField.addEventListener('blur', validateTitle);
descriptionField.addEventListener('blur', validateDescription);
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
            }        }
    }
}
function validateTitle() {
    // Lấy giá trị của trường tiêu đề
    const titleValue = titleField.value.trim();

    // Kiểm tra xem trường có rỗng hay không
    if (titleValue === '') {
        document.querySelector('#fieldTitle-error').innerHTML = 'Vui lòng nhập Tiêu đề';
    } else if (titleValue.length < 50 || titleValue.length > 200) {
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

function checkRequiredFields() {
    var requiredFields = divs[currentDiv].querySelectorAll('input[required], select[required], textarea[required]');
    var isValid = true;

    for (var i = 0; i < requiredFields.length; i++) {
        var fieldName = requiredFields[i].name;
        var errorMessageElement = divs[currentDiv].querySelector('[asp-validation-for="' + fieldName + '"]');
        var requiredMessage = errorMessageElement ? errorMessageElement.getAttribute('data-val-required') : 'Vui lòng nhập giá trị.';
        var spanElement = document.getElementById(fieldName + '-error');

        if (!requiredFields[i].value.trim() || requiredFields[i].selectedIndex === 0) {
            isValid = false;

            if (errorMessageElement) {
                errorMessageElement.textContent = requiredMessage;
                errorMessageElement.style.display = 'block';
            }

            if (spanElement) {
                spanElement.style.display = 'block';
            }
        } else {
            if (errorMessageElement) {
                errorMessageElement.textContent = '';
                errorMessageElement.style.display = 'none';
            }

            if (spanElement) {
                spanElement.style.display = 'none';
            }
        }
    }

    return isValid;
}

var areaInput = document.getElementById('fieldArea');
areaInput.addEventListener('blur', function () {
    validateField(areaInput, 'fieldArea-error', 'Vui lòng nhập diện tích');
});

areaInput.addEventListener('input', function () {
    validateField(areaInput, 'fieldArea-error', 'Vui lòng nhập diện tích');
});

function validateField(inputElement, errorElementId, errorMessage) {
    var value = inputElement.value;
    var errorElement = document.getElementById(errorElementId);

    if (value.trim() === '') {
        errorElement.textContent = errorMessage;
        errorElement.style.display = 'block';
    } else {
        errorElement.textContent = '';
        errorElement.style.display = 'none';
    }
}
