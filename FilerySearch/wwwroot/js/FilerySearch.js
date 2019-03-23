//var multiSelectBox; //reference to multiselect Element
//var numOfPages = undefined; //total number of pages per query
//var resultFromSRV = undefined;//result of backend Server per query
//var currentPageNum = undefined; //current number of fetch page by query
//var invalidParams = undefined; //does all params are valid

//function addNew(widgetId, value) {
//    var widget = $("#" + widgetId).getKendoMultiSelect();
//    var dataSource = widget.dataSource;

//    dataSource.add({
//        ID: 0,
//        Name: value
//    });

//    dataSource.one("requestEnd", function (args) {
//        if (args.type !== "create") {
//            return;
//        }

//        var newValue = args.response[0].ID;

//        dataSource.one("sync", function () {
//            widget.value(widget.value().concat([newValue]));
//        });
//    });

//    dataSource.sync();
//    var mutliselectvalue = widget.value();
//    mutliselectvalue.push(value);
//    widget.value(mutliselectvalue);
//}

//function highlightSingleWord(str, word, className) {
//    str = String(str);
//    var regex = new RegExp(`${word}`, "gi");
//    return str.replace(regex, "<span class=\"" + className + "\">" + word + "</span>");
//}

//function highlightWords(str, words, className) {
//    str = String(str);
//    words.forEach(function (value, index, array) {
//        var regex = new RegExp(`${value}`, "gi");
//        str = str.replace(regex, "<span class=\"" + className + "\">" + value + "</span>");
//    });

//    return str;
//}

//function clearSearchOccur() {
//    $("#findOccurTreeList").empty();
//    currentPageNum = undefined;
//    numOfPages = undefined;
//    resultFromSRV = undefined;
//    invalidParams = undefined;
//    $('#searchResultHeadline h2').remove();

//}


//function generateRequestURL() {
//    let fromDate = $('#datepickerFrm').val();
//    let toDate = $('#datepickerTo').val();
//    let searchWords = multiSelectBox.dataItems().map(x => x.Name);
//    let mustMatchAll = $('#matchAllWords')[0].checked;

//    //add check to all fields..
//    if (fromDate == "" || toDate == "" || searchWords.length < 1) {
//        alert("Invalid Params.");
//        invalidParams = true;
//        return false;
//    }

//    let requestParams = `fromDate=${fromDate}&toDate=${toDate}&mustMatchAll=${mustMatchAll}&companyID=${supplierCompanyID}&supplierBranchID=${supplierBranchID}`;

//    const searchWordParams = searchWords.reduce(
//        (acc, word) => `${acc}&searchword=${word}`,
//        ''
//    );

//    requestParams = requestParams + searchWordParams;

//    invalidParams = false;
//    return '/api/Supplier/SearchOccurences?' + requestParams;
//}

//function resetPage() {
//    $("#findOccurTreeList").empty();
//    $('#datepickerFrm').val(null);
//    $('#datepickerTo').val(null);
//    currentPageNum = undefined;
//    numOfPages = undefined;
//    resultFromSRV = undefined;
//    invalidParams = undefined;
//    $('#searchResultHeadline h2').remove();


//    multiSelectBox.value([]);
//}



//function findOccurencesInSupplierFiles(fetchAnyway = false, pageNum = 1) {

//    //print to screen(testing purposes) the words we are searching for.
//    //console.log("generateRequestURL", generateRequestURL());

//    //call async function to get data from WebAPI

//    $(".StockFileSpinner").show();
//    $("#findOccurTreeList").empty();
//    $('#searchResultHeadline h2').remove();

//    findOccurencesInSupplierFilesAsync(fetchAnyway, pageNum).then(function (result) {
//        resultFromSRV = result;
//        numOfPages = result.TotalPagesNum;
//        if (currentPageNum === undefined)
//            currentPageNum = result.CurrentPage;

//        let newHearStr = '<h2>Search Results: (Took:' + resultFromSRV.Duration + ')</h2>';
//        let newHeader = $(newHearStr);
//        //console.log("function findOccurencesInSupplierFiles","currentPageNum",currentPageNum);
//        //console.log("function findOccurencesInSupplierFiles","numOfPages", numOfPages);


//        //if exceed quota ask user if he wants to bring data anyway
//        //Status = "ExccededQuota"
//        if (resultFromSRV.Status == "ExccededQuota") {
//            $('#searchResultHeadline').append(newHeader);
//            $("#findOccurTreeList").append("<strong>ExccededQuota</strong>");

//            //show dialog and try again...
//            DialogBox(`searching in suppiler files (${resultFromSRV.result}MB) might take more than 20 seconds , Are you Sure you want to continue?`);
//        }
//        //normal

//        else if (resultFromSRV.Status == "Success") {

//            $('#searchResultHeadline').append(newHeader);
//            resultFromSRV.result.forEach(function (item) {
//                $("#findOccurTreeList").append(generateMatchedFileHTMLElement(item));   //start append data results...

//            });
//        }

//        //Status = "NotFound"

//        else if (resultFromSRV.Status == "NotFound") {

//            $('#searchResultHeadline').append(newHeader);

//            $("#findOccurTreeList").append("<strong>Not Found</strong>");
//        }
//    }).then(function () { $(".StockFileSpinner").hide(); }).catch(e => console.dir(e));
//}

//function postData(url = ``, data = {}) {
//    // Default options are marked with *
//    return fetch(url, {
//        method: "POST", // *GET, POST, PUT, DELETE, etc.
//        mode: "cors", // no-cors, cors, *same-origin
//        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
//        credentials: "same-origin", // include, *same-origin, omit
//        headers: {
//            "Content-Type": "application/json; charset=utf-8",
//            // "Content-Type": "application/x-www-form-urlencoded",
//        },
//        redirect: "follow", // manual, *follow, error
//        referrer: "no-referrer", // no-referrer, *client
//        body: JSON.stringify(data), // body data type must match "Content-Type" header
//    })
//        .then(response => response.json()); // parses response to JSON
//}

//function DialogBox(dialogBoxTitle) {
//    if (confirm(dialogBoxTitle)) {
//        findOccurencesInSupplierFiles(true); //fetch data from Server
//        return true;
//    } else {
//        return false;
//    }
//}

////async function
//async function findOccurencesInSupplierFilesAsync(fetchAnyway = false, pageNum = 1) {

//    var serviceURL = generateRequestURL();
//    //await the response of the custom fetch call

//    serviceURL += `&searchAnyway=${fetchAnyway}`;
//    serviceURL += `&pageNum=${pageNum}`;

//    await $(".StockFileSpinner").show();

//    let response = await postData(serviceURL);
//    return response;
//}

//function generateMatchedFileHTMLElement(fileProps) {

//    var li = document.createElement('li'), ul = document.createElement('ul'), h4 = document.createElement('h4'),
//        txt = document.createTextNode(`${fileProps.FilePath} (${fileProps.MatchedLines.length} hits)`);

//    h4.className = "FilePropsHeader";
//    h4.appendChild(txt);
//    li.appendChild(h4);


//    for (let matchedLine of fileProps.MatchedLines) {
//        let matctedLineHTMLElement = document.createElement('li');

//        let textWithHighlight = `Line ${matchedLine.MatchedSentenceNum}: ${matchedLine.MatchedSentenceContent}`;

//        textWithHighlight = highlightWords(textWithHighlight, matchedLine.MatchedWords, "highlight"); //highlight EACH search word found in sentence

//        matctedLineHTMLElement.innerHTML = textWithHighlight;
//        ul.appendChild(matctedLineHTMLElement);
//    }

//    //foreach MatchedLine put inside li

//    ul.className = ul.className + "flow";
//    li.appendChild(ul);
//    return li;
//}

//function addItem() {

//    var itemText = $("#add-textbox").val();
//    if (!itemText) {
//        alert("Please specify the text for the new item.");
//        return false;
//    }

//    var text = $("#add-textbox").val();
//    var item = listBox.add({ name: text });

//    text.value = '';
//    $("#add-textbox").val('');
//    return true;

//}

//function setStartOfDay(date) {
//    date.setHours(0);
//    date.setMinutes(0);
//    date.setSeconds(0);
//    return kendo.toString(date, "yyyy/MM/dd HH:mm");
//}

//function setEndOfDay(date) {
//    date.setHours(23);
//    date.setMinutes(59);
//    date.setSeconds(59);
//    return kendo.toString(date, "yyyy/MM/dd HH:mm");
//}


//function fetchMoreOccurData() {
//    if ((numOfPages != undefined) && (currentPageNum != undefined) && (numOfPages >= currentPageNum)) {
//        //console.log("function fetchMoreOccurData", "numOfPages", numOfPages, "currentPageNum", currentPageNum++)
//        //alert("Fetching more data...")
//        findOccurencesInSupplierFiles(true, currentPageNum++);

//    }
//    else if (numOfPages < currentPageNum) { //no more new data available...
//        return;
//    }
//}


////execute without functions
//$("#datepickerFrm").kendoDateTimePicker({
//    value: setStartOfDay(new Date()),
//    format: "yyyy/MM/dd HH:mm", parseFormats: ["yyyy/MM/dd", "HH:mm"], timeFormat: "HH:mm"
//});




//var datepickerTo = $("#datepickerTo").kendoDateTimePicker({
//    value: setEndOfDay(new Date()),
//    format: "yyyy/MM/dd HH:mm", parseFormats: ["yyyy/MM/dd", "HH:mm"], timeFormat: "HH:mm",
//    close: function (e) {
//        if (e.view === "date") {
//            var value = setEndOfDay(new Date(datepickerTo[0].value));
//            datepickerTo[0].value = value;
//        }
//    }
//});



//$("#SearchWordslistbox").kendoMultiSelect({
//    filter: "startswith",
//    dataTextField: "Name",
//    dataValueField: "Name",
//    placeholder: "Type keywords to search within files...",
//    noDataTemplate: $("#noDataTemplate").html()
//});

//multiSelectBox = $("#SearchWordslistbox").data("kendoMultiSelect");
///******************************************************************************/

////page ready
//$(document).ready(function () {
//    ////alert("companyID= " + supplierCompanyID);
//    ////alert("branchID= " + supplierBranchID);
//    $("#findOccurTreeList").scroll(function () {
//        //    //console.log("$(window).height() == $(document).height()");
//        //    //console.log($(window).height() == $(document).height());
//        if ($("#findOccurTreeList").scrollTop() == ($("#findOccurTreeList")[0].scrollHeight - $("#findOccurTreeList").height())) {
//            fetchMoreOccurData();
//        }
//    });
//    $("#searchBtn").click(function () {
//        if (invalidParams != undefined && !invalidParams)
//            $(".StockFileSpinner").show();
//    });
//    //});

//});





//function initPage() {
//    $(".StockFileSpinner").hide();
//    const windowHeight = $(window).height();   // returns height of browser viewport
//    //calc relative to viewer res
//    const searchContainerHeight = Math.round(windowHeight * 0.77);
//    //set relative to viewer res

//    $("#mainSearchParamsContainer").height(searchContainerHeight);
//    $("#findOccurTreeList").height(searchContainerHeight * 0.66);

//}
//initPage();
