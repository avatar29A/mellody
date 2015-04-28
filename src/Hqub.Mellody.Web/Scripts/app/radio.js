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

    //
    // Function for working with query list

    // Send request to valid syntax your query.
    this.checkQuery = function() {
        alert("selected type query: " + this.typeQuery());
    }

    // 
    // Copy text query
    this.copyQuery = function(query) {
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

    //====================================================================

    //
    // Send request on create music station
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

