var MAX_TRY_COUNT = 20;

var MusicSearchInputViewModel = function(controlId, addCallback) {
    var self = this;

    this.add = addCallback;
    this.controlId = controlId;
    this.currentType = '';


    this.init = function (currentType) {

        self.currentType = currentType;
        var apiMethod = '';
        if (self.currentType == 'Artist') {
            apiMethod = 'artist.search';
        } else if (self.currentType == 'Album') {
            apiMethod = 'album.search';
        } else {
            apiMethod = 'track.search';
        }

        //Настраиваем редактор запросов: music-search-input
        var control = $(self.controlId);
        control.select2({
            minimumInputLength: 3,
            placeholder: "Example: Ozzy Osbourne",
            ajax: {
                url: "http://ws.audioscrobbler.com/2.0/?method=" + apiMethod,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        artist: params.term, // search term
                        album: params.term,
                        track: params.term,
                        api_key: '41c8ac8ec4db9fd204021a72a9469b8b',
                        format: 'json',
                        limit: 10
                    };
                },
                processResults: function (data) {
                    var r = undefined;
                    if (self.currentType == 'Artist') {
                        r = data.results.artistmatches.artist;
                    } else if (self.currentType == 'Album') {
                        r = data.results.albummatches.album;
                    } else {
                        r = data.results.trackmatches.track;
                    }

                    return {
                        results: self.searchProcess(r)
                    }
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; },
            templateResult: searchFormatResult,
            templateSelection: searchFormatSelection
        });

        this.searchProcess = function (entities) {

            return $(entities).map(function (idx, entity) {
                var image = '';
                if (entity.image && entity.image.length > 0) {
                    image = entity.image[0]['#text'];
                } else {
                    image = '/Content/Images/note.png';
                }

                if (!entity.mbid)
                    entity.mbid = guid();

                return { id: entity.mbid, text: entity.name, image: image, artist: entity.artist }
            });
        }

        control.on("select2:select", function (e) {
            self.add(e.params.data);
        });
    }
}

var PlaylistViewModel = function () {
    var self = this;

    this.isExecute = ko.observable(false);
    this.itemToAdd = ko.observable({});
    this.typeQuery = ko.observable("Artist");

    this.mediaQueriesSupported = ko.observableArray([]);
    this.historyStations = ko.observableArray([]);
    this.musicSearchInputVM = new MusicSearchInputViewModel('.query-input', self.itemToAdd);

    // METHODS

    this.initialize = function() {
        self.getHistoryStations();
        self.musicSearchInputVM.init(self.typeQuery());
    }

    this.addQuery = function () {
        if ((this.itemToAdd() != {}) && (this.mediaQueriesSupported.indexOf(this.itemToAdd()) < 0)) { // Prevent blanks and duplicates

            if (this.mediaQueriesSupported().length == 5) {
                Show("you reached limit in beta version", "Number of requests is limited to five.");
                return;
            }

            var name;
            if (self.typeQuery() == 'Album' || self.typeQuery() == 'Track') {
                name = self.itemToAdd().artist + ' - ' + self.itemToAdd().text;
            }else {
                name = self.itemToAdd().text;
            }

            this.mediaQueriesSupported.push({
                name: name,
                typeQuery: self.typeQuery(),
                mbid: self.itemToAdd().id,
                image: self.itemToAdd().image
            });
        }
        self.itemToAdd({});
        self.clearInputField();
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

    this.typeQueryChanged = function (data, event) {
        self.musicSearchInputVM.init(self.typeQuery());
        return true;
    }

    //
    // Function for working with query list

    // Send request to valid syntax your query.
    this.checkQuery = function() {
        alert("selected type query: " + this.typeQuery());
    }

    this.clearInputField = function() {
        $('.query-input').val(null).trigger('change');
    }

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

    this.play_station = function (data, event) {
        goto_station(data.Id);
    }

    this.getHistoryStations = function() {
        get('/Station/GetHistoryStations', function(data) {
            if (data.IsError) {
                alert(data.Error);
                return;
            }

            self.historyStations(data.Stations);
        });
    }

    //.ctor
    this.initialize();
}

//
// Station View Model
// 

var StationViewModel = function(player) {
    var self = this;
    this.isExecute = ko.observable(true);
    this.player = player;

    this.currentTrack = ko.observable({});
    this.playlist = ko.observable({});

    this.trycounter = 0;

    this.notifySubscribers = function() {
        // Настраиваем регулятор громкости:
        $("#volume_slider").on("changed", function (e, val) {
            self.volume(val);
        });
    }

    this.notifySubscribers();

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

        $('#info-block').fadeOut('slow', function() {

            self.isExecute(true);

            CreateStation([query], RunStation, DefaultErrorHandle, function() {
                self.isExecute(false);
                $('#info-block').fadeIn('slow');
            });
        });
    }

    this.load_playlist = function () {
        $('#info-block').fadeOut('slow', function () {

            self.isExecute(true);

            get('/Station/Get/?id=' + station_id + '&source=' + self.player.name, function (data) {
                if (data.IsError || data.length == 0) {
                    alert("Station is empty :(");
                    return;
                }

                self.playlist(data);
                self.run(data.Tracks);
            });
        });
    }

    this.run = function (tracks) {
        var track = tracks[0];

        if (!track || (track.ExternalId == "" && player.name === "Youtube") ||
                        (track.Url == "" && player.name === "Vkontakte")) {
            if (++self.trycounter >= MAX_TRY_COUNT) {
                alert("I can't find tracks for this station. Please try another station.");
                window.location = '/Playlist';
                return;
            }
            self.nextTrack();
            return;
        }

        self.currentTrack(track);
        self.player.cue(track);

        self.isExecute(false);

        $('#info-block').fadeIn('slow');
    }

    this.play_track = function(data) {
        self.run([data]);
    }

    this.play_station = function (data) {
        goto_station(data.Id);
    }

    //
    // Volume functions

    this.volume_on = function() {
        self.volume(100);
    }

    this.volume_off = function() {
        if (self.player.isMuted()) {
            self.player.unmute();
            return;
        }

        Player.mute();
    }

    this.volume = function(volume) {
        self.player.setVolume(volume);
        $('#volume_slider').slider("value", volume);
    }

    // ~ Volum functions

    this.pause_play = function() {
        if (self.player.isPlaying()) {
            replace_image_src('pause_white.png', 'play_white.png', '.play-pause-control');
            self.player.pause();
        } else {
            replace_image_src('play_white.png', 'pause_white.png', '.play-pause-control');
            self.player.play();
        }
    }

    this.playerAgentRunning = false;
    this.onPlayerStateChange = function (event) {
       if (event.data == YT.PlayerState.CUED) {
            self.pause_play(); 
        } else if (event.data == YT.PlayerState.ENDED) {
            self.nextTrack();
        } else if (event.data == -1) {
            if (!self.playerAgentRunning) {
                setTimeout(function () {
                    self.playerAgentRunning = false;
                    if (Player.getPlayerState() == -1) {
                        self.nextTrack();
                    }
                }, 10000);
            }
        }
    }

    this.nextTrack = function () {
        self.player.pause();
        self.load_playlist();
    }

    this.changePlayer = function (playerName) {
        self.player.pause();
        if (playerName == 'Vkontakte') {
            self.player = new VkPlayer();
        }else {
            self.player = new YoutubePlayer();
        }

        self.load_playlist();
    }
}

//
// Init knockout.js
//

var vm = null;

function InitlalizePlaylistVM() {
    vm = new PlaylistViewModel();
    ko.applyBindings(vm);
}

function InitlalizeStationVM(player) {
    vm = new StationViewModel(player);
    ko.applyBindings(vm);
}