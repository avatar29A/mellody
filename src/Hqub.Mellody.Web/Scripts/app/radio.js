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
        } else {
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
        executeOnServer(new RadioDTO(self.mediaQueriesSupported()), '/Playlist/Index', function (data) {
            self.setWait(false);
            if (data.IsError) {
                console.log("Error: " + data.Message + " (" + data.statusCode + ")");
                Show("error", data.Message);
                return;
            }

            window.location.href = '/Station/Index/' + data.StationId;
        });
    }
}

var StationViewModel = function() {
    var self = this;

    this.load_playlist = function() {
        get('/Station/Get/' + station_id, function (data) {
            if (data.IsError || data.length == 0) {
                alert("Station is empty :(");
                return;
            }

            self.play(data);
        });
    }

    // play current playlist:
    this.play = function (tracks) {
        var videos = $(tracks).map(function() {
            return this.VideoId;
        }).get();

        Player.cuePlaylist(videos);
    }

    this.onPlayerStateChange = function (event) {
        if (event.data == YT.PlayerState.CUED) {
            Player.playVideo();

        }else if (event.data == YT.PlayerState.ENDED) {
            self.load_playlist();
        }
    }

    this.nextTrack = function () {
       
    }

}

var vm = null;

function InitlalizePlaylistVM() {
    vm = new PlaylistViewModel();
    ko.applyBindings(vm);
}

function InitlalizeStationVM() {
    vm = new StationViewModel();
    vm.load_playlist();
    ko.applyBindings(vm);
}

