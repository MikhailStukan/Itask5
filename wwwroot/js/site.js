// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    autoComplete();
});


function autoComplete() {
    $('#recipient').autocomplete({
        source: '/api/search'
    });
}

var intervalId = window.setInterval(function () {
    fetchMessages();
}, 5000);

function fetchMessages() {
    var username = $("#userName").text();
    console.log(username);
    $.ajax({
        url: "/api/messages",
        type: "POST",
        data: JSON.stringify(username),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            updateTable(response);
        },
        error: function (response) {
            console.log('error:' + response);
        },
    });
};


function updateTable(response) {
    console.log(response);
    $("#messagesData").empty();
    for (let i = 0; i < response.length; i++) {
            appendRowOfData(response[i]);
        }
};

function appendRowOfData(message) {
    console.log("Sender: " + message.sender_Name, "Id: " + message.id, "Title: " + message.title, "Content: " + message.content);
    $("#messagesData").append(
        "<tr>" +
        "<td>" + message.sender_Name + "</td>" +
        "<td>" + message.title + "</td>" +
        "<td>" + message.content + "</td>" +
        "</tr>"
        )
};

