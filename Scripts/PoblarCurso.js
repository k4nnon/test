window.onload = function () {
    var scrollY = parseInt('<%=Request.Form["scrollY"] %>');
    if (!isNaN(scrollY)) {
        window.scrollTo(0, scrollY);
    }
};
window.onscroll = function () {
    var scrollY = document.body.scrollTop;
    if (scrollY === 0) {
        if (window.pageYOffset) {
            scrollY = window.pageYOffset;
        }
        else {
            scrollY = (document.body.parentElement) ? document.body.parentElement.scrollTop : 0;
        }
    }
    if (scrollY > 0) {
        var input = document.getElementById("scrollY");
        if (input === null) {
            input = document.createElement("input");
            input.setAttribute("type", "hidden");
            input.setAttribute("id", "scrollY");
            input.setAttribute("name", "scrollY");
            document.forms[0].appendChild(input);
        }
        input.value = scrollY;
    }
};

function GetDynamicTextBox(value) {
        
    return '<input name = "DynamicTextBox" type="text" value = "' + value + '" />' +
            '<input type="button" value="Borrar" onclick = "RemoveTextBox(this)" />'
}
function AddTextBox() {
    
    
    var div = document.createElement('DIV');
    div.innerHTML = GetDynamicTextBox("");
    document.getElementById("TextBoxContainer").appendChild(div);
   

}

function RemoveTextBox(div) {
    document.getElementById("TextBoxContainer").removeChild(div.parentNode);
}

function RecreateDynamicTextboxes() {
   
    var values = eval('<%=Values%>');
    if (values !== null) {
        var html = "";
        for (var i = 0; i < values.length; i++) {
            html += "<div>" + GetDynamicTextBox(values[i]) + "</div>";
        }
        document.getElementById("TextBoxContainer").innerHTML = html;
    }
}
