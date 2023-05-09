
document.getElementById("toggle").addEventListener("click", function (event) {
        event.preventDefault()
    });
function userclick() {

    var tg = document.getElementById("toggleUser");


    if (tg.style.display == "none") {
        tg.style.display = "block";
    } else {
        tg.style.display = "none";
    }
}
//yeu thich
document.getElementById("viewlist").addEventListener("click", function (event) {
    event.preventDefault()
});
function yeuthich() {
    const btn = this.querySelector("#btn_save_ad");
    const img = this.querySelector("#img_save_ad");
    btn.addEventListener('click', () => {
        if (img.getAttribute("src") === "https://static.chotot.com/storage/icons/saveAd/save-ad.svg") {
            img.setAttribute("src", "https://static.chotot.com/storage/icons/saveAd/save-ad-active.svg");
            alert("Đã thêm vào mục yêu thích!");
        }
        else {
            alert("Đã bỏ yêu thích !");
            img.setAttribute("src", "https://static.chotot.com/storage/icons/saveAd/save-ad.svg");
        }

    });
}
const ads = document.querySelectorAll(".SaveAd_saveAdWrapper___sBMh");
ads.forEach(SaveAd_saveAdWrapper___sBMh => {
    yeuthich.call(SaveAd_saveAdWrapper___sBMh);
});




