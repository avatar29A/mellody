//
//Utils for any times.
//********************


//
// Work with Ajax


//
// Ajax get request on server. Invoke callback function after.
get = function (url, callback) {
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

//
// Post model on server (Ajax) and then mapping on VM. After will invoke callback function.
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


//
// Post model on server (Ajax). Invoke callback function after.
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

//
// ************************

//
// Work with cookies


function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function setCookie(name, value, options) {
    options = options || {};

    var expires = options.expires;

    if (typeof expires == "number" && expires) {
        var d = new Date();
        d.setTime(d.getTime() + expires * 1000);
        expires = options.expires = d;
    }
    if (expires && expires.toUTCString) {
        options.expires = expires.toUTCString();
    }

    value = encodeURIComponent(value);

    var updatedCookie = name + "=" + value;

    for (var propName in options) {
        updatedCookie += "; " + propName;
        var propValue = options[propName];
        if (propValue !== true) {
            updatedCookie += "=" + propValue;
        }
    }

    document.cookie = updatedCookie;
}

//
// *****************

//
// Message dialogs

function Show(title, message) {
    $.Dialog({
        overlay: true,
        shadow: true,
        flat: true,
        icon: '',
        title: title,
        content: message,
        width: 500,
        padding: 10
    });
}