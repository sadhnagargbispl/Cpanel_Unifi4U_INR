//********************************************* Async Progress Modal (Start) *********************************************//
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);

function beginReq(sender, args) {
    $find(AsyncProgressModal).show();
}

function endReq(sender, args) {
    $find(AsyncProgressModal).hide();
}
//********************************************* Async Progress Modal (End) *********************************************//