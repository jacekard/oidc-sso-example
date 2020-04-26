function assignAdminRoleToUser() {
    $('.admin-checkbox > input').change(function () {
        var checked = $(this)[0].checked;
        var userId = $(this).attr('user');
        $.ajax({
            url: 'admin/',
            type: 'PUT',
            data: { userId: userId, adminRole: checked },
            success: function () {
                location.reload();
            },
            error: function () {
                location.reload();
                alert("Something went wrong! We're sorry");
            }
        });
    });
}

function removeUser() {
    $('.user-checkbox > input').change(function () {
        var userId = $(this).attr('user');
        $.ajax({
            url: 'user/',
            type: 'DELETE',
            data: { userId: userId },
            beforeSend: function () {
                alert("User will be removed.");
            },
            success: function () {
                location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
                alert("Something went wrong! We're sorry");
            }
        });
    });
}

function assignProjectToUser() {
    console.log("xd");
    $('.project-checkbox > input').change(function () {
        var checked = $(this)[0].checked;
        var userId = $(this).attr('user');
        var projectId = $(this).attr('id');
        var data = { userId: userId, projectId: projectId, giveRights: checked };

        $.ajax({
            url: '../project/',
            type: 'PUT',
            data: data,
            error: function (a, b, c, ) {
                console.log(a + b + c);
                location.reload();
                alert("Something went wrong! We're sorry");
            }
        });
    });
}


(function () {
    "use strict";

    $('[data-toggle="tooltip"]').tooltip();
    assignAdminRoleToUser();
    assignProjectToUser();
    removeUser();
})();