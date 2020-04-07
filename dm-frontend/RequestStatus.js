(function (factory) {
    if (typeof module === "object" && typeof module.exports === "object") {
        var v = factory(require, exports);
        if (v !== undefined) module.exports = v;
    }
    else if (typeof define === "function" && define.amd) {
        define(["require", "exports", "./HitApi", "./HtmlElementsId", "./globals"], factory);
    }
})(function (require, exports) {
    "use strict";
    exports.__esModule = true;
    var HitApi_1 = require("./HitApi");
    var HtmlElementsId_1 = require("./HtmlElementsId");
    var globals_1 = require("./globals");
    var UserRequestStatus = /** @class */ (function () {
        function UserRequestStatus() {
            this.domElements = new HtmlElementsId_1.HtmlElementsData();
        }
        UserRequestStatus.prototype.generateRequestData = function (requestStatus) {
            if (requestStatus === void 0) { requestStatus = "ALL"; }
            var userName = document.getElementById(new HtmlElementsId_1.HtmlElementsData().search).getAttribute(this.domElements.userName);
            var sortAttribute = document.getElementById(this.domElements.thead).getAttribute(this.domElements.sortAttributr);
            var sortType = document.getElementById(this.domElements.thead).getAttribute(this.domElements.sortType);
            var uri = globals_1.BASEURL + "/sorting?status=" + requestStatus;
            return uri;
        };
        UserRequestStatus.prototype.requestStatusResult = function (status) {
            status = status.toLowerCase();
            var uri = this.generateRequestData(status);
            new HitApi_1.HitApi().HitGetApi(uri);
        };
        return UserRequestStatus;
    }());
    exports.UserRequestStatus = UserRequestStatus;
});
