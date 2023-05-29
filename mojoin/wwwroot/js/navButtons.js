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
    if (currentDiv >=0) {
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
function checkRequiredFields() {
    var requiredFields = divs[currentDiv].querySelectorAll('input[required], select[required], textarea[required]');
    var isValid = true;

    for (var i = 0; i < requiredFields.length; i++) {
        if (!requiredFields[i].value) {
            isValid = false;
            requiredFields[i].classList.add('error');
            requiredFields[i].style.border = '2px solid red';
            requiredFields[i].style.color = 'red';
            requiredFields[i].focus();

            var fieldName = requiredFields[i].name;
            var errorMessageElement = divs[currentDiv].querySelector('[asp-validation-for="' + fieldName + '"]');

            // Lấy thông tin 'required' từ mô hình
            var metadataElement = divs[currentDiv].querySelector('[data-valmsg-for="' + fieldName + '"]');
            var requiredMessage = metadataElement ? metadataElement.getAttribute('data-val-required') : 'Vui lòng nhập giá trị.';

            if (errorMessageElement) {
                errorMessageElement.textContent = requiredMessage;
                errorMessageElement.style.display = 'block';
            }
        } else {
            requiredFields[i].classList.remove('error');
            requiredFields[i].style.border = '1px solid #ccc';
            requiredFields[i].style.color = 'inherit';
            var errorMessageElement = divs[currentDiv].querySelector('[asp-validation-for="' + requiredFields[i].name + '"]');
            if (errorMessageElement) {
                errorMessageElement.textContent = '';
                errorMessageElement.style.display = 'none';
            }
        }

        // Xóa thay đổi CSS khi người dùng chọn hoặc nhập lại trường bị lỗi
        requiredFields[i].addEventListener('input', function () {
            this.classList.remove('error');
            this.style.border = '1px solid #ccc';
            this.style.color = 'inherit';
            var errorMessageElement = divs[currentDiv].querySelector('[asp-validation-for="' + this.name + '"]');
            if (errorMessageElement) {
                errorMessageElement.textContent = '';
                errorMessageElement.style.display = 'none';
            }
        });
    }

   

    return isValid;
}












