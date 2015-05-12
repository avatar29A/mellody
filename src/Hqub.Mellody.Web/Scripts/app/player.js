var Player;

function GetDefaultPlayerSettings() {
    return {
        width: '700',
        height: '390',
        loop: 0,
        showinfo:0,
        playerVars: {
            controls: 0
        }
    };
}

function InitalYoutubePlayer(settings, events) {
    if (!events)
        events = {};

    if (!settings)
        settings = GetDefaultPlayerSettings();
        

    Player = new YT.Player('player', {
        height: settings.height,
        width: settings.width,
        loop: settings.loops,
        playerVars: settings.playerVars,
        events: events
    });

    return Player;
}