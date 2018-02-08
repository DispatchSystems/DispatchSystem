function setDocInfo(data) {
    var obj = JSON.parse(data);

    for (var val of obj.nui['modify']) {
        var current = $(val.selector);
        current.text(val.text);
        if (val.append)
            current.append(val.append);
    }
}
function setLocalize(file) {
    $.ajax({
        method: "GET",
        url: "../lang.json",
        dataType: "text",
        success: function(data) {
            setDocInfo(data);
        }
    });
}
$(function() {
    setLocalize('../lang.json');
});
