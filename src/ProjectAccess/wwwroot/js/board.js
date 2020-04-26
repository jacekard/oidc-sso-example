var isNewTaskActive;

function buildTask(title, desc, category) {
    var template =
        "<div id='newCanbanTask' class='CanbanTask'><p>{0}</p><input id='addNewTaskTitle'</input>".format(title)
        + "<p>{0}</p><input id='addNewTaskText' type='text'></input><br />".format(desc)
        + "<button class='saveTaskButton' type='button' onclick='saveNewTask({0})'".format(category)
        + "onsubmit='return false'>Save</button><button class='cancelTaskButton' type='button' onclick='cancelNewTask(this)'"
        + "onsubmit='return false'>Cancel</button><div>";

    return template;
}

function AppendNewTask(id, title, desc) {
    var template = "<div id='{0}' class='CanbanTask'>".format(id)
        + "<i id='delete-{0}' class='delete-icon material-icons'>delete_forever</i>".format(id)
        + "<h5 class='title-{0}'>{1}</h5>".format(id, title)
        + "<p class='desc-{0}'>{1}</p></div>".format(id, desc);

    return template;
}

function makeSortableColumns() {
    $("#ColTodo, #ColProg, #ColDone").sortable({
        connectWith: ".CanbanColumnContent",
        activate: function (e, ui) {
            $(ui.item[0]).css('border', '3px dashed grey');
            console.log("asd");
        },
        stop: function (e, ui) {
            changeTaskStatus(ui.item[0]);
            $(ui.item[0]).css('cursor', 'grab');
            $(ui.item[0]).css('border', 'none');
        }
    }).disableSelection();
}

function newTaskEvents() {
    $(".CanbanColumnContent").off().on('click', function (event) {
        if (event.target === event.currentTarget) {
            if (isNewTaskActive) return;
            var category = $(this).attr("id");
            var selector = "#{0}".format(category);
            var title = "Name";
            var desc = "Description";
            var template = buildTask(title, desc, category);
            isNewTaskActive = true;
            $(selector).append(template);
        }
    });
}

function getStatusCode(status) {
    if (status === "ColTodo")
        return 0;
    else if (status === "ColProg")
        return 1;
    else
        return 2;
}

function saveNewTask(category) {
    var status = $(category).attr('id');
    var title = $("#addNewTaskTitle").val();
    var text = $("#addNewTaskText").val();
    var projectId = $("#projectId").text();
    var url = "../api/tasks/{0}".format(projectId); 

    console.log(url);
    if (title === "" || text === "") {
        alert("fields can't be empty!");
        return;
    }

    var data = {
        Title: title,
        Text: text,
        Status: getStatusCode(status)
    };

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        success: function (taskId) {
            var selector = "#{0}".format(status);
            var template = AppendNewTask(taskId, title, text);
            cancelNewTask("#addNewTaskTitle");
            $(selector).append(template);
        },
        error: function (a,b,c) {
            console.log(a);
            console.log(b);
            console.log(c);
            //location.reload();
            //alert("Something went wrong! We're sorry");
        }
    });
}

function cancelNewTask(e) {
    $(e).parent().remove();
    isNewTaskActive = false;
}

function changeTaskStatus(item) {
    var id = $(item).attr('id');
    var status = $(item).parent().attr('id');
    var url = "../api/tasks/{0}".format(id);
    $.ajax({
        url: url,
        type: 'PUT',
        data: { status: getStatusCode(status) },
        error: function (a, b, c) {
        }
    });
}

function deleteTaskButton() {
    $(".delete-icon").click(function () {
        var id = $(this).attr('id').replace("delete-", "");
        var url = "../api/tasks/{0}".format(id);

        $.ajax({
            url: url,
            type: 'DELETE',
            accepts: "application/json; charset=utf-8",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            success: function () {
                $("#{0}".format(id)).remove();
            },
            error: function (a, b, c) {
                location.reload();
                alert("Something went wrong! We're sorry");
            }
        });
    });
}

$(function () {
    "strict";

    $('[data-toggle="tooltip"]').tooltip();
    isNewTaskActive = false;
    newTaskEvents();
    makeSortableColumns();
    deleteTaskButton();
});
