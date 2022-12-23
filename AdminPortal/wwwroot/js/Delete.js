function ShowPopup(id) {
    document.querySelector("._" + id).style.display = "flex";
}

function HidePopup(id) {
    document.querySelector("._" + id).style.display = "none";
}

function DeleteUser(id) {
    $.ajax({
        type: 'GET',
        url: '/Admin/DeleteUser',
        data: {
            UserId: id
        },
        success: function (id) {
            HidePopup(id);
            document.querySelector('[id=' + '"' + id + '"]').remove();
        }
    });
}