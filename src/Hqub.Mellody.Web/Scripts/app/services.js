/* Station services */

//return ID station
function CreateStation(queries, success, error, finaly) {
    executeOnServer(new RadioDTO(queries), '/Station/Create', function (data) {
        if (data.IsError) {
            if (error)
                error(data);
        }else if (success)
            success(data);

        if (finaly)
            finaly(data);
    });
}

function RunStation(data) {
    window.location.href = '/Station/Index/' + data.StationId;
}

function DefaultErrorHandle(data) {
    if (data.IsError) {
        console.log("Error: " + data.Message + " (" + data.statusCode + ")");
        Show("error", data.Message);
    }
}