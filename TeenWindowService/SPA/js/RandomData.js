var RandomDataClass = function () {

};

RandomDataClass.prototype = function () {

    var Init = function (e) {
        $("#Random").on("click", null, e, function () {
            
            RandomDataScreen.PopulateRandomData();
        });
    };

    var HideScreen = function () {

    };

    var ShowScreen = function () {

    };

    var BuildRandomRecord = function () {
        var Chan = new Chance();
        var Gender = Chan.gender();

        var ret = {};

        ret.PublicRecId = -1;
        ret.PrivateRecId = -1;
        ret.FirstName = Chan.first({ gender: Gender });
        ret.LastName = Chan.last();
        ret.PhoneArea = Chan.integer({ min: 100, max: 999 });
        ret.PhonePrefix = Chan.integer({ min: 100, max: 999 });
        ret.PhoneSuffix = Chan.integer({ min: 1000, max: 9999 });
        ret.Street1 = Chan.address();
        ret.Street2 = ""
        ret.City = Chan.city();
        ret.State = Chan.state();
        ret.Zip = Chan.zip();
        ret.Sex = Gender.substring(0, 1);
        ret.Bus = Chan.integer({ min: 1, max: 3 });
        ret.Grade = Chan.integer({ min: 6, max: 12 });
        ret.GuestOf = '';

        return ret;
    }

    var PopulateRandomData = function () {
        var rec = BuildRandomRecord();        
        DataManager.WriteRecord(rec, "Y");
    };

    return {
        Init: Init,
        HideScreen: HideScreen,
        ShowScreen: ShowScreen,
        PopulateRandomData: PopulateRandomData,
        BuildRandomRecord: BuildRandomRecord
    };
}();

var RandomDataScreen = new RandomDataClass();