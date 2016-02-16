var ListEntriesClass = function () {
    this.ExtraMessage = "Welcome";
    this.InverseSearch = false;
};

ListEntriesClass.prototype = function () {

    var Init = function (e) {
        DataManager.ReadAllRecords(function (list) {
            BuildList(list);
        });

        $("#Filter").on('keyup change', function () {
            ListEntriesScreen.FilterNames();
        });

        $("#FilterAll").on("click", null, null, function () {
            var FilterState = $(this).attr("data-checkinfilter");
            FilterState = FilterState === "Y" ? FilterState = "N" : FilterState = "Y";
            $(this).attr("data-checkinfilter", FilterState);
            ListEntriesScreen.FilterNames();
        });

        $("#Filter01").on("click", null, null, function () {
            var FilterState = $(this).attr("data-bus01");
            FilterState = FilterState === "Y" ? FilterState = "N" : FilterState = "Y";
            $(this).attr("data-bus01", FilterState);
            ListEntriesScreen.FilterNames();
        });

        $("#Filter02").on("click", null, null, function () {
            var FilterState = $(this).attr("data-bus02");
            FilterState = FilterState === "Y" ? FilterState = "N" : FilterState = "Y";
            $(this).attr("data-bus02", FilterState);
            ListEntriesScreen.FilterNames();
        });

        $("#Filter03").on("click", null, null, function () {
            var FilterState = $(this).attr("data-bus03");
            FilterState = FilterState === "Y" ? FilterState = "N" : FilterState = "Y";
            $(this).attr("data-bus03", FilterState);
            ListEntriesScreen.FilterNames();
        });

        $("#Filter04").on("click", null, null, function () {
            var FilterState = $(this).attr("data-car01");
            FilterState = FilterState === "Y" ? FilterState = "N" : FilterState = "Y";
            $(this).attr("data-car01", FilterState);
            ListEntriesScreen.FilterNames();
        });

        $("#newbut").on("click", null, null, function () {
            var SearchPattern = $("#Filter").val().toUpperCase();

            if (SearchPattern === "RANDOM") {
                RandomDataScreen.PopulateRandomData();
                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                return;
            }
            if (SearchPattern === "RANDOM10") {
                for (x = 0; x < 10; x++)
                {
                    RandomDataScreen.PopulateRandomData();
                }
                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                return;
            }
            if (SearchPattern === "RANDOM100") {
                for (x = 0; x < 100; x++) {
                    RandomDataScreen.PopulateRandomData();
                }
                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                return;
            }

            if (SearchPattern === "CLEAR") {
                DataManager.ClearData(function () { setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1); })
                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                return;
            }

            if (SearchPattern === "STATUS") {
                if (DataManager.UseWebSQL) alert("Using Web SQL"); else alert("Using IndexDB")
                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                return;
            }

            if (SearchPattern === "DESKTOP") {
                // var DesktopUrl = "desktop/TeenCheckin.application?SERV=delnar101.cloudapp.net";
                var DesktopUrl = "desktop/TeenCheckin.application?SERV=James-PC";
                window.open(DesktopUrl, '_blank');
                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                return;
            }

            ListEntriesScreen.HideScreen();
            EntryFormScreen.New(function () {
                EntryFormScreen.ShowScreen();
            });
            
        });

        var SyncSpinExecuting = false;
        var SyncAngle = 0;

        $("#SyncObject").on("click", null, null, function () {
            if (SyncSpinExecuting === true) {
                return;
            }
            SyncSpinExecuting = true;            

            setTimeout(function SpinIt() {
                var SyncObject = $("#SyncObject");
                var SyncValue = "rotate(" + SyncAngle + "deg)";
                SyncObject.css("transform", SyncValue);
                SyncObject.css("-moz-transform", SyncValue);
                SyncObject.css("-ms-transform", SyncValue);
                SyncObject.css("-webkit-transform", SyncValue);
                SyncObject.css("-o-transform", SyncValue);
                SyncAngle += 1;
                if (SyncAngle > 359) SyncAngle = SyncAngle - 359;
                if (SyncSpinExecuting === true) {
                    setTimeout(SpinIt, 1);
                }                
            }, 1);

            // Sync Process
            // 1.  Check for access to Service.  If fail Stop
            ServerComm.ServerAvailable(function (e) {  // Success for Server Comm
                console.log(e);
                if (e !== "GOOD") return; // Nothing to do unless the server says "GOOD";
                // 2.  Grab all dirty records
                DataManager.ReadDirtyRecords(function (ret) {
                    // 3.  Upload Dirty Records to server
                    ServerComm.PostDataToServer(ret, function () {  // Success to Post
                        ServerComm.GetDataFromServer(function (list) {
                            DataManager.ClearData();

                            if(list.length <= 0) 
                            {
                                setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1);
                                return;
                            }

                            var cnt = 0;

                            DataManager.WriteRecord(list[cnt], "N", function c() {
                                cnt += 1;
                                if (list.length <= cnt) {
                                    SyncSpinExecuting = false;
                                    setTimeout(function ()
                                    {
                                        ListEntriesScreen.ExtraMessage = "Sync Complete";
                                        ListEntriesScreen.ShowScreen();
                                    }, 1);
                                    return;
                                }
                                DataManager.WriteRecord(list[cnt], "N", c);
                            });

                        }, function () {
                            console.log("Failed to Read From Server.. :(")
                            SyncSpinExecuting = false;
                        });
                    }, function () {  // Failed to Post
                        console.log("Failed to Post To server.. :(")
                        SyncSpinExecuting = false;
                    });
                });
            }, 
            function () {  // Failed to Server Comm
                console.log("Failed to talk with server.. :(")
                SyncSpinExecuting = false;
            });
            // 3.  Upload Dirty Records to server
            // 4.  Request data refresh.
            // 5.  Rebuild Screen
            // 6.  Stop Spinner
            

        });

    };

    var FilterNames = function () {
        console.log("Filtering Names");
        var SearchPattern = $("#Filter").val().toUpperCase();
        var CheckInFilterState = $('#FilterAll').attr("data-checkinfilter");
        var Bus01FilterState = $('#Filter01').attr("data-bus01");
        var Bus02FilterState = $('#Filter02').attr("data-bus02");
        var Bus03FilterState = $('#Filter03').attr("data-bus03");
        var Car01FilterState = $('#Filter04').attr("data-car01");
        var RecCount = 0;
        var UseBusFilter = false;
        if (Bus01FilterState === 'Y') UseBusFilter = true;
        if (Bus02FilterState === 'Y') UseBusFilter = true;
        if (Bus03FilterState === 'Y') UseBusFilter = true;
        if (Car01FilterState === 'Y') UseBusFilter = true;

        // Need to lookup inverse first
        var InversePattern = false;
        if (SearchPattern === "NOT") { SearchPattern = ""; InversePattern = true; }

        $("#ListItems a").each(function (i, e) {

            var itemCheckedIn = $(e).find("[data-checkedin]").attr("data-checkedin")
            var itemBus = $(e).find("[data-bus]").attr("data-bus")

            var itemCheckInLookup = 'N';

            if (InversePattern)
            {
                if ((CheckInFilterState === 'Y') && (itemCheckedIn !== itemCheckInLookup)) {
                    $(this).hide();
                    return;
                }
            }
            else
            {
                if ((CheckInFilterState === 'Y') && (itemCheckedIn === itemCheckInLookup)) {
                    $(this).hide();
                    return;
                }
            }

            if (UseBusFilter) {
                var BusFilter = false;
                if ((Bus01FilterState === 'Y') && (itemBus === '1')) BusFilter = true;
                if ((Bus02FilterState === 'Y') && (itemBus === '2')) BusFilter = true;
                if ((Bus03FilterState === 'Y') && (itemBus === '3')) BusFilter = true;
                if ((Car01FilterState === 'Y') && (itemBus === 'D')) BusFilter = true;

                if (BusFilter === false) {
                    $(this).hide();
                    return;
                }
            }

            ListEntriesScreen.InverseSearch = false;
            if (SearchPattern === "RANDOM")  { SearchPattern = ""; }
            if (SearchPattern === "RANDOM10") { SearchPattern = ""; }
            if (SearchPattern === "RANDOM100") { SearchPattern = ""; }
            if (SearchPattern === "CLEAR") { SearchPattern = ""; }
            if (SearchPattern === "STATUS") { SearchPattern = ""; }
            if (SearchPattern === "DESKTOP") { SearchPattern = ""; }
            

            if (SearchPattern === "") {
                $(this).show();
                RecCount = RecCount + 1;
                return;
            }

            var SearchText = $(this).html().toUpperCase();

            var comp = SearchText.indexOf(SearchPattern);

            if (comp >= 0) {
                $(this).show();
                RecCount = RecCount + 1;
            }
            else {
                $(this).hide();
            }
        });
        $("#FilterCount").text(RecCount + " records. " + ListEntriesScreen.ExtraMessage);
        ListEntriesScreen.ExtraMessage = "";
    }

    var HideScreen = function () {
        $("#ListEntries").hide();
        // setTimeout(function () { ListEntriesScreen.ShowScreen(); }, 1000);
    };

    
    var ShowScreenFlag = false;
    var ShowScreen = function () {
        DataManager.ReadAllRecords(function (list) {
            BuildList(list);
            $("#ListEntries").show();
        });
    };

    var BuildList = function (list) {

        var DayOfWeek = "";

        var weekdays = new Array(7);
        weekdays[0] = "Sunday";
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";

        var current_date = new Date();

        var weekday_value = current_date.getDay();
        var year_value = current_date.getYear();

        DayOfWeek = weekdays[weekday_value];
        $("#ListItems").empty();
        for (var x = 0; x < list.length; x++) {
            if (list[x].DisplayText === undefined) continue;
            if (list[x].DisplayText === "") continue;

            var item = $("<a href='#'></a>");
            item.addClass("list-group-item")
            item.on("click", null, null, function (e) {
                if ($(this).hasClass("active")) return; // Nothing to do if it's already active..
                $("#ListEntries .active .list-group-item-text").hide();  // hide the text before moving acitve..
                $("#ListEntries .active").removeClass("active");
                $(this).addClass("active");
                $(this).find(".list-group-item-text").show();
                return false;
            });

            var display = $("<h4></h4>").text(list[x].DisplayText);
            display.addClass("list-group-item-heading");
            item.append(display);

            var CheckInBox = $("<div></div>");
            CheckInBox.attr("title", DayOfWeek);
            CheckInBox.attr("data-id", list[x].Value);
            CheckInBox.attr("data-weekday", weekday_value);
            CheckInBox.attr("data-year", year_value);
            CheckInBox.attr("data-checkedin", list[x].CheckIn);
            CheckInBox.attr("data-bus", list[x].Bus);
            
            CheckInBox.on("click", null, null, function (e) {
                var privateId = $(this).attr("data-id");
                var dayOfWeek = $(this).attr("data-weekday");
                var year = $(this).attr("data-year");
                var CheckedIn = $(this).attr("data-checkedin");
                var CallingObject = $(this);               
                CheckedIn = (CheckedIn === 'Y') ? CheckedIn = 'N' : CheckedIn = 'Y';                
                DataManager.CheckInTeen(privateId, CheckedIn, function () {
                    CallingObject.attr("data-checkedin", CheckedIn);
                    ListEntriesScreen.ExtraMessage = "Update Recorded";
                    ListEntriesScreen.ResetFilters();
                });
                return false;
            });



            var EditBox = $("<div></div>");
            EditBox.attr("data-edit", "Y");
            EditBox.attr("data-id", list[x].Value);
            EditBox.on("click", null, null, function (e) {
                ListEntriesScreen.HideScreen();
                var privateId = $(this).attr("data-id");
                EntryFormScreen.Load(privateId, function () {
                    EntryFormScreen.ShowScreen();
                });                
            });

            var secondaryDisplay = $("<div></div>");
            secondaryDisplay.addClass("list-group-item-text");
            secondaryDisplay.addClass("secondaryDisplay")
            secondaryDisplay.append(CheckInBox);
            secondaryDisplay.append(EditBox);
            secondaryDisplay.hide();
            item.append(secondaryDisplay);

            $("#ListItems").append(item)
        }

        ListEntriesScreen.ResetFilters();
    };

    var ResetFilters = function()
    {
        $("#Filter").val("");

        $('#FilterAll').attr("data-checkinfilter", "N");
        
        $('#Filter01').attr("data-bus01", "N");
        $('#Filter02').attr("data-bus02", "N");
        $('#Filter03').attr("data-bus03", "N");
        $('#Filter04').attr("data-car01", "N");
        ListEntriesScreen.FilterNames();
        console.log("Reseting Filters");
    }

    return {
        Init: Init,
        HideScreen: HideScreen,
        ShowScreen: ShowScreen,
        BuildList: BuildList,
        FilterNames: FilterNames,
        ResetFilters: ResetFilters
    };
}();

var ListEntriesScreen = new ListEntriesClass();
