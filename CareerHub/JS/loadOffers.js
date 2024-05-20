$(document).ready(function () {
    var offset = 9; // Initial offset
    var limit = 18; // Initial limit

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            
            LoadMoreData(offset, limit);
            offset += limit;
            limit += 9;
        }
    });
});

function LoadMoreData(offset, limit) {
    $.ajax({
        type: "POST",
        url: "index.aspx/LoadMoreJobs",
        data: JSON.stringify({ offset: offset, limit: limit }),
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

function openJobPage(jobId) {
    // Construct the URL of the new page
    var url = "jobDetails.aspx?jobId=" + jobId;

    // Navigate to the new page
    window.location.href = url;
}