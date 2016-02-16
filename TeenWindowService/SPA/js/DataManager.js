var DataManagerClass = function () {
    this.db = null;
    this.NextPrivateId = 0;
    this.UseWebSQL = true;
};

DataManagerClass.prototype = new function () {

    var CreateTables;
    var WriteRecord;
    var ReadAllRecords;
    var GetNextPrivateId;
    var ReadSingleRecord;
    var CheckInTeen;
    var ClearData;
    var ReadDirtyRecords;

    var Init = function () {        
        // In the following line, you should include the prefixes of implementations you want to test.
        window.indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
        // DON'T use "var indexedDB = ..." if you're not in a function.
        // Moreover, you may need references to some window.IDB* objects:
        window.IDBTransaction = window.IDBTransaction || window.webkitIDBTransaction || window.msIDBTransaction;
        window.IDBKeyRange = window.IDBKeyRange || window.webkitIDBKeyRange || window.msIDBKeyRange;
        // (Mozilla has never prefixed these objects, so we don't need window.mozIDB*)

        if (window.indexedDB) {
            DataManager.UseWebSQL = false;
            console.log("using index db");
        }
        else {
            console.log("using web sql");
        }

        // DataManager.UseWebSQL = true;

        // DataManager.UseWebSQL = true;

        // Uncomment this is you want to for WEBDB
        // DataManager.UseWebSQL = true;

        if (DataManager.UseWebSQL) {
            DataManager.db = openDatabase('teendb', '1.0', 'Teen Checkin Web Base', 2 * 1024 * 1024);
            DataManager.CreateTables = DataManager.CreateTables_WebSQL;
            DataManager.WriteRecord = DataManager.WriteRecord_WebSQL;
            DataManager.ReadAllRecords = DataManager.ReadAllRecords_WebSQL;
            DataManager.GetNextPrivateId = DataManager.GetNextPrivateId_WebSQL;
            DataManager.ReadSingleRecord = DataManager.ReadSingleRecord_WebSQL;
            DataManager.CheckInTeen = DataManager.CheckInTeen_WebSQL;
            DataManager.ClearData = DataManager.ClearData_WebSQL;
            DataManager.ReadDirtyRecords = DataManager.ReadDirtyRecords_WebSQL;
        }
        else {
            DataManager.CreateTables = DataManager.CreateTables_IndexedDB;
            DataManager.WriteRecord = DataManager.WriteRecord_IndexedDB;
            DataManager.ReadAllRecords = DataManager.ReadAllRecords_IndexedDB;
            DataManager.GetNextPrivateId = DataManager.GetNextPrivateId_IndexedDB;
            DataManager.ReadSingleRecord = DataManager.ReadSingleRecord_IndexedDB;
            DataManager.CheckInTeen = DataManager.CheckInTeen_IndexedDB;
            DataManager.ClearData = DataManager.ClearData_IndexedDB;
            DataManager.ReadDirtyRecords = DataManager.ReadDirtyRecords_IndexedDB;

            var request = window.indexedDB.open("teendb", 1);            
            request.onerror = function (event) {
                console.log("Database error: " + event.target.errorCode);
            };
            request.onupgradeneeded = function (event) {
                var db = request.result;
                var objectStore = db.createObjectStore("teens", { autoIncrement: true });
                objectStore.createIndex("Dirty", "Dirty", { unique: false });
            }
            request.onsuccess = function (event) {                
                DataManager.db = request.result;
            };

        }
            

    };

    //*************************
    // WEB SQL impementation
    //**************************

    var CreateTables_WebSQL = function () {
        DataManager.db.transaction(function (tx) {
            var CreateCommand = 'CREATE TABLE IF NOT EXISTS teens (PublicRecId, PrivateRecId, FirstName, LastName, PhoneArea, PhonePrefix, PhoneSuffix, ' +
                                'Street1, Street2, City, State, Zip, Sex, Bus, Grade, GuestOf, CheckIn, Dirty, NewRec, UpdateRec, UpdateCheckin, GuestOfKey)';
            tx.executeSql(CreateCommand, null, function (tx, results) { });
        });
    };
    var WriteRecord_WebSQL = function (e, Dirty, CallBack) {
        // 15 records
        if (e.PrivateRecId != -1)
        {
            DataManager.db.transaction(function (tx) {
                var UpdateCommand = 'UPDATE teens set FirstName=?, LastName=?, PhoneArea=?, PhonePrefix=?, PhoneSuffix=?, ' +
                                    'Street1=?, Street2=?, City=?, State=?, Zip=?, Sex=?, Bus=?, Grade=?, GuestOf=?, Dirty=?, ' +
                                    'UpdateRec= ?, UpdateCheckIn = ?, CheckIn = ?' +
                                    'where PrivateRecId = {private}'
                UpdateCommand = UpdateCommand.replace("{private}", e.PrivateRecId);

                var d = tx.executeSql(UpdateCommand, [e.FirstName, e.LastName, e.PhoneArea, e.PhonePrefix, e.PhoneSuffix,
                                                      e.Street1, e.Street2, e.City, e.State, e.Zip, e.Sex, e.Bus, e.Grade, e.GuestOf, 'Y',
                                                      'Y', e.UpdateCheckin, e.CheckIn], function (tx, results)
                                                      {
                                                          if (CallBack !== undefined) CallBack();
                                                      });
            });
        }
        else
        {
            DataManager.GetNextPrivateId(function (privateId) {
                var InsertCmd = 'INSERT INTO teens (PublicRecId, PrivateRecId, FirstName, LastName, PhoneArea, PhonePrefix, PhoneSuffix,' +
                                                    'Street1, Street2, City, State, Zip, Sex, Bus, Grade, GuestOf, Dirty, CheckIn,' +
                                                    'NewRec, UpdateRec, UpdateCheckin, GuestOfKey) ' +
                                                    ' VALUES(?, ?, ?, ?, ?, ?, ?, ' +
                                                    ' ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ' +
                                                    ' ?, ?, ?, ?)';                
                e.PrivateRecId = privateId;
                DataManager.db.transaction(function (tx) {

                    if (Dirty === "Y")
                    {
                        e.GuestOfKey = chance.guid();
                        e.Dirty = 'Y';
                        e.NewRec = 'Y';
                        e.UpdateRec = 'N';
                        // e.UpdateCheckin = 'N';
                        // e.CheckIn = 'N';
                    }

                    var d = tx.executeSql(InsertCmd, [e.PublicRecId, e.PrivateRecId, e.FirstName, e.LastName, e.PhoneArea, e.PhonePrefix, e.PhoneSuffix,
                                                      e.Street1, e.Street2, e.City, e.State, e.Zip, e.Sex, e.Bus, e.Grade, e.GuestOf, e.Dirty, e.CheckIn,
                                                      e.NewRec, e.UpdateRec, e.UpdateCheckin, e.GuestOfKey],
                                                      function (tx, results) {
                                                          if (CallBack !== undefined) CallBack();
                    });
                });
            });
        }
    };
    var ReadAllRecords_WebSQL = function (callback) {
        DataManager.db.transaction(function (tx) {
            var SelectCommand = "SELECT LastName, FirstName, PhoneArea, PhonePrefix, PhoneSuffix, Bus, PrivateRecId, CheckIn, GuestOfKey from teens order by LastName, FirstName, PhoneArea, PhonePrefix, PhoneSuffix";
            tx.executeSql(SelectCommand, [], function (tx, results) {
                var len = results.rows.length, i;
                var ret = [];
                ret[0] = { DisplayText: "", Value : -1};
                for (i = 0; i < len; i++)
                {
                    var Rec = results.rows.item(i);
                    var DisplayText = Rec.LastName + ", " + Rec.FirstName + " (" + Rec.PhoneArea + ") " + Rec.PhonePrefix + "-" + Rec.PhoneSuffix;
                    var Value = Rec.PrivateRecId;
                    var Bus = Rec.Bus;                    
                    var CheckIn = Rec.CheckIn;
                    var GuestOfKey = Rec.GuestOfKey;
                    ret.push({ DisplayText: DisplayText, Value: Value, Bus: Bus, CheckIn: CheckIn, GuestOfKey: GuestOfKey });
                }
                callback(ret);
            });
        });

    };
    var GetNextPrivateId_WebSQL = function (callback) {
        DataManager.db.transaction(function (tx) {
            var SelectCommand = "SELECT max(PrivateRecId) as cnt from teens";
            tx.executeSql(SelectCommand, [], function (tx, results) {
                var len = results.rows.length, i;
                if (len > 0) {
                    var ret = results.rows.item(0).cnt;
                    if (ret === undefined) ret = 0;
                    if (ret === null) ret = 0;
                    callback(ret + 1);
                }
            });
        });
    };
    var ReadSingleRecord_WebSQL = function (privateId, callback) {
        DataManager.db.transaction(function (tx) {
            var SelectCommand = "select PublicRecId, PrivateRecId, FirstName, LastName, PhoneArea, PhonePrefix, PhoneSuffix, Street1, Street2, City, State, Zip, Sex, Bus, Grade, GuestOf from teens where PrivateRecId = {private}";
            SelectCommand = SelectCommand.replace("{private}", privateId);
            tx.executeSql(SelectCommand, null, function (tx, results) {
                var len = results.rows.length, i;
                if (len == 0) { callback(null); return; };

                var ret = {};

                ret.PublicRecId = results.rows.item(0).PublicRecId;
                ret.PrivateRecId = results.rows.item(0).PrivateRecId;
                ret.FirstName = results.rows.item(0).FirstName;
                ret.LastName = results.rows.item(0).LastName;
                ret.PhoneArea = results.rows.item(0).PhoneArea;
                ret.PhonePrefix = results.rows.item(0).PhonePrefix;
                ret.PhoneSuffix = results.rows.item(0).PhoneSuffix;
                ret.Street1 = results.rows.item(0).Street1;
                ret.Street2 = results.rows.item(0).Street2;
                ret.City = results.rows.item(0).City;
                ret.State = results.rows.item(0).State;
                ret.Zip = results.rows.item(0).Zip;
                ret.Sex = results.rows.item(0).Sex;
                ret.Bus = results.rows.item(0).Bus;
                ret.Grade = results.rows.item(0).Grade;
                ret.GuestOf = results.rows.item(0).GuestOf;
                callback(ret);
            });
        });
    };    
    var CheckInTeen_WebSQL = function (privateId, checkedIn, callback) {
        var UpdateComamnd = "update teens set CheckIn=?, Dirty=?, UpdateCheckin=? where PrivateRecId={private}"
        UpdateComamnd = UpdateComamnd.replace("{private}", privateId);
        console.log(privateId);
        console.log(UpdateComamnd);
        DataManager.db.transaction(function (tx) {
            tx.executeSql(UpdateComamnd, [checkedIn, 'Y', 'Y'], function (tx, results) {
                console.log(results);
                callback()
            });
        });
    };
    var ClearData_WebSQL = function (callback) {
        var DeleteComamnd = "DROP TABLE teens"
        DataManager.db.transaction(function (tx) {
            tx.executeSql(DeleteComamnd, null, function (tx, results) {
                var CreateCommand = 'CREATE TABLE IF NOT EXISTS teens (PublicRecId, PrivateRecId, FirstName, LastName, PhoneArea, PhonePrefix, PhoneSuffix, ' +
                                    'Street1, Street2, City, State, Zip, Sex, Bus, Grade, GuestOf, CheckIn, Dirty, NewRec, UpdateRec, UpdateCheckin, GuestOfKey)';
                tx.executeSql(CreateCommand, null, function (tx, results) {
                    if(callback !== undefined) callback();
                });
            });
        });
    };
    var ReadDirtyRecords_WebSQL = function (callback) {
        DataManager.db.transaction(function (tx) {
            var SelectCommand = "select PublicRecId, PrivateRecId, FirstName, LastName, PhoneArea, PhonePrefix, PhoneSuffix, Street1, Street2, " +
                                "City, State, Zip, Sex, Bus, Grade, GuestOf, CheckIn, Dirty, NewRec, UpdateRec, UpdateCheckin from teens where Dirty = 'Y'";
            tx.executeSql(SelectCommand, null, function (tx, results) {
                var len = results.rows.length, i;
                if (len == 0) { callback(null); return; };

                var ret = [];

                for (i = 0; i < len; i++) {
                    var rec = {};
                    rec.PublicRecId = results.rows.item(i).PublicRecId;
                    rec.PrivateRecId = results.rows.item(i).PrivateRecId;
                    rec.FirstName = results.rows.item(i).FirstName;
                    rec.LastName = results.rows.item(i).LastName;
                    rec.PhoneArea = results.rows.item(i).PhoneArea;
                    rec.PhonePrefix = results.rows.item(i).PhonePrefix;
                    rec.PhoneSuffix = results.rows.item(i).PhoneSuffix;
                    rec.Street1 = results.rows.item(i).Street1;
                    rec.Street2 = results.rows.item(i).Street2;
                    rec.City = results.rows.item(i).City;
                    rec.State = results.rows.item(i).State;
                    rec.Zip = results.rows.item(i).Zip;
                    rec.Sex = results.rows.item(i).Sex;
                    rec.Bus = results.rows.item(i).Bus;
                    rec.Grade = results.rows.item(i).Grade;
                    rec.GuestOf = results.rows.item(i).GuestOf;
                    rec.CheckIn = results.rows.item(i).CheckIn;
                    rec.Dirty = results.rows.item(i).Dirty;
                    rec.NewRec = results.rows.item(i).NewRec;
                    rec.UpdateRec = results.rows.item(i).UpdateRec;
                    rec.UpdateCheckin = results.rows.item(i).UpdateCheckin;
                    ret.push(rec);
                }

                console.log(ret);
                callback(ret);
            });
        });
    };

    //*************************
    // INDEXDB impementation
    //**************************

    var CreateTables_IndexedDB = function () {
        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(DataManager.CreateTables, 100);
            return;
        }
        if (DataManager.db.objectStoreNames.contains("teens")) return; // objectStore exists

    };
    var WriteRecord_IndexedDB = function (e, Dirty, CallBack) {
        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(function () { DataManager.WriteRecord(e); }, 100);
            return;
        }
        var transaction = DataManager.db.transaction(["teens"], "readwrite");
        var objectStore = transaction.objectStore("teens");
        transaction.oncomplete = function (event) {
            console.log("Transaction Complete")
        };
        transaction.onerror = function (event) {
            console.log("Transaction failed");
        };

        var RecId = -1;
        if (e.PrivateRecId !== undefined)
            RecId = parseInt(e.PrivateRecId);


        if (RecId == -1) {  // Add Record..
            if (Dirty === "Y") {
                e.GuestOfKey = chance.guid();
                e.Dirty = 'Y';
                e.NewRec = 'Y';
                e.UpdateRec = 'N';
                // e.UpdateCheckin = 'N';
                // e.CheckIn = 'N';
            }
            var request = objectStore.add(e);
            request.onsuccess = function (event) {
                // event.target.result == customerData[i].ssn;
                if (CallBack !== undefined) CallBack();
            };
        } else {
            e.Dirty = 'Y';
            e.UpdateRec = 'Y';
            var cursorReq = objectStore.openCursor(RecId);
            cursorReq.onsuccess = function (event) {
                var cursor = event.target.result;
                if (cursor) {
                    var data = cursor.value;
                    for (var key in data) {
                        if (e[key] === undefined) continue;  // Not part of the update..
                        data[key] = e[key];
                    }
                    cursor.update(data);
                }
                if (CallBack !== undefined) CallBack();
            };
        }        
    };
    var ReadAllRecords_IndexedDB = function (callback) {
        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(function () { DataManager.ReadAllRecords(callback); }, 100);
            return;
        }
        var transaction = DataManager.db.transaction(["teens"], "readonly");
        var objectStore = transaction.objectStore("teens");
        var ret = [];
        var cbRef = callback;
        objectStore.openCursor().onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                var Rec = cursor.value;
                var DisplayText = Rec.LastName + ", " + Rec.FirstName + " (" + Rec.PhoneArea + ") " + Rec.PhonePrefix + "-" + Rec.PhoneSuffix;
                var Value = cursor.key;
                var Bus = Rec.Bus;                    
                var CheckIn = Rec.CheckIn;
                var GuestOfKey = Rec.GuestOfKey;
                ret.push({ DisplayText: DisplayText, Value: Value, Bus: Bus, CheckIn: CheckIn, GuestOfKey: GuestOfKey });
                cursor.continue();
            }
            else  {
                cbRef(ret);
            }
        }
    };
    var GetNextPrivateId_IndexedDB = function (callback) {
        callback(-1);
    };
    var ReadSingleRecord_IndexedDB = function (privateId, callback) {
        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(function () { DataManager.ReadSingleRecord(privateId, callback); }, 100);
            return;
        }

        DataManager.db.transaction("teens").objectStore("teens").get(parseInt(privateId)).onsuccess = function (event) {
            event.target.result.PrivateRecId = parseInt(privateId);
            callback(event.target.result);
        }
    };
    var CheckInTeen_IndexedDB = function (privateId, checkedIn, callback) {

        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(function () { DataManager.CheckInTeen(privateId, checkedIn, callback); }, 100);
            return;
        }
        var transaction = DataManager.db.transaction(["teens"], "readwrite");
        var objectStore = transaction.objectStore("teens");
        transaction.oncomplete = function (event) {
            console.log("Transaction Complete")
        };
        transaction.onerror = function (event) {
            console.log("Transaction failed");
        };

        var RecId = parseInt(privateId);

        var cursorReq = objectStore.openCursor(parseInt(RecId));
        cursorReq.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                var data = cursor.value;
                data.CheckIn = checkedIn;
                data.Dirty = 'Y';
                data.UpdateCheckin = 'Y';
                cursor.update(data);
                callback();
            }
        };
    };
    var ClearData_IndexedDB = function (callback) {
        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(function () { DataManager.ClearData(callback); }, 100);
            return;
        }
        // open a read/write db transaction, ready for adding the data
        var transaction = DataManager.db.transaction(["teens"], "readwrite");
        var objectStore = transaction.objectStore("teens");
        transaction.oncomplete = function (event) {
        };

        objectStore.clear();
    };
    var ReadDirtyRecords_IndexedDB = function (callback) {
        if (DataManager.db === null)  // Avoid race condition
        {
            setTimeout(function () { DataManager.ReadDirtyRecords(callback); }, 100);
            return;
        }
        var transaction = DataManager.db.transaction(["teens"], "readonly");
        var objectStore = transaction.objectStore("teens");
        var ret = [];
        var cbRef = callback;
        objectStore.openCursor().onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                if (cursor.value.Dirty === 'Y')
                {
                    ret.push(cursor.value);
                }
                cursor.continue();
            }
            else {
                cbRef(ret);
            }
        }
    };

    return {
        Init: Init,
        CreateTables: CreateTables,
        WriteRecord: WriteRecord,
        ReadAllRecords: ReadAllRecords,
        GetNextPrivateId: GetNextPrivateId,
        ReadSingleRecord: ReadSingleRecord,
        CheckInTeen: CheckInTeen,
        ClearData: ClearData,
        ReadDirtyRecords: ReadDirtyRecords,
// Web SQL implementation
        CreateTables_WebSQL: CreateTables_WebSQL,
        WriteRecord_WebSQL: WriteRecord_WebSQL,
        ReadAllRecords_WebSQL: ReadAllRecords_WebSQL,
        GetNextPrivateId_WebSQL: GetNextPrivateId_WebSQL,
        ReadSingleRecord_WebSQL: ReadSingleRecord_WebSQL,
        CheckInTeen_WebSQL: CheckInTeen_WebSQL,
        ClearData_WebSQL: ClearData_WebSQL,
        ReadDirtyRecords_WebSQL: ReadDirtyRecords_WebSQL,
// INDEXDB implementation
        CreateTables_IndexedDB: CreateTables_IndexedDB,
        WriteRecord_IndexedDB: WriteRecord_IndexedDB,
        ReadAllRecords_IndexedDB: ReadAllRecords_IndexedDB,
        GetNextPrivateId_IndexedDB: GetNextPrivateId_IndexedDB,
        ReadSingleRecord_IndexedDB: ReadSingleRecord_IndexedDB,
        CheckInTeen_IndexedDB: CheckInTeen_IndexedDB,
        ClearData_IndexedDB: ClearData_IndexedDB,
        ReadDirtyRecords_IndexedDB: ReadDirtyRecords_IndexedDB
    };

}();

var DataManager = new DataManagerClass();

