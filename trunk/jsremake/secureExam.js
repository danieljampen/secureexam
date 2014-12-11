"use strict";

/*
    SecureExam v2.0.0
    code.google.com/p/secureexam
    (c) 2014 by ZHAW, Simon Lukes, Daniel Jampen. All rights reserved.
    lukessim@students.zhaw.ch, jampedan@students.zhaw.ch
 */



/**
 * Prints Hours:Minutes:Seconds
 */
Date.prototype.toHHMMSSString = function () {
    this.h = (this.getHours() < 10) ? "0" + this.getHours() : this.getHours();
    this.m = (this.getMinutes() < 10) ? "0" + this.getMinutes() : this.getMinutes();
    this.s = (this.getSeconds() < 10) ? "0" + this.getSeconds() : this.getSeconds();
    return this.h + ":" + this.m + ":" + this.s;
};

/**
 * Prints Hours:Minutes:Seconds:Miliseconds
 */
Date.prototype.toHHMMSSMSString = function () {
    return this.toHHMMSSString() + ":" + this.getMilliseconds();
};

/**
 * SecureExam Namespace.
 * @namespace
 */
var SecureExam = SecureExam || {};
/**
 * SecureExam.Const Namespace.
 * @namespace
 * @memberof SecureExam
 */
SecureExam.Const = {};
/**
 * SecureExam.Const.Security Namespace.
 * @namespace
 * @memberof SecureExam.Const
 */
SecureExam.Const.Cryptography = {};
/**
 * SecureExam.ErrorCode Namespace.
 * @namespace
 * @memberof SecureExam
 */
SecureExam.ErrorCode = {};
/**
 * SecureExam.Event Namespace.
 * @namespace
 * @memberof SecureExam
 */
SecureExam.Event = {};
/**
 * SecureExam.Event.InternetAccess Namespace.
 * @namespace
 * @memberof SecureExam.Event
 */
SecureExam.Event.InternetAccess = {};
/**
 * SecureExam.Event.SecureTime Namespace.
 * @namespace
 * @memberof SecureExam.Event
 */
SecureExam.Event.SecureTime = {};
/**
 * SecureExam.Lib Namespace.
 * @namespace
 * @memberof SecureExam
 */
SecureExam.Lib = {};
/**
 * SecureExam.Lib.Security Namespace.
 * @namespace
 * @memberof SecureExam.Lib
 */
SecureExam.Lib.Security = {};
/**
 * SecureExam.UI Namespace.
 * @namespace
 * @memberof SecureExam
 */
SecureExam.UI = {};
/**
 * SecureExam.UI.ViewMode Namespace.
 * @namespace
 * @memberof SecureExam.UI
 */
SecureExam.UI.ViewMode = {};

/** @constant
    @type {int}
    @memberof SecureExam.Const.Cryptography
*/
SecureExam.Const.Cryptography.SHA256ITERATIONS = 100000;

/** @constant
    @memberof SecureExam.Event
    @event
*/
SecureExam.Event.TIMELEFT = "timeleft";
/** @constant
    @memberof SecureExam.Event
    @event
*/
SecureExam.Event.AUTOSAVE = "autosave";
/** @constant
    @memberof SecureExam.Event
    @event
*/
SecureExam.Event.EXAMTIMEEXPIRED = "examtimeexpired";
/** @constant
    @memberof SecureExam.Event.SecureTime
    @event
*/
SecureExam.Event.SecureTime.TIMEERROR = "timeerror";
/** @constant
    @memberof SecureExam.Event.SecureTime
    @event
*/
SecureExam.Event.SecureTime.TABCHANGE = "tabchange";
/** @constant
    @memberof SecureExam.Event.InternetAccess
    @event
*/
SecureExam.Event.InternetAccess.ONLINE = "online";
/** @constant
    @memberof SecureExam.Event.InternetAccess
    @event
*/
SecureExam.Event.InternetAccess.OFFLINE = "offline";

/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.TOOEARLY = 0;
/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.TOOLATE = 1;
/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.ALREADYEXPORTED = 2;
/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.INVALIDUSERSECRET = 3;
/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.INVALIDARGUMENT = 4;
/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.INVALIDEVENT = 5;
/** @constant
    @type {int}
    @memberof SecureExam.ErrorCode
*/
SecureExam.ErrorCode.CONFIRMAUTOSAVERESTORE = 6;


/**
 * Static Logger Class.
 * each logger added must have a log method, which will be called when a message is recieved
 * @memberof SecureExam
 * @class
 * @this Logger
 * @static
 */
SecureExam.Logger = new(function () {
    var that = this;
    this.ErrorLevel = {
        info: 2, // all logs
        warning: 1, // warning & error
        error: 0 // error only
    };
    this.loggers = [];
    this.loggers[this.ErrorLevel.info] = [];
    this.loggers[this.ErrorLevel.warning] = [];
    this.loggers[this.ErrorLevel.error] = [];
    
    /**
     * sends message to loggers with appropriate log-level
     * @param {string} msg - The log message
     * @param {string} sender - The function which generated the log message.
     * @param {SecureExam.Logger.ErrorLevel} errorLevel - Minimal error level for the message
     * @private
     */
    this.logToAll = function (msg, sender, errorLevel) {
        if (that.checkIfLoggerAvailable(errorLevel)) {
            var message = "[secureExam::" + new Date().toLocaleDateString() + "-" + new Date().toLocaleTimeString() + "] ";
            switch (errorLevel) {
            case this.ErrorLevel.info:
                message += "[INFO] ";
                break;
            case this.ErrorLevel.warning:
                message += "[WARNING] ";
                break;
            case this.ErrorLevel.error:
                message += "[ERROR] ";
                break;
            }
            message += "[" + sender + "] ";
            message += msg;

            for (var j = errorLevel; j < that.loggers.length; j++) {
                for (var i = 0; i < that.loggers[j].length; i++) {
                    that.loggers[j][i].log(message);
                }
            }
        }
    }

    /**
     * checks if there's any logger added which would receive the message
     * @param {SecureExam.Logger.ErrorLevel} errorLevel - The ErrorLevel of the given message
     * @private
     */
    this.checkIfLoggerAvailable = function (errorLevel) {
        for (var i = errorLevel; i < that.loggers.length; i++) {
            if (that.loggers[i].length >= 1) {
                return true;
            }
        }
        return false;
    }

    return {
        /**
         * Function to add a log entry
         * @param {string} msg - The log message
         * @param {string} sender - The function which generated the log message.
         * @param {SecureExam.Logger.ErrorLevel} errorLevel - Minimal error level for the message
         * @memberof SecureExam.Logger
         */
        log: function (msg, sender, errorLevel) {
            that.logToAll(msg, sender, errorLevel);
        },
        /**
         * Function to add a new Logger
         * @param {class} logger - The logger-class to be added. Must have a log() method!
         * @param {SecureExam.Logger.ErrorLevel} errorLevel - Minimal error level for messages which the logger should receive
         * @memberof SecureExam.Logger
         */
        addLogger: function (logger, errorLevel) {
            that.loggers[errorLevel].push(logger);
        },
        /**
         * Function to remove a Logger
         * @param {class} logger - The logger-class to be removed.
         * @memberof SecureExam.Logger
         */
        removeLogger: function (logger) {
            for (var i = 0; i < that.loggers.length; i++) {
                if (that.loggers[i] === logger) {
                    that.loggers.splice(i, 1);
                }
            }
        },
        /**
         * ErrorLevel Enum
         * @memberof SecureExam.Logger
         */
        ErrorLevel: that.ErrorLevel
    }
});


/**
 * HTMLInfo Class.
 * Stores HTML Div id's
 * @memberof SecureExam.Lib
 * @class
 * @param {string} divIDUserDB - ID of the Div with the UserDB
 * @param {string} divIDEncryptedData - ID of the Div with the encrypted data
 * @param {string} divIDQuestions - ID of the Div in whitch the questions will be printed
 * @param {string} viewMode - ViewMode: Scroll or Page
 * @this HTMLInfo
 */
SecureExam.Lib.HTMLInfo = function (divIDUserDB, divIDEncryptedData, divIDQuestions, viewMode) {
    var that = this;
    this.DivUserDB = document.getElementById(divIDUserDB);
    this.DivEncryptedData = document.getElementById(divIDEncryptedData);
    this.DivQuestions = document.getElementById(divIDQuestions);
    this.viewMode = viewMode;
}


/**
 * SecureExamSettings Class.
 * Stores Settings to current Exam instance with abbility to save and load from localdb
 * @memberof SecureExam.Lib
 * @class
 * @param {string} userSecret - The UserSecret provided by the participant
 * @this SecureExam.Lib.SecureExamSettings
 */
SecureExam.Lib.SecureExamSettings = function (userSecret) {
    var that = this;
    /** @type {Date}*/
    this.examStartTime = new Date();
    /** @type {?Date}*/
    this.examExportedTime = null;
    /** @type {?Date}*/
    this.examExpireTime = null;
    /** @type {int}*/
    this.examStartings = 1;
    /** @type {int}*/
    this.examFocusChanges = 0;
    /** @type {boolean}*/
    this.examValid = true;
    /** @type {?string}*/
    this.invalidLog = null;
    /** @type {string}*/
    this.studentSecret = userSecret;
    /** @type {?Date}*/
    this.overallStartTime = null;
    /** @type {?Date}*/
    this.overallEndTime = null;
    /**
     * Function to load Settings from localDB or create a new one
     * @function
     */
    this.load = function () {
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
                SecureExam.Logger.log("loading new instance", "SecureExamSettings", SecureExam.Logger.ErrorLevel.info);
            }
        } catch (err) {
            SecureExam.Logger.log("loading old instance failed, created new one", "SecureExamSettings", SecureExam.Logger.ErrorLevel.warning);
        }
    }();
    /**
     * Function to save the settings to localdb
     * @function
     */
    this.save = function () {
        var exportString = 'secureExamInfos(' + that.examStartTime + ',' + that.examExportedTime + ',' + that.examStartings + ',' + that.studentSecret + ',' + that.examFocusChanges + ',' + that.examValid + ',' + that.examExpireTime + ',' + that.invalidLog + ',' + that.overallStartTime + ',' + that.overallEndTime + ')';
        window.localStorage.setItem("secureExam", CryptoJS.AES.encrypt(exportString, that.studentSecret));
        SecureExam.Logger.log("saved instance to loaldDB", "SecureExamSettings", SecureExam.Logger.ErrorLevel.info);
    }
};


/**
 * InternetAccessCheck Class.
 * class to check internet Access and if so, fire events to listeners
 * @memberof SecureExam.Lib.Security
 * @class
 * @this SecureExam.Lib.Security.InternetAccessCheck
 */
SecureExam.Lib.Security.InternetAccessCheck = function () {
    var that = this;
    /** @type {int}*/
    this.intervalTimeout = 1000;
    /** @type {string}*/
    this.imgURL = "http://waikiki.zhaw.ch/~rege/t-menu/header_left.jpg";
    /** @type {?int}*/
    this.interval = null;
    /** @type {boolean}*/
    this.started = false;
    /** @type {Array.<SecureExam.Event.InternetAccess, function()>}*/
    this.eventListeners = [];
    this.eventListeners[SecureExam.Event.InternetAccess.ONLINE] = [];
    this.eventListeners[SecureExam.Event.InternetAccess.OFFLINE] = [];

    /**
     * Function which is called when the image is able to load => internet online
     * @function
     */
    this.onLoad = function () {
        SecureExam.Logger.log("internet online", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.error);
        that.riseEvent(SecureExam.Event.InternetAccess.ONLINE, "internet online");
    }
    
    /**
     * Function which is called when the image is not able to load => internet offline
     * @function
     */
    this.onError = function () {
        SecureExam.Logger.log("internet offline", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
        that.riseEvent(SecureExam.Event.InternetAccess.OFFLINE, "internet offline");
    }
    
    /**
     * Function which is periodicaly called to check for an active internet connection
     * @function
     */
    this.check = function () {
        SecureExam.Logger.log("checking internet", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
        var img = new Image();
        img.onload = that.onLoad;
        img.onerror = that.onError;
        img.src = that.imgURL + '?t=' + new Date().getTime();
    }
    
    /**
     * Function to rise an event
     * @function
     * @param {SecureExam.Event.InternetAccess} event - event that should be rised
     * @param {string} msg - event message
     */
    this.riseEvent = function (event, msg) {
        for (var i = 0; i < that.eventListeners[event].length; i++) {
            that.eventListeners[event][i](msg);
        }
    }

    /**
     * Function which starts the internet access check interval
     * there needs to be an active eventlistener. otherwise it doesn't do nothing ;)
     * @function
     * @private
     */
    this.start = function () {
        if (that.eventListeners[SecureExam.Event.InternetAccess.ONLINE].length > 0 || that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE].length > 0) {
            SecureExam.Logger.log("started...", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
            that.interval = window.setInterval(that.check, that.intervalTimeout);
            that.started = true;
        }
    }

    /**
     * Function to stop the online checks
     * @function
     * @private
     */
    this.stop = function () {
        SecureExam.Logger.log("stopped...", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
        clearInterval(that.interval);
        that.started = false;
    }

    /**
     * Function for adding a new eventlistener
     * @function
     * @private
     * @param {SecureExam.Event.InternetAccess} event - event that should be rised
     * @param {function()} listener - function which gets called
     */
    this.addEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case SecureExam.Event.InternetAccess.ONLINE:
            that.eventListeners[SecureExam.Event.InternetAccess.ONLINE].push(listener);
            SecureExam.Logger.log("added listener to ONLINE", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
            break;
        case SecureExam.Event.InternetAccess.OFFLINE:
            that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE].push(listener);
            SecureExam.Logger.log("added listener to OFFLINE", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
            break;
        }

        if (that.interval === null && that.started) {
            that.start();
        }
    }

    /**
     * Function for removing a new eventlistener
     * @function
     * @private
     * @param {SecureExam.Event.InternetAccess} event - to which event the listener has been added
     * @param {function()} listener - function which got called
     */
    this.removeEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case SecureExam.Event.InternetAccess.ONLINE:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.InternetAccess.ONLINE].length; i++) {
                if (that.eventListeners[SecureExam.Event.InternetAccess.ONLINE][i] === listener) {
                    that.eventListeners[SecureExam.Event.InternetAccess.ONLINE].splice(i, 1);
                }
                SecureExam.Logger.log("removed listener from ONLINE", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
            }
            break;
        case tSecureExam.Event.InternetAccess.OFFLINE:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE].length; i++) {
                if (that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE][i] === listener) {
                    that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE].splice(i, 1);
                }
                SecureExam.Logger.log("removed listener from OFFLINE", "InternetAccessCheck", SecureExam.Logger.ErrorLevel.info);
            }
            break;
        }
    }

    return {
        /**
         * Function for removing a new eventlistener
         * @function
         * @memberof SecureExam.Lib.Security.InternetAccessCheck
         * @param {int} timeout - timeout for interval in milliseconds 
         */
        setIntervalTimeout: function (timeout) {
            that.stop();
            that.intervalTimeout = timeout;
            that.start();
        },
        /**
         * Function for startic the service
         * @function
         * @memberof SecureExam.Lib.Security.InternetAccessCheck
         */
        start: function () {
            that.start();
        },
        /**
         * Function for stopping the service
         * @function
         * @memberof SecureExam.Lib.Security.InternetAccessCheck
         */
        stop: function () {
            that.stop();
        },
        /**
         * Function for adding a new eventlistener
         * @function
         * @memberof SecureExam.Lib.Security.InternetAccessCheck
         * @param {SecureExam.Event.InternetAccess} event - event that should be rised
         * @param {function()} listener - function which gets called
         */
        addEventListener: function (event, listener) {
            that.addEventListener(event, listener);
        },
        /**
         * Function for removing a new eventlistener
         * @function
         * @memberof SecureExam.Lib.Security.InternetAccessCheck
         * @param {SecureExam.Event.InternetAccess} event - to which event the listener has been added
         * @param {function()} listener - function which got called
         */
        removeEventListener: function (event, listener) {
            that.removeEventListener(event, listener);
        },
        /**
         * Function for removing all eventlisteners
         * @function
         * @memberof SecureExam.Lib.Security.InternetAccessCheck
         */
        removeAllEventListeners: function () {
            for (var i = 0; i < that.eventListeners[SecureExam.Event.InternetAccess.ONLINE].length; i++) {
                that.removeEventListener(SecureExam.Event.InternetAccess.ONLINE, that.eventListeners[SecureExam.Event.InternetAccess.ONLINE][i]);
            }
            for (var i = 0; i < that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE].length; i++) {
                that.removeEventListener(SecureExam.Event.InternetAccess.OFFLINE, that.eventListeners[SecureExam.Event.InternetAccess.OFFLINE][i]);
            }
        }
    }
}

/**
 * SecureTime Class.
 * class to handle save, reliable and non-manipulatable Time.
 * @memberof SecureExam.Lib.Security
 * @class
 * @this SecureExam.Lib.Security.SecureTime
 */
SecureExam.Lib.Security.SecureTime = function () {
    var that = this;
    /** @type {boolean}*/
    this.started = false;
    /** @type {int}*/
    this.INTERNALUPDATEINTERVAL = 1000;
    /** @type {int}*/
    this.INTERNALCLOCKMAXVARIANCE = 50;
    /** @type {int}*/
    this.TIMEHISTORYMAXVARIANCE = 50;
    /** @type {Array.<Date>}*/
    this.timeHistory = new Array();
    /** @type {int}*/
    this.internalClockMilliseconds = 0;
    /** @type {Array.<SecureExam.Event.SecureTime, function()>}*/
    this.eventListeners = [];
    this.eventListeners[SecureExam.Event.SecureTime.TIMEERROR] = [];
    this.eventListeners[SecureExam.Event.SecureTime.TABCHANGE] = [];
    /** @type {?Date}*/
    this.internalClockStartTime = null;
    /** @type {?int}*/
    this.interval = null;

    /**
     * Function to update internal time and timehistory.
     * is called each second
     * @function
     */
    this.update = function () {
        SecureExam.Logger.log("updating time", "secureDate", SecureExam.Logger.ErrorLevel.info);
        var systemTime = new Date();
        that.internalClockMilliseconds += that.INTERNALUPDATEINTERVAL;
        var internalTime = that.getInternalTime();

        // verify InternalTime
        if (that.dateCompare(internalTime, systemTime)) {
            // compensate cpu lag in internal Time (systemTime is more accurate)
            var diff = systemTime.getTime() - internalTime.getTime();
            SecureExam.Logger.log("systime: " + systemTime.toHHMMSSMSString() + " internal: " + internalTime.toHHMMSSMSString(), "secureDate", SecureExam.Logger.ErrorLevel.info);
            if (diff > 0) {
                that.internalClockStartTime.setTime(that.internalClockStartTime.getTime() + diff);
                SecureExam.Logger.log("compensing cpu lag: " + diff + "ms", "secureDate", SecureExam.Logger.ErrorLevel.warning);
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
                    that.riseEvent(SecureExam.Event.SecureTime.TIMEERROR, "HistoryTime([" + i + "]," + recalculatedTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!");
                    SecureExam.Logger.log("HistoryTime([" + i + "]," + recalculatedTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!", "secureDate", SecureExam.Logger.ErrorLevel.error);
                    return false;
                }
            }
            SecureExam.Logger.log("UPDATED: status OK | internalclock @ " + internalTime.toHHMMSSMSString() + " historynewest @ " + that.timeHistory[that.timeHistory.length - 1].toHHMMSSMSString(), "secureDate", SecureExam.Logger.ErrorLevel.info);
            return true;
        } else {
            that.riseEvent(SecureExam.Event.SecureTime.TIMEERROR, "InternalTime(" + internalTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!");
            SecureExam.Logger.log("InternalTime(" + internalTime.toHHMMSSMSString() + ") and SystemTime(" + systemTime.toHHMMSSMSString() + ") not in sync!", "secureDate", SecureExam.Logger.ErrorLevel.error);
            return false;
        }
    }

    /**
     * Function to compare dates
     * @function
     * @param {Date} actualTime - actual time
     * @param {Date} timeToVerify - time which will be checked
     * @param {Date} maxVariance - max variance between the times in milliseconds
     * @returns {int}
     */
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
    
    /**
     * Function to rise an event
     * @function
     * @param {SecureExam.Event.SecureTime} event - event that should be rised
     * @param {string} msg - event message
     */
    this.riseEvent = function (event, msg) {
        for (var i = 0; i < that.eventListeners[event].length; i++) {
            that.eventListeners[event][i](msg);
        }
    }

    /**
     * Function to start the clock
     * @function
     * @private
     */
    this.start = function () {
        that.internalClockStartTime = new Date();
        that.interval = window.setInterval(that.update, that.INTERNALUPDATEINTERVAL);
        SecureExam.Logger.log("started...", "secureDate", SecureExam.Logger.ErrorLevel.info);
        this.started = true;
    }

    /**
     * Function to stop the clock
     * @function
     * @private
     */
    this.stop = function () {
        clearInterval(that.interval);
        SecureExam.Logger.log("stopped...", "secureDate", SecureExam.Logger.ErrorLevel.info);
        this.started = false;
    }

    /**
     * Function which gets called on tab change
     * @function
     * @param {object} e - event object
     */
    this.visibilityChanged = function (e) {
        if (document.hidden) {
            SecureExam.Logger.log("tab hidden", "secureDate", SecureExam.Logger.ErrorLevel.warning);
            that.riseEvent(SecureExam.Event.SecureTime.TABCHANGE, "hidden");
        } else {
            SecureExam.Logger.log("tab visible", "secureDate", SecureExam.Logger.ErrorLevel.warning);
            that.riseEvent(SecureExam.Event.SecureTime.TABCHANGE, "visible");
        }
    }

    /**
     * Function to add a new tabchangelistener
     * @function
     */
    this.addVisibilityChangeListener = function () {
        document.addEventListener("visibilitychange", that.visibilityChanged);
    }

    /**
     * Function to remove a tabchangelistener
     * @function
     */
    this.removeVisibilityChangeListener = function () {
        document.removeEventListener("visibilitychange", that.visibilityChanged);
    }

    /**
     * Function which returns the internal time
     * @function
     * @private
     * @returns {Date}
     */
    this.getInternalTime = function () {
        var date = new Date(that.internalClockStartTime);
        date.setMilliseconds(date.getMilliseconds() + that.internalClockMilliseconds);
        return date;
    }
    
    /**
     * Function for adding a new eventlistener
     * @function
     * @private
     * @param {SecureExam.Event.SecureTime} event - event that should be rised
     * @param {function()} listener - function which gets called
     */
    this.addEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case SecureExam.Event.SecureTime.TIMEERROR:
            that.eventListeners[SecureExam.Event.SecureTime.TIMEERROR].push(listener);
            SecureExam.Logger.log("added listener to TIMEERROR", "secureDate", SecureExam.Logger.ErrorLevel.info);
            if (that.started && that.interval === null) {
                that.start();
            }
            break;
        case SecureExam.Event.SecureTime.TABCHANGE:
            that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE].push(listener);
            SecureExam.Logger.log("added listener to TABCHANGE", "secureDate", SecureExam.Logger.ErrorLevel.info);

            if (that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE].length == 1) {
                that.addVisibilityChangeListener();
            }
            break;
        }
    }
    
    /**
     * Function for removing a new eventlistener
     * @function
     * @private
     * @param {SecureExam.Event.SecureTime} event - to which event the listener has been added
     * @param {function()} listener - function which got called
     */
    this.removeEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case SecureExam.Event.SecureTime.TIMEERROR:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.SecureTime.TIMEERROR].length; i++) {
                if (that.eventListeners[SecureExam.Event.SecureTime.TIMEERROR][i] === listener) {
                    that.eventListeners[SecureExam.Event.SecureTime.TIMEERROR].splice(i, 1);
                }
            }
            SecureExam.Logger.log("removed listener from TIMEERROR", "secureDate", SecureExam.Logger.ErrorLevel.info);
            break;
        case SecureExam.Event.SecureTime.TABCHANGE:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE].length; i++) {
                if (that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE][i] === listener) {
                    that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE].splice(i, 1);
                }
            }
            SecureExam.Logger.log("removed listener from TABCHANGE", "secureDate", SecureExam.Logger.ErrorLevel.info);

            if (that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE].length == 0) {
                that.removeVisibilityChangeListener();
            }
            break;
        }
    }

    return {
        /**
         * Function which returns the internal time
         * @function
         * @returns {Date}
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        getInternalTime: function () {
            return that.getInternalTime();
        },
        /**
         * Function to start the clock
         * @function
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        start: function () {
            that.start();
        },
        /**
         * Function to stop the clock
         * @function
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        stop: function () {
            that.stop();
        },
        /**
         * Function for adding a new eventlistener
         * @function
         * @param {SecureExam.Event.SecureTime} event - event that should be rised
         * @param {function()} listener - function which gets called
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        addEventListener: function (event, listener) {
            that.addEventListener(event, listener);
        },
        /**
         * Function for removing a new eventlistener
         * @function
         * @param {SecureExam.Event.SecureTime} event - to which event the listener has been added
         * @param {function()} listener - function which got called
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        removeEventListener: function (event, listener) {
            that.removeEventListener(event, listener);
        },
        /**
         * Function for removing all event listeners
         * @function
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        removeAllEventListeners: function () {
            for (var i = 0; i < that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE].length; i++) {
                that.removeEventListener(SecureExam.Event.SecureTime.TABCHANGE, that.eventListeners[SecureExam.Event.SecureTime.TABCHANGE][i]);
            }
            for (var i = 0; i < that.eventListeners[SecureExam.Event.SecureTime.TIMEERROR].length; i++) {
                that.removeEventListener(SecureExam.Event.SecureTime.TIMEERROR, that.eventListeners[SecureExam.Event.SecureTime.TIMEERROR][i]);
            }
        },
        /**
         * Function for seting the history tTime variance
         * @function
         * @param {int} ms - Variance in milliseconds
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        setHistoryTimeMaxVariance: function (ms) {
            that.TIMEHISTORYMAXVARIANCE = ms;
        },
        /**
         * Function for seting the internal time variance
         * @function
         * @param {int} ms - Variance in milliseconds
         * @memberof SecureExam.Lib.Security.SecureTime
         */
        setInternalTimeMaxVariance: function (ms) {
            that.INTERNALCLOCKMAXVARIANCE = ms;
        }
    }
}

/**
 * XMLLogger Class.
 * class to store logs and export them as xml string.
 * @memberof SecureExam.Lib
 * @class
 * @param {int} limit - max size of logger cache
 * @this SecureExam.Lib.XMLLogger
 */
SecureExam.Lib.XMLLogger = function (limit) {
    var that = this;
    /** @type {int}*/
    this.limit = limit;
    /** @type {Array.<string>}*/
    this.entrys = [];

    /**
     * Function to log a message to the cache
     * @function
     * @param {string} msg - message to log
     */
    this.log = function (msg) {
        if (that.entrys.length > that.limit) {
            that.entrys.shift();
        }
        that.entrys.push(msg);
    }

    /**
     * Function to export the whole log as xml document
     * @function
     * @returns {string}
     */
    this.exportLog = function () {
        var xml = '<log>';
        for (var i = 0; i < that.entrys.length; i++) {
            xml += '<entry>' + that.entrys[i] + '</entry>';
        }
        xml += '</log>';
        return xml;
    }
}


/**
 * EBookReader Class.
 * class to handle eBook-reader-mode
 * @memberof SecureExam.UI.ViewMode
 * @class
 * @this SecureExam.UI.ViewMode.EBookReader
 */
SecureExam.UI.ViewMode.EBookReader = function () {
    var that = this;
    /** @type {Div}*/
    this.questionDiv = document.getElementById("exam");
    /** @type {Array.<Div>}*/
    this.quesions = [];
    /** @type {int}*/
    this.currentQuestion = 0;
    /** @type {int}*/
    this.totalQuestions = 0;
    
    /**
     * Function to show a question
     * @function
     * @params {int} id - question id
     * @private
     */
    this.showQuestion = function( id ) {
        that.questions[that.currentQuestion].style.display = 'block';
    };
    
    /**
     * Function to hide the current question
     * @function
     * @private
     */
    this.hideCurrentQuestion = function( ) {
        that.hideQuestion(that.currentQuestion);
    };
    
    /**
     * Function to hide a question
     * @function
     * @params {int} id - question id
     * @private
     */
    this.hideQuestion = function( id ) {
        that.questions[id].style.display = 'none';
    };

    /**
     * Initializes all questions, starts with showing the first question
     * @function
     * @params {int} id - question id
     * @constructs
     * @private
     */
    this.init = function () {
        that.questions = that.questionDiv.children;
        that.totalQuestions = that.questions.length;

        for( var i = 1; i < that.totalQuestions; i++) {
            that.hideQuestion(i);
        }
        that.showQuestion(1);
    }();
    
    return {
        /**
         * Function for showing the next question
         * @function
         * @memberof SecureExam.UI.ViewMode.EBookReader
         */
        next: function() {
            that.hideCurrentQuestion();
            that.currentQuestion = (that.currentQuestion + 1) % that.totalQuestions;
            that.showQuestion( that.currentQuestion );
        },
        /**
         * Function for showing the previous question
         * @function
         * @memberof SecureExam.UI.ViewMode.EBookReader
         */
        prev: function() {
            that.hideCurrentQuestion();
            that.currentQuestion = (that.currentQuestion === 0)?that.totalQuestions -1: that.currentQuestion -1;
            that.showQuestion( that.currentQuestion );
        },
        /**
         * Function for showing a specific question
         * @function
         * @param {int} nr - question id
         * @memberof SecureExam.UI.ViewMode.EBookReader
         */
        toQuestion: function (nr) {
            if( nr >= 0 && nr < that.totalQuestions) {
                that.hideCurrentQuestion();
                that.showQuestion(nr);
            }
        }
    };
}



/**
 * Exam Class.
 * Main class, starts and stops everything. dont mess with it! ;)
 * @memberof SecureExam
 * @class
 * @this SecureExam.Exam
 */
SecureExam.Exam = function (htmlInfo) {
    var that = this;
    /** @type {SecureExam.Lib.Security.InternetAccessCheck} */
    this.InternetAccess = new SecureExam.Lib.Security.InternetAccessCheck();
    /** @type {SecureExam.Lib.Security.SecureTime} */
    this.SecureTime = new SecureExam.Lib.Security.SecureTime();
    /** @type {?SecureExam.Lib.SecureExamSettings} */
    this.Settings = null;
    /** @type {boolean} */
    this.running = (this.Settings === null) ? false : true;
    /** @type {?int} */
    this.timeLeft = null;
    /** @type {?int} */
    this.timeLeftInterval = null;
    /** @type {int} */
    this.autoSaveTimeout = 5000;
    /** @type {?int} */
    this.autoSaveInterval = null;
    /** @type {boolean} */
    this.confirmAutoSaveRestore = false;
    /** @type {boolean} */
    this.eBookReaderExport = true;
    /** @type {Array.<SecureExam.Event, function()>} */
    this.eventListeners = [];
    this.eventListeners[SecureExam.Event.TIMELEFT] = [];
    this.eventListeners[SecureExam.Event.AUTOSAVE] = [];
    this.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED] = [];
    /** @type {Object} */
    this.User = {
        firstname: null,
        lastname: null,
        immNumber: null,
        secret: null,
        fullUserID: function () {
            return that.User.firstname + that.User.lastname + that.User.immNumber;
        },
        toString: function () {
            return that.User.firstname + that.User.lastname + that.User.immNumber + that.User.secret;
        }
    };
    if (htmlInfo instanceof SecureExam.Lib.HTMLInfo) {
        /** @type {SecureExam.Lib.HTMLInfo} */
        this.HTMLInfo = htmlInfo
    } else {
        throw SecureExam.ErrorCode.INVALIDARGUMENT;
    }
    /** @type {SecureExam.Lib.XMLLogger} */
    this.xmlLogger = new SecureExam.Lib.XMLLogger(100);
    SecureExam.Logger.addLogger(this.xmlLogger, SecureExam.Logger.ErrorLevel.info);

    this.init = function (firstname, lastname, immNumber, secret, continueRestore) {
        that.User.firstname = firstname;
        that.User.lastname = lastname;
        that.User.immNumber = immNumber;
        that.User.secret = secret;

        that.Settings = new SecureExam.Lib.SecureExamSettings(that.User.toString());

        if (that.Settings.examExportedTime === null) {
            if (!continueRestore) {
                if (!that.tryRestoreSavePoint(that.User.toString())) {
                    that.decryptQuestions(that.User.toString());
                }
            } else {
                that.tryRestoreSavePoint(that.User.toString(), continueRestore);
            }
            if (that.Settings.overallEndTime >= new Date()) {
                if (that.Settings.overallStartTime <= new Date()) {
                    that.calculateTimeLeft();
                    that.Settings.save();
                    that.addEventListener(SecureExam.Event.TIMELEFT, that.examTimeExpiredCheck);
                    that.InternetAccess.start();
                    that.SecureTime.start();
                    that.startItervals();
                    SecureExam.Logger.log("exam started!", "exam", SecureExam.Logger.ErrorLevel.info);
                } else {
                    SecureExam.Logger.log("can not start exam, too early!", "exam", SecureExam.Logger.ErrorLevel.info);
                    throw SecureExam.ErrorCode.TOOEARLY;
                }
            } else {
                SecureExam.Logger.log("can not start exam, too late!", "exam", SecureExam.Logger.ErrorLevel.info);
                throw SecureExam.ErrorCode.TOOLATE;
            }
        } else {
            SecureExam.Logger.log("exam has already been done", "exam", SecureExam.Logger.ErrorLevel.info);
            throw SecureExam.ErrorCode.ALREADYEXPORTED;
        }
    }

    this.startItervals = function () {
        // AutoSave
        if (that.autoSaveInterval === null && that.eventListeners[SecureExam.Event.AUTOSAVE].length !== 0) {
            that.autoSaveInterval = window.setInterval(that.autoSave, that.autoSaveTimeout);
            SecureExam.Logger.log("started autoSaveInterval", "exam", SecureExam.Logger.ErrorLevel.info);
        }

        // TimeLeft
        if (that.timeLeftInterval === null)
            that.timeLeftInterval = window.setInterval(that.calculateTimeLeft, 1000);
        SecureExam.Logger.log("started timeLeftInterval", "exam", SecureExam.Logger.ErrorLevel.info); {}
    }

    this.tryRestoreSavePoint = function (userSecret, continueRestore) {
        if (window.localStorage.getItem("secureExamAutoSave") !== null) {
            try {
                var dec = CryptoJS.AES.decrypt(window.localStorage.getItem("secureExamAutoSave"), userSecret);
                var decString = dec.toString(CryptoJS.enc.Utf8);
                if (that.confirmAutoSaveRestore && !continueRestore) {
                    throw SecureExam.ErrorCode.CONFIRMAUTOSAVERESTORE;
                }
                that.HTMLInfo.DivQuestions.innerHTML = dec.toString(CryptoJS.enc.Utf8);
                that.calculateTimeLeft(true);
                SecureExam.Logger.log("restored savepoint", "exam", SecureExam.Logger.ErrorLevel.info);
                return true;
            } catch (e) {
                if (e === SecureExam.ErrorCode.CONFIRMAUTOSAVERESTORE) {
                    throw SecureExam.ErrorCode.CONFIRMAUTOSAVERESTORE;
                } else {
                    SecureExam.Logger.log("savepoint found but from different user", "exam", SecureExam.Logger.ErrorLevel.info);
                }
            }
        }
        SecureExam.Logger.log("there is no old savepoint", "exam", SecureExam.Logger.ErrorLevel.info);
        return false;
    }

    this.decryptQuestions = function (userSecret) {
        try {
            SecureExam.Logger.log("decrypting questions...", "exam", SecureExam.Logger.ErrorLevel.info);
            // load userKeyDB & questionDiv
            var userKeyDB = that.HTMLInfo.DivUserDB.innerHTML.split(";");
            var questionDiv = that.HTMLInfo.DivEncryptedData.innerHTML.split(",");

            // remove empty last entry from userKeyDB
            userKeyDB.pop();

            // userKeyDB: split username from secret
            for (var i = 0; i < userKeyDB.length; i++) {
                userKeyDB[i] = userKeyDB[i].split(",");
            }
            SecureExam.Logger.log("userKeyDB loaded with [" + userKeyDB.length + "] entrys", "exam", SecureExam.Logger.ErrorLevel.info);

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
                    SecureExam.Logger.log("found matching cipherparameters to usersecret", "exam", SecureExam.Logger.ErrorLevel.info);
                    break;
                }

            }

            // generate key
            var userSecretSalted = that.User.toString() + saltB64;
            var key = CryptoJS.SHA256(userSecretSalted);
            for (var i = 0; i < SecureExam.Const.Cryptography.SHA256ITERATIONS - 1; i++) {
                key = CryptoJS.SHA256(key);
            }
            SecureExam.Logger.log("key hashing complete", "exam", SecureExam.Logger.ErrorLevel.info);

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
            SecureExam.Logger.log("masterkey decrypted", "exam", SecureExam.Logger.ErrorLevel.info);

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
            SecureExam.Logger.log("data decrypted", "exam", SecureExam.Logger.ErrorLevel.info);

            var decryptedData = decryptedString.split(",");
            that.Settings.overallStartTime = new Date(Number(decryptedData[0]));
            that.Settings.overallEndTime = new Date(Number(decryptedData[1]));
            that.Settings.examExpireTime = new Date(that.Settings.examStartTime.getTime() + (Number(decryptedData[2]) * 60 * 1000));
            that.riseEvent(SecureExam.Event.TIMELEFT, decryptedData[2] + ":00");
            SecureExam.Logger.log("important times decrypted and set", "exam", SecureExam.Logger.ErrorLevel.info);

            // print questions, iterate as there are maybe some "," in the text.. 
            var questionsHTML = "";
            for (var i = 3; i < decryptedData.length; i++) {
                questionsHTML += decryptedData[i];
            }
            that.HTMLInfo.DivQuestions.innerHTML = questionsHTML;
        } catch (e) {
            SecureExam.Logger.log("Invalid usersecret!", "exam", SecureExam.Logger.ErrorLevel.error);
            throw SecureExam.ErrorCode.INVALIDUSERSECRET;
        }
    }

    this.stop = function () {
        if (that.Settings.examExportedTime === null) {
            SecureExam.Logger.log("stopping exam", "exam", SecureExam.Logger.ErrorLevel.info);
            that.Settings.examExportedTime = new Date();

            that.export();
            that.Settings.save();

            if( that.timeLeft <= 0 ) {
                that.riseEvent(SecureExam.Event.EXAMTIMEEXPIRED, null);
            }
        }
        that.removeAllEventListeners();
        that.InternetAccess.stop();
        that.SecureTime.stop();
    }

    this.removeAllEventListeners = function () {
        for (var i = 0; i < that.eventListeners[SecureExam.Event.AUTOSAVE].length; i++) {
            that.removeEventListener(SecureExam.Event.AUTOSAVE, that.eventListeners[SecureExam.Event.AUTOSAVE][i]);
        }
        for (var i = 0; i < that.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED].length; i++) {
            that.removeEventListener(SecureExam.Event.EXAMTIMEEXPIRED, that.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED][i]);
        }
        for (var i = 0; i < that.eventListeners[SecureExam.Event.TIMELEFT].length; i++) {
            that.removeEventListener(SecureExam.Event.TIMELEFT, that.eventListeners[SecureExam.Event.TIMELEFT][i]);
        }
        that.SecureTime.removeAllEventListeners();
        that.InternetAccess.removeAllEventListeners();
    }

    this.export = function () {
        if( that.eBookReaderExport ) {
            that.autoSave();
            window.localStorage.setItem("secureExamUserSecret", that.User.toString());
            window.localStorage.setItem("secureExamLog", that.xmlLogger.exportLog());
        } else {
            var xml = that.generateExportXML();
            var encXml = CryptoJS.AES.encrypt(xml, that.User.toString());
            xml += encXml.iv.toString() + "," + encXml.ciphertext.toString();

            // generate download
            var blob = new Blob([xml], {
                type: "text/plain;charset=utf-8"
            });
            saveAs(blob, "exam_" + that.User.firstname + that.User.lastname + that.User.immNumber + ".xml.enc");   
        }
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
        xml += that.xmlLogger.exportLog();
        xml += '</exam>';
        return xml;
    }

    this.examTimeExpiredCheck = function () {
        if (that.timeLeft <= 0 || that.Settings.overallEndTime <= new Date()) {
            SecureExam.Logger.log("exam expired", "exam", SecureExam.Logger.ErrorLevel.info);
            that.stop();
        }
    }

    this.calculateTimeLeft = function (forcePrint) {
        var now = new Date();
        var diff = (that.Settings.examExpireTime - now) / 1000;
        if (diff > 0 || forcePrint) {
            var seconds = (Math.floor(diff % 60) < 10) ? "0" + Math.floor(diff % 60) : Math.floor(diff % 60);
            var minutes = (Math.floor(diff / 60) < 10) ? "0" + Math.floor(diff / 60) : Math.floor(diff / 60);

            if (minutes >= 10 && seconds !== "00") {
                return;
            }
            var newTime = minutes + ":" + seconds;
            if (newTime !== that.timeLeft) {
                that.timeLeft = Number((minutes * 60) + seconds);
                that.riseEvent(SecureExam.Event.TIMELEFT, newTime);
            }
        } else {
            that.timeLeft = diff;
            that.examTimeExpiredCheck();
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
        that.riseEvent(SecureExam.Event.AUTOSAVE, new Date());
    }

    this.addEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case SecureExam.Event.TIMELEFT:
            that.eventListeners[SecureExam.Event.TIMELEFT].push(listener);
            SecureExam.Logger.log("added listener to TIMELEFT", "exam", SecureExam.Logger.ErrorLevel.info);
            break;
        case SecureExam.Event.AUTOSAVE:
            that.eventListeners[SecureExam.Event.AUTOSAVE].push(listener);
            if (that.running && that.eventListeners[SecureExam.Event.AUTOSAVE].length === 1) {
                that.startItervals();
            }
            SecureExam.Logger.log("added listener to TIMELEFT", "exam", SecureExam.Logger.ErrorLevel.info);
            break;
        case SecureExam.Event.EXAMTIMEEXPIRED:
            that.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED].push(listener);
            SecureExam.Logger.log("added listener to EXAMTIMEEXPIRED", "exam", SecureExam.Logger.ErrorLevel.info);
            break;
        }
    }

    this.removeEventListener = function (event, listener) {
        switch (event.toLowerCase()) {
        case SecureExam.Event.TIMELEFT:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.TIMELEFT].length; i++) {
                if (that.eventListeners[SecureExam.Event.TIMELEFT][i] === listener) {
                    that.eventListeners[SecureExam.Event.TIMELEFT].splice(i, 1);
                }
            }
            if (that.eventListeners[SecureExam.Event.TIMELEFT].length === 0) {
                clearInterval(that.timeLeftInterval);
            }
            SecureExam.Logger.log("removed listener from TIMELEFT", "exam", SecureExam.Logger.ErrorLevel.info);
            break;
        case SecureExam.Event.AUTOSAVE:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.AUTOSAVE].length; i++) {
                if (that.eventListeners[SecureExam.Event.AUTOSAVE][i] === listener) {
                    that.eventListeners[SecureExam.Event.AUTOSAVE].splice(i, 1);
                }
            }
            if (that.eventListeners[SecureExam.Event.AUTOSAVE].length === 0) {
                clearInterval(that.autoSaveInterval);
            }
            SecureExam.Logger.log("removed listener from AUTOSAVE", "exam", SecureExam.Logger.ErrorLevel.info);
            break;
        case SecureExam.Event.EXAMTIMEEXPIRED:
            for (var i = 0; i < that.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED].length; i++) {
                if (that.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED][i] === listener) {
                    that.eventListeners[SecureExam.Event.EXAMTIMEEXPIRED].splice(i, 1);
                }
            }
            SecureExam.Logger.log("removed listener from EXAMTIMEEXPIRED", "exam", SecureExam.Logger.ErrorLevel.info);
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
                that.init(firstname, lastname, immNumber, secret);
            } else
                throw SecureExam.ErrorCode.INVALIDARGUMENT;
        },
        stop: function () {
            that.stop();
        },
        addEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case SecureExam.Event.InternetAccess.ONLINE:
            case SecureExam.Event.InternetAccess.OFFLINE:
                that.InternetAccess.addEventListener(event, listener);
                break;
            case SecureExam.Event.SecureTime.TIMEERROR:
            case SecureExam.Event.SecureTime.TABCHANGE:
                that.SecureTime.addEventListener(event, listener);
                break;
            case SecureExam.Event.TIMELEFT:
            case SecureExam.Event.AUTOSAVE:
            case SecureExam.Event.EXAMTIMEEXPIRED:
                that.addEventListener(event, listener);
                break;
            default:
                throw SecureExam.ErrorCode.INVALIDEVENT;
            }
        },
        removeEventListener: function (event, listener) {
            switch (event.toLowerCase()) {
            case SecureExam.Event.InternetAccess.ONLINE:
            case SecureExam.Event.InternetAccess.OFFLINE:
                that.InternetAccess.addEventListener(event, listener);
                break;
            case SecureExam.Event.SecureTime.TIMEERROR:
            case SecureExam.Event.SecureTime.TABCHANGE:
                that.SecureTime.addEventListener(event, listener);
                break;
            case SecureExam.Event.TIMELEFT:
            case SecureExam.Event.AUTOSAVE:
            case SecureExam.Event.EXAMTIMEEXPIRED:
                that.removeEventListener(event, listener);
                break;
            default:
                throw SecureExam.ErrorCode.INVALIDEVENT;
            }
        },
        exportExam: function () {
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
                throw SecureExam.ErrorCode.INVALIDARGUMENT;
            }
        },
        setHistoryTimeMaxVariance: function (ms) {
            if (ms >= 20) {
                that.SecureTime.setHistoryTimeMaxVariance(ms);
            } else {
                throw SecureExam.ErrorCode.INVALIDARGUMENT;
            }
        },
        setAutoSaveTimout: function (ms) {
            that.autoSaveTimeout = ms;
        },
        isExported: function () {
            return (that.Settings.examExportedTime !== null);
        },
        setConfirmAutoSaveRestore: function (value) {
            if( value === true || value === false ) {
                that.confirmAutoSaveRestore = value;
            } else {
                throw SecureExam.ErrorCode.INVALIDARGUMENT;
            }
        },
        setEBookReaderExport: function( value ) {
            if( value === true || value === false ) {
                that.eBookReaderExport = value;     
            } else {
                throw SecureExam.ErrorCode.INVALIDARGUMENT;
            }
        },
        continueAutoSaveRestore: function (continueRestore) {
            if (continueRestore) {
                that.confirmAutoSaveRestore = false;
                that.init(that.User.firstname, that.User.lastname, that.User.immNumber, that.User.secret, true);
                that.confirmAutoSaveRestore = true;
            } else {
                that.confirmAutoSaveRestore = false;
                window.localStorage.removeItem("secureExamAutoSave");
                that.init(that.User.firstname, that.User.lastname, that.User.immNumber, that.User.secret);
                that.confirmAutoSaveRestore = true;
            }
        }
    }
}