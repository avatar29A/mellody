var RadioViewModel = function () {
    var self = this;

    this.itemToAdd = ko.observable("");
    this.mediaQueriesSupported = ko.observableArray([]);

    this.typeQuery = ko.observable("Artist");

    this.addQuery = function () {
        if ((this.itemToAdd() != "") && (this.mediaQueriesSupported.indexOf(this.itemToAdd()) < 0)) // Prevent blanks and duplicates
            this.mediaQueriesSupported.push({
                name: this.itemToAdd(),
                typeQuery: this.typeQuery()
            });
        this.itemToAdd("");
    };

    this.checkQuery = function() {
        alert("selected type query: " + this.typeQuery());
    }

    this.copyQuery = function(query) {
        self.itemToAdd(query.name);
    };

    this.editQuery = function(query) {
        self.copyQuery(query);
        self.removeQuery(query);
    };

    this.removeQuery = function (query) {
        self.mediaQueriesSupported.remove(query);
    };

    this.runStation = function() {
        executeOnServer(new RadioDTO(self.mediaQueriesSupported()), '/Music/Index/', function (data) {
            console.log(data);
        });
    }
}

function InitlalizeRadioVM() {
    var vm = new RadioViewModel();
    ko.applyBindings(vm);
}

