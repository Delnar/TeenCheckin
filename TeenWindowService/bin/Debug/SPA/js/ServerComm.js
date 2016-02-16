var ServerCommClass = function () {
    this.ServerHost = window.location.host;

};

ServerCommClass.prototype = function () {

    var Init = function () {
        // ServerComm.ServerHost = "http://localhost:8000/TeenService/";
        // alert(ServerComm.ServerHost);
        // /wcfTeenServiceX/TeenServiceX.svc/json/
    };

    var ServerAvailable = function (success, failed) {
        $.ajax({                  
            // url: "http://localhost:8000/TeenService/ServerStatus",
            url: "http://" + ServerComm.ServerHost + ":8000/TeenService/ServerStatus",
            type: 'GET',
            xhrFields: {
                withCredentials: false
            },
            cache: false
        }).done(function (e) {
            success(e);
        }).error(function () {
            alert("Server available failed");
            failed();
        });
    };

    var PostDataToServer = function (ret, success, failed) {

        $.ajax({
            // url: "http://localhost:8000/TeenService/PostData",
            url: "http://" + ServerComm.ServerHost + ":8000/TeenService/PostData",
            contentType: "application/json; charset=utf-8",
            cache: false,
            type: 'POST',
            dataType: 'json',
            xhrFields: {
                withCredentials: false
            },
            data: JSON.stringify(ret),
            traditional: false

        }).done(function (e) {
            if (e === "GOOD")
                success();
            else
            {
                alert("Post Data Failed. Reason: " + e);
                failed(e);
            }
        }).error(function (errorInfo) {
            alert("Post Data Failed");
            failed("CONNECTION");
        });
    }

    var GetDataFromServer = function (success, failed) {
        $.ajax({
            // url: "http://localhost:8000/TeenService/GetData",
            url: "http://"+ ServerComm.ServerHost + ":8000/TeenService/GetData",
            contentType: "application/json; charset=utf-8",
            cache: false,
            type: 'GET',
            xhrFields: {
                withCredentials: false
            },
            dataType: 'json',
        }).done(function (e) {
            success(e);
        }).error(function (errorInfo) {
            alert("Get Data Failed");
            failed("CONNECTION");
        });
    }

    return {
        Init: Init,
        ServerAvailable: ServerAvailable,
        PostDataToServer: PostDataToServer,
        GetDataFromServer: GetDataFromServer
    };
}();

var ServerComm = new ServerCommClass();