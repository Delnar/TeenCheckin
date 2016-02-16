
var AppClass = function () {
    this.ScreenList = new Array();
    this.ScreenIds = new Array();
};

AppClass.prototype = new function () {
    
    var Init = function() {
        App.ScreenIds.forEach(function(entry) {
            App.ScreenList[entry] = new ScreenClass(entry);
        });
    }

    var Start = function()
    {
        confirmBackspaceNavigations();
        DataManager.Init();
        DataManager.CreateTables();

        App.ScreenIds.forEach(function (entry) {
            App.ScreenList[entry].Init(App.ScreenList[entry]);
        });
    }

    return {
        Start: Start,
        Init: Init
    };

}();

var App = new AppClass();

App.ScreenIds = ["StartScreen", "EntryForm", "ListEntries","RandomData"];
App.Init();

// App.ScreenList["EntryForm"].Start = true;
App.ScreenList["ListEntries"].Start = true;
App.ScreenList["StartScreen"].InitFunc = function(e)
{
    $("#Enter").on("click", null, e, function(e)
    {
        e.data.HideScreen(e.data);
        App.ScreenList["FirstScreen"].ShowScreen(App.ScreenList["FirstScreen"]);
    })
}

App.ScreenList["EntryForm"].InitFunc = function (e)
{
    if (typeof EntryFormScreen === "undefined") return; // Not been defined.. 
    EntryFormScreen.Init(e);
}

App.ScreenList["ListEntries"].InitFunc = function (e) {
    if (typeof ListEntriesScreen === "undefined") return; // Not been defined.. 
    ListEntriesScreen.Init(e);
}

App.ScreenList["RandomData"].InitFunc = function (e) {
    if (typeof RandomDataScreen === "undefined") return; // Not been defined.. 
    RandomDataScreen.Init(e);
}

$(document).ready(App.Start);

function confirmBackspaceNavigations() {
    var lastUserInputWasBackspace = false
    $(document).keydown(function (event) {
        lastUserInputWasBackspace = event.which == 8 ? true : false
    })
    $(document).mousedown(function () {
        lastUserInputWasBackspace = false
    })
    $(window).on('beforeunload', function () {
        if (lastUserInputWasBackspace) {
            lastUserInputWasBackspace = false;
            return "Are you sure you want to leave this page?"
        }
    })
}