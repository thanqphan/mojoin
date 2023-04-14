// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var myDiv = document.getElementById("open");

myDiv.addEventListener("click", function () {
    if (myDiv.style.display === "none") {
        myDiv.style.display = "block";
    } else {
        myDiv.style.display = "none";
    }
});
