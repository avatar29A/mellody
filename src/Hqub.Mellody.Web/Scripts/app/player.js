var Player;

function GetDefaultPlayerSettings() {
    return {
        width: '850',
        height: '450',
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

function YoutubePlayer() {
 
    this.isPlaying = function() {
        return Player.getPlayerState() == YT.PlayerState.PLAYING;
    }

    this.cue = function(tracks) {
        Player.cuePlaylist(tracks);
    }

    this.pause = function() {
        Player.pauseVideo();
    }

    this.play = function() {
        Player.playVideo();
    }

    this.setVolume = function(val) {
        Player.unMute();
        Player.setVolume(val);
    }

    this.mute = function() {
        Player.mute();
    }

    this.unmute = function() {
        Player.unMute();
    }

    this.isMuted = function() {
        return Player.isMuted();
    }
}

function VkPlayer() {
    this.isPlaying = function () {
        return Player.getPlayerState() == YT.PlayerState.PLAYING;
    }

    this.cue = function (tracks) {
        Player.cuePlaylist(tracks);
    }

    this.pause = function () {
        Player.pauseVideo();
    }

    this.play = function () {
        Player.playVideo();
    }

    this.setVolume = function (val) {
        Player.unMute();
        Player.setVolume(val);
    }

    this.mute = function () {
        Player.mute();
    }

    this.unmute = function () {
        Player.unMute();
    }

    this.isMuted = function () {
        return Player.isMuted();
    }
}