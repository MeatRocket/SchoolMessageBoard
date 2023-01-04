window.addEventListener("load", function () {
    document.querySelector(".loading").style.display = "none";
});
window.addEventListener('beforeunload', function (event) {
    document.querySelector('.loading').style.display = 'flex';
});


