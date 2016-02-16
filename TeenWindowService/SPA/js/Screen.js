var ScreenClass = function (divId) {
    this.divId = divId;
    this.Start = false;
    this.InitFunc = null;
};

ScreenClass.prototype = function () {

    var Init = function (e) {
        if(!e.Start) 
            HideScreen(e);

        if(e.InitFunc) e.InitFunc(e);
    }

    var HideScreen = function(e)  {
        $("#" + e.divId).hide();
    }

    var ShowScreen = function(e)  {
        $("#" + e.divId).show();
    }

    return  {
        Init: Init,
        HideScreen: HideScreen,
        ShowScreen: ShowScreen       
    };
}();

