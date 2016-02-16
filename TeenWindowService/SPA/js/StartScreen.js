var StartScreenClass = function () {
    this.DivId = null;
};

StartScreenClass.prototype = function () {

    var Init = function()
    {
        this.DivId = "StartScreen";
    };

    var HideScreen = function()
    {

    };

    var ShowScreen = function()
    {

    };

    return {
        Init: Init,
        HideScreen: HideScreen,
        ShowScreen: ShowScreen     
    };
}();

var SelectionScreen = new SelectionScreenClass();