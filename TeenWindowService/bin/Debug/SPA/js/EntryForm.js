var EntryFormClass = function () {
    this.DivId = null;
};

EntryFormClass.prototype = function () {

    var Init = function (e) {
        this.DivId = "EntryForm";
        $("#ef_NoPhone").on("click", null, e, function (e) {
            EntryFormScreen.NoPhone();
        });

        $("#ef_noAddress").on("click", null, e, function (e) {
            EntryFormScreen.NoAddress();
        });

        $("#ef_save").on("click", null, e, function () {
            EntryFormScreen.Save();
        });

        $("#ef_cancel").on("click", null, e, function () {
            EntryFormScreen.Cancel();
        });

        $("#ef_zip1").on("click", null, e, function () {
            $('#ef_city').val("Redding");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96001");
        });
        $("#ef_zip2").on("click", null, e, function () {
            $('#ef_city').val("Redding");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96002");
        });
        $("#ef_zip3").on("click", null, e, function () {
            $('#ef_city').val("Redding");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96003");
        });
        $("#ef_zip7").on("click", null, e, function () {
            $('#ef_city').val("Anderson");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96007");
        });
        $("#ef_zip19").on("click", null, e, function () {
            $('#ef_city').val("Shasta Lake City");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96019");
        });
        $("#ef_zip79").on("click", null, e, function () {
            $('#ef_city').val("Shasta Lake City");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96079");
        });
        $("#ef_zip89").on("click", null, e, function () {
            $('#ef_city').val("Shasta Lake City");
            $('#ef_state').val("CA");
            $('#ef_zip').val("96089");
        });

    };

    var HideScreen = function () {
        $("#EntryForm").hide();
    };

    var ShowScreen = function () {
        $("#EntryForm").show();
    };

    var ReadDataFromForm = function () {
        var ret = {};

        ret.PublicRecId = $('#ef_firstName').attr("data-publicid");            
        ret.PrivateRecId = $('#ef_firstName').attr("data-privateid");
        ret.FirstName = $('#ef_firstName').val();
        ret.LastName = $('#ef_lastName').val();
        ret.PhoneArea = $('#ef_phoneArea').val();
        ret.PhonePrefix = $('#ef_phonePrefix').val();
        ret.PhoneSuffix = $('#ef_phoneSuffix').val();
        ret.Street1 = $('#ef_street1').val();
        ret.Street2 = $('#ef_street2').val();
        ret.City = $('#ef_city').val();
        ret.State = $('#ef_state').val();
        ret.Zip = $('#ef_zip').val();
        // ret.Sex = $('#ef_sex').val();
        ret.Bus = "U";

        if ($("#ef_sex-m").prop("checked")) {
            ret.Sex = $("#ef_sex-m").val();
        }

        if ($("#ef_sex-f").prop("checked")) {
            ret.Sex = $("#ef_sex-f").val();
        }

        if ($('#ef_grade-6').prop("checked")) {
            ret.Grade = $('#ef_grade-6').val();
        }
        if ($('#ef_grade-7').prop("checked")) {
            ret.Grade = $('#ef_grade-7').val();
        }
        if ($('#ef_grade-8').prop("checked")) {
            ret.Grade = $('#ef_grade-8').val();
        }
        if ($('#ef_grade-9').prop("checked")) {
            ret.Grade = $('#ef_grade-9').val();
        }
        if ($('#ef_grade-10').prop("checked")) {
            ret.Grade = $('#ef_grade-10').val();
        }
        if ($('#ef_grade-11').prop("checked")) {
            ret.Grade = $('#ef_grade-11').val();
        }
        if ($('#ef_grade-12').prop("checked")) {
            ret.Grade = $('#ef_grade-12').val();
        }
        if ($('#ef_grade-U').prop("checked")) {
            ret.Grade = $('#ef_grade-U').val();
        }

        for (var x = 0; x < 4; x++) {
            if ($("#ef_bus-" + x).prop("checked")) {
                ret.Bus = $("#ef_bus-" + x).val();
            }
        }

        if ($("#ef_check-Y").prop("checked")) {
            ret.CheckIn = $("#ef_check-Y").val();
            ret.UpdateCheckin = 'Y';                
        }
        if ($("#ef_check-N").prop("checked")) {
            ret.CheckIn = $("#ef_check-N").val();
            ret.UpdateCheckin = 'Y';
        }

        // ret.Grade = $('#ef_grade').val();
        ret.GuestOf = $("#ef_guest").val();

        return ret;
    };

    var NoPhone = function () {
        $("#ef_phoneArea").val("530");
        $("#ef_phonePrefix").val("555");
        $("#ef_phoneSuffix").val("5555");
    };
    var NoAddress = function () {
        $("#ef_street1").val("UNKNOWN");
        $("#ef_street2").val("UNKNOWN");
        $("#ef_city").val("UNKNOWN");
        $("#ef_state").val("CA");
        $("#ef_zip").val("99999");
    };

    var Cancel = function () {
        EntryFormScreen.HideScreen();
        ListEntriesScreen.ShowScreen();
        // EntryFormScreen.AddErrorMessage("Hello Tester");
    };

    var Save = function () {
        var Data = EntryFormScreen.ReadDataFromForm();
        if (Data.FirstName == "") { EntryFormScreen.AddErrorMessage("First Name is required"); return; }
        if (Data.LastName == "") { EntryFormScreen.AddErrorMessage("Last Name is required"); return; }
        if (Data.PhoneArea == "") { EntryFormScreen.AddErrorMessage("Phone Area is required"); return; }
        if (Data.PhonePrefix == "") { EntryFormScreen.AddErrorMessage("Phone Prefix is required"); return; }
        if (Data.PhoneSuffix == "") { EntryFormScreen.AddErrorMessage("Phone Suffix is required"); return; }
        if (Data.Street1 == "") { EntryFormScreen.AddErrorMessage("First Street Line is required"); return; }
        if (Data.City == "") { EntryFormScreen.AddErrorMessage("City is required"); return; }
        if (Data.State == "") { EntryFormScreen.AddErrorMessage("State is required"); return; }
        if (Data.Zip == "") { EntryFormScreen.AddErrorMessage("Zip is required"); return; }

        DataManager.WriteRecord(Data, "Y", function () {            
            setTimeout(function ()
            {
                ListEntriesScreen.ShowScreen();
                EntryFormScreen.HideScreen();
            }, 10);
        });
    };

    var LoadGuestsOfCombo = function (TeenId, callback) {
        DataManager.ReadAllRecords(function (e) {
            $("#ef_guest").empty();
            var opt = $("<option></option>").attr("value", '').text("None");
            $("#ef_guest").append(opt);
            var PrivateId = $('#ef_firstName').attr("data-privateid");
            var GuestOf = $('#ef_firstName').attr("data-guestOf");
            $.each(e, function (key, rec) {
                if ((rec.Value == TeenId) && (PrivateId != -1)) return; // We don't want to add our selves to the list..
                var opt = $("<option></option>").attr("value", rec.GuestOfKey).text(rec.DisplayText);
                $("#ef_guest").append(opt);
            });
            callback();
        });
    };

    var New = function(CallBack)
    {
        EntryFormScreen.LoadGuestsOfCombo(-1, function () {
            $('#ef_firstName').attr("data-publicid", -1);
            $('#ef_firstName').attr("data-privateid", -1);
            $('#ef_firstName').attr("data-guestOf", '');
            $('#ef_firstName').val("");
            $('#ef_lastName').val("");
            $('#ef_phoneArea').val("");
            $('#ef_phonePrefix').val("");
            $('#ef_phoneSuffix').val("");
            $('#ef_street1').val("");
            $('#ef_street2').val("");
            $('#ef_city').val("");
            $('#ef_state').val("");
            $('#ef_zip').val("");
            $('#ef_sex').val("");
            $('#ef_grade').val("");
            $('#ef_guest').val(0);
            if (CallBack !== undefined) CallBack();
        });
    }

    var Load = function (PrivateId, CallBack) {
        EntryFormScreen.LoadGuestsOfCombo(PrivateId, function () {
            $('#ef_firstName').attr("data-publicid", -1);
            DataManager.ReadSingleRecord(PrivateId, function (e) {
                console.log(e);
                if (e == null) {
                    $('#ef_firstName').attr("data-publicid", -1);
                    $('#ef_firstName').attr("data-privateid", -1);
                    return;
                };

                $('#ef_firstName').attr("data-publicid", e.PublicRecId);
                $('#ef_firstName').attr("data-privateid", e.PrivateRecId);
                $('#ef_firstName').val(e.FirstName);
                $('#ef_lastName').val(e.LastName);
                $('#ef_phoneArea').val(e.PhoneArea);
                $('#ef_phonePrefix').val(e.PhonePrefix);
                $('#ef_phoneSuffix').val(e.PhoneSuffix);
                $('#ef_street1').val(e.Street1);
                $('#ef_street2').val(e.Street2);
                $('#ef_city').val(e.City);
                $('#ef_state').val(e.State);
                $('#ef_zip').val(e.Zip);
                // $('#ef_sex').val(e.Sex);
                $('#ef_grade').val(e.Grade);
                $('#ef_guest').val(e.GuestOf);

                switch(e.Sex)
                {
                    case "M":
                        $('#ef_sex-m').prop("checked", true);
                        $('#ef_sex-f').prop("checked", false);
                        break;
                    case "F":
                        $('#ef_sex-f').prop("checked", true);
                        $('#ef_sex-m').prop("checked", false);
                        break;
                }
                switch(e.Grade)
                {
                    case "6":
                        $('#ef_grade-6').prop("checked", true);
                        break;
                    case "7":
                        $('#ef_grade-7').prop("checked", true);
                        break;
                    case "8":
                        $('#ef_grade-8').prop("checked", true);
                        break;
                    case "9":
                        $('#ef_grade-9').prop("checked", true);
                        break;
                    case "10":
                        $('#ef_grade-10').prop("checked", true);
                        break;
                    case "11":
                        $('#ef_grade-11').prop("checked", true);
                        break;
                    case "12":
                        $('#ef_grade-12').prop("checked", true);
                        break;
                    case "-1":
                        $('#ef_grade-u').prop("checked", true);
                        break;
                    default:
                        $('#ef_grade-u').prop("checked", true);
                        break;
                }

                var BusIdx = -1;
                switch(e.Bus)
                {
                    case "1":
                        BusIdx = 0;
                        break;
                    case "2":
                        BusIdx = 1;
                        break;
                    case "3":
                        BusIdx = 2;
                        break;
                    case "D":
                        BusIdx = 3;
                        break;
                }

                for (var x = 0; x < 4; x++) {
                    if (BusIdx == x)
                        $("#ef_bus-" + x).prop("checked", true);
                    else
                        $("#ef_bus-" + x).prop("checked", false)
                }

                switch(e.CheckIn)
                {
                    case "Y":
                        $("#ef_check-Y").prop("checked", true);
                        break;
                    case "N":
                        $("#ef_check-N").prop("checked", true);
                        break;
                }
                if (CallBack !== undefined) CallBack();
            });
        });
    };


    var AddErrorMessage = function(Message)
    {
        var but = $('<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>');
        var div = $('<div class="alert alert-danger alert-dismissible" role="alert">').append(but);
        div.append(Message);
        $("#Messages").append(div);
    }


    return {
        Init: Init,
        HideScreen: HideScreen,
        ShowScreen: ShowScreen,
        NoPhone: NoPhone,
        NoAddress: NoAddress,
        ReadDataFromForm: ReadDataFromForm,
        Cancel: Cancel,
        Save: Save,
        Load: Load,
        New: New,
        LoadGuestsOfCombo: LoadGuestsOfCombo,
        AddErrorMessage: AddErrorMessage
    };
}();

var EntryFormScreen = new EntryFormClass();