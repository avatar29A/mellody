var Player;
var JPlayer;

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

function InitialJPlayer() {

    var solution = "flash";

    JPlayer = $("#jquery_jplayer_1").jPlayer({
        ready: function (event) {

        },
        solution: solution,
        swfPath: "/Scripts/jplayer/Jplayer.swf",
        supplied: "mp3, oga",
        oggSupport: true,
        nativeSupport: true,
        preload: true,
        error: function (event) {
            console.log(event.jPlayer.error);
            console.log(event.jPlayer.error.type);

            if (vm)
                vm.nextTrack();

        },
        ended: function () {
            if (vm)
                vm.nextTrack();
        }
    });
}

function YoutubePlayer() {
    this.name = "Youtube";

    this.isPlaying = function() {
        return Player.getPlayerState() == YT.PlayerState.PLAYING;
    }

    this.cue = function(track) {
        Player.cuePlaylist([track.ExternalId]);
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
    this.name = "Vkontakte";
    this.vol = 0;
    this.isMuted = false;
    var self = this;
    

    this.isPlaying = function () {
        return Player.getPlayerState() == YT.PlayerState.PLAYING;
    }

    this.cue = function (track) {
        var stream = {
            title: '',
            mp3: track.Url
        };

        $(JPlayer).jPlayer("setMedia", stream);
        self.play();
    }

    this.pause = function () {
        $(JPlayer).jPlayer('pause');
    }

    this.play = function () {
        $(JPlayer).jPlayer("play");
    }

    this.setVolume = function (val) {
        self.unmute();
        self.vol = val / 100;
        $(JPlayer).jPlayer("volume", self.vol);
    }

    this.mute = function () {
        self.isMuted = true;
        self.setVolume(0);
    }

    this.unmute = function () {
        self.isMuted = false;
        self.setVolume(self.vol);
    }

    this.isMuted = function () {
        return self.isMuted;
    }
}