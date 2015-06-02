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

//
// ************************

//
// Playlist utils

function searchFormatResult(item) {
    var urlImage = item.image ? item.image : '/Content/Images/singer.png';

    var markup = "<div><img width='24px'" + "src='" + urlImage + "'/><span style='margin-left:5px; text-align: center'>" + item.text + "</span></div>";
    return markup;
}

function searchFormatSelection(item) {
    var urlImage = item.image ? item.image : '/Content/Images/search.png';

    var artistName = item.artist ? item.artist + ' - ' : '';

    return "<div><img style='margin-top:2px;'  width='24px' height='24px'" + "src='" + urlImage + "'/><span style='margin-left:10px; text-align: center'>" + artistName + item.text + "</span></div>";
}



//
// Station utils

function replace_links_on_lastfm() {
    // Добавляем к ссылке 'подробнее на ласт.фм' атрибут target="_blank"
    var links = $('.wiki-text > a');

    if (links.length > 0) {
        var more_on_lastfm_link = links[links.length - 1];
        $(more_on_lastfm_link).attr("target", '_blank');
        $(more_on_lastfm_link).text("Узнать больше о " + session.current_artist + " на last.fm");
    }
}

function go_url(url) {
    window.open(url, '_blank');
};

function replace_image_src (replace_from, replace_to, control_id) {
    $(control_id).attr('src', $(control_id).attr('src').replace(replace_from, replace_to));
}

function goto_station(stationId) {
    window.location.href = '/Station/Index/' + stationId;
}