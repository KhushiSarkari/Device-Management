(function (factory) {
    if (typeof module === "object" && typeof module.exports === "object") {
        var v = factory(require, exports);
        if (v !== undefined) module.exports = v;
    }
    else if (typeof define === "function" && define.amd) {
        define(["require", "exports"], factory);
    }
})(function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var HtmlElementsData = /** @class */ (function () {
        function HtmlElementsData() {
            this.upArrow = "mdl-data-table__header--sorted-descending";
            this.downArrow = "mdl-data-table__header--sorted-ascending";
            this.search = "waterfall-exp";
            this.previous = "previous";
            this.next = "next";
            this.thead = "tableHead";
            this.sortAttributr = "sort";
            this.sortType = "sortby";
            this.resultcount = " result-count";
            this.requestStatus = "request-status";
            this.userName = "user"; // set user search in userdefine search box
            this.devicesearch = "device";
            this.deviceSerial = "serialNumber";
        }
        return HtmlElementsData;
    }());
    exports.HtmlElementsData = HtmlElementsData;
});
