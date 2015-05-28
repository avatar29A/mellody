var PlaylistViewModel = function () {
    var self = this;

    this.isExecute = ko.observable(false);

    this.itemToAdd = ko.observable("");
    this.mediaQueriesSupported = ko.observableArray([]);

    this.typeQuery = ko.observable("Artist");

    this.addQuery = function () {
        if ((this.itemToAdd() != "") && (this.mediaQueriesSupported.indexOf(this.itemToAdd()) < 0)) { // Prevent blanks and duplicates

            if (this.mediaQueriesSupported().length == 5) {
                Show("you reached limit in beta version", "Number of requests is limited to five.");
                return;
            }

            this.mediaQueriesSupported.push({
                name: this.itemToAdd(),
                typeQuery: this.typeQuery()
            });
        }
        this.itemToAdd("");
    };

    this.placeholder = ko.computed(function() {
        if (self.typeQuery() == 'Artist') {
            return "Example: Ozzy Osbourne";
        }

        if (self.typeQuery() == 'Genre') {
            return "Example: rock";
        }
        else {
            return "Example: Black Sabbath - Paranoid";
        }
    });

    //
    // Function for working with query list

    // Send request to valid syntax your query.
    this.checkQuery = function() {
        alert("selected type query: " + this.typeQuery());
    }

    // 
    // Copy text query
    this.copyQuery = function (query) {
        self.typeQuery(query.typeQuery);
        self.itemToAdd(query.name);
    };

    // Copy text query for edit. It is also remove current query.
    this.editQuery = function(query) {
        self.copyQuery(query);
        self.removeQuery(query);
    };

    // Remove query
    this.removeQuery = function (query) {
        self.mediaQueriesSupported.remove(query);
    };

    this.setWait = function(val) {
        self.isExecute(val);
    };

    //====================================================================

    //
    // Send request on create music station
    this.runStation = function () {
        if (self.isExecute())
            return;

        self.setWait(true);

        CreateStation(self.mediaQueriesSupported(), RunStation, DefaultErrorHandle, function() { self.setWait(false); });
    }
}

var StationViewModel = function() {
    var self = this;
   
    this.isExecute = ko.observable(false);
    this.isBusy = ko.observable(true);

    this.currentTrack = ko.observable({});
    this.playlist = ko.observable({});

    this.notifySubscribers = function() {
        // Настраиваем регулятор громкости:
        $("#volume_slider").on("changed", function (e, val) {
            self.volume(val);
        });
    }

    this.notifySubscribers();

    this.TestClick = function() {
        self.isBusy(false);
    }

    this.create_station_by_similar_artist = function (artist) {
        var selectedSimilarArtist = {
            name: artist.ArtistName,
            typeQuery: "Artist"
        };

        self.create_station(selectedSimilarArtist);
    }

    this.create_station_by_genre = function(genre) {
        var selectedGenre = {
            name: genre,
            typeQuery: "Genre"
        };

        self.create_station(selectedGenre);
    }

    this.create_station = function(query) {
        if (self.isExecute())
            return;

        self.isExecute(true);

        CreateStation([query], RunStation, DefaultErrorHandle, function () { self.isExecute(false); });
    }

    this.load_playlist = function () {
        $('#info-block').fadeOut('slow', function() {
            get('/Station/Get/' + station_id, function (data) {
                if (data.IsError || data.length == 0) {
                    alert("Station is empty :(");
                    return;
                }

                self.playlist(data);
                self.play(data.Tracks);

                $('#info-block').fadeIn('slow');
            });
        });
    }

    // play current playlist:
    this.play = function(tracks) {
        var videos = $(tracks).map(function() {
            return this.VideoId;
        }).get();

        self.setCurrentTrack(tracks[0]);
        Player.cuePlaylist(videos);
    }

    this.play_track = function(data, event) {
        self.play([data]);
    }

    this.play_station = function (data, event) {
        window.location.href = '/Station/Index/' + data.Id;
    }

    this.volume_on = function() {
        self.volume(100);
    }

    this.volume_off = function() {
        if (Player.isMuted()) {
            Player.unMute();
            return;
        }

        Player.mute();
    }

    this.volume = function(volume) {
        Player.unMute();
        Player.setVolume(volume);
        $('#volume_slider').slider("value", volume);
    }

    this.pause_play = function() {
        if (Player.getPlayerState() == YT.PlayerState.PLAYING) {
            replace_image_src('pause_white.png', 'play_white.png', '.play-pause-control');
            Player.pauseVideo();
        } else {
            replace_image_src('play_white.png', 'pause_white.png', '.play-pause-control');
            Player.playVideo();
        }
    }

    this.like = function() {
        alert("For work this button need login.");
    }

    this.ban = function() {
        alert("For work this button need login.");
    }

    this.setCurrentTrack = function(track) {
        self.currentTrack(track);
    }

    this.onPlayerStateChange = function (event) {
        console.log('PlayerStateChange = ' + event.data);

        if (event.data == YT.PlayerState.CUED) {
            self.pause_play(); 
        } else if (event.data == YT.PlayerState.ENDED) {
            self.load_playlist();
        } else if (event.data == YT.PlayerState.PLAYING) {

        }
    }

    this.nextTrack = function() {
        self.load_playlist();
    }
}

var vm = null;

function InitlalizePlaylistVM() {
    vm = new PlaylistViewModel();
    ko.applyBindings(vm);
}

function InitlalizeStationVM() {
    vm = new StationViewModel();
    ko.applyBindings(vm);
}

