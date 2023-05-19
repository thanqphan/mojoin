var currentDiv = 0;
var divs = document.querySelectorAll('.form-group');
function nextDiv() {
    if (currentDiv < divs.length - 1) {
        currentDiv++;
        updateDivVisibility();
    }
}

function prevDiv() {
    if (currentDiv > 0) {
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


