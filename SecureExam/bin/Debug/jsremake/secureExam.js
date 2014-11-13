"use strict";

/*
    SecureExam v2.0.0
    code.google.com/p/secureexam
    (c) 2014 by ZHAW, Simon Lukes, Daniel Jampen. All rights reserved.
    lukessim@students.zhaw.ch, jampedan@students.zhaw.ch
 */

/*
 *  Date Prototypes
 */

Date.prototype.toHHMMSSMSString = function () {
    this.h = (this.getHours() < 10) ? "0" + this.getHours() : this.getHours();
    this.m = (this.getMinutes() < 10) ? "0" + this.getMinutes() : this.getMinutes();
    this.s = (this.getSeconds() < 10) ? "0" + this.getSeconds() : this.getSeconds();
    this.ms = this.getMilliseconds();
    return this.h + ":" + this.m + ":" + this.s + ":" + this.ms;
};

// namespaces
var SecureExam = SecureExam || {};
SecureExam.Lib = {};
SecureExam.Lib.Security = {};

// constantes
SecureExam.Const = {};
SecureExam.Const.Cryptography = {};
SecureExam.Const.Cryptography.SHA256ITERATIONS = 100000;

/*
 *	Class Logger
 *	Constructor-Arguments: - none
 *
 *
 *  description: Logger for SecureExam Output
 */
SecureExam.Logger = new(function () {
    var that = this;
    this.DEBUG = true;
    this.loggers = [];

    this.logToAll = function (msg, sender) {
        if (that.DEBUG) {
            var message = "[secureExam::" + new Date().toLocaleDateString() + "-" + new Date().toLocaleTimeString() + "] ";
            if (sender !== undefined) {
                message += "[" + sender + "] ";
            }
            message += msg;

            for (var i = 0; i < that.loggers.length; i++) {
                that.loggers[i].log(message);
            }
        }
    }

    return {
        log: function (msg, sender) {
            that.logToAll(msg, sender);
        },
        addLogger: function (logger) {
            that.loggers.push(logger);
        },
        removeLogger: function (logger) {
            for (var i = 0; i < that.loggers.length; i++) {
                if (that.loggers[i] === logger) {
                    that.loggers.splice(i, 1);
                }
            }
        }
    }
});

/*
 *	Class HTMLInfo
 *	Constructor-Arguments: - divIDUserDB
 *                          - divIDEncryptedData
 *                          - divIDQuestions
 *
 *
 *  description: Stores HTML Div id's
 */
SecureExam.Lib.HTMLInfo = function (divIDUserDB, divIDEncryptedData, divIDQuestions) {
    var that = this;
    this.DivUserDB = document.getElementById(divIDUserDB);
    this.DivEncryptedData = document.getElementById(divIDEncryptedData);
    this.DivQuestions = document.getElementById(divIDQuestions);
}

/*
 *	Class SecureExamSettings
 *	Constructor-Arguments: - none
 *
 *
 *  description: Stores Settings to current Exam instance with abbility to save and load from localdb
 */
SecureExam.Lib.SecureExamSettings = function (userSecret) {
    var that = this;
    that.examStartTime = new Date();
    that.examExportedTime = null;
    that.examExpireTime = null;
    that.examStartings = 1;
    that.examFocusChanges = 0;
    that.examValid = true;
    that.invalidLog = null;
    that.studentSecret = userSecret;
    that.overallStartTime = null;
    that.overallEndTime = null;
    that.load = function () {
        try {
            var oldSEInfo = window.localStorage.getItem("secureExam");
            var dec = CryptoJS.AES.decrypt(oldSEInfo, that.studentSecret);
            var decString = dec.toString(CryptoJS.enc.Utf8);
            var oldData = decString.substring(16, decString.length - 1).split(",");
            if (decString.substring(0, 15) === "secureExamInfos" && oldData[3] === userSecret) {
                that.examStartTime = new Date(oldData[0]);
                that.examExportedTime = (oldData[1] !== "null") ? new Date(oldData[1]) : null;
                that.examStartings += Number(oldData[2]);
                that.studentSecret = oldData[3];
                that.examFocusChanges = oldData[4];
                that.examValid = (oldData[5] == "true") ? true : false;
                that.examExpireTime = new Date(oldData[6]);
                that.invalidLog = oldData[7];
                that.overallStartTime = new Date(oldData[8]);
                that.overallEndTime = new Date(oldData[9]);
                SecureExam.Logger.log("loading new instance", "SecureExamSettings");
            }
        } catch (err) {
            SecureExam.Logger.log("loading old instance failed, created new one", "SecureExamSettings");
        }
    }();
    that.save = function () {
        var exportString = 'secureExamInfos(' + that.examStartTime + ',' + that.examExportedTime + ',' + that.examStartings + ',' + that.studentSecret + ',' + that.examFocusChanges + ',' + that.examValid + ',' + that.examExpireTime + ',' + that.invalidLog + ',' + that.overallStartTime + ',' + that.overallEndTime + ')';
        window.localStorage.setItem("secureExam", CryptoJS.AES.encrypt(exportString, that.studentSecret));
        SecureExam.Logger.log("saved instance to loaldDB", "SecureExamSettings");
    }
};

/*
 *	Class Observer
 *	Constructor-Arguments: - action = function to call when observer gets notified
 *
 *
 *  description: Observer Class for Observable Classes ;)
 */
SecureExam.Lib.Observer = function (action) {
    var that = this;
    this.action = action;

    this.notify = function (msg) {
        that.action(msg);
    }
}

/*
 *	Class SecureExam.lib.security.InternetAccessCheck()
 *	Constructor-Arguments: none
 *
 *  description: class to check internet Access and if so, fire events to observers
 */
SecureExam.Lib.Security.InternetAccessCheck = function () {
    var that = this;
    this.intervalTimeout = 1000;
    this.imgURL = "http://waikiki.zhaw.ch/~rege/t-menu/header_left.jpg";
    this.interval = null;
    this.EVENTS = {
        ONLINE: "online",
        OFFLINE: "offline"
    };
    this.eventListeners = [];
    this.eventListeners[this.EVENTS.ONLINE] = [];
    this.eventListeners[this.EVENTS.OFFLINE] = [];

    this.onLoad = function () {
        SecureExam.Logger.log("internet online", "InternetAccessCheck");
        that.riseEvent(that.EVENTS.ONLINE, "internet online");
    }

    this.onError = function () {
        SecureExam.Logger.log("internet offline", "InternetAccessCheck");
        that.riseEvent(that.EVENTS.OFFLINE, "internet offline");
    }

    this.check = function () {
        SecureExam.Logger.log("checking internet", "InternetAccessCheck");
        var img = new Image();
        img.onload = that.onLoad;
        img.onerror = that.onError;
        img.src = that.imgURL + '?t=' + new Date().getTime();
    }

    this.riseEvent = function (event, msg) {
        for (var i = 0; i < that.eventListeners[event].length; i++) {
            that.eventListeners[event][i](msg);
        }
    }

    this.start = function () {
        SecureExam.Logger.log("started...", "InternetAccessCheck");
        that.interval = window.setInterval(that.check, that.intervalTimeout);
    }

    this.stop = function () {
        SecureExam.Logger.log("started...", "InternetAccessCheck");
        clearInterval(that.inteval);
    }

    return {
        getState: function () {
            return that.state;
        },
        setIntervalTimeout: function (timeout) {
            that.stop();
            that.intervalTimeout = timeout;
            that.start();
        },
        addEventListener: function (event, observer) {
            switch (event.toLowerCase()) {
            case that.EVENTS.ONLINE:
                that.eventListeners[that.EVENTS.ONLINE].push(observer);
                SecureExam.Logger.log("added listener to ONLINE", "InternetAccessCheck");
                break;
            case that.EVENTS.OFFLINE:
                that.eventListeners[that.EVENTS.OFFLINE].push(observer);
                SecureExam.Logger.log("added listener to OFFLINE", "InternetAccessCheck");
                break;
            }
            if (that.interval == null && that.eventListeners[that.EVENTS.ONLINE].length == 1) {
                that.start();
            }
        },
        removeEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case that.EVENTS.ONLINE:
                for (var i = 0; i < that.eventListeners[that.EVENTS.TIMEERROR].length; i++) {
                    if (that.eventListeners[that.EVENTS.TIMEERROR][i] === listener) {
                        that.eventListeners[that.EVENTS.TIMEERROR].splice(i, 1);
                    }
                    SecureExam.Logger.log("removed listener from ONLINE", "InternetAccessCheck");
                }
                break;
            case that.EVENTS.OFFLINE:
                for (var i = 0; i < that.eventListeners[that.EVENTS.OFFLINE].length; i++) {
                    if (that.eventListeners[that.EVENTS.OFFLINE][i] === listener) {
                        that.eventListeners[that.EVENTS.OFFLINE].splice(i, 1);
                    }
                    SecureExam.Logger.log("removed listener from OFFLINE", "InternetAccessCheck");
                }
                break;
            }

            if (that.interval != null && that.eventListeners[that.EVENTS.ONLINE].length == 0) {
                that.stop();
            }
        }
    }
}

/*
 *	Class SecureTime
 *	Constructor-Arguments: - none
 *
 *
 *  description: class to handle save, reliable and non-manipulatable Time.
 */

SecureExam.Lib.Security.SecureTime = function () {
    var that = this;
    this.INTERNALUPDATEINTERVAL = 1000;
    this.INTERNALCLOCKMAXVARIANCE = 50;
    this.TIMEHISTORYMAXVARIANCE = 50;
    this.EVENTS = {
        TIMEERROR: "timeerror",
        TABCHANGE: "tabchange"
    };

    this.timeHistory = new Array();
    this.internalClockMilliseconds = 0;
    this.eventListeners = [];
    this.eventListeners[this.EVENTS.TIMEERROR] = [];
    this.eventListeners[this.EVENTS.TABCHANGE] = [];
    this.internalClockStartTime = null;
    this.interval = null;

    this.update = function () {
        SecureExam.Logger.log("updating time", "secureDate");
        var systemTime = new Date();
        that.internalClockMilliseconds += that.INTERNALUPDATEINTERVAL;
        var internalTime = that.getInternalTime();

        // verify InternalTime
        if (that.dateCompare(internalTime, systemTime)) {
            // compensate cpu lag in internal Time (systemTime is more accurate)
            var diff = systemTime.getTime() - internalTime.getTime();
            SecureExam.Logger.log("systime: " + systemTime.toHHMMSSMSString() + " internal: " + internalTime.toHHMMSSMSString(), "secureDate");
            if (diff > 0) {
                that.internalClockStartTime.setTime(that.internalClockStartTime.getTime() + diff);
                SecureExam.Logger.log("compensing cpu lag: " + diff + "ms", "secureDate");
            }

            // check with History
            if (that.timeHistory.length == 10) that.timeHistory.shift();
            that.timeHistory.push(new Date(systemTime));

            for (var i = 0; i < that.timeHistory.length; i++) {
                var historyOffset = i * that.INTERNALUPDATEINTERVAL;
                var historyVariance = i * that.TIMEHISTORYMAXVARIANCE;
                var recalculatedTime = new Date(that.timeHistory[that.timeHistory.length - 1 - i]);
                recalculatedTime.setTime(recalculatedTime.getTime() + historyOffset);
                if (!that.dateCompare(systemTime, recalculatedTime, historyVariance)) {
                    that.riseEvent(that.EVENTS.TIMEERROR, "HistoryTime([" + i + "]," + recalculatedTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!");
                    SecureExam.Logger.log("HistoryTime([" + i + "]," + recalculatedTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!", "secureDate");
                    return false;
                }
            }
            SecureExam.Logger.log("UPDATED: status OK | internalclock @ " + internalTime.toHHMMSSMSString() + " historynewest @ " + that.timeHistory[that.timeHistory.length - 1].toHHMMSSMSString(), "secureDate");
            return true;
        } else {
            that.riseEvent(that.EVENTS.TIMEERROR, "InternalTime(" + internalTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!");
            SecureExam.Logger.log("InternalTime(" + internalTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!", "secureDate");
            return false;
        }
    }

    this.dateCompare = function (actualTime, timeToVerify, maxVariance) {
        var maxVariance = (typeof maxVariance !== "undefined") ? maxVariance : that.INTERNALCLOCKMAXVARIANCE;
        timeToVerify = new Date(timeToVerify);
        actualTime = new Date(actualTime);

        var variance = timeToVerify.getTime() - actualTime.getTime();
        if (variance <= maxVariance && variance >= -maxVariance) {
            return true;
        }
        return false;
    }

    this.riseEvent = function (event, msg) {
        for (var i = 0; i < that.eventListeners[event].length; i++) {
            that.eventListeners[event][i](msg);
        }
    }

    this.start = function () {
        that.internalClockStartTime = new Date();
        that.interval = window.setInterval(that.update, that.INTERNALUPDATEINTERVAL);
        SecureExam.Logger.log("started...", "secureDate");
    }

    this.stop = function () {
        clearInterval(that.interval);
        SecureExam.Logger.log("stopped...", "secureDate");
    }

    this.visibilityChanged = function (e) {
        if (document.hidden) {
            SecureExam.Logger.log("tab hidden", "secureDate");
            that.riseEvent(that.EVENTS.TABCHANGE, "hidden");
        } else {
            SecureExam.Logger.log("tab visible", "secureDate");
            that.riseEvent(that.EVENTS.TABCHANGE, "visible");
        }
    }

    this.addVisibilityChangeListener = function () {
        document.addEventListener("visibilitychange", that.visibilityChanged);
    }

    this.removeVisibilityChangeListener = function () {
        document.removeEventListener("visibilitychange", that.visibilityChanged);
    }

    this.getInternalTime = function () {
        var date = new Date(that.internalClockStartTime);
        date.setMilliseconds(date.getMilliseconds() + that.internalClockMilliseconds);
        return date;
    }

    return {
        getInternalTime: function () {
            return that.getInternalTime();
        },
        addEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case that.EVENTS.TIMEERROR:
                that.eventListeners[that.EVENTS.TIMEERROR].push(listener);
                SecureExam.Logger.log("added listener to TIMEERROR", "secureDate");

                if (that.eventListeners[that.EVENTS.TIMEERROR].length == 1) {
                    that.start();
                }
                break;
            case that.EVENTS.TABCHANGE:
                that.eventListeners[that.EVENTS.TABCHANGE].push(listener);
                SecureExam.Logger.log("added listener to TABCHANGE", "secureDate");

                if (that.eventListeners[that.EVENTS.TABCHANGE].length == 1) {
                    that.addVisibilityChangeListener();
                }
                break;
            }
        },
        removeEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case that.EVENTS.TIMEERROR:
                for (var i = 0; i < that.eventListeners[that.EVENTS.TIMEERROR].length; i++) {
                    if (that.eventListeners[that.EVENTS.TIMEERROR][i] === listener) {
                        that.eventListeners[that.EVENTS.TIMEERROR].splice(i, 1);
                    }
                }
                SecureExam.Logger.log("removed listener from TIMEERROR", "secureDate");

                if (that.eventListeners[that.EVENTS.TIMEERROR].length == 0) {
                    that.stop();
                }
                break;
            case that.EVENTS.TABCHANGE:
                for (var i = 0; i < that.eventListeners[that.EVENTS.TABCHANGE].length; i++) {
                    if (that.eventListeners[that.EVENTS.TABCHANGE][i] === listener) {
                        that.eventListeners[that.EVENTS.TABCHANGE].splice(i, 1);
                    }
                }
                SecureExam.Logger.log("removed listener from TABCHANGE", "secureDate");

                if (that.eventListeners[that.EVENTS.TABCHANGE].length == 0) {
                    that.removeVisibilityChangeListener();
                }
                break;
            }
        },
        setHistoryTimeMaxVariance: function (ms) {
            that.TIMEHISTORYMAXVARIANCE = ms;
        },
        setInternalTimeMaxVariance: function (ms) {
            that.INTERNALCLOCKMAXVARIANCE = ms;
        }
    }
}

/*
 *	Class exam
 *	Constructor-Arguments: - htmlinfo : object contains html divs for outputs
 *
 *
 *  description: class to handle save, reliable and non-manipulatable Time.
 */
SecureExam.Exam = function (htmlInfo) {
    var that = this;
    this.InternetAccess = new SecureExam.Lib.Security.InternetAccessCheck();
    this.SecureTime = new SecureExam.Lib.Security.SecureTime();
    this.Settings = null;
    this.timeLeft = null;
    this.timeLeftInterval = null;
    this.autoSaveTimeout = 5000;
    this.autoSaveInterval = null;
    this.EVENTS = {
        TIMELEFT: "timeleft",
        AUTOSAVE: "autosave",
        EXAMTIMEEXPIRED: "examtimeexpired"
    };
    this.ERRORCODES = {
        TOOEARLY: 0,
        TOOLATE: 1,
        ALREADYEXPORTED: 2,
        INVALIDUSERSECRET: 3,
        INVALIDARGUMENT: 4,
        INVALIDEVENT: 5
    }
    this.eventListeners = [];
    this.eventListeners[this.EVENTS.TIMELEFT] = [];
    this.eventListeners[this.EVENTS.AUTOSAVE] = [];
    this.eventListeners[this.EVENTS.EXAMTIMEEXPIRED] = [];
    this.User = {
        firstname: null,
        lastname: null,
        immNumber: null,
        secret: null,
        toString: function () {
            return that.User.firstname + that.User.lastname + that.User.immNumber + that.User.secret;
        }
    };

    if (htmlInfo instanceof SecureExam.Lib.HTMLInfo) {
        this.HTMLInfo = htmlInfo
    } else {
        throw this.ERRORCODES.INVALIDARGUMENT;
    }

    this.init = function (firstname, lastname, immNumber, secret) {
        that.User.firstname = firstname;
        that.User.lastname = lastname;
        that.User.immNumber = immNumber;
        that.User.secret = secret;
        
        that.Settings = new SecureExam.Lib.SecureExamSettings(that.User.toString());
        
        if (that.Settings.examExportedTime === null) {

            if (!that.tryRestoreSavePoint(that.User.toString())) {
                that.decryptQuestions(that.User.toString());
            }
            if (that.Settings.overallEndTime >= new Date()) {
                if (that.Settings.overallStartTime <= new Date()) {
                    that.calculateTimeLeft();
                    that.Settings.save();
                    that.addEventListener(that.EVENTS.TIMELEFT, that.examTimeExpiredCheck);
                } else {
                    throw that.ERRORCODES.TOOEARLY;
                }
            } else {
                throw that.ERRORCODES.TOOLATE;
            }
        } else {
            throw that.ERRORCODES.ALREADYEXPORTED;
        }
    }

    this.tryRestoreSavePoint = function (userSecret) {
        if (window.localStorage.getItem("secureExamAutoSave") !== null) {
            try {
                var dec = CryptoJS.AES.decrypt(window.localStorage.getItem("secureExamAutoSave"), userSecret);
                that.HTMLInfo.DivQuestions.innerHTML = dec.toString(CryptoJS.enc.Utf8);
                SecureExam.Logger.log("restored savepoint", "exam");
                return true;
            } catch (e) {
                SecureExam.Logger.log("savepoint found but from different user", "exam");
            }
        }
        SecureExam.Logger.log("there is no old savepoint", "exam");
        return false;
    }

    this.decryptQuestions = function (userSecret) {
        try {
            SecureExam.Logger.log("decrypting questions...", "exam");
            // load userKeyDB & questionDiv
            var userKeyDB = that.HTMLInfo.DivUserDB.innerHTML.split("<br>");
            var questionDiv = that.HTMLInfo.DivEncryptedData.innerHTML.split(",");

            // remove empty last entry from userKeyDB
            userKeyDB.pop();

            // userKeyDB: split username from secret
            for (var i = 0; i < userKeyDB.length; i++) {
                userKeyDB[i] = userKeyDB[i].split(",");
            }
            SecureExam.Logger.log("userKeyDB loaded with [" + userKeyDB.length + "] entrys", "exam");

            // get encryptedMasterkey cypher for this user
            var masterkeyCypher;
            var masterkeyIV;
            var saltB64;
            var fullUserID = that.User.firstname + that.User.lastname + that.User.immNumber;
            for (var i = 0; i < userKeyDB.length; i++) {
                if (fullUserID === userKeyDB[i][0]) {
                    masterkeyCypher = CryptoJS.enc.Hex.parse(userKeyDB[i][1]);
                    masterkeyIV = CryptoJS.enc.Hex.parse(userKeyDB[i][2]);
                    saltB64 = userKeyDB[i][3];
                    SecureExam.Logger.log("found matching cipherparameters to usersecret", "exam");
                    break;
                }

            }

            // generate key
            var userSecretSalted = that.User.toString() + saltB64;
            var key = CryptoJS.SHA256(userSecretSalted);
            for (var i = 0; i < SecureExam.Const.Cryptography.SHA256ITERATIONS - 1; i++) {
                key = CryptoJS.SHA256(key);
            }
            SecureExam.Logger.log("key hashing complete", "exam");

            // decrypt masterkey
            var masterKeyCipherParams = CryptoJS.lib.CipherParams.create({
                ciphertext: masterkeyCypher,
                key: key,
                iv: masterkeyIV,
                algorithm: CryptoJS.algo.AES,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.PKCS7,
                blockSize: 4,
                formatter: CryptoJS.format.OpenSSL
            });

            var decMasterKey = CryptoJS.AES.decrypt(masterKeyCipherParams, key, {
                iv: masterkeyIV
            });
            var masterKeyString = decMasterKey.toString(CryptoJS.enc.Utf8);
            var masterKey = CryptoJS.enc.Hex.parse(masterKeyString);
            SecureExam.Logger.log("masterkey decrypted", "exam");

            // decrypt questions
            var questionsIV = CryptoJS.enc.Hex.parse(questionDiv[0]);
            var questionsCipher = CryptoJS.enc.Hex.parse(questionDiv[1]);

            var questionsCipherParams = CryptoJS.lib.CipherParams.create({
                ciphertext: questionsCipher,
                key: masterKey,
                iv: questionsIV,
                algorithm: CryptoJS.algo.AES,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.PKCS7,
                blockSize: 4,
                formatter: CryptoJS.format.OpenSSL
            });

            var decrypted = CryptoJS.AES.decrypt(questionsCipherParams, masterKey, {
                iv: questionsIV
            });
            var decryptedString = decrypted.toString(CryptoJS.enc.Utf8);
            SecureExam.Logger.log("data decrypted", "exam");

            var decryptedData = decryptedString.split(",");
            that.Settings.overallStartTime = new Date(Number(decryptedData[0]));
            that.Settings.overallEndTime = new Date(Number(decryptedData[1]));
            that.Settings.examExpireTime = new Date(that.Settings.examStartTime.getTime() + (Number(decryptedData[2])*60*1000));
            SecureExam.Logger.log("important times decrypted and set", "exam");

            // print questions, iterate as there are maybe some "," in the text.. 
            var questionsHTML = "";
            for( var i = 3; i < decryptedData.length; i++) {
                questionsHTML += decryptedData[i];
            }
            that.HTMLInfo.DivQuestions.innerHTML = questionsHTML;
        } catch (e) {
            throw that.ERRORCODES.INVALIDUSERSECRET;
            return;
        }
    }

    this.stop = function () {
        that.Settings.examExportedTime = new Date();
        
        that.autoSave();
        that.export();
        that.Settings.save();
        that.removeAllEventListeners();
    }

    this.removeAllEventListeners = function () {
        for (var i = 0; i < that.eventListeners.length; i++) {
            for (var j = 0; k < that.eventListeners[i].length; j++) {
                that.removeEventListener(that.eventListeners[i], that.eventListeners[i][j]);
            }
        }
    }

    this.export = function () {
        var xml = that.generateExportXML();
        var encXml = CryptoJS.AES.encrypt(xml, that.User.toString());
        xml += encXml.iv.toString() + "," + encXml.ciphertext.toString();

        // generate download
        var blob = new Blob([xml], {
            type: "text/plain;charset=utf-8"
        });
        var ret = saveAs(blob, "exam_" + that.User.firstname + that.User.lastname + that.User.immNumber + ".xml.enc");
    }

    // todo: include logs
    this.generateExportXML = function () {
        var xml = '<?xml version="1.0" encoding="iso-8859-1"?>';
        xml += '<exam>';
        xml += '<student>';
        xml += '<firstname>' + that.User.firstname + '</firstname>';
        xml += '<lastname>' + that.User.lastname + '</lastname>';
        xml += '<nr>' + that.User.immNumber + '</nr>';
        xml += '</student>';
        xml += '<examInfos>';
        xml += '<startTime>' + that.Settings.examStartTime.toUTCString() + '</startTime>';
        xml += '<endTime>' + that.Settings.examExportedTime.toUTCString() + '</endTime>';
        xml += '<focusChanges>' + that.Settings.examFocusChanges + '</focusChanges>';
        xml += '<examStartings>' + that.Settings.examStartings + '</examStartings>';
        xml += '</examInfos>';
        if (that.Settings.examValid) {
            xml += '<questions>';
            var questions = document.getElementById("exam");
            for (var i = 0; i < questions.children.length; i++) {
                var question = questions.children[i];
                xml += '<question>';
                for (var j = 0; j < question.children.length; j++) {
                    var pTag = question.children[j];
                    switch (pTag.className) {
                    case "questionText":
                        xml += '<legend>' + pTag.innerHTML + '</legend>';
                        break;
                    case "answer":
                        for (var k = 0; k < pTag.children.length; k++) {
                            var inputTag = pTag.children[k];
                            switch (inputTag.tagName) {
                            case "INPUT":
                                switch (inputTag.type) {
                                case "checkbox":
                                    xml += '<input type="checkbox" ' + (inputTag.checked == true ? "checked=\"true\" " : "") + '/>';
                                    break;
                                }
                                break;
                            case "TEXTAREA":
                                xml += '<input type="text">' + inputTag.value + '</input>';
                                break;
                            }
                        }
                        break;
                    }
                }
                xml += '</question>';
            }
            xml += '</questions>';
        } else {
            xml += '<examInvalid>' + that.Settings.invalidLog + '</examInvalid>';
        }
        xml += '</exam>';
        return xml;
    }

    this.examTimeExpiredCheck = function () {
        if (that.timeLeft <= 0 || that.Settings.examExportedTime <= new Date() || that.Settings.examExpireTime <= new Date()) {
            that.riseEvent(that.EVENTS.EXAMTIMEEXPIRED, null);
            that.stop();
        }
    }

    this.calculateTimeLeft = function () {
        var now = new Date();
        var diff = (that.Settings.examExpireTime - now) / 1000;
        var seconds = (Math.floor(diff % 60) < 10) ? "0" + Math.floor(diff % 60) : Math.floor(diff % 60);
        var minutes = (Math.floor(diff / 60) < 10) ? "0" + Math.floor(diff / 60) : Math.floor(diff / 60);

        if (minutes >= 10 && seconds !== "00") {
            return;
        }
        var newTime = minutes + ":" + seconds;
        if (newTime !== that.timeLeft) {
            that.timeLeft = newTime;
            that.riseEvent(that.EVENTS.TIMELEFT, that.timeLeft);
        }
    }

    this.autoSave = function () {
        var questionDivs = document.getElementsByClassName("question");
        var questions = questionDivs[0].parentNode.children;
        for (var i = 0; i < questions.length; i++) {
            var answerForms = questions[i].children[1].children;
            for (var j = 0; j < answerForms.length; j++) {
                switch (answerForms[j].tagName) {
                case "INPUT":
                    switch (answerForms[j].type) {
                    case "checkbox":
                        if (answerForms[j].checked) answerForms[j].setAttribute("checked", true);
                        break;
                    }
                    break;
                case "TEXTAREA":
                    answerForms[j].innerText = answerForms[j].value;
                    break;
                }
            }
        }
        window.localStorage.setItem("secureExamAutoSave", CryptoJS.AES.encrypt(document.getElementById("questions").innerHTML, that.User.toString()));
        that.riseEvent(that.EVENTS.AUTOSAVE, new Date());
    }

    this.addEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case that.EVENTS.TIMELEFT:
            that.eventListeners[that.EVENTS.TIMELEFT].push(listener);
            if (that.eventListeners[that.EVENTS.TIMELEFT].length === 1) {
                that.timeLeftInterval = window.setInterval(that.calculateTimeLeft, 1000);
            }
            break;
        case that.EVENTS.AUTOSAVE:
            that.eventListeners[that.EVENTS.AUTOSAVE].push(listener);
            if (that.eventListeners[that.EVENTS.AUTOSAVE].length === 1) {
                that.autoSaveInterval = window.setInterval(that.autoSave, that.autoSaveTimeout);
            }
            break;
        }
    }

    this.removeEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case that.EVENTS.TIMELEFT:
            for (var i = 0; i < that.eventListeners[that.EVENTS.TIMELEFT].length; i++) {
                if (that.eventListeners[that.EVENTS.TIMELEFT][i] === listener) {
                    that.eventListeners[that.EVENTS.TIMELEFT].splice(i, 1);
                }
            }
            if (that.eventListeners[that.EVENTS.TIMELEFT].length === 0) {
                clearInterval(that.timeLeftInterval);
            }
            break;
        case that.EVENTS.AUTOSAVE:
            for (var i = 0; i < that.eventListeners[that.EVENTS.AUTOSAVE].length; i++) {
                if (that.eventListeners[that.EVENTS.AUTOSAVE][i] === listener) {
                    that.eventListeners[that.EVENTS.AUTOSAVE].splice(i, 1);
                }
            }
            if (that.eventListeners[that.EVENTS.AUTOSAVE].length === 0) {
                clearInterval(that.autoSaveInterval);
            }
            break;
        }
    }

    this.riseEvent = function (event, msg) {
        for (var i = 0; i < that.eventListeners[event].length; i++) {
            that.eventListeners[event][i](msg);
        }
    }


    return {
        start: function (firstname, lastname, immNumber, secret) {
            if (firstname !== undefined && lastname !== undefined && immNumber !== undefined && secret !== undefined) {
                that.init(firstname, lastname, immNumber, secret.toUpperCase());
            } else
                throw that.ERRORCODES.INVALIDARGUMENT;
        },
        stop: function () {
            that.stop();
        },
        addEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case "online":
            case "offline":
                that.InternetAccess.addEventListener(event, listener);
                break;
            case "timeerror":
            case "tabswitch":
                that.SecureTime.addEventListener(event, listener);
                break;
            case "timeleftchange":
            case "autosave":
            case "examtimeexpired":
                that.addEventListener(event, listener);
                break;
            default:
                throw that.ERRORCODES.INVALIDEVENT;
            }
        },
        removeEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case "online":
            case "offline":
                that.InternetAccess.addEventListener(event, listener);
                break;
            case "timeerror":
            case "tabswitch":
                that.SecureTime.addEventListener(event, listener);
                break;
            case "timeleftchange":
            case "autosave":
            case "examtimeexpired":
                that.removeEventListener(event, listener);
                break;
            default:
                throw that.ERRORCODES.INVALIDEVENT;
            }
        },
        export: function () {
            if (that.Settings.examExportedTime === null) {
                that.stop();
            } else {
                that.export();
            }
        },
        setInternalTimeMaxVariance: function (ms) {
            if (ms >= 20) {
                that.SecureTime.setInternalTimeMaxVariance(ms);
            } else {
                throw that.ERRORCODES.INVALIDARGUMENT;
            }
        },
        setHistoryTimeMaxVariance: function (ms) {
            if (ms >= 20) {
                that.SecureTime.setHistoryTimeMaxVariance(ms);
            } else {
                throw that.ERRORCODES.INVALIDARGUMENT;
            }
        },
        setAutoSaveTimout: function (ms) {
            that.autoSaveTimeout = ms;
        },
        isExported: function () {
            return (that.Settings.examExportedTime !== null);
        },
        ERRORCODES: that.ERRORCODES
    }
}



function isOnline(e) {
    console.log("isonline");
}

function timeWrong(e) {
    console.log("timewrong");
}

function tabChange(e) {
    console.log("tabchange");
}

function timeLeftChanged(newTime) {
    var time = new Date(newTime);
    console.log("timeleft: " + time.toLocaleTimeString());
}

function examExpired(e) {
    if (exam.isExported) {
        // blabla
    } else {
        // bla
    }
}

// GUI: 4 eingabefelder
//        relogin (neue prüfung?)



function run() {
    SecureExam.Logger.addLogger(console);
    //SecureExam.Const.Cryptography.SHA256ITERATIONS = $SHA256ITERATIONS$;

    var htmlInfo = new SecureExam.Lib.HTMLInfo("userDB", "data", "questions");
    var exam = new SecureExam.Exam(htmlInfo);
    //try {
    exam.start("Daniel", "Jampen", "S12198320", "AD8DC0FEB4");

    exam.addEventListener("online", isOnline);
    //exam.addEventListener("offline", isOnline);
    exam.addEventListener("timeerror", timeWrong);
    exam.setInternalTimeMaxVariance(25);
    exam.setHistoryTimeMaxVariance(25);
    exam.addEventListener("tabswitch", tabChange);
    exam.addEventListener("timeleftchange", timeLeftChanged);
    exam.addEventListener("examtimeexpired", null);
    /*} catch (e) {
        switch (e) {
        case exam.ERRORCODES.ALREADYEXPORTED:
            console.log(e);
            break;
        case exam.ERRORCODES.INVALIDARGUMENT:
            console.log(e);
            break;
        case exam.ERRORCODES.INVALIDEVENT:
            console.log(e);
            break;
        case exam.ERRORCODES.INVALIDUSERSECRET:
            console.log(e);
            break;
        case exam.ERRORCODES.TOOEARLY:
            console.log(e);
            break;
        case exam.ERRORCODES.TOOLATE:
            console.log(e);
            break;
        }
    }*/
}