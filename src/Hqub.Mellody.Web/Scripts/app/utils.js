executeOnServerModel = function (model, url, callback) {

    return $.ajax({
        url: url,
        type: 'POST',
        data: ko.mapping.toJSON(model),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (callback)
                callback(data);
            else {
                if (data.redirect) {
                    location.href = resolveUrl(data.url);
                } else {
                    ko.mapping.fromJS(data, null, model);
                }
            }
        },
        error: function (error) {
            alert("There was an error posting the data to the server: " + (error.responseText || error.statusText));
        }
    });

};

get = function(url, callback) {
    return $.ajax({
        url: url,
        type: 'GET',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (callback)
                callback(data);
            else {
                if (data.redirect) {
                    location.href = resolveUrl(data.url);
                }
            }
        },
        error: function (error) {
            alert("There was an error posting the data to the server: " + (error.responseText || error.statusText));
        }
    });
}

executeOnServer = function (model, url, callback) {

    return $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (callback)
                callback(data);
            else {
                if (data.redirect) {
                    location.href = resolveUrl(data.url);
                }
            }
        },
        error: function (error) {
            alert("There was an error posting the data to the server: " + (error.responseText || error.statusText));
        }
    });

}

resolveUrl = function (url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
};