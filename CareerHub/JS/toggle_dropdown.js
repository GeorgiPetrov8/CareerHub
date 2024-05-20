function toggleDropdown() {
    var dropdownContent = document.getElementById("myDropdown");
    if (dropdownContent.style.display === "block") {
        dropdownContent.style.display = "none";
    } else {
        dropdownContent.style.display = "block";
    }
    event.stopPropagation();
}

window.addEventListener("click", function (event) {
    if (!event.target.matches('.dropdown span')) {
        document.getElementById("myDropdown").style.display = "none";
    }
});
