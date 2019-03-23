var filesList = undefined;

(function () {
    document.getElementById("filepicker").addEventListener("change", function (event) {
        //let output = document.getElementById("queryResult");
        let files = event.target.files;
        filesList = files;
        //for (let i = 0; i < files.length; i++) {
        //    //let item = document.createElement("li");
        //    //item.innerHTML = files[i].webkitRelativePath;
        //    //output.appendChild(item);
        //    filesList.push();
        //};
    }, false);
})();

function SearchOccurences() {
    let output = document.getElementById("queryResult");
    for (let i = 0; i < filesList.length; i++) {
        let item = document.createElement("li");
        item.innerHTML = filesList[i].webkitRelativePath;
        output.appendChild(item);
    };

}



//if ($.fn.datetimepicker) {
//    alert("$('.datetimepicker').datetimepicker();");
//}

$(document).ready(function () {
    $("#frmDate").datetimepicker();
    $("#toDate").datetimepicker();


});


