//arguments[0] 为Selenium找到的元素
//arguments[1] 为要显示的时间,单位为秒
var tempE = arguments[0];
tempE.scrollIntoViewIfNeeded();

var width = arguments[0].offsetWidth;
var height = arguments[0].offsetHeight;
var top = getTop(tempE);
var left = getLeft(tempE);

var element = document.createElement('div');
element.style.position = 'absolute';
element.style.border = '2px solid red';
element.style.width = width+'px';
element.style.height = height+'px';
element.style.top = top + 'px';
element.style.left = left + 'px';
element.style.zIndex = '9999';

document.body.appendChild(element);

var i = arguments[1]*10
highlight(i, element);

function highlight(i,e) {
    if (i > 0) {
        if (i % 2 === 1) {
            e.style.border = '2px solid rgba(0,0,0,0)';
        }
        else {
            e.style.border = '2px solid red';
        }
        i = i - 1;
        var that = this;
        window.setTimeout(function () { highlight(i, e); }, 100);
    }
    else {
        document.body.removeChild(e);
    }
}

function getTop(e) {
    var offset = e.offsetTop;
    if (e.offsetParent !== null) offset += getTop(e.offsetParent);
    return offset;
}

function getLeft(e) {
    var offset = e.offsetLeft;
    if (e.offsetParent !== null) offset += getLeft(e.offsetParent);
    return offset;
}