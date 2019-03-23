
//(function () {
    
//})();


//if ($.fn.datetimepicker) {
//    alert("$('.datetimepicker').datetimepicker();");
//}

$(document).ready(function () {
    $("#frmDate").datetimepicker();
    //$("#toDate").datetimepicker();
});

//document.getElementById("filepicker").addEventListener("change", function (event) {
//    let output = document.getElementById("queryResult");
//    let files = event.target.files;

//    for (let i = 0; i < files.length; i++) {
//        let item = document.createElement("li");
//        item.innerHTML = files[i].webkitRelativePath;
//        output.appendChild(item);
//    };
//}, false);