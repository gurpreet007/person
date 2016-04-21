var CUR_YR = (new Date()).getFullYear();

var DT_PCKR_OPTS = { 
    dateFormat: "dd-M-yy", 
    changeMonth: true, 
    changeYear: true,
    yearRange: (CUR_YR - 10) + ':' + (CUR_YR + 1)
};

//function doWaterMark(myid, wmtext) {
//    var slctr = '#' + myid;
//    $(slctr).focus(function () {
//        if ($(slctr).hasClass('watermarked')) {
//            $(slctr).removeClass('watermarked');
//            if ($(slctr).val() == wmtext) {
//                $(slctr).val('');
//            }
//        }
//    });
//    $(slctr).focusout(function () {
//        if ($(slctr).val() == '') {
//            $(slctr).addClass('watermarked');
//            $(slctr).val(wmtext);
//        }
//        else if ($(slctr).val() == wmtext) {
//            $(slctr).addClass('watermarked');
//        }
//    });
//    $(slctr).change(function () {
//        if ($(slctr).hasClass('watermarked')) {
//            $(slctr).removeClass('watermarked');
//        }
//    });
//    $(slctr).trigger('focusout');
//}

function doAutoComp(myid, myurl) {
    var slctr = '#' + myid;
    $.ajax({
        type: "POST",
        url: myurl,
        dataType: "json",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var datafromServer = data.d.split(":");
            $(slctr).autocomplete({
                source: function (req, responseFn) {
                    //var str = req.term;
                    //var arr = str.split(" ");
                    
                    var rep = req.term.replace(/ /g, '.*');
                    var matcher = new RegExp(rep, "i");
                    var a = $.grep(datafromServer, function (item, index) {
                        return matcher.test(item);
                    });
                    responseFn(a);
                },
                change: function (event, ui) {
                    if (myid == 'txtploc') {
                        $('#txtpcloc').val($(slctr).val());
                    }
                }
            });
        }
    });
}
function CheckLoc(id) {
    //match anything followed by '-' followed by 9 digits
    return /^.*-\d{9}$/.test($('#' + id).val());
}
function CheckEmpid(id) {
    //match 6 digits
    return /^\d{6}$/.test($('#' + id).val());
}
function CheckName(id) {
    //match anything followed by '(' followed by 6 digits followed by ')'
    return /^.*\(\d{6}\)$/.test($('#' + id).val());
}