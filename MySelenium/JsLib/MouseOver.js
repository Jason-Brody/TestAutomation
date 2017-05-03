Element.prototype.mouseover = function () {
    var moEvent = document.createEvent('MouseEvent');
    moEvent.initMouseEvent("mouseover", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
    this.dispatchEvent(moEvent);
}
//arguments[0].scrollIntoViewIfNeeded();
arguments[0].mouseover();

