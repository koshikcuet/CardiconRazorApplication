$(function () {
    var usr = getLS("User", '');
    if (isEmpty(usr) == false) {
        $("#mnwel").html('<a class="nav-link" href="#">Welcome: ' + usr + '</a>');
    }
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input:hidden[name="__RequestVerificationToken"]').val().toString()
        }
    });
});

function delReg() {
    let flts = new registration;
    flts.atn = Number($('#delid').val());
    $.post('/regis/reglist?handler=Delreg', JSON.stringify(flts), function (data) {
        location.href = "/regis/reglist";
    }, "json").fail(function () {
        location.href = "/regis/reglist";
    });
}
function delAbs() {
    let flts = new registration;
    flts.atn = Number($('#delid').val());
    $.post('/absmain/abslist?handler=Delabs', JSON.stringify(flts), function (data) {
        location.href = "abslist";
    }, "json").fail(function () {
        location.href = "abslist";
    });
}

function findreg() {
    //$("#wloader").show();
    var mob = $('#mobile').val();
    $.getJSON('/absmain/abstract?handler=Find&mobile=' + mob, function (data: registration) {
        //$("#wloader").hide();
        //$("#btnFSearch").removeAttr("disabled");
        if (data.nam == "invalid") {
            $("#nam").val('');
            $("#email").val('');
           ShowError("Mobile number not found.");
           return false;
        }
        $("#nam").val(data.nam);
        $("#email").val(data.email);

    }).fail(function () {
        //$("#wloader").hide();
        ShowError("Can't connect to server.");
        //$("#btnFSearch").removeAttr("disabled");
    });
}

function sendSMS(id) {
    $.getJSON('/api/wapi/regsms/' + id, function (data: registration) {
        if (data.nam == "invalid") {
            $("#nam").val('');
            $("#email").val('');
            ShowError("Mobile number not found.");
            return false;
        }
        ShowSuccess("SMS Sent Successfully.");

    }).fail(function () {
        ShowError("Can't connect to server.");
    });
}

//function checkSSN() {
//    let flts = new registration;
//    flts.fnam = "Mahbub";
//    flts.lnam = "Rahman";
//    //$("#wloader").show();
//    $.post('/login?handler=GetTime', JSON.stringify(flts), function (data: registration) {
//        //$("#wloader").hide();
//        //$("#btnFSearch").removeAttr("disabled");
//        //if (data.success == false) {

//        //    ShowError(data.message);
//        //    return false;
//        //}

//        setLS('FindReq', JSON.stringify(data));
//        ShowSuccess(data.fnam + ' , ' + data.lnam);

//    }, "json").fail(function () {
//        //$("#wloader").hide();
//        ShowError("Can't connect to server.");
//        //$("#btnFSearch").removeAttr("disabled");
//    });
//}

//function checkSS() {
//    //$("#wloader").show();
//    $.getJSON('/login?handler=Reg&nam=MD&lnm=Mahbub', function (data: registration) {
//        //$("#wloader").hide();
//        //$("#btnFSearch").removeAttr("disabled");
//        //if (data.success == false) {

//        //    ShowError(data.message);
//        //    return false;
//        //}

//        setLS('FindReq', JSON.stringify(data));
//        ShowSuccess(data.fnam + ' , ' + data.lnam);

//    }).fail(function () {
//        //$("#wloader").hide();
//        ShowError("Can't connect to server.");
//        //$("#btnFSearch").removeAttr("disabled");
//    });
//}