$(document).ready(function () {
    let offset = 9; // Initial offset
    let limit = 18; // Initial limit
    let email = getCookie('AuthCookie');

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {

            LoadMoreData(offset, limit, email);
            offset += limit;
            limit += 9;
        }
    });
});

function LoadMoreData(offset, limit, email) {
    $.ajax({
        type: "POST",
        url: "profile.aspx/LoadMoreJobs",
        data: JSON.stringify({ offset: offset, limit: limit, email:email }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#mainContent").append(response.d);

        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            console.log("Failed to load more data. Please check console for details.");
        }
    });
}

function getCookie(name) {
    let cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            return cookie.substring((name + '=').length, cookie.length);
        }
    }
    return null;
}

function deleteJob(jobId) {
    document.getElementById('hiddenJobId').value = jobId;
    document.getElementById('hiddenDeleteButton').click();
}

function openJobPage(jobId) {
    // Construct the URL of the new page
    var url = "jobDetails.aspx?jobId=" + jobId;

    // Navigate to the new page
    window.location.href = url;
}